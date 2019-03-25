using System;
using Blog.API.Services.Abstractions;
using Blog.API.ViewModels.Stories;
using Blog.Data.Abstractions;
using Blog.Model.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using System.Linq;
using Blog.API.Notifications.Models;
using Microsoft.AspNetCore.SignalR;
using Blog.API.Notifications;
using System.Collections.Generic;

namespace Blog.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class StoriesController : ControllerBase
    {
        IStoryRepository _storyRepository;
        ILikeRepository _likeRepository;
        IUserRepository _userRepository;
        IShareRepository _shareRepository;
        IHubContext<NotificationsHub> _hubContext;
        IMapper _mapper;
        public StoriesController(IStoryRepository storyRepository,ILikeRepository likeRepository,IUserRepository userRepository,IShareRepository shareRepository,IHubContext<NotificationsHub> hubContext, IMapper mapper)
        {
            _storyRepository = storyRepository;
            _likeRepository = likeRepository;
            _userRepository = userRepository;
            _shareRepository = shareRepository;
            _hubContext = hubContext;
            _shareRepository = shareRepository;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public ActionResult<StoryDetailViewModel> GetStoryDetail(string id)
        {
            var story = _storyRepository.GetSingle(s => s.Id == id, s => s.Owner, s => s.Likes);
            var userId = HttpContext.User.Identity.Name;
            var liked = story.Likes.Exists(l => l.UserId == userId);

            return _mapper.Map<Story,StoryDetailViewModel>(
                story,
                opt => opt.AfterMap((src,dest) => dest.Liked = liked)
            );
        }

        
        [HttpPost]
        public ActionResult<StoryCreationViewModel> Post([FromBody] UpdateStoryViewModel model)
        {
           if(!ModelState.IsValid) return BadRequest(ModelState);

           var ownerId = HttpContext.User.Identity.Name;
           var creationTime = ((DateTimeOffset)(DateTime.UtcNow)).ToUnixTimeSeconds();
           var storyId = Guid.NewGuid().ToString();
           var story = new Story {
               Id = storyId,
               Title = model.Title,
               Content = model.Content,
               CreationTime = creationTime,
               Draft = true,
               LastEditTime = creationTime,
               Tags = model.Tags,
               OwnerId = ownerId
           };
           _storyRepository.Add(story);
           _storyRepository.Commit();

           return new StoryCreationViewModel {
               StoryId = storyId
           };
        }

        [HttpPatch("{id}")]
        public ActionResult Patch(string id, [FromBody]UpdateStoryViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            
            var userId = HttpContext.User.Identity.Name;
            if (!_storyRepository.IsOwner(id, userId) && !_storyRepository.IsInvited(id, userId)) return StatusCode(403, "You are not the allowed to edit this story");

            var newStory = _storyRepository.GetSingle(s => s.Id == id, s => s.Shares);

            newStory.Title = model.Title;
            newStory.LastEditTime = ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds();
            newStory.Tags = model.Tags;
            newStory.Content = model.Content;

            _storyRepository.Update(newStory);
            _storyRepository.Commit();

            var usersToNotify = newStory.Shares.Select(s => s.UserId).Concat(new List<string> { userId, newStory.OwnerId }).Where(uid => uid != userId);
            foreach (var uid in usersToNotify) {
                _hubContext.Clients.User(uid).SendAsync(
                    "notification",
                    new Notification<StoryEditPayload>
                    {
                        NotificationType = NotificationType.STORY_EDIT,
                        Payload = new StoryEditPayload {
                            Id = newStory.Id,
                            StoryTitle = newStory.Title,
                            LastEditTime = newStory.LastEditTime,
                            Tags = newStory.Tags,
                            Content = newStory.Content
                        }
                    }
                );
            }
            
            return NoContent();
        }

        [HttpPost("{id}/publish")]
        public ActionResult Publish (string id)
        {
            var ownerId = HttpContext.User.Identity.Name;
            if(!_storyRepository.IsOwner(id, ownerId)) return Forbid("Вы не владелец этой статьи");

            var newStory = _storyRepository.GetSingle(s => s.Id == id);
            newStory.Draft = false;
            newStory.PublishTime = ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds();
            
            _storyRepository.Update(newStory);
            _storyRepository.Commit();

            return NoContent();
        }

        [HttpGet("drafts")]
        public ActionResult<DraftsViewModel> GetDrafts() 
        {
            var ownerId = HttpContext.User.Identity.Name;

            var drafts = _storyRepository.FindBy(story => story.OwnerId == ownerId && story.Draft);
            return new DraftsViewModel {
                Stories = drafts.Select(_mapper.Map<DraftViewModel>).ToList()
            };
        }
        [HttpGet("user/{id}")]
         public ActionResult<OwnerStoriesViewModel> Get (string id)
         {
             var stories = _storyRepository.FindBy(s => s.OwnerId == id && !s.Draft);
             return new OwnerStoriesViewModel {
                 Stories = stories.Select(_mapper.Map<OwnerStoryViewModel>).ToList()
             };
         }   

        [HttpDelete("{id}")]
        public ActionResult Delete (string id)
        {
            var ownerId = HttpContext.User.Identity.Name;
            if(!_storyRepository.IsOwner(id, ownerId)) return Forbid("Вы не владелец этой статьи");

            _storyRepository.DeleteWhere(story => story.Id == id);
            _storyRepository.Commit();

            return NoContent();
        }

        [HttpPost("{id}/toggleLike")]
        public ActionResult  ToggleLike(string id) 
        {
            var userId = HttpContext.User.Identity.Name;

            var story = _storyRepository.GetSingle(s => s.Id == id, s => s.Likes);
            if(userId == story.OwnerId) return BadRequest("Нельзя лайкнуть свою запись");

            var user = _userRepository.GetSingle(s => s.Id == userId);
            var existingLike = story.Likes.Find(l => l.UserId == userId);
            var payload = new LikeRelatedPayload
            {
                UserName = user.UserName,
                StoryTitle = story.Title
            };
            if(existingLike == null)
            {
                _hubContext.Clients.User(story.OwnerId).SendAsync(
                    "notification",
                    new Notification<LikeRelatedPayload>
                    {
                        NotificationType = NotificationType.LIKE,
                        Payload = payload
                    }
                );
                _likeRepository.Add(new Like 
                {
                    UserId = userId,
                    StoryId = id
                });
            }
            else 
            {
                _hubContext.Clients.User(story.OwnerId).SendAsync(
                        "notification",
                        new Notification<LikeRelatedPayload>
                        {
                            NotificationType = NotificationType.UNLIKE,
                            Payload = payload
                        }
                );   
                _likeRepository.Delete(existingLike);
            }
            _likeRepository.Commit();

            return NoContent();
        }

        [HttpGet()]
        public ActionResult<StoriesViewModel> GetStories()
        {
            var stories = _storyRepository.AllIncluding(s => s.Owner);
            return new StoriesViewModel {
                Stories = stories.Select(_mapper.Map<StoryViewModel>).ToList()
            };
        }
        [HttpPost("{id}/share")]
        public ActionResult Share (string id, [FromBody]ShareViewModel model)
        {
           var ownerId = HttpContext.User.Identity.Name;
           if(!_storyRepository.IsOwner(id,ownerId)) return Forbid("Вы не автор этой статьи");

           var userToShare = _userRepository.GetSingle(u => u.UserName == model.UserName);
           if(userToShare == null)
           {
               return BadRequest(new {message = "Нету пользователя с таким именем"});
           }  
           var owner = _userRepository.GetSingle(u => u.Id == ownerId);
           var story = _storyRepository.GetSingle(s => s.Id == id,s => s.Shares);
           if(story.OwnerId == userToShare.Id)
           {
               return BadRequest(new {username = "Вы не можете поделиться своей статьей"});
           } 

           var existingShare = story.Shares.Find(s => s.UserId == userToShare.Id);
           if(existingShare == null)
           {
               _shareRepository.Add(new Share 
               {
                   StoryId = id,
                   UserId = userToShare.Id
               });
               _shareRepository.Commit();
               _hubContext.Clients.User(userToShare.Id).SendAsync(
                   "notification",
                   new Notification<ShareRelatedPayload>
                   {
                       NotificationType = NotificationType.SHARE,
                       Payload = new ShareRelatedPayload
                       {
                           StoryTitle = story.Title,
                           Username = owner.UserName
                       }
                   }
               );
           }
           return NoContent();
        }

         [HttpGet("shared")]
        public ActionResult<ShareDraftsViewModel> GetSharedToYouDrafts()
        {
            var userId = HttpContext.User.Identity.Name;

            var stories = _shareRepository.StoriesSharedToUser(userId).Where(s => s.Draft);
            var usernames = stories.Select(s => s.Owner.UserName).Distinct().ToList();
            
            return new ShareDraftsViewModel {
                UsersDrafts = usernames.Select(username => new UserDrafts {
                    Username = username,
                    Drafts = stories
                        .Where(s => s.Owner.UserName == username)
                        .Select(_mapper.Map<DraftViewModel>)
                        .ToList()
                }).ToList()
            };
        }
    }
}
using System;
using Blog.API.Services.Abstractions;
using Blog.API.ViewModels.Stories;
using Blog.Data.Abstractions;
using Blog.Model.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using System.Linq;

namespace Blog.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class StoriesController : ControllerBase
    {
        IStoryRepository _storyRepository;
        ILikeRepository _likeRepository;
        IMapper _mapper;
        public StoriesController(IStoryRepository storyRepository,ILikeRepository likeRepository, IMapper mapper)
        {
            _storyRepository = storyRepository;
            _likeRepository = likeRepository;
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
        public ActionResult Patch(string id, [FromBody] UpdateStoryViewModel model)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);

            var ownerId = HttpContext.User.Identity.Name;
            if(!_storyRepository.IsOwner(id, ownerId)) return Forbid("Вы не владелец этой статьи");

            var newStory = _storyRepository.GetSingle(id);
            newStory.Title = model.Title;
            newStory.Content = model.Content;
            newStory.Tags = model.Tags;
            newStory.LastEditTime = ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds();

            _storyRepository.Update(newStory);
            _storyRepository.Commit();

            return NoContent();
        }

        [HttpPost("{id}/publish")]
        public ActionResult Post (string id)
        {
            var ownerId = HttpContext.User.Identity.Name;
            if(!_storyRepository.IsOwner(id, ownerId)) return Forbid("Вы не владелец этой статьи");

            var newStory = _storyRepository.GetSingle(id);
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

            var existingLike = story.Likes.Find(l => l.UserId == userId);
            if(existingLike == null)
            {
                _likeRepository.Add(new Like 
                {
                    UserId = userId,
                    StoryId = id
                });
            }
            else 
            {
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
    }
}
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blog.Data;
using Blog.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Blog.Model.Entities;

namespace Blog.Mocker
{
    class Program
    {
        static readonly List<string> MEDIUM_USERS = new List<string> {
            "geekrodion",
            "mdfarragher",
            "pavithram",
            "jeremylikness",
            "st.mas29",
            "QuickCode",
            "mikezrimsek"
        };

        static List<Like> GenerateLikes(Pack pack)
    {
        return pack.Users.SelectMany(user => {
            var notUserStories = pack.Stories.Where(s => s.OwnerId != user.Id).ToList();
            var numberOfLikes = Convert.ToInt32((new Random()).Next(notUserStories.Count) * 0.2);

            List<Story> inner(List<Story> result, List<Story> storiesLeft, int iterationsLeft) {
                if (iterationsLeft == 0) return result;

                var storyIndex = (new Random()).Next(storiesLeft.Count);
                var newResult = result.Concat(new List<Story> { storiesLeft[storyIndex] }).ToList();
                var newStoriesLeft = storiesLeft.Where((_, i) => i != storyIndex).ToList();

                return inner(newResult, newStoriesLeft, iterationsLeft - 1);
            }

            var storiesToLike = inner(new List<Story> {}, notUserStories, numberOfLikes);
            var likes = storiesToLike.Select(s => new Like
            {
                UserId = user.Id,
                StoryId = s.Id
            });
            return likes;
        }).ToList();
    }

        static async Task MainAsync()
        {
            var medium = new Medium();
            var pack = await medium.GetPack(MEDIUM_USERS);

            var contextOptions = new DbContextOptionsBuilder<BlogContext>()
                .UseNpgsql("Server=localhost;Port=5432;Database=blog;Username=blogadmin;Password=12345")
                .Options;
            var blogContext = new BlogContext(contextOptions);

            var usersRepository = new UserRepository(blogContext);
            var users = pack.Users.Where(u => usersRepository.IsUsernameUniq(u.UserName)).ToList();
            users.ForEach(usersRepository.Add);
            usersRepository.Commit();
            Console.WriteLine($"{users.Count} new users added");

            var storiesRepository = new StoryRepository(blogContext);
            var stories = pack.Stories.Where(s => 
                storiesRepository.GetSingle(os => os.Title == s.Title && os.PublishTime == s.PublishTime) == null
            ).ToList();
            stories.ForEach(storiesRepository.Add);
            storiesRepository.Commit();
            Console.WriteLine($"{stories.Count} new stories added");

            var likeRepository = new LikeRepository(blogContext);
            var likes = GenerateLikes(new Pack { Users = pack.Users, Stories = stories });
            likes.ForEach(likeRepository.Add);
            likeRepository.Commit();
            Console.WriteLine($"{likes.Count} new likes added");
        }
        static  void Main(string[] args)
        {
                Task.Run(async () =>
                {
                    await MainAsync();
                }).GetAwaiter().GetResult();
             
        }
    }
}

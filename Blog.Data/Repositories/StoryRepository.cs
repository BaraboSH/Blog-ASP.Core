using Blog.Data.Abstractions;
using Blog.Model.Entities;

namespace Blog.Data.Repositories
{
    public class StoryRepository:EntityBaseRepository<Story>,IStoryRepository 
    {
        public StoryRepository(BlogContext context) : base(context)
        {
        }

        public bool IsOwner(string storyId, string userId)
        {
            var story = this.GetSingle(s => s.Id == storyId, s => s.Shares);
            return story.OwnerId == userId;
        }
    }
}
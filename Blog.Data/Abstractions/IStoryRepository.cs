using Blog.Model.Entities;

namespace Blog.Data.Abstractions
{
    public interface IStoryRepository:IEntityBaseRepository<Story>
    {
        bool IsOwner(string storyId, string userId);
    }
}
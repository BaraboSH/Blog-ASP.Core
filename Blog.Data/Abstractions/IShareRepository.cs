using System.Collections.Generic;
using Blog.Model.Entities;
namespace Blog.Data.Abstractions
{
    public interface IShareRepository :IEntityBaseRepository<Share>
    {
        List<Story> StoriesSharedToUser(string userId);
    }
}
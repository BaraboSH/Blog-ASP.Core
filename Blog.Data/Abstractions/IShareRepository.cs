using Blog.Model.Entities;
using System.Collections.Generic;

namespace Blog.Data.Abstractions
{
    public interface IShareRepository:IEntityBaseRepository<Share>
    {
       List<Story> StoriesSharedToUser(string id);  
    }
}
using Blog.Data.Abstractions;
using Blog.Model.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Blog.Data.Repositories
{
    public class LikeRepository : EntityBaseRepository<Like>, ILikeRepository
    {
        public LikeRepository(BlogContext context) : base(context)
        {
        }
    }
}
using Blog.Model.Entities;
namespace Blog.Data.Abstractions
{
    public interface ILikeRepository 
    {
        void Delete(Like entity);
        void Commit();
        void Add(Like entity);
    }
}
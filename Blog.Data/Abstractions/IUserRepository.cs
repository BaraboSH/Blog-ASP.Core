using Blog.Model.Entities;
namespace Blog.Data.Abstractions
{
    public interface IUserRepository:IEntityBaseRepository<User>
    {
       bool IsEmailUniq (string email);
       bool IsUsernameUniq (string username);  
    }
}
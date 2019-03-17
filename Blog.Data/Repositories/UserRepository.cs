using Blog.Data.Abstractions;
using Blog.Model.Entities;

namespace Blog.Data.Repositories
{
    public class UserRepository : EntityBaseRepository<User>, IUserRepository
    {
        public UserRepository(BlogContext context):base(context){}
        public bool IsEmailUniq(string email)
        {
            var user = this.GetSingle(u => u.Email == email);
            return user == null;
        }

        public bool IsUsernameUniq(string username)
        {
            var user = this.GetSingle(u => u.UserName == username);
            return user == null;
        }
    }
}
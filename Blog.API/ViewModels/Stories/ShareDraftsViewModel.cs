using System.Collections.Generic;
namespace Blog.API.ViewModels.Stories
{
    public class UserDrafts
    {
        public string Username { get; set; }
        public List<DraftViewModel> Drafts { get; set; }
    }
    public class ShareDraftsViewModel
    {
        public List<UserDrafts> UsersDrafts {get;set;}
    }
}
using System.Collections.Generic;
using Blog.API.ViewModels.Stories;

namespace Blog.API.ViewModels.Shares
{
    public class UserDrafts
    {
      public string Username { get; set; }
      public List<DraftViewModel> Drafts { get; set; }
    }
    public class SharedDraftsViewModel
    {
        public List<UserDrafts> UsersDrafts { get; set; }
    }
}
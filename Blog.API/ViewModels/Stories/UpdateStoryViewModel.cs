using System.Collections.Generic;
namespace Blog.API.ViewModels.Stories
{
    public class UpdateStoryViewModel
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public List<string> Tags { get; set; }
    }
}
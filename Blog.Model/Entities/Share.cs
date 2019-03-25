using System.ComponentModel.DataAnnotations.Schema;
using Blog.Model.Abstractions;

namespace Blog.Model.Entities
{
    [Table("Share")]
    public class Share 
    {
        public string StoryId { get; set; }
        public Story Story { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
    }
}
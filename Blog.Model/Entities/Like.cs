using Blog.Model.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.Model.Entities
{
    [Table("Like")]
    public class Like 
    {
        [Key]
        public int Id { get; set; }
        public string StoryId { get; set; }
        public Story Story { get; set; } 
        public string UserId { get; set; }
        public User User { get; set; }   
    }
}
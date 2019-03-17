using Blog.Model.Abstractions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace Blog.Model.Entities
{
    [Table("User")]
    public class User : IEntityBase
    {
        public string Id { get; set; }
        [Required]
        [MaxLength(60)]
        public string UserName { get; set; }
        [Required]
        [MaxLength(60)]
        public string Email { get; set; }
        public string Password { get; set; }
        public ICollection<Story> Stories { get; set; }
        public ICollection<Like> Likes { get; set; }
    }
}
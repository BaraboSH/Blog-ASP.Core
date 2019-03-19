using System.Linq;
using Microsoft.EntityFrameworkCore;
using Blog.Model.Entities;

namespace Blog.Data
{
    public class BlogContext:DbContext
    {
       public DbSet<User> Users { get; set; }
       public BlogContext(DbContextOptions<BlogContext> options):base(options){ }
       protected override void OnModelCreating(ModelBuilder modelBuilder)
       {
           foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
           {
              relationship.DeleteBehavior = DeleteBehavior.SetNull; 
           }
           modelBuilder.Entity<Story>()
            .HasOne(s => s.Owner)
            .WithMany(u => u.Stories)
            .HasForeignKey(s => s.OwnerId);

            modelBuilder.Entity<Like>()
            .HasKey(l => new{l.StoryId,l.UserId});

            modelBuilder.Entity<Like>()
            .HasOne(s => s.Story)
            .WithMany(i => i.Likes)
            .HasForeignKey(x => x.StoryId)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Like>()
            .HasOne(s => s.User)
            .WithMany(i => i.Likes)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
       }

    }
}
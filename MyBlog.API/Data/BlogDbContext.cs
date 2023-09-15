using Microsoft.EntityFrameworkCore;
using MyBlog.API.Models.Entities;

namespace MyBlog.API.Data
{
    public class BlogDbContext: DbContext
    {
        public BlogDbContext(DbContextOptions options): base(options)
        {

        }

        public DbSet<Post> Posts { get; set; }
    }
}

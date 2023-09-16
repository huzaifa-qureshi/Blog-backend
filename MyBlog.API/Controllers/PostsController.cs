using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBlog.API.Data;
using MyBlog.API.Models.DTO;
using MyBlog.API.Models.Entities;
using MyBlog.API.Models.NewFolder;

namespace MyBlog.API.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class PostsController : Controller
    {
        private readonly BlogDbContext dbContext;

        public PostsController(BlogDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllPosts()
        {
            var posts = await dbContext.Posts.ToListAsync();

            return Ok(posts);
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetPostById")]
        public async Task<IActionResult> GetPostById(Guid id)
        {
            
            var post = await dbContext.Posts.FirstOrDefaultAsync(x => x.Id == id);

            if (post != null) 
            {
                return Ok(post);
            }
            else
            {
                return NotFound();
            }
            
        }

        [HttpPost]
        public async Task<IActionResult> AddPost(AddPostRequest addPostRequest){

            var post = new Post()
            {
                Title = addPostRequest.Title,
                Content = addPostRequest.Content,
                Author = addPostRequest.Author,
                FeaturedImageUrl = addPostRequest.FeaturedImageUrl,
                PublishDate = addPostRequest.PublishDate,
                UpdatedDate = addPostRequest.UpdatedDate,
                Summary = addPostRequest.Summary,
                UrlHandle = addPostRequest.UrlHandle,
                Visible = addPostRequest.Visible,

            };
            post.Id = Guid.NewGuid();
            await dbContext.Posts.AddAsync(post);
            await dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPostById), new { id = post.Id }, post);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdatePost([FromRoute] Guid id, UpdatePostRequest updatepostrequest)
        {

            var existingPost = await dbContext.Posts.FindAsync(id);

            if (existingPost != null)
            {
                existingPost.Author = updatepostrequest.Author;
                existingPost.Title = updatepostrequest.Title;
                existingPost.Content = updatepostrequest.Content;
                existingPost.FeaturedImageUrl = updatepostrequest.FeaturedImageUrl;
                existingPost.PublishDate = updatepostrequest.PublishDate;
                existingPost.UpdatedDate = updatepostrequest.UpdatedDate;
                existingPost.Summary = updatepostrequest.Summary;
                existingPost.UrlHandle = updatepostrequest.UrlHandle;
                existingPost.Visible = updatepostrequest.Visible;

                await dbContext.SaveChangesAsync();

                return Ok(existingPost);
            }

            return NotFound();
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeletePost(Guid id)
        {
            var existingpost = await dbContext.Posts.FindAsync(id);

            if (existingpost != null)
            {
                dbContext.Remove(existingpost);
                await dbContext.SaveChangesAsync();
                return Ok(existingpost);
            }

            return NotFound();
        }
    }
}

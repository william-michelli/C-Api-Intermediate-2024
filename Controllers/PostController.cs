using DotnetAPI.Data;
using DotnetAPI.Dtos;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class PostController : Controller
    {
        private readonly DataContextDapper _dapper;

        public PostController(IConfiguration config)
        {
            _dapper = new DataContextDapper(config);
        }

        #region Get

        [HttpGet("Posts")]
        public IEnumerable<Post> GetPosts()
        {
            string sql = "SELECT * FROM TutorialAppSchema.Posts";

            return _dapper.LoadData<Post>(sql); ;
        }

        [HttpGet("PostSingle/{id}")]
        public Post GetPostSingle(int id)
        {
            string sql = $"SELECT [PostId],[UserId],[PostTitle],[PostContent],[PostCreated],[PostUpdates] FROM TutorialAppSchema.Posts WHERE PostId={id}";

            return _dapper.LoadDataSingle<Post>(sql);
        }

        [HttpGet("PostsByUser/{id}")]
        public IEnumerable<Post> GetPostsByUser(int id)
        {
            string sql = $"SELECT [PostId],[UserId],[PostTitle],[PostContent],[PostCreated],[PostUpdates] FROM TutorialAppSchema.Posts WHERE UserId={id}";

            return _dapper.LoadData<Post>(sql);
        }

        [HttpGet("MyPosts")]
        public IEnumerable<Post> GetMyPosts()
        {
            string id = this.User.FindFirst("userId")?.Value;

            string sql = $"SELECT [PostId],[UserId],[PostTitle],[PostContent],[PostCreated],[PostUpdates] FROM TutorialAppSchema.Posts WHERE UserId={id}";

            return _dapper.LoadData<Post>(sql);
        }

        #endregion

        #region Post, Put e Delete

        [HttpPost("Post")]
        public IActionResult AddPost(AddPostDTO post)
        {
            string? id = this.User.FindFirst("userId")?.Value;

            string sql = $"INSERT INTO TutorialAppSchema.Posts([UserId],[PostTitle],[PostContent],[PostCreated],[PostUpdates]) VALUES('{id}','{post.PostTitle}','{post.PostContent}', GETDATE(), GETDATE() )";

            if (_dapper.ExecuteSql(sql))
            {
                return Ok();
            }

            throw new Exception("Failed to create new post");
        }

        [HttpPut("Post")]
        public IActionResult EditPost(EditPostDTO post)
        {
            string? id = this.User.FindFirst("userId")?.Value;

            //SO ATUALIZA SE O USUARIO QUE CRIOU O POST FOR MESMO QUE ESTA TENTANDO ATUALIZAR
            string sql = $"UPDATE TutorialAppSchema.Posts SET PostContent = '{post.PostContent}', PostTitle = '{post.PostTitle}', PostUpdates = GETDATE() WHERE PostId={post.PostId} AND UserId={id}";

            if (_dapper.ExecuteSql(sql))
            {
                return Ok();
            }

            throw new Exception("Failed to edit post");
        }

        [HttpDelete("Post/{postId}")]
        public IActionResult DeletePost(int postId)
        {
            string? id = this.User.FindFirst("userId")?.Value;

            //SO DELETA SE O USUARIO QUE CRIOU O POST FOR MESMO QUE ESTA TENTANDO DELETAR
            string sql = $"DELETE FROM TutorialAppSchema.Posts WHERE PostId={postId} AND UserId={id}";

            if (_dapper.ExecuteSql(sql))
            {
                return Ok();
            }

            throw new Exception("Failed to delete post");
        }

        #endregion


        [HttpGet("PostsBySearch/{search}")]
        public IEnumerable<Post> PostsBySearch(string search)
        {
            string id = this.User.FindFirst("userId")?.Value;

            string sql = $"SELECT * FROM TutorialAppSchema.Posts WHERE PostTitle LIKE '%{search}%' OR PostContent LIKE '%{search}%'";

            return _dapper.LoadData<Post>(sql);
        }

    }
}

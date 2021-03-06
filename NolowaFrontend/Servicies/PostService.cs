using NolowaFrontend.Models;
using NolowaFrontend.Models.Base;
using NolowaFrontend.Servicies.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NolowaFrontend.Servicies
{
    public interface IPostService
    {
        Task<ResponseBaseEntity<List<Post>>> GetMyPostsAsync(long userId);
        Task<ResponseBaseEntity<List<Post>>> GetPostsAsync(long id);
        Task<ResponseBaseEntity<Post>> InsertPostAsync(Post post);
    }

    public class PostService : ServiceBase, IPostService
    {
        //private const string parentEndPoint = "Post";
        //private const string parentEndPoint = "Posts";

        public override string ParentEndPoint => "Posts";

        public async Task<ResponseBaseEntity<List<Post>>> GetMyPostsAsync(long userId)
        {
            return await DoGet<List<Post>>($"{userId}/Posts");
        }

        public async Task<ResponseBaseEntity<List<Post>>> GetPostsAsync(long id)
        {
            return await DoGet<List<Post>>($"{id}/Followers");
        }

        public async Task<ResponseBaseEntity<Post>> InsertPostAsync(Post post)
        {
            return await DoPost<Post, Post>($"New", post);
        }
    }
}

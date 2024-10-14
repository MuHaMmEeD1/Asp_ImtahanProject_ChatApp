using Asp_ImtahanProject_ChatApp.Business.Abstract;
using Asp_ImtahanProject_ChatApp.DataAccess.Abstract;
using Asp_ImtahanProject_ChatApp.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Asp_ImtahanProject_ChatApp.Business.Concrete
{
    public class PostService : IPostService
    {
        private readonly IPostDal _postDal;
        private readonly IUserFriendService _userFriendService;

        public PostService(IPostDal postDal, IUserFriendService userFriendService)
        {
            _postDal = postDal;
            _userFriendService = userFriendService;
        }

        public async Task AddAsync(Post post)
        {
            await _postDal.AddAsync(post);
        }

        public async Task DeleteAsync(int id)
        {
            var postToDelete = await _postDal.GetAsync(p=>p.Id==id);
            if (postToDelete != null)
            {
                await _postDal.DeleteAsync(postToDelete);
            }
        }

        public async Task<IEnumerable<Post>> GetAllAsync(Expression<Func<Post, bool>> filter = null)
        {
            return await _postDal.GetListAsync(filter);
        }

        public async Task<Post> GetByIdAsync(int id)
        {
            return await _postDal.GetAsync(p => p.Id == id);
        }

        public async Task<List<Post>> GetFrendsPostsAsync(string userId)
        {
           
            return await _userFriendService.GetUserFriendsPostsListAsync(userId);

        
        } 
        
        public async Task<List<Post>> GetMyPostsAsync(string userId, string TagName = null)
        {
           if(TagName == null)
            {
            return await _postDal.GetListAsync(p=>p.UserId == userId);

            }
            return await _postDal.GetListAsync(p => p.UserId == userId && p.PostTags.Any(pt => pt.Tag.Name == TagName));

        }

        public async Task<List<Post>> GetIncludeListAsync(Expression<Func<Post, bool>> filter = null)
        {
            return await _postDal.GetIncludeListAsync(filter);
        }

        public async Task UpdateAsync(Post post)
        {
            await _postDal.UpdateAsync(post);
        }

        public async Task<List<Post>> GetMyFriendVideoPostAsync(string userId)
        {
            var friendPosts = await _userFriendService.GetUserFriendsPostsListAsync(userId) ?? new List<Post>();

           
            return friendPosts
                .Where(p => p.VideoLink != null)
                .OrderByDescending(p => p.DateTime) 
                .Take(3) 
                .ToList();
        }

    }

}

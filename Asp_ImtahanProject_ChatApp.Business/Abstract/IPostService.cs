using Asp_ImtahanProject_ChatApp.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Asp_ImtahanProject_ChatApp.Business.Abstract
{
    public interface IPostService
    {
        Task AddAsync(Post post);
        Task<Post> GetByIdAsync(int id);
        Task<IEnumerable<Post>> GetAllAsync(Expression<Func<Post, bool>> filter = null);
        Task<List<Post>> GetFrendsPostsAsync(string userId);
        Task<List<Post>> GetMyFriendVideoPostAsync(string userId);
        Task<List<Post>> GetMyPostsAsync(string userId, string TagName = null);
        Task<List<Post>> GetIncludeListAsync(Expression<Func<Post, bool>> filter = null);
        Task UpdateAsync(Post post);
        Task DeleteAsync(int id);
    }
}

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

        public PostService(IPostDal postDal)
        {
            _postDal = postDal;
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

        public async Task UpdateAsync(Post post)
        {
            await _postDal.UpdateAsync(post);
        }
    }

}

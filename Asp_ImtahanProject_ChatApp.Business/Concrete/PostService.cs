using Asp_ImtahanProject_ChatApp.Business.Abstract;
using Asp_ImtahanProject_ChatApp.DataAccess.Abstract;
using Asp_ImtahanProject_ChatApp.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}

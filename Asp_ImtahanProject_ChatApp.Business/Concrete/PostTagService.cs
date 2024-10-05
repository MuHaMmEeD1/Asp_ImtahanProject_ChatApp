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
    public class PostTagService : IPostTagService
    {
        private readonly IPostTagDal _postTagDal;

        public PostTagService(IPostTagDal postTagDal)
        {
            _postTagDal = postTagDal;
        }

        public async Task AddAsync(PostTag postTag) {

            await _postTagDal.AddAsync(postTag);
        
        }


    }
}

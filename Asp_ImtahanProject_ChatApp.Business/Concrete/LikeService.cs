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
    public class LikeService : ILikeService
    {
        private readonly ILikeDal _likeDal;

        public LikeService(ILikeDal likeDal)
        {
            _likeDal = likeDal;
        }

        public async Task AddAsync(Like like)
        {
            await _likeDal.AddAsync(like);
        }

        public async Task DeleteAsync(Like like)
        {
            await _likeDal.DeleteAsync(like);
        }

        public async Task<Like> GetById(int id)
        {
            return await _likeDal.GetAsync(l=>l.Id == id);
        }
    }
}

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
    public class FriendshipRequestService : IFriendshipRequestService
    {
        private readonly IFriendshipRequestDal _friendshipRequestDal;

        public FriendshipRequestService(IFriendshipRequestDal friendshipRequestDal)
        {
            _friendshipRequestDal = friendshipRequestDal;
        }

        public async Task AddAsync(FriendshipRequest friendshipRequest)
        {
            await _friendshipRequestDal.AddAsync(friendshipRequest);
        }

        public async Task DeleteAsync(int id)
        {
            FriendshipRequest friendshipRequest = await _friendshipRequestDal.GetAsync(fr => fr.Id == id); 
            await _friendshipRequestDal.DeleteAsync(friendshipRequest);

        }

        public async Task<FriendshipRequest> GetByIdAsync(int id)
        {
           return await _friendshipRequestDal.GetAsync(fr => fr.Id == id); 
        }

        public async Task<List<FriendshipRequest>> GetListAsync(string userId)
        {
            return await _friendshipRequestDal.GetListAsync(fr=>fr.UserId == userId);
        }
    }
}

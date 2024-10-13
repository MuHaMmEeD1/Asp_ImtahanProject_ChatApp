using Asp_ImtahanProject_ChatApp.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asp_ImtahanProject_ChatApp.Business.Abstract
{
    public interface IFriendshipRequestService
    {
        Task AddAsync(FriendshipRequest friendshipRequest);
        Task DeleteAsync(int id);
        Task DeleteUserIdAndOutherIdAsync(string userId, string outherId);
        Task<List<FriendshipRequest>> GetListAsync(string userId);
        Task<List<FriendshipRequest>> GetListDidItAppearAsync(string userId);
        Task<List<FriendshipRequest>> GetListuUnansweredAsync(string userId);
        Task<FriendshipRequest> GetByIdAsync(int id);
        Task UpdateAsync(FriendshipRequest friendshipRequest);


    }
}

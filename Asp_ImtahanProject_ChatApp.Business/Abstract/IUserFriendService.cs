using Asp_ImtahanProject_ChatApp.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asp_ImtahanProject_ChatApp.Business.Abstract
{
    public interface IUserFriendService
    {
        Task AddAsync(UserFriend userFriend);
        Task DeleteAsync(int id);
        Task<List<UserFriend>> GetUserFriendsOrUFFListAsync(string myUserId, string outherUserName = "");



    }
}

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
    public class UserFriendService : IUserFriendService
    {
        private readonly IUserFriendDal _userFriendDal;

        public UserFriendService(IUserFriendDal userFriendDal)
        {
            _userFriendDal = userFriendDal;
        }

        public async Task AddAsync(UserFriend userFriend)
        {
            await _userFriendDal.AddAsync(userFriend);
        }

        public async Task DeleteAsync(int id)
        {
            UserFriend userFriend = await _userFriendDal.GetAsync(uf=>uf.Id == id);

            await _userFriendDal.DeleteAsync(userFriend);
        }

        public async Task<List<UserFriend>> GetUserFriendsOrUFFListAsync(string myUserId , string outherUserName = "")
        {

            return outherUserName == "" ?
                  await _userFriendDal.GetListAsync(uf => uf.UserFriendFirstId == myUserId || uf.UserFriendSecondId == myUserId)
                 :
                  await _userFriendDal.GetListAsync(uf => (uf.UserFriendFirstId == myUserId && (uf.UserFriendSecond.FirstName+ " "+ uf.UserFriendSecond.LastName) == outherUserName) 
                  ||
                  (uf.UserFriendSecondId == myUserId && (uf.UserFriendFirst.FirstName+" "+uf.UserFriendFirst.LastName) == outherUserName));





        }
    }
}

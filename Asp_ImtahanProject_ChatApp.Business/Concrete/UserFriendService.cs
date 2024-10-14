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

        public async Task DeleteUserIdAdnOutherIdAsync(string userId, string outherId)
        {
            UserFriend userFriend = (await _userFriendDal.GetListAsync(ufd => (ufd.UserFriendFirstId == userId && ufd.UserFriendSecondId == outherId) || (ufd.UserFriendFirstId == outherId && ufd.UserFriendSecondId == userId)))[0];

            await _userFriendDal.DeleteAsync(userFriend);
        }

        public async Task<List<UserFriend>> GetMyFriendAsync(string userId)
        {
            return await _userFriendDal.GetListAsync(uf=>uf.UserFriendFirstId == userId || uf.UserFriendSecondId == userId);
        }

        public async Task<List<UserFriend>> GetUserFriendsOrUFFListAsync(string myUserId, string outherUserName = "")
        {
            if (string.IsNullOrEmpty(outherUserName))
            {
                return await _userFriendDal.GetListAsync(uf => uf.UserFriendFirstId == myUserId || uf.UserFriendSecondId == myUserId);
            }

            var userFriends = await _userFriendDal.GetListAsync(uf =>
                (uf.UserFriendFirstId == myUserId && (uf.UserFriendSecond.FirstName + " " + uf.UserFriendSecond.LastName).ToLower().Contains(outherUserName.ToLower()))
                ||
                (uf.UserFriendSecondId == myUserId && (uf.UserFriendFirst.FirstName + " " + uf.UserFriendFirst.LastName).ToLower().Contains(outherUserName.ToLower())));

            return userFriends
                .OrderByDescending(uf =>
                    (uf.UserFriendFirstId == myUserId && (uf.UserFriendSecond.FirstName + " " + uf.UserFriendSecond.LastName).ToLower().StartsWith(outherUserName.ToLower()))
                    ||
                    (uf.UserFriendSecondId == myUserId && (uf.UserFriendFirst.FirstName + " " + uf.UserFriendFirst.LastName).ToLower().StartsWith(outherUserName.ToLower())) ? 1 : 0)
                .ThenBy(uf => uf.UserFriendFirstId == myUserId ?
                    (uf.UserFriendSecond.FirstName + " " + uf.UserFriendSecond.LastName)
                    : (uf.UserFriendFirst.FirstName + " " + uf.UserFriendFirst.LastName))
                .ToList();
        }


        public async Task<List<Post>> GetUserFriendsPostsListAsync(string userId)
        {
            List<Post> posts = new List<Post>();

            var postListFirst =   (await _userFriendDal.GetListAsync(ufd => (ufd.UserFriendFirstId == userId ))).Select(uf => uf.UserFriendSecond.Posts);

            var postListSecound = (await _userFriendDal.GetListAsync(ufd => (ufd.UserFriendSecondId == userId))).Select(uf => uf.UserFriendFirst.Posts);


            foreach (var item1 in postListFirst)
            {
                foreach (var item2 in item1)
                {
                    posts.Add(item2);
                }
            }  
            
            foreach (var item1 in postListSecound)
            {
                foreach (var item2 in item1)
                {
                    posts.Add(item2);
                }
            }


            return posts;

        }
    }
}

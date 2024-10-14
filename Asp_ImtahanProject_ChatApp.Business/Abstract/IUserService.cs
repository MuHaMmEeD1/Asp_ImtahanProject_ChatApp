using Asp_ImtahanProject_ChatApp.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asp_ImtahanProject_ChatApp.Business.Abstract
{
    public interface IUserService
    {
        Task<User> GetUserByIdAsync(string userId);
        Task UpdateAsync(User user);
        Task<bool> VerifyPasswordAsync(User user, string password);
        Task<bool> ChangePasswordAsync(User user, string newPassword);


    }
}

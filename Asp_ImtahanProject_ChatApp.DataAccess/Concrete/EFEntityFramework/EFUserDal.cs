using Asp_ImtahanProject_ChatApp.Core.Concrete.EntityFramework;
using Asp_ImtahanProject_ChatApp.DataAccess.Abstract;
using Asp_ImtahanProject_ChatApp.DataAccess.Data;
using Asp_ImtahanProject_ChatApp.Entities.Concrete;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asp_ImtahanProject_ChatApp.DataAccess.Concrete.EFEntityFramework
{
    public class EFUserDal : EFEntityRepositoryBase<User, ZustDbContext>, IUserDal
    {
        private readonly ZustDbContext _context; 
        public EFUserDal(ZustDbContext context) : base(context)
        {
            _context = context;
        }


        public async Task<User> GetByIdAsync(string userId)
        {
           return await _context.Users.FindAsync(userId);
 
        }

       
    }
}

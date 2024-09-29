using Asp_ImtahanProject_ChatApp.Core.Concrete.EntityFramework;
using Asp_ImtahanProject_ChatApp.DataAccess.Abstract;
using Asp_ImtahanProject_ChatApp.DataAccess.Data;
using Asp_ImtahanProject_ChatApp.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asp_ImtahanProject_ChatApp.DataAccess.Concrete.EFEntityFramework
{
    public class EFFriendshipRequestDal : EFEntityRepositoryBase<FriendshipRequest, ZustDbContext>, IFriendshipRequestDal
    {
        public EFFriendshipRequestDal(ZustDbContext context) : base(context)
        {
        }
    }
}

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
    public class EFMessageDal : EFEntityRepositoryBase<Message, ZustDbContext>, IMessageDal
    {
        public EFMessageDal(ZustDbContext context) : base(context)
        {
        }
    }
}

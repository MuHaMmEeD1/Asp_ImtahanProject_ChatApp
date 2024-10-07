using Asp_ImtahanProject_ChatApp.Core.Abstract;
using Asp_ImtahanProject_ChatApp.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Asp_ImtahanProject_ChatApp.DataAccess.Abstract
{
    public interface IPostDal : IEntityRepository<Post>
    {
        Task<List<Post>> GetIncludeListAsync(Expression<Func<Post, bool>> filter = null);

    }
}

using Asp_ImtahanProject_ChatApp.Core.Concrete.EntityFramework;
using Asp_ImtahanProject_ChatApp.DataAccess.Abstract;
using Asp_ImtahanProject_ChatApp.DataAccess.Data;
using Asp_ImtahanProject_ChatApp.Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Asp_ImtahanProject_ChatApp.DataAccess.Concrete.EFEntityFramework
{
    public class EFPostDal : EFEntityRepositoryBase<Post, ZustDbContext>, IPostDal
    {
        private readonly ZustDbContext _context;

        public EFPostDal(ZustDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Post>> GetIncludeListAsync(Expression<Func<Post, bool>> filter = null)
        {
            IQueryable<Post> query = _context.Posts
                .Include(p => p.User)
                .Include(p => p.Comments)
                    .ThenInclude(c => c.User)
                .Include(p => p.Comments)
                    .ThenInclude(p => p.ReplyToComments);
                
                   

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return await query.ToListAsync();
        }


    }
}

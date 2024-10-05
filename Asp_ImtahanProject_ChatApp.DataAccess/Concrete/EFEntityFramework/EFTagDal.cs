using Asp_ImtahanProject_ChatApp.Core.Concrete.EntityFramework;
using Asp_ImtahanProject_ChatApp.DataAccess.Abstract;
using Asp_ImtahanProject_ChatApp.DataAccess.Data;
using Asp_ImtahanProject_ChatApp.Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asp_ImtahanProject_ChatApp.DataAccess.Concrete.EFEntityFramework
{
    public class EFTagDal : EFEntityRepositoryBase<Tag, ZustDbContext>, ITagDal
    {
        private readonly ZustDbContext _context;

        public EFTagDal(ZustDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Tag> GetOrCreateTagAsync(string tagName)
        {

            var tag = await _context.Tags.FirstOrDefaultAsync(t => t.Name == tagName);

           
            if (tag == null)
            {
                tag = new Tag { Name = tagName };

                
                _context.Tags.Add(tag);
                await _context.SaveChangesAsync(); 
            }

            return tag; 
        }
    }
}

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
    public class TagService : ITagService
    {
        private readonly ITagDal _tagDal;

        public TagService(ITagDal tagDal)
        {
            _tagDal = tagDal;
        }

        public async Task<Tag> GetOrCreateTagAsync(string tagName)
        {
            return await _tagDal.GetOrCreateTagAsync(tagName);
        }
    }
}

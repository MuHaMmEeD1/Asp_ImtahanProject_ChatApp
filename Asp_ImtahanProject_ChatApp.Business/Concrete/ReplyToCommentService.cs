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
    public class ReplyToCommentService : IReplyToCommentService
    {
        private readonly IReplyToCommentDal _replyToCommentDal;

        public ReplyToCommentService(IReplyToCommentDal replyToCommentDal)
        {
            _replyToCommentDal = replyToCommentDal;
        }

        public async Task AddAsync(ReplyToComment replyToComment)
        {
            await _replyToCommentDal.AddAsync(replyToComment);
        }
    }
}

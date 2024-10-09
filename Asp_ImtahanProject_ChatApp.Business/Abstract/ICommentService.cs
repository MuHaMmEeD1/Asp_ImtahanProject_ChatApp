using Asp_ImtahanProject_ChatApp.DataAccess.Abstract;
using Asp_ImtahanProject_ChatApp.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asp_ImtahanProject_ChatApp.Business.Abstract
{
    public interface ICommentService
    {
        Task AddAsync(Comment comment);
        Task<Comment> GetByIdAsync(int id);

    }
}

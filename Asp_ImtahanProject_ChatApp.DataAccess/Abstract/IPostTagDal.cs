using Asp_ImtahanProject_ChatApp.Core.Abstract;
using Asp_ImtahanProject_ChatApp.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asp_ImtahanProject_ChatApp.DataAccess.Abstract
{
    public interface IPostTagDal : IEntityRepository<PostTag>
    {

        Task AddAsync(PostTag postTag);

    }
}

using Asp_ImtahanProject_ChatApp.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asp_ImtahanProject_ChatApp.Business.Abstract
{
    public interface IMessageService
    {

        Task AddAsync(Message message);
        Task<List<Message>> GetUserIdMessagesAsync(string userId, string otherUserId);
        Task UpdateAsync(Message message);
        Task<Message> GetByIdAsync(int id);
 



    }
}

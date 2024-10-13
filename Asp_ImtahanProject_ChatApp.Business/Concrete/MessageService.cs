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
    public class MessageService : IMessageService
    {
        private readonly IMessageDal _messageDal;

        public MessageService(IMessageDal messageDal)
        {
            _messageDal = messageDal;
        }

        public async Task AddAsync(Message message)
        {
            await _messageDal.AddAsync(message);
        }

        public async Task<Message> GetByIdAsync(int id)
        {
            return await _messageDal.GetAsync(m => m.Id == id);
        }

        public async Task<List<Message>> GetUserIdMessagesAsync(string userId, string otherUserId)
        {
            var messages = await _messageDal.GetListAsync(m =>
                (m.UserId == userId && m.RecipientUserId == otherUserId) || (m.UserId == otherUserId && m.RecipientUserId == userId));



            return messages.OrderBy(m => m.DateTime).ToList();
        }


        public async Task UpdateAsync(Message message)
        {
            await _messageDal.UpdateAsync(message);
        }
    }
}

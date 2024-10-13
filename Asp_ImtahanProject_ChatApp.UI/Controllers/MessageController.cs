using Asp_ImtahanProject_ChatApp.Business.Abstract;
using Asp_ImtahanProject_ChatApp.Entities.Concrete;
using Asp_ImtahanProject_ChatApp.UI.Models.MessageModels;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Asp_ImtahanProject_ChatApp.UI.Controllers
{
    public class MessageController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IMessageService _messageService;

        public MessageController(IMapper mapper, IMessageService messageService)
        {
            _mapper = mapper;
            _messageService = messageService;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] MessageCreateModel model)
        {

            Message message = _mapper.Map<Message>(model);
            message.Seen = false;
            message.DateTime = DateTime.Now;

            await _messageService.AddAsync(message);

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Messages([FromBody]MessageGetDataModel model)
        {

            List<Message> messages = await _messageService.GetUserIdMessagesAsync(model.UserId, model.OtherUserId);
            List<MessageModel> messageModels = _mapper.Map<List<MessageModel>>(messages); 

            return Ok(messageModels);

        }  [HttpGet]
        public async Task<IActionResult> HeaderMessages(MessageGetDataModel model)
        {

            List<Message> messages = await _messageService.GetUserIdMessagesAsync(model.UserId, model.OtherUserId);
            List<MessageHeaderModel> messageModels = _mapper.Map<List<MessageHeaderModel>>(messages);

            return Ok(messageModels);

        }

        [HttpPost("{messageId}")]
        public async Task<IActionResult> Update(int messageId)
        {
            Message message = await _messageService.GetByIdAsync(messageId);
            message.Seen = true;
            await _messageService.UpdateAsync(message);

            return Ok();

        }

    }
}

using Asp_ImtahanProject_ChatApp.Entities.Concrete;

namespace Asp_ImtahanProject_ChatApp.UI.Models.MessageModels
{
    public class MessageModel
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public string? MessageStr { get; set; }
        public string? DateTime { get; set; }
        public bool Seen { get; set; }

    }
}

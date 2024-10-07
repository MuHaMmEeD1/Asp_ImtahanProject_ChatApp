using Asp_ImtahanProject_ChatApp.UI.Models.ReplyToCommentModels;

namespace Asp_ImtahanProject_ChatApp.UI.Models.CommentModels
{
    public class CommentModel
    {
        public int Id { get; set; }
        public string? Text { get; set; }
        public string? DateTime { get; set; }
        public string? UserProfileImageUrl { get; set; }
        public string? UserName { get; set; }
        public string? UserId { get; set; }

        public ICollection<ReplyToCommentModel>? ReplyToComments { get; set; }
    }
}

using Asp_ImtahanProject_ChatApp.Entities.Concrete;
using Asp_ImtahanProject_ChatApp.UI.Models.CommentModels;
using Asp_ImtahanProject_ChatApp.UI.Models.LikeModels;

namespace Asp_ImtahanProject_ChatApp.UI.Models.PostModels
{
    public class PostModel
    {
        public int PostId { get; set; }
        public string? Text { get; set; }
        public string? ImageUrl { get; set; }
        public string? VideoLink { get; set; }
        public int LikeCount { get; set; }
        public string? DateTime { get; set; }
        public string? UserProfileImageUrl { get; set; }
        public string? UserName { get; set; }
        public string? UserId { get; set; }



        public virtual ICollection<CommentModel>? Comments { get; set; }
        public virtual ICollection<LikeModel>? Likes { get; set; }
    }
}

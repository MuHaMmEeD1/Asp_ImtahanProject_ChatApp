namespace Asp_ImtahanProject_ChatApp.UI.Models.FriendshipRequestModels
{
    public class FriendshipRequestModel
    {
        public int Id { get; set; }
        public string? OutherUserProfileImageUrl { get; set; }
        public string? OutherUserName { get; set; }
        public bool? Response { get; set; }
        public string? DateTime { get; set; }
        public string? OtherUserId { get; set; }
        public bool DidItAppear { get; set; }

    }
}

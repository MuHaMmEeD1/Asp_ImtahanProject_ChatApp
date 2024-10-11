using Asp_ImtahanProject_ChatApp.Core.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asp_ImtahanProject_ChatApp.Entities.Concrete
{
    public class FriendshipRequest : IEntity
    {

        public int Id { get; set; } 
        public bool? Response { get; set; }
        public DateTime DateTime { get; set; }


        public string? UserId { get; set; }
        public virtual User? User { get; set; }


        public string? OtherUserId { get; set; }
        public virtual User? OtherUser { get; set; }



    }
}

using Asp_ImtahanProject_ChatApp.Core.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asp_ImtahanProject_ChatApp.Entities.Concrete
{
    public class UserFriend : IEntity
    {
        public int Id { get; set; }
        public string? UserFriendFirstId { get; set; }
        public virtual User? UserFriendFirst { get; set; }
        public string? UserFriendSecondId { get;set; }
        public virtual User? UserFriendSecond { get; set; }

    }
}

using Asp_ImtahanProject_ChatApp.Core.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asp_ImtahanProject_ChatApp.Entities.Concrete
{
    public class Message : IEntity
    {
        public int Id { get; set; }
        public string? MessageStr { get; set; }
        public DateTime DateTime { get; set; } 

        public string? UserId { get; set; }
        public virtual User? User { get; set; }


        public string? RecipientUserId { get; set; }
        public virtual User? RecipientUser { get; set; }


    }
}

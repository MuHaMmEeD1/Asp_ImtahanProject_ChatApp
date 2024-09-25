using Asp_ImtahanProject_ChatApp.Core.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asp_ImtahanProject_ChatApp.Entities.Concrete
{
    public class PostTag : IEntity
    {
        public int Id { get; set; } 
        public int PostId { get; set; }
        public virtual Post? Post { get; set; }

        public int TagId { get; set; }
        public virtual Tag? Tag { get; set; }


    }
}

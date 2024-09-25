using Asp_ImtahanProject_ChatApp.Core.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asp_ImtahanProject_ChatApp.Entities.Concrete
{
    public class Tag : IEntity
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public virtual ICollection<Post>? Posts { get; set; }
    }
}

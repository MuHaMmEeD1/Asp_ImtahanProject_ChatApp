using Asp_ImtahanProject_ChatApp.Core.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asp_ImtahanProject_ChatApp.Entities.Concrete
{
    public class Post : IEntity
    {
        public int Id { get; set; } 
        public string? Text { get; set; }
        public string? ImageUrl { get; set; }
        public string? VideoLink { get; set; }
        public DateTime DateTime { get; set; }

        public string? UserId { get; set; } 
        public virtual User? User { get; set; }  

        public virtual ICollection<PostTag>? PostTags { get; set; }

        public virtual ICollection<Comment>? Comments { get; set; } 
    }
}

using Asp_ImtahanProject_ChatApp.Entities.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Asp_ImtahanProject_ChatApp.DataAccess.Data
{
    public class ZustDbContext : IdentityDbContext
    {
        public ZustDbContext(DbContextOptions options) : base(options)
        {
        }


      



        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<FriendshipRequest> FriendshipRequests { get; set; }
        public virtual DbSet<Like> Likes { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<PostTag> PostTags { get; set;}
        public virtual DbSet<ReplyToComment> ReplyToComments { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserFriend> UserFriends { get; set; }

    }
}

using Asp_ImtahanProject_ChatApp.Business.Abstract;
using Asp_ImtahanProject_ChatApp.Business.Concrete;
using Asp_ImtahanProject_ChatApp.DataAccess.Abstract;
using Asp_ImtahanProject_ChatApp.DataAccess.Concrete.EFEntityFramework;
using Asp_ImtahanProject_ChatApp.DataAccess.Data;
using Asp_ImtahanProject_ChatApp.Entities.Concrete;
using Asp_ImtahanProject_ChatApp.UI.Hubs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddSignalR();



// Dal Start

builder.Services.AddScoped<ICommentDal, EFCommentDal>();
builder.Services.AddScoped<IFriendshipRequestDal, EFFriendshipRequestDal>();
builder.Services.AddScoped<IMessageDal, EFMessageDal>();
builder.Services.AddScoped<IPostDal, EFPostDal>();
builder.Services.AddScoped<IPostTagDal, EFPostTagDal>();
builder.Services.AddScoped<IReplyToCommentDal, EFReplyToCommentDal>();
builder.Services.AddScoped<ITagDal, EFTagDal>();
builder.Services.AddScoped<IUserDal, EFUserDal>();
builder.Services.AddScoped<IUserFriendDal, EFUserFriendDal>();

// Dal End

// Service Start

builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<IFriendshipRequestService, FriendshipRequestService>();
builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<IPostTagService, PostTagService>();
builder.Services.AddScoped<IReplyToCommentService, ReplyToCommentService>();
builder.Services.AddScoped<ITagService, TagService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserFriendService, UserFriendService>();

// Service End

// DB Start

string connectionString = builder.Configuration.GetConnectionString("Default")!;

builder.Services.AddDbContext<ZustDbContext>(option =>
{
    option.UseSqlServer(connectionString, b => b.MigrationsAssembly("Asp_ImtahanProject_ChatApp.UI"))
    .UseLazyLoadingProxies();
}
);

builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<ZustDbContext>()
    .AddDefaultTokenProviders();

// DB End





var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapHub<FriendHub>("/friendHub");


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Register}/{action=Index}");

app.Run();

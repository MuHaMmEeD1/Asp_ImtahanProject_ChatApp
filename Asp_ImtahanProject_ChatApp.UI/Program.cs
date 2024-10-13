using Asp_ImtahanProject_ChatApp.Business.Abstract;
using Asp_ImtahanProject_ChatApp.Business.Concrete;
using Asp_ImtahanProject_ChatApp.DataAccess.Abstract;
using Asp_ImtahanProject_ChatApp.DataAccess.Concrete.EFEntityFramework;
using Asp_ImtahanProject_ChatApp.DataAccess.Data;
using Asp_ImtahanProject_ChatApp.Entities.Concrete;
using Asp_ImtahanProject_ChatApp.UI.AutoMapp;
using Asp_ImtahanProject_ChatApp.UI.Hubs;
using Asp_ImtahanProject_ChatApp.UI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(PostProfile));
builder.Services.AddAutoMapper(typeof(CommentProfile));
builder.Services.AddAutoMapper(typeof(ReplyToCommentProfile));
builder.Services.AddAutoMapper(typeof(LikeProfile));
builder.Services.AddAutoMapper(typeof(FriendshipRequestProfile));
builder.Services.AddAutoMapper(typeof(UserFriendProfile));
builder.Services.AddAutoMapper(typeof(MessageProfile));


// Data Access Layer (DAL) Configuration
builder.Services.AddScoped<ICommentDal, EFCommentDal>();
builder.Services.AddScoped<IFriendshipRequestDal, EFFriendshipRequestDal>();
builder.Services.AddScoped<ILikeDal, EFLikeDal>();
builder.Services.AddScoped<IMessageDal, EFMessageDal>();
builder.Services.AddScoped<IPostDal, EFPostDal>();
builder.Services.AddScoped<IPostTagDal, EFPostTagDal>();
builder.Services.AddScoped<IReplyToCommentDal, EFReplyToCommentDal>();
builder.Services.AddScoped<ITagDal, EFTagDal>();
builder.Services.AddScoped<IUserDal, EFUserDal>();
builder.Services.AddScoped<IUserFriendDal, EFUserFriendDal>();

// Service Layer Configuration
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<IFriendshipRequestService, FriendshipRequestService>();
builder.Services.AddScoped<ILikeService, LikeService>();
builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<IPostTagService, PostTagService>();
builder.Services.AddScoped<IReplyToCommentService, ReplyToCommentService>();
builder.Services.AddScoped<ITagService, TagService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserFriendService, UserFriendService>();
builder.Services.AddScoped<IPhotoService, PhotoService>();

// Database Configuration
string connectionString = builder.Configuration.GetConnectionString("Default")!;

builder.Services.AddDbContext<ZustDbContext>(options =>
{
    options.UseSqlServer(connectionString, b => b.MigrationsAssembly("Asp_ImtahanProject_ChatApp.UI"))
           .UseLazyLoadingProxies();
});

builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<ZustDbContext>()
    .AddDefaultTokenProviders();


// Configure JSON Options for Serialization
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();


app.UseAuthentication();

app.Use(async (context, next) =>
{
    if (!context.User.Identity.IsAuthenticated && context.Request.Path.Value.StartsWith("/Home"))
    {
        context.Response.Redirect("/Register/Index");
        return;
    }
    await next.Invoke();
});

app.UseAuthorization();



// Map SignalR Hub
app.MapHub<FriendHub>("/friendHub");

// Map Controller Routes
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Register}/{action=Login}");

app.Run();

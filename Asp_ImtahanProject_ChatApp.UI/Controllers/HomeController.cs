using Asp_ImtahanProject_ChatApp.Business.Abstract;
using Asp_ImtahanProject_ChatApp.Entities.Concrete;
using Asp_ImtahanProject_ChatApp.UI.Dtos;
using Asp_ImtahanProject_ChatApp.UI.Models.HomeModels;
using Asp_ImtahanProject_ChatApp.UI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Cryptography.Xml;

namespace Asp_ImtahanProject_ChatApp.UI.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {

        private readonly IPostService _postService;
        private readonly ITagService _tagService;
        private readonly IPostTagService _postTagService;
        private readonly IPhotoService _photoService;

        public HomeController(IPostService postService, ITagService tagService, IPostTagService postTagService, IPhotoService photoService)
        {
            _postService = postService;
            _tagService = tagService;
            _postTagService = postTagService;
            _photoService = photoService;
        }

        public ActionResult Index()
        {
            return View(new PostModel());
        }

        [HttpPost]
        public async Task<ActionResult> CreatePost(PostModel model)
        {
            if (ModelState.IsValid)
            {
                string? imageUrl = null;
                if (model.Photo != null && model.Photo.Length > 0)
                {
                    var photoDto = new PhotoCreationDto { File = model.Photo };
                    imageUrl = await _photoService.UploadImageAsync(photoDto);

                    if (string.IsNullOrEmpty(imageUrl))
                    {
                        ModelState.AddModelError(string.Empty, "Photo upload failed.");
                        return View("Index", model);
                    }
                }

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var newPost = new Post
                {
                    Text = model.Message,
                    ImageUrl = imageUrl,
                    VideoLink = model.VideoUrl,
                    DateTime = DateTime.Now,
                    UserId = userId,
                    PostTags = new List<PostTag>()
                };

                await _postService.AddAsync(newPost);
              

                if (!string.IsNullOrEmpty(model.Tags))
                {
                    var tagNames = model.Tags.Split('#').Select(t => t.Trim()).ToList();

                    foreach (var tagName in tagNames)
                    {
                        var tag = await _tagService.GetOrCreateTagAsync(tagName);
                        var postTag = new PostTag
                        {
                            Post = newPost,
                            Tag = tag
                        };
                        newPost.PostTags.Add(postTag);

                        await _postTagService.AddAsync(postTag);
                    }
                }


                return RedirectToAction("Index");
            }

            return View("Index", model);
        }



        public ActionResult Notifications()
        {
            return View();
        }

        public ActionResult Messages() 
        { 
            return View();
        }

        public ActionResult Friends() 
        {
            return View();
        }

        public ActionResult MyProfile()
        {
            return View();
        }

        public ActionResult Setting()
        {
            return View();
        }
    }

}

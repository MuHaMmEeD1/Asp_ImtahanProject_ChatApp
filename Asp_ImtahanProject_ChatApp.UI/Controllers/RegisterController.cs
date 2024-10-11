using Asp_ImtahanProject_ChatApp.Business.Abstract;
using Asp_ImtahanProject_ChatApp.Entities.Concrete;
using Asp_ImtahanProject_ChatApp.UI.Models.RegisterModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Asp_ImtahanProject_ChatApp.UI.Controllers
{
   
    public class RegisterController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IUserService _userService;

        public RegisterController(UserManager<User> userManager, SignInManager<User> signInManager, IUserService userService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userService = userService;
        }
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Index()
        {
            return View(new RegisterModel());
        }

        [HttpPost]
        public async Task<ActionResult> Index(RegisterModel rm)
        {
            if (!ModelState.IsValid || !rm.Privacy)
            {
                if (!rm.Privacy)
                {
                    ModelState.AddModelError(nameof(rm.Privacy), "You must accept the privacy policy.");
                }
                return View(rm);
            }

            var user = new User {
                UserName = rm.Name, 
                Email = rm.Email, 
                ProfileImageUrl= "/assets/images/defaultProfileImage.png" ,
                FirstName = "Default",
                LastName = "Default"
                
            };
            var result = await _userManager.CreateAsync(user, rm.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Login", "Register");
            }

            foreach (var error in result.Errors)
            {
                Console.WriteLine(error.Description);
            }

            return View(rm);
        }
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Login()
        {
            return View(new LoginModel());
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginModel lm)
        {
            if (!ModelState.IsValid)
            {
                return View(lm);
            }

            var user = await _userManager.FindByNameAsync(lm.UsernameOrEmail)
                       ?? await _userManager.FindByEmailAsync(lm.UsernameOrEmail);

            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user, lm.Password, lm.RememberMe, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            Console.WriteLine("Invalid login attempt.");
            return View(lm);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Register");
        }

    }
}

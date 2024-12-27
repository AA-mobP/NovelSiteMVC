using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NovelSiteMVC.Models;
using NovelSiteMVC.ViewModels;
using System.Security.Claims;

namespace NovelSiteMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signinManager;
        private readonly IWebHostEnvironment Environment;

        public AccountController(AppDbContext _context, UserManager<ApplicationUser> _userManager, SignInManager<ApplicationUser> _signinManager, IWebHostEnvironment _Environment)
        {
            context = _context;
            userManager = _userManager;
            signinManager = _signinManager;
            Environment = _Environment;
        }

        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            //is model valid
            if (!ModelState.IsValid)
                return View(model);

            ApplicationUser? user = await userManager.FindByNameAsync(model.UserName);
            if (user is not null) //username MUST be not repeated
            {
                ModelState.AddModelError("UserName", "this username has been taken.");
                return View(model);
            }
            user = await userManager.FindByEmailAsync(model.Email);
            if (user is not null)
            {
                ModelState.AddModelError("Email", "this email already exists.");
                return View(model);
            }
            user = new();
            //mapping properties
            user.UserName = model.UserName;
            user.Email = model.Email;
            user.Country = model.Country;
            user.PasswordHash = model.Password;

            //save in db
            IdentityResult result = await userManager.CreateAsync(user, user.PasswordHash);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);
                return View(model);
            }
            await signinManager.SignInAsync(user, false);//false because it's Register
            return RedirectToAction("Login");
        }

        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            //check model state
            if (!ModelState.IsValid)
                return View(model);

            //check if user exist...
            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            //check username
            ApplicationUser? user = await userManager.FindByNameAsync(model.LoginKey);
            if (user is null)//username not valid
            {
                //check email
                user = await userManager.FindByEmailAsync(model.LoginKey);
                if (user is null)//email not valid
                {
                    ModelState.AddModelError("LoginKey", "you need to enter valid Email or Username to login, try again.");
                    return View(model);
                }
            }
            //reaching here means (username || email) is valid
            //check password
            //be careful! you MUST send ViewModel Password not the user!
            bool isPasswordCorrect = await userManager.CheckPasswordAsync(user, model.Password);

            if (!isPasswordCorrect)
            {
                ModelState.AddModelError("Password", "Password isn't Correct");
                return View(model);
            }
            //reaching here means (username || email) is valid && password is correct
            //sign in user
            await signinManager.SignInAsync(user, model.RememberMe);
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Signout()
        {
            await signinManager.SignOutAsync();
            return RedirectToAction("Register");
        }

        public async Task<IActionResult> Profile()
        {
            ApplicationUser? user = await userManager.FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (user is null)
                return BadRequest();

            ProfileViewModel profile = new()
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Country = user.Country,
                PhotoUrl = user.PhotoUrl,
                BackgroundPhotoUrl = user.BackgroundPhotoUrl
            };

            return View(profile);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(ProfileViewModel profile, IFormFileCollection imageFiles)
        {
            ApplicationUser? user = context.Users.FirstOrDefault(x => x.Id == profile.Id);
            user.Email = profile.Email;
            user.Country = profile.Country;

            context.Update(user);
            await context.SaveChangesAsync();

            return RedirectToAction(nameof(Profile));
        }
    }
}

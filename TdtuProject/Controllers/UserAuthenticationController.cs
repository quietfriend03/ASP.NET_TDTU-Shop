using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TdtuProject.Models.DTO;
using TdtuProject.Repository.Interface;

namespace TdtuProject.Controllers
{
    public class UserAuthenticationController : Controller
    {
        private readonly IUserAuthenticationService _userAuthenticationService;
        public UserAuthenticationController(IUserAuthenticationService userAuthenticationService) 
        {
            this._userAuthenticationService = userAuthenticationService;
        }

        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegistrationModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            model.Role = "user";
            var result = await _userAuthenticationService.ResitrationAsync(model);
            TempData["msg"] = result.Message;
            return RedirectToAction(nameof(Register));
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _userAuthenticationService.LoginAsync(model);
            if(result.StatusCode == 1)
            {
                return RedirectToAction("Display","Dashboard");
            }
            else
            {
                TempData["msg"] = result.Message;
                return RedirectToAction(nameof(Login));
            }
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _userAuthenticationService.LogoutAsync();
            return RedirectToAction(nameof(Login));
        }

        public async Task<IActionResult> Reg()
        {
            var model = new RegistrationModel()
            {
                UserName = "admin",
                Name = "admin123",
                Email = "admin@gmail.com",
                Password = "Admin@123",
            };
            model.Role = "admin";
            var result = await _userAuthenticationService.ResitrationAsync(model);
            return Ok(result);
        }

        [Authorize]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult>ChangePassword(ChangePasswordModel model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await _userAuthenticationService.ChangePasswordAsync(model, User.Identity.Name);
            TempData["msg"] = result.Message;
            return RedirectToAction(nameof(ChangePassword));
        }
    }
}

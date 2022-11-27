using Dashboard.Models;
using Dashboard.Models.Account;
using Dashboard.Models.Viewmodel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Claims;

namespace Dashboard.Controllers.Account
{
    public class AccountController : Controller
    {
        public AccountController(DashboardDbContext context)
        {
            Context = context;
        }

        public DashboardDbContext Context { get; }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginSignUpViewModel model)
        {
            if (ModelState.IsValid)
            {
                var data = Context.Users.Where(e => e.Username == model.Username).SingleOrDefault();
                if (data != null)
                {
                    bool isValid = (data.Username == model.Username && data.Password == model.Password);
                    if (isValid)
                    {
                        var identity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, model.Username) }, CookieAuthenticationDefaults.AuthenticationScheme);
                        var principal = new ClaimsPrincipal(identity);
                        HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                        HttpContext.Session.SetString("Username", model.Username);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        TempData["errorPassword"] = "Invalid Password!";
                    }
                }
                else
                {
                    TempData["errorUsername"] = "Username Not Found!";
                    return View(model);
                }
            }
            {
                return View(model);
            }
        }

        public IActionResult LogOut()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var storedCookies = Request.Cookies.Keys;
            foreach (var cookies in storedCookies)
            {
                Response.Cookies.Delete(cookies);
            }
            return RedirectToAction("Login", "Account");
        }

        public IActionResult SignUp()
        {
            return View();
        }

        [AcceptVerbs("Post", "Get")]
        public IActionResult UserNameIsExist(string username)
        {
            var data = Context.Users.Where(e => e.Username == username).SingleOrDefault();
            if (data != null)
            {
                return Json($"Username {username} already exist!");
            }
            else
            {
                return Json(true);
            }
        }

        [HttpPost]
        public IActionResult SignUp(SignUpUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var data = new User()
                {
                    Username = model.Username,
                    Email = model.Email,
                    Password = model.Password,
                    Mobile = model.Mobile,
                };
                Context.Users.Add(data);
                Context.SaveChanges();
                TempData["SuccessMessage"] = "Register Successful.";
                return RedirectToAction("Login");
            }
            {
                TempData["errorMessage"] = "Empty form can't be submitted!";
                return View(model);
            }
        }
    }
}
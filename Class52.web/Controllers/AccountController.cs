using Class52.data;
using Class52.web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Class52.web.Controllers
{
    public class AccountController : Controller
    {
        string connectionString = "Data Source=.\\sqlexpress;Initial Catalog=Blogs;Integrated Security=true";
        public IActionResult NewUser()
        {
            return View();
        }

        [HttpPost]
        public IActionResult NewUser(string name, string email, string phoneNumber, string password)
        {
            var userRepos = new UserRepository(connectionString);
            User u = new User
            {
                Name = name,
                Email = email,
                PhoneNumber = phoneNumber,
            };
            userRepos.NewUser(u, password);

            return Redirect("/account/login");
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var userRepos = new UserRepository(connectionString);
            var user = userRepos.Login(email, password);
            if (user == null)
            {
                TempData["invalid"] = "Invalid login. Please try again.";
                return Redirect("/account/login");
            }
            var claims = new List<Claim>
            {
                new Claim("user", email) //this will store the users email into the login cookie
            };

            HttpContext.SignInAsync(new ClaimsPrincipal(
                new ClaimsIdentity(claims, "Cookies", "user", "role")))
                .Wait();
            return Redirect("/home/newAd");

        }

        public IActionResult LogOut()
        {
            HttpContext.SignOutAsync().Wait();
            return Redirect("/home/index");
        }



    }
}

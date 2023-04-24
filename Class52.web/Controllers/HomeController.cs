using Class52.data;
using Class52.web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace Class52.web.Controllers
{
    public class HomeController : Controller
    {
        string connectionString = "Data Source=.\\sqlexpress;Initial Catalog=Blogs;Integrated Security=true";
        public IActionResult Index()
        {
            var adRepos = new AdRepository(connectionString);
            var vm = new IndexViewModel
            {
                Ads = adRepos.GetAds()
            };
            var currentUserEmail = User.Identity.Name;
            if (currentUserEmail != null)
            {
                var userRepos = new UserRepository(connectionString);
                var user = userRepos.GetUserByEmail(currentUserEmail);
                if (user != null)
                {
                    vm.CurrentUserId = user.UserId;
                }

            }
            return View(vm);
        }

        [Authorize]
        public IActionResult NewAd()
        {
            var currentUserEmail = User.Identity.Name;
            var userRepos = new UserRepository(connectionString);
            var user = userRepos.GetUserByEmail(currentUserEmail);
            if (user != null)
            {
                return View(new NewAdViewModel
                {
                    CurrentUser = user
                });
            }

            return Redirect("/account/login");
        }


        [HttpPost]
        public IActionResult NewAd(int userId, string name, string phoneNumber, string description)
        {
            var adRepos = new AdRepository(connectionString);
            adRepos.NewAd(new Ad
            {
                UserId = userId,
                Name = name,
                PhoneNumber = phoneNumber,
                Description = description
            });
            return Redirect("/home/Index");
        }

        [Authorize]
        public IActionResult MyAccount()
        {
            var adRepos = new AdRepository(connectionString);
            var currentUserEmail = User.Identity.Name;
            if (currentUserEmail == null)
            {
                return Redirect("/account/login");
            }
            var userRepos = new UserRepository(connectionString);
            var user = userRepos.GetUserByEmail(currentUserEmail);
            var vm = new IndexViewModel
            {
                Ads = adRepos.GetMyAds(user.UserId)
            };

            return View(vm);
        }

        public IActionResult Delete (int adId, string previousPage)
        {
            var adRepos = new AdRepository(connectionString);
            adRepos.DeleteAd(adId);
            return Redirect(previousPage);
        }

    }
}
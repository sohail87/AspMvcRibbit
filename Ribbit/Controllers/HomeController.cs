using RibbitMvc.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RibbitMvc.Controllers
{
    public class HomeController : RibbitControllerBase
    {
        public HomeController() : base()
        {

        }
        // GET: Home
        public ActionResult Index()
        {
            if (!Security.IsAuthenticated)
            {
                //viewname and object model passed in
                return View("Landing", new LoginSignupViewModel());
            }

            var timeline = Ribbits.GetTimelineFor(Security.UserId).ToArray();

            return View("Timeline", timeline);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Follow(string username)
        {
            if (!Security.IsAuthenticated)
            {
                return RedirectToAction("Index");
            }
            Users.Follow(username, Security.GetCurrentUser());
            //return RedirectToAction("Index");
            return GoToReferrer();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Unfollow(string username)
        {
            if (!Security.IsAuthenticated)
            {
                return RedirectToAction("Index");
            }

            Users.Unfollow(username, Security.GetCurrentUser());

            return GoToReferrer();
        }
         
        public ActionResult Profiles()
        {
            var users = Users.All(true);

            return View(users);

            throw new NotImplementedException();
        }

        public ActionResult Followers()
        {
            if (!Security.IsAuthenticated)
            {
                return RedirectToAction("Index");
            }

            var user = Users.GetAllFor(Security.UserId);

            return View("Buddies", new BuddiesViewModel()
            {
                User = user,
                Buddies = user.Followers
            });
        }

        public ActionResult Following()
        {
            if (!Security.IsAuthenticated)
            {
                return RedirectToAction("Index");
            }

            var user = Users.GetAllFor(Security.UserId);

            return View("Buddies", new BuddiesViewModel()
            {
                User = user,
                Buddies = user.Followings
            });
        }
        [HttpGet]
        [ChildActionOnly] 
        // if we try to make a request directly for these action method we are going to get an exception
        public ActionResult Create()
        {
            //the get version is very simple, we simply return a partial view
            return PartialView("_CreateRibbitPartial", new CreateRibbitViewModel());
        }

        [HttpPost]
        [ChildActionOnly]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateRibbitViewModel model)
        {
            if (ModelState.IsValid)
            {
                Ribbits.Create(Security.UserId, model.Status);
                //because this a child action, we cannot redirect to another action, if we want to redirect we have to use the below
                //THE BELOW PRODUCES A HTTP HEADER ERROR
                //Response.Redirect("/");
                //var timeline = Ribbits.GetTimelineFor(Security.UserId).ToArray();
                //return View("Timeline", timeline);
            }
            return PartialView("_CreateRibbitPartial", model);
        }

    }
}
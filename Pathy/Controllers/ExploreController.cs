using Microsoft.AspNetCore.Mvc;
using Pathy.Models;
using Pathy_BusinessAccessLayer;

namespace Pathy.Controllers
{
    public class ExploreController : Controller
    {

        [HttpGet]
        public IActionResult SuggestedFriends()
        {
            clsSuggestedFriendsViewModel model = new clsSuggestedFriendsViewModel();
            model.loadSuggestedUsersToFollowList();            

            return View(model);
        }

        [HttpPost]
        public IActionResult FollowUser(int userID)
        {

            if(!clsFollowers.followUser(clsGlobal.logedInUser.userID,userID))
            {
                TempData["newFollowMessage"] = "Failed to follow user";
            }
                
            return RedirectToAction("SuggestedFriends", "Explore");
        }
    }
}

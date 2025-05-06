using Microsoft.AspNetCore.Mvc;
using Pathy.Models;

namespace Pathy.Controllers
{
    public class LikesController : Controller
    {
        [HttpPost]
        public IActionResult UsersWhoLikesPost(int postID)
        {            
            clsUsersWhoLikesPostViewModel model = new clsUsersWhoLikesPostViewModel();

            model.loadUsersWhoLikesPost(postID);          

            return View(model);
        }
    }
}

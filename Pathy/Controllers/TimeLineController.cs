using Microsoft.AspNetCore.Mvc;
using Pathy.Models;
using Pathy_BusinessAccessLayer;

namespace Pathy.Controllers
{
    public class TimeLineController : Controller
    {

        [HttpGet]
        public IActionResult TimeLine()
        {
            clsTimeLineViewModel model = new clsTimeLineViewModel();

            model.loadAllPostsList();

            return View(model);
        }



        [HttpPost]
        public IActionResult AddNewPost(clsTimeLineViewModel model)
        {
            if(!ModelState.IsValid)
            {
                return View("TimeLine",model);
            }

            clsPost post = new clsPost();

            post.mode = clsPost.enMode.AddNew;
            post.accountID = clsGlobal.account.accountID;
            post.creationDateTime = DateTime.Now;
            post.textContent = model.postTextContent;
            post.imageURL = "http://demo-image.png";
            post.videoURL = "http://video-image.png";

            post.save();

            model.loadAllPostsList();

            TempData["NewPostMessage"] = "Posted successfully";

            model.postTextContent = "";

            return View("TimeLine",model);
        }


        [HttpPost]
        public IActionResult LikePost(int postID)
        {
            clsLike like = new clsLike();

            like.mode = clsLike.enMode.Addnew;
            like.postID = postID;
            like.likerUserID = clsGlobal.logedInUser.userID;

            like.save();

            return RedirectToAction("TimeLine","TimeLine");
        }

        [HttpPost]
        public IActionResult UnlikePost(int postID)
        {
            clsLike like = clsLike.findLikeByPostIDAndLikerUserID(postID,clsGlobal.logedInUser.userID);

            like.mode = clsLike.enMode.Delete;
            like.save();

            return RedirectToAction("TimeLine", "TimeLine");
        }



        [HttpPost]
        public IActionResult OpenPostCommentsPage(int postID)
        {           
            return RedirectToAction("PostComments","Comments", new {postID = postID});
        }
    }
}
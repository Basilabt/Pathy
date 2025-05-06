using Microsoft.AspNetCore.Mvc;
using Pathy.Models;
using Pathy_BusinessAccessLayer;

namespace Pathy.Controllers
{
    public class CommentsController : Controller
    {
        [HttpGet]
        public IActionResult PostComments(int postID)
        {
            
            clsPostCommentsViewModel model = new clsPostCommentsViewModel();
            model.postID = postID;
            model.loadPostComments();
            

            return View(model);
        }

        [HttpPost]
        public IActionResult AddNewComment(clsPostCommentsViewModel model)
        {
            clsComment comment = new clsComment();

            comment.commenterUserID = clsGlobal.logedInUser.userID;
            comment.postID = model.postID;
            comment.commentText = model.commentText;
            comment.save();

            return RedirectToAction("PostComments","Comments",new {model.postID});
        }

        [HttpPost] 
        public IActionResult DeleteComment(clsPostCommentsViewModel model)
        {

            clsComment comment = clsComment.getCommentByCommentID(model.commentID);

            comment.mode = clsComment.enMode.Delete;
            comment.save();

            return RedirectToAction("PostComments", "Comments", new { model.postID });

        }

        [HttpPost]
        public IActionResult OpenUpdateCommentView(clsPostCommentsViewModel model)
        {
            clsUpdateCommentViewModel updateCommentViewModel = new clsUpdateCommentViewModel();
            updateCommentViewModel.commentID = model.commentID;
            updateCommentViewModel.commentText = model.commentText;
            updateCommentViewModel.postID = model.postID;

            return View("UpdatePostComment", updateCommentViewModel);
        }

        [HttpPost]
        public IActionResult UpdateComment(clsUpdateCommentViewModel model)
        {
            clsComment comment = clsComment.getCommentByCommentID(model.commentID);

            comment.mode = clsComment.enMode.Update;
            comment.commentText = model.commentText;
            

            comment.save();

            return RedirectToAction("PostComments", "Comments", new { comment.postID });
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Pathy.Models;
using Pathy_BusinessAccessLayer;

namespace Pathy.Controllers
{
    public class ProfileController : Controller
    {
        [HttpGet]
        public IActionResult LoggedUserProfile()
        {
            clsUserProfileViewModel model = new clsUserProfileViewModel();

            model.loadCurrentUserPosts();

            return View(model);
        }




        [HttpGet]
        public IActionResult EditUserProfileInfo()
        {
            clsEditUserProfileInfoViewModel model = new clsEditUserProfileInfoViewModel();

            model.loadUserInfo();

            return View(model);
        }

        [HttpPost]
        public IActionResult EditUserProfileInfo(clsEditUserProfileInfoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string currentUsername = clsGlobal.logedInUser.username;

            if (currentUsername != model.username && clsUser.doesUserExistByUsername(model.username))
            {              
                ModelState.AddModelError("username", "Username already exist");
                return View(model);
            }

            string currentEmail = clsGlobal.logedInUser.person.email;

            if (currentEmail != model.email && clsPerson.doesEmailExist(model.email))
            {
                ModelState.AddModelError("email", "Email already exist");
                return View(model);
            }

            clsGlobal.logedInUser.mode = clsUser.enMode.Update;
            clsGlobal.logedInUser.person.mode = clsPerson.enMode.Update;
            clsGlobal.account.mode = clsAccount.enMode.Update;
           
            clsGlobal.logedInUser.person.firstName = model.firstName;
            clsGlobal.logedInUser.person.secondName = model.secondName;
            clsGlobal.logedInUser.person.thirdName = model.thirdName;
            clsGlobal.logedInUser.person.lastName = model.lastName;
            clsGlobal.logedInUser.person.email = model.email;
            clsGlobal.logedInUser.person.phoneNumber = model.phoneNumber;
            clsGlobal.logedInUser.person.gender = model.gender ?? 1;


            if (clsGlobal.logedInUser.person.save())
            {

                clsGlobal.logedInUser.personID = clsGlobal.logedInUser.person.personID;
                clsGlobal.logedInUser.username = model.username;
                clsGlobal.logedInUser.password = model.password;
                clsGlobal.logedInUser.photoURL = "";

                if (clsGlobal.logedInUser.save())
                {                   
                     TempData["UpdateInformationMessage"] = "Your information updated successfully!";                                
                }
                else
                {
                    TempData["UpdateInformationMessage"] = "Failed to update information";
                }

            }
            else
            {
                TempData["UpdateInformationMessage"] = "Failed to update information";
            }

            return RedirectToAction("TimeLine", "TimeLine"); 
        }


        [HttpPost]
        public IActionResult DeleteUserPost(int postID)
        {
            clsPost post = clsPost.getPostByPostID(postID);

            post.mode = clsPost.enMode.Delete;

            if(post.save())
            {
                TempData["CurrentUserPostActionMessage"] = "Post deleted successfully";

            } else
            {
                TempData["CurrentUserPostActionMessage"] = "Failed to delete post";
            }

            return RedirectToAction("LoggedUserProfile", "Profile");
        }





        [HttpGet]
        public IActionResult EditUserPost(int postID)
        {
            clsPost post = clsPost.getPostByPostID(postID);

            clsEditUserPostViewModel model = new clsEditUserPostViewModel();

            model.postID = post.postID;
            model.textContent = post.textContent;
            model.creationDateTime = post.creationDateTime;
            model.imageURL = post.imageURL;
            model.videoURL = post.videoURL;

          
            return View("EditUserPost",model);
        }

        [HttpPost]
        public IActionResult EditUserPost(clsEditUserPostViewModel model)
        {

            clsPost post = clsPost.getPostByPostID(model.postID);
            
            post.mode=clsPost.enMode.Update;
            post.textContent = model.textContent;
            post.imageURL = model.imageURL;
            post.videoURL = model.videoURL;

            if (post.save())
            {
                TempData["CurrentUserPostActionMessage"] = "Post Updated successfully";
            }
            else
            {
                TempData["CurrentUserPostActionMessage"] = "Failed to update post";
            }

            return RedirectToAction("LoggedUserProfile", "Profile");
        }






        [HttpGet] 
        public IActionResult GetUserFollowers()
        {
            clsUserFollowersViewModel model = new clsUserFollowersViewModel();
            model.loadUserFollowers(clsGlobal.logedInUser.userID);

            return View("_UserFollowers",model);
        }


        [HttpPost]
        public IActionResult RemoveFollower(int followedUserID , int followerUserID)
        {
            clsFollowers.unfollowUser(followerUserID, followedUserID);
          
            return RedirectToAction("GetUserFollowers", "Profile");
        }







        [HttpGet]
        public IActionResult GetUserFollwings()
        {
           clsUserFollowingViewModel model = new clsUserFollowingViewModel();
            model.loadUserFollowings(clsGlobal.logedInUser.userID);

            return View("_UserFollowings", model);
        }


        [HttpPost]
        public IActionResult UnfollowUser(int followedUserID, int followerUserID)
        {
            clsFollowers.unfollowUser(followerUserID, followedUserID);
           
            return RedirectToAction("GetUserFollwings", "Profile");
        }


    }
}

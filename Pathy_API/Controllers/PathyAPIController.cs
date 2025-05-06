using FactWebsiteApp.Models.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Pathy_API.DTOs;
using Pathy_API.DTOs.ChatDTOs;
using Pathy_API.DTOs.CommentsDTO;
using Pathy_API.DTOs.FollowDTO;
using Pathy_API.DTOs.FollowersDTO;
using Pathy_API.DTOs.FollowingsDTO;
using Pathy_API.DTOs.InformationDTOs;
using Pathy_API.DTOs.LikesDTO;
using Pathy_API.DTOs.LoginDTOs;
using Pathy_API.DTOs.PostsDTOs.DeleteDTOs;
using Pathy_API.DTOs.PostsDTOs.NewPostDTOs;
using Pathy_API.DTOs.PostsDTOs.TimelineDTOs;
using Pathy_API.DTOs.PostsDTOs.UpdateDTOs;
using Pathy_API.DTOs.PostsDTOs.UserPostsDTOs;
using Pathy_API.DTOs.RegisterDTOs;
using Pathy_API.DTOs.UnfollowDTO;
using Pathy_API.DTOs.UpdateDTOs;
using Pathy_API.DTOs.UsersDTO;
using Pathy_BusinessAccessLayer;
using Pathy_DataAccessLayer.DTOs;
using System.Reflection;



namespace Pathy_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PathyAPIController : ControllerBase
    {

        [HttpPost("Account/Login", Name = "Login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Login([FromBody] clsLoginRequestDTO requestDTO)
        {
            if (string.IsNullOrEmpty(requestDTO.username) || string.IsNullOrEmpty(requestDTO.password))
            {
                return BadRequest("Username And Password Can't Be Null");
            }

            if (!clsUser.doesUserExistByUsername(requestDTO.username))
            {
                return BadRequest("Username Doesn't Exist");
            }

            clsUser user = clsUser.loginByUsernameAndPassword(requestDTO.username, requestDTO.password);

            if (user == null)
            {
                return Unauthorized("Incorrect Password");
            }


            clsLoginLog loginLog = new clsLoginLog();
            loginLog.mode = clsLoginLog.enMode.AddNew;
            loginLog.accountID = clsAccount.getAccountByUserID(user.userID).accountID;
            loginLog.loginDateTime = DateTime.Now;

            loginLog.save();


            return Ok(new clsLoginResponseDTO { userID = user.userID, personID = user.personID, accountID = clsAccount.getAccountByUserID(user.userID).accountID, username = user.username, photoURL = (user.photoURL == null) ? "default.png" : user.photoURL, jwt = "Not-live-yet" });
        }




        [HttpPost("Account/Register", Name = "Register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Register([FromBody] clsRegisterRequestDTO requestDTO)
        {

            if (clsUser.doesUserExistByUsername(requestDTO.username))
            {
                return Conflict("Username already exists. Please choose a different username.");
            }

            if (clsPerson.doesEmailExist(requestDTO.email))
            {
                return Conflict("Email already exists. Please choose a different email.");
            }


            clsPerson person = new clsPerson();
            person.firstName = requestDTO.firstName;
            person.secondName = requestDTO.secondName;
            person.thirdName = requestDTO.thirdName;
            person.lastName = requestDTO.lastName;
            person.email = requestDTO.email;
            person.phoneNumber = requestDTO.phoneNumber;
            person.gender = requestDTO.gender;


            if (person.save())
            {
                clsUser user = new clsUser();
                user.personID = person.personID;
                user.username = requestDTO.username;
                user.password = requestDTO.password;
                user.photoURL = "";

                if (user.save())
                {
                    clsAccount account = new clsAccount();
                    account.userID = user.userID;
                    account.accountTypeID = (int)clsAccountType.enType.Normal;
                    account.accountNumber = Guid.NewGuid().ToString();

                    if (account.save())
                    {
                        return Ok(new clsRegisterResponseDTO { isSucceed = true, message = "New account created successfully" });
                    }
                    else
                    {
                        return BadRequest("Failed to create new account");
                    }

                }
                else
                {
                    return BadRequest("Failed to create new account");
                }

            }

            return BadRequest("Failed to create new account");
        }





        [HttpPost("User/Update/Information", Name = "UpdateInformation")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdateInformation([FromBody] clsUpdateInformationRequestDTO requestDTO)
        {

            if (int.IsNegative(requestDTO.userID) || int.IsNegative(requestDTO.personID))
            {
                return BadRequest("User account not found");
            }

            if (string.IsNullOrEmpty(requestDTO.username) || string.IsNullOrEmpty(requestDTO.password))
            {
                return BadRequest("Username and password can't be Null");
            }

            if (string.IsNullOrEmpty(requestDTO.firstName))
            {
                return BadRequest("Firstname can't be null");
            }

            if (string.IsNullOrEmpty(requestDTO.secondName))
            {
                return BadRequest("Secondname can't be null");
            }

            if (string.IsNullOrEmpty(requestDTO.thirdName))
            {
                return BadRequest("Thirdname can't be null");
            }

            if (string.IsNullOrEmpty(requestDTO.lastName))
            {
                return BadRequest("Lastname can't be null");
            }

            if (string.IsNullOrEmpty(requestDTO.email))
            {
                return BadRequest("Email can't be null");
            }

            if (string.IsNullOrEmpty(requestDTO.phoneNumber))
            {
                return BadRequest("Phone number can't be null");
            }


            string currentUsername = requestDTO.currentUsername;

            if (currentUsername != requestDTO.username && clsUser.doesUserExistByUsername(requestDTO.username))
            {
                return Conflict("Username already exists. Please choose a different username.");
            }

            string currentEmail = requestDTO.currentEmail;

            if (currentEmail != requestDTO.currentEmail && clsUser.doesUserExistByUsername(requestDTO.username))
            {
                return Conflict("Email already exists. Please choose a different email.");
            }



            clsPerson person = new clsPerson();

            person.mode = clsPerson.enMode.Update;
            person.personID = requestDTO.personID;
            person.firstName = requestDTO.firstName;
            person.secondName = requestDTO.secondName;
            person.thirdName = requestDTO.thirdName;
            person.lastName = requestDTO.lastName;
            person.email = requestDTO.email;
            person.phoneNumber = requestDTO.phoneNumber;
            person.gender = requestDTO.gender;


            clsUser user = new clsUser();
            user = clsUser.getUserByUserID(requestDTO.userID);


            if (clsEncryptor.ComputeHash(requestDTO.password) != user.password)
            {
                return Unauthorized("Incorrect password");
            }


            user.mode = clsUser.enMode.Update;
            user.username = requestDTO.username;
            user.password = requestDTO.password;
            user.personID = requestDTO.personID;
            user.photoURL = requestDTO.photoURL;


            if (person.save() && user.save())
            {
                return Ok(new clsUpdateInformationResponseDTO { isSucceed = true, message = "Information updated successfully" });
            }


            return Unauthorized("Failed to update information");
        }





        [HttpPost("User/GetProfileCardInformation", Name = "GetUserProfileCardInformation")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetUserProfileCardInformation([FromBody] clsCurrentUserInformationRequestDTO requestDTO)
        {
            if (int.IsNegative(requestDTO.userID) || int.IsNegative(requestDTO.personID))
            {
                return BadRequest("Invalid id was supplied");
            }

            clsPerson person = clsPerson.getPersonByPersonID(requestDTO.personID);
            clsUser user = clsUser.getUserByUserID(requestDTO.userID);

            if (person == null || user == null)
            {
                return BadRequest("User not dound");
            }


            int followers = clsUser.getNumberOfUserFollowersByUserID(requestDTO.userID);
            int followings = clsUser.getNumberOfUserFollowingsByUserID(requestDTO.userID);

            return Ok(new clsCurrentUserInformationResponseDTO { username = user.username, photoURL = (user.photoURL == null) ? "default.png" : user.photoURL, fullName = person.fullname, numberOfFollowers = followers, numberOfFollowings = followings, accountType = (int)user.accountType });
        }




        [HttpPost("User/GetAllInformation", Name = "GetAllUserInformation")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetAllUserInformation([FromBody] clsGetAllUserInformationRequestDTO requestDTO)
        {
            if (int.IsNegative(requestDTO.userID) || int.IsNegative(requestDTO.personID))
            {
                return BadRequest("Invalid id was supplied");
            }

            clsPerson person = clsPerson.getPersonByPersonID(requestDTO.personID);
            clsUser user = clsUser.getUserByUserID(requestDTO.userID);

            if (person == null || user == null)
            {
                return BadRequest("User not dound");
            }



            return Ok(new clsGetAllUserInformationResponseDTO { username = user.username, firstName = person.firstName, secondName = person.secondName, thirdName = person.thirdName, lastName = person.lastName, email = person.email, phoneNumber = person.phoneNumber, gender = person.gender });
        }




        [HttpPost("User/GetUserPosts", Name = "GetUserPosts")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetUserPosts([FromBody] clsGetUserPostsRequestDTO requestDTO)
        {
            if (int.IsNegative(requestDTO.userID))
            {
                return BadRequest("Invalid id was supplied");
            }


            clsUser user = clsUser.getUserByUserID(requestDTO.userID);

            if (user == null)
            {
                return BadRequest("User not found");
            }


            return Ok(clsPost.getPostsByUserIDAsDTO(user.userID));
        }





        [HttpPost("Post/Add", Name = "AddPost")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult AddPost([FromBody] clsAddNewPostRequestDTO requestDTO)
        {

            if (int.IsNegative(requestDTO.accountID))
            {
                return BadRequest("Invalid account ID");
            }

            if (string.IsNullOrEmpty(requestDTO.textContent) && string.IsNullOrEmpty(requestDTO.imageURL) && string.IsNullOrEmpty(requestDTO.videoURL))
            {
                return BadRequest("Invalid content");
            }

            clsPost post = new clsPost();

            post.mode = clsPost.enMode.AddNew;
            post.accountID = requestDTO.accountID;
            post.textContent = requestDTO.textContent;
            post.imageURL = requestDTO.imageURL;
            post.videoURL = requestDTO.videoURL;
            post.creationDateTime = DateTime.Now;

            if (!post.save())
            {
                return BadRequest("Failed to create new post");
            }


            return Ok(new clsAddNewPostResponseDTO { isSucceed = true, message = "New post created successfully" });
        }


        [HttpPost("Post/Delete", Name = "DeletePost")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult DeletePost([FromBody] clsDeletePostRequestDTO requestDTO)
        {

            if (int.IsNegative(requestDTO.postID))
            {
                return BadRequest("Invalid id was supplied");
            }

            clsPost post = clsPost.getPostByPostID(requestDTO.postID);

            if (post == null)
            {
                return BadRequest("Post not found");
            }

            post.mode = clsPost.enMode.Delete;

            if (!post.save())
            {
                return StatusCode(500, new clsUpdatePostResponseDTO { isSucceed = false, message = "Failed to delete the post. Please try again later." });
            }


            return Ok(new clsDeletePostResponseDTO { isSucceed = true, message = "Post Deleted Succesfully" });
        }



        [HttpPost("Post/Update", Name = "UpdatePost")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdatePost([FromBody] clsUpdatePostRequestDTO requestDTO)
        {

            if (int.IsNegative(requestDTO.postID))
            {
                return BadRequest("Invalid id was supplied");
            }

            clsPost post = clsPost.getPostByPostID(requestDTO.postID);

            if (post == null)
            {
                return BadRequest("Post not found");
            }


            post.mode = clsPost.enMode.Update;
            post.textContent = requestDTO.textContent;
            post.imageURL = requestDTO.imageURL;
            post.videoURL = requestDTO.videoURL;

            if (!post.save())
            {
                return StatusCode(500, new clsUpdatePostResponseDTO { isSucceed = false, message = "Failed to update the post. Please try again later." });
            }

            return Ok(new clsUpdatePostResponseDTO { isSucceed = true, message = "Post Updated Succesfully" });
        }



        [HttpPost("Post/GetTimelinePosts", Name = "GetTimelinePosts")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetTimelinePosts([FromBody] clsGetTimelinePostsRequestDTO requestDTO)
        {
            if (int.IsNegative(requestDTO.userID))
            {
                return BadRequest("Invalid id was supplied");
            }


            clsUser user = clsUser.getUserByUserID(requestDTO.userID);

            if (user == null)
            {
                return BadRequest("User not found");
            }

            List<clsPost> posts = clsPost.getAllPosts(requestDTO.userID);

            List<clsGetTimelinePostsResponseDTO> postsDTO = new List<clsGetTimelinePostsResponseDTO>();
            foreach (var post in posts)
            {
                clsGetTimelinePostsResponseDTO postDTO = new clsGetTimelinePostsResponseDTO
                {
                    postID = post.postID,
                    accountID = post.accountID,
                    fullname = post.account.user.person.fullname,
                    username = post.account.user.username,
                    profileImageURL = post.account.user.photoURL,
                    creationDateTime = post.creationDateTime,
                    textContent = post.textContent,
                    imageURL = post.imageURL,
                    videoURL = post.videoURL
                };

                postsDTO.Add(postDTO);
            }


            return Ok(postsDTO);
        }




        [HttpPost("User/SuggestedUsersToFollow", Name = "GetSuggestedUsersToFollow")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetSuggestedUsersToFollow([FromBody] clsSuggestedUsersToFollowRequestDTO requestDTO)
        {
            if (int.IsNegative(requestDTO.userID))
            {
                return BadRequest("Invalid User ID was supplied");
            }

            List<clsUser> users = clsUser.getSuggestedUsersToFollowList(requestDTO.userID, requestDTO.numberOfUsers);

            if (users.Count == 0)
            {
                return BadRequest("No Suggested Users Found");
            }

            List<clsSuggestedUsersToFollowResponseDTO> list = new List<clsSuggestedUsersToFollowResponseDTO>();
            foreach (var user in users)
            {
                list.Add(new clsSuggestedUsersToFollowResponseDTO
                {
                    suggestedUserID = user.userID,
                    suggestedUserFullname = user.person.fullname,
                    suggestedUserUsername = user.username,
                    suggestedUserFirstname = user.person.firstName,
                    suggestedUserLastname = user.person.lastName,
                    suggestedUserNumberOfFollowers = user.numberOfFollowers,
                    suggestedUserNumberOfFollowings = user.numberOfFollowings,
                    suggestedUserPhotoURL = user.photoURL
                });
            }

            return Ok(list);
        }




        [HttpPost("User/Follow", Name = "FollowUser")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult FollowUser([FromBody] clsFollowUserRequestDTO requestDTO)
        {

            if (int.IsNegative(requestDTO.followerUserID) || int.IsNegative(requestDTO.followedUserID))
            {
                return BadRequest("Invalid User ID");
            }

            if (!clsUser.doesUserExistByUserID(requestDTO.followerUserID))
            {
                return BadRequest($"User with {requestDTO.followerUserID} does not exist");
            }

            if (!clsUser.doesUserExistByUserID(requestDTO.followedUserID))
            {
                return BadRequest($"User with {requestDTO.followedUserID} does not exist");
            }

            if (!clsFollowers.followUser(requestDTO.followerUserID, requestDTO.followedUserID))
            {
                return BadRequest("Failed to follow user");
            }

            return Ok(new clsFollowUserResponseDTO { isSucceed = true, message = "Followed Successfully" });
        }




        [HttpPost("User/Unfollow", Name = "UnfollowUser")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult UnfollowUser([FromBody] clsUnfollowUserRequestDTO requestDTO)
        {

            if (int.IsNegative(requestDTO.followerUserID) || int.IsNegative(requestDTO.followedUserID))
            {
                return BadRequest("Invalid UserID");
            }

            if (!clsUser.doesUserExistByUserID(requestDTO.followerUserID))
            {
                return BadRequest($"User with {requestDTO.followerUserID} does not exist");
            }

            if (!clsUser.doesUserExistByUserID(requestDTO.followedUserID))
            {
                return BadRequest($"User with {requestDTO.followedUserID} does not exist");
            }

            if (!clsFollowers.unfollowUser(requestDTO.followerUserID, requestDTO.followedUserID))
            {
                return BadRequest("Failed to unfollow user");
            }


            return Ok(new clsUnfollowUserResponseDTO { isSucceed = true, message = "Unfollowed successfully" });
        }



        [HttpPost("User/GetUserFollowers", Name = "GetUserFollowers")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetUserFollowers([FromBody] clsGetUserFollowersRequestDTO requestDTO)
        {
            if (int.IsNegative(requestDTO.userID))
            {
                return BadRequest("Invalid User ID was supplied");
            }

            List<clsUser> users = clsUser.getUserFollowers(requestDTO.userID);

            List<clsGetUserFollowersResponseDTO> list = new List<clsGetUserFollowersResponseDTO>();
            foreach (var user in users)
            {

                list.Add(new clsGetUserFollowersResponseDTO
                {
                    userID = user.userID,
                    username = user.username,
                    fullname = user.person.fullname,
                    firstname = user.person.firstName,
                    lastname = user.person.lastName,
                    numberOfFollowings = user.numberOfFollowings,
                    numberOfFollowers = user.numberOfFollowers,
                    photoURL = user.photoURL

                });


            }

            return Ok(list);
        }




        [HttpPost("User/GetUserFollowings", Name = "GetUserFollowings")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetUserFollowings([FromBody] clsGetUserFollowingsRequestDTO requestDTO)
        {
            if (int.IsNegative(requestDTO.userID))
            {
                return BadRequest("Invalid User ID was supplied");
            }

            List<clsUser> users = clsUser.getUserFollowings(requestDTO.userID);

            List<clsGetUserFollowingsResponseDTO> list = new List<clsGetUserFollowingsResponseDTO>();
            foreach (var user in users)
            {

                list.Add(new clsGetUserFollowingsResponseDTO
                {
                    userID = user.userID,
                    username = user.username,
                    fullname = user.person.fullname,
                    firstname = user.person.firstName,
                    lastname = user.person.lastName,
                    numberOfFollowings = user.numberOfFollowings,
                    numberOfFollowers = user.numberOfFollowers,
                    photoURL = user.photoURL

                });


            }

            return Ok(list);
        }





        [HttpPost("Likes/LikePost", Name = "LikePost")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult LikePost([FromBody] clsLikePostRequestDTO requestDTO)
        {
            if (int.IsNegative(requestDTO.userID))
            {
                return BadRequest("Invalid UserID Was Supplied");
            }

            if (int.IsNegative(requestDTO.postID))
            {
                return BadRequest("Invalid PostID was supplied");
            }

            bool isSucceed = clsLike.addNewLike(new clsLikeDTO { likerUserID = requestDTO.userID, postID = requestDTO.postID }) != -1;

            return Ok(new clsLikePostResponseDTO { isSucceed = isSucceed });
        }



        [HttpPost("Likes/UnlikePost", Name = "UnlikePost")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult UnlikePost([FromBody] clsUnlikePostRequestDTO requestDTO)
        {


            if (int.IsNegative(requestDTO.userID))
            {
                return BadRequest("Invalid UserID Was Supplied");
            }

            if (int.IsNegative(requestDTO.postID))
            {
                return BadRequest("Invalid PostID was supplied");
            }

            clsLike like = clsLike.findLikeByPostIDAndLikerUserID(requestDTO.postID, requestDTO.userID);
            like.mode = clsLike.enMode.Delete;

            bool isSucceed = like.save();

            return Ok(new clsLikePostResponseDTO { isSucceed = isSucceed });
        }


        [HttpPost("Likes/GetNumberOfPostLikes", Name = "GetNumberOfPostLikes")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetNumberOfPostLikes([FromBody] clsGetNumberOfPostLikesRequestDTO requestDTO)
        {
            if (int.IsNegative(requestDTO.postID))
            {
                return BadRequest("Invalid PostID was supplied");
            }

            int numberOfPostLikes = clsLike.getNumberOfPostLikes(requestDTO.postID);

            return Ok(new clsGetNumberOfPostLikesResponseDTO { numberOfPostLikes = numberOfPostLikes });
        }


        [HttpPost("Likes/GetUsersWhoLikePost", Name = "GetUsersWhoLikePost")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetUsersWhoLikePost([FromBody] clsGetUsersWhoLikePostRequestDTO requestDTO)
        {
            if (int.IsNegative(requestDTO.postID))
            {
                return BadRequest("Invalid PostID was supplied");
            }

            List<clsUser> users = clsLike.getUsersWhoLikePostByPostID(requestDTO.postID);

            List<clsGetUsersWhoLikePostResponseDTO> list = new List<clsGetUsersWhoLikePostResponseDTO>();
            foreach (var user in users)
            {
                user.loadUserCompositeObjects();


                list.Add(new clsGetUsersWhoLikePostResponseDTO
                {
                    userID = user.userID,
                    username = user.username,
                    fullname = user.person.fullname,
                    firstname = user.person.firstName,
                    lastname = user.person.lastName,
                    numberOfFollowings = user.numberOfFollowings,
                    numberOfFollowers = user.numberOfFollowers,
                    photoURL = user.photoURL

                });


            }


            return Ok(list);
        }


        [HttpPost("Likes/IsPostLikedByUser", Name = "IsPostLikedByUser")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult IsPostLikedByUser([FromBody] clsIsPostLikedByUserRequestDTO requestDTO)
        {
            if (int.IsNegative(requestDTO.userID))
            {
                return BadRequest("Invalid UserID Was Supplied");
            }

            if (int.IsNegative(requestDTO.postID))
            {
                return BadRequest("Invalid PostID was supplied");
            }

            bool isPostLikedByUser = clsPost.isPostLikedByUser(requestDTO.postID, requestDTO.userID);

            return Ok(new clsIsPostLikedByUserResponseDTO { isPostLikeByUser = isPostLikedByUser });
        }



        [HttpPost("Comments/GetNumberOfPostComments", Name = "GetNumberOfPostComments")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetNumberOfPostComments([FromBody] clsGetNumberPostCommentsRequestDTO requestDTO)
        {

            if (int.IsNegative(requestDTO.postID))
            {
                return BadRequest("Invalid PostID");
            }

            if (requestDTO.postID == 0)
            {
                return BadRequest("Invalid PostID");
            }

            int numberOfPostComments = clsComment.getNumberOfPostComments(requestDTO.postID);

            return Ok(new clsGetNumberOfPostCommentsResponseDTO { numberOfPostComments = numberOfPostComments });
        }






      
        [HttpPost("Comments/GetPostComments", Name = "GetPostComments")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetPostComments([FromBody] clsGetPostCommentsRequestDTO requestDTO)
        {

            if (int.IsNegative(requestDTO.postID))
            {
                return BadRequest("Invalid PostID");
            }

            if (requestDTO.postID == 0)
            {
                return BadRequest("Invalid PostID");
            }


            List<clsComment> list = clsComment.getPostComments(requestDTO.postID);

            List<clsGetPostCommentsResponseDTO> dtoList = new List<clsGetPostCommentsResponseDTO>();
            foreach (var comment in list)
            {
                comment.loadCompositeObjects();
                comment.commenterUser.loadUserCompositeObjects();
               

                clsGetPostCommentsResponseDTO response = new clsGetPostCommentsResponseDTO
                {
                    commentID = comment.commentID,
                    postID = comment.postID,
                    username = comment.commenterUser.username,
                    fullname = comment.commenterUser.person.fullname,
                    commenterUserID = comment.commenterUser.userID,
                    photoURL = comment.commenterUser.photoURL,
                    commentText = comment.commentText
                };

                dtoList.Add(response);
            }

            return Ok(dtoList);
        }







        [HttpPost("Comments/AddNewComment", Name = "AddNewComment")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult AddNewComment([FromBody] clsAddNewCommentRequestDTO requestDTO)
        {
            if (int.IsNegative(requestDTO.postID) || requestDTO.postID == 0)
            {
                return BadRequest("Invalid PostID");
            }

            if (int.IsNegative(requestDTO.commenterUserID) || requestDTO.commenterUserID == 0)
            {
                return BadRequest("Invalid Commenter UserID");
            }

            clsComment comment = new clsComment();

            comment.postID = requestDTO.postID;
            comment.commenterUserID = requestDTO.commenterUserID;
            comment.commentText = requestDTO.commentText;
            comment.mode = clsComment.enMode.AddNew;

            bool isSucceed = comment.save();

            return Ok(new clsAddNewCommentResponseDTO { isSucceed = isSucceed });
        }



        [HttpPost("Comments/DeleteComment", Name = "DeleteComment")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult DeleteComment([FromBody] clsDeleteCommentRequestDTO requestDTO)
        {
            if (int.IsNegative(requestDTO.commentID) || requestDTO.commentID == 0)
            {
                return BadRequest("Invalid CommentID");
            }

            clsComment comment = clsComment.getCommentByCommentID(requestDTO.commentID);
            comment.mode = clsComment.enMode.Delete;

            bool isSucceed = comment.save();

            return Ok(new clsDeleteCommentResponseDTO { isSucceed = isSucceed });
        }


        [HttpPost("Comments/UpdateComment", Name = "UpdateComment")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult UpdateComment([FromBody] clsUpdateCommentRequestDTO requestDTO)
        {
            if (int.IsNegative(requestDTO.commentID) || requestDTO.commentID == 0)
            {
                return BadRequest("Invalid CommentID");
            }

            clsComment comment = clsComment.getCommentByCommentID(requestDTO.commentID);

            comment.commentText = requestDTO.commentText;
            comment.mode = clsComment.enMode.Update;

            bool isSucceed = comment.save();

            return Ok(new clsUpdateCommentResponseDTO { isSucceed = isSucceed });
        }


        [HttpPost("Chats/GetUsersToChatWith",Name = "GetUsersToChatWith")]
        public IActionResult GetUsersToChatWith([FromBody] clsGetUsersToChatWithRequestDTO requestDTO)
        {
            
            if(requestDTO.userID <= 0)
            {
                return BadRequest("Invalid User ID");
            }

            clsUser user = clsUser.getUserByUserID(requestDTO.userID);
            if(user == null)
            {
                return BadRequest("User Does Not Exist");
            }

            List<clsUser> users = clsMessage.getUsersToChatWith(requestDTO.userID);

            List<clsGetUsersToChatWithResponseDTO> list = new List<clsGetUsersToChatWithResponseDTO>();
            foreach(var item in users)
            {
                item.loadUserCompositeObjects();

                clsGetUsersToChatWithResponseDTO dto = new clsGetUsersToChatWithResponseDTO 
                {
                    userID = item.userID,
                    personID = item.personID,
                    firstName = item.person.firstName ,
                    lastName = item.person.lastName ,
                    username = item.username,
                    photoURL = item.photoURL,
                
                };

                list.Add(dto);
            }

            return Ok(list);
        }




        [HttpPost("Chats/GetMessagesSentToUser",Name = "GetMessagesSentToUser")]
        public IActionResult GetMessagesSentToUser([FromBody] clsGetSentMessagesToUserRequestDTO requestDTO)
        {

            if(requestDTO.senderUserID <= 0 || requestDTO.recipientUserID <= 0)
            {
                return BadRequest("Invalid UserID");
            }

            List<clsMessageDTO> list = clsMessage.getMessagesSentFromToAsDTO(new clsMessageDTO { senderAccountID = requestDTO.senderUserID , recieverAccountID = requestDTO.recipientUserID});

            return Ok(list);
        }


        [HttpPost("Chats/SendMessage", Name = "SendMessage")]
        public IActionResult SendMessage([FromBody] clsSendMessageRequestDTO requestDTO) 
        {
            if(requestDTO.senderUserID <= 0 || requestDTO.recipientUserID <= 0)
            {
                return BadRequest("Invalid UserID");
            }

            clsAccount senderAccount = clsAccount.getAccountByUserID(requestDTO.senderUserID);
            clsAccount recieverAccount = clsAccount.getAccountByUserID(requestDTO.recipientUserID);
            senderAccount.loadCompositeObjects();
            recieverAccount.loadCompositeObjects();
        
            clsMessage message = new clsMessage();
            message.mode = clsMessage.enMode.AddNew;
            message.senderAccountID = senderAccount.userID;
            message.recieverAccountID = recieverAccount.userID;
            message.message = requestDTO.message;

            bool isSucceed = message.save();

            return Ok(new clsSendMessageResponseDTO { isSucceed = isSucceed});
        }

    }
}
    
using Pathy_BusinessAccessLayer;
using Pathy_DataAccessLayer.DTOs;

namespace Pathy.Models
{
    public class clsUsersWhoLikesPostViewModel
    {
        public List<clsUser> usersWhoLikePost = new List<clsUser>();

        public void loadUsersWhoLikesPost(int postID)
        {
            this.usersWhoLikePost = clsLike.getUsersWhoLikePostByPostID(postID);    
            
            foreach(var user in this.usersWhoLikePost)
            {
                user.loadUserCompositeObjects();
            }
        }

    }
}

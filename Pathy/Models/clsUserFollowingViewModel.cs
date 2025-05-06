using Pathy_BusinessAccessLayer;

namespace Pathy.Models
{
    public class clsUserFollowingViewModel
    {   
        public int userID { get; set; }

        public int followerUserID { get; set; }
        public int followedUserID { get; set; }

        public List<clsUser> followings = new List<clsUser>();

        public void loadUserFollowings(int userID)
        {
            this.followings = clsUser.getUserFollowings(userID);
        }
    }
}

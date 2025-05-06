using Pathy_BusinessAccessLayer;

namespace Pathy.Models
{
    public class clsUserFollowersViewModel
    {
       public int followerUserID { get; set; }

        public int followedUserID { get; set; }

        public List<clsUser> followers = new List<clsUser>();

       public void loadUserFollowers(int userID)
       {
            this.followers = clsUser.getUserFollowers(userID);
       }

    }
}

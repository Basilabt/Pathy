using Pathy_BusinessAccessLayer;

namespace Pathy.Models
{
    public class clsSuggestedFriendsViewModel
    {
        public int userID { get; set; }

        public List<clsUser> suggestedUsersToFollow = new List<clsUser>();
        public void loadSuggestedUsersToFollowList()
        {
            this.suggestedUsersToFollow = clsUser.getSuggestedUsersToFollowList(clsGlobal.logedInUser.userID,20);
        }

       
    }
}

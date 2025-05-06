using Pathy_BusinessAccessLayer;

namespace Pathy.Models
{
    public class clsUserProfileViewModel
    {
        public enum enCardType
        {
            CurrentUserProfileCard = 1,
            NewUserProfileCard = 2             
        }

        public int userID { get; set; }
        public string fullname { get; set; }
        public string username { get; set; }
        public int numberOfFollowers { get; set; }
        public int numberOfFollowings { get; set; } 
        public string photoURL { get; set; }
        public int accountType { get; set; }
        public enCardType cardType { get; set; }
        
        public List<clsPost> posts = new List<clsPost>();
        
        public void loadCurrentUserPosts()
        {
            this.posts = clsPost.getPostsByUserID(clsGlobal.logedInUser.userID);
        }


    }
}

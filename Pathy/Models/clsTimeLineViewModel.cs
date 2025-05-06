using Pathy_BusinessAccessLayer;
using System.ComponentModel.DataAnnotations;

namespace Pathy.Models
{
    public class clsTimeLineViewModel
    {
        [Required(ErrorMessage = "Post content can't be empty")]
        public string? postTextContent { get; set; }

        public int postID {  get; set; }

        public List<clsPost> allPostsList = new List<clsPost>();

        public List<clsPost> currentUserPosts = new List<clsPost>();


        public void loadAllPostsList()
        {
            this.allPostsList = clsPost.getAllPosts(clsGlobal.logedInUser.userID);      
        }



    }
}

using Pathy_DataAccessLayer.DTOs;
using Pathy_BusinessAccessLayer;

namespace Pathy.Models
{
    public class clsPostCommentsViewModel
    {
        public int postID { set; get; }

        public int commentID { set; get; }
        public string commentText { set; get; }

        public  List<clsComment> postComments = new List<clsComment>();

        public void loadPostComments()
        {          
            this.postComments = clsComment.getPostComments(this.postID);   
        }
    }
}

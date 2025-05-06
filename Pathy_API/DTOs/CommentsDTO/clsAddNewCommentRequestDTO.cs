namespace Pathy_API.DTOs.CommentsDTO
{
    public class clsAddNewCommentRequestDTO
    {     
        public int postID { get; set; }
        public int commenterUserID { get; set; }
        public string commentText { get; set; }
    }
}

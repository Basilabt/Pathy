namespace Pathy_API.DTOs.CommentsDTO
{
    public class clsGetPostCommentsResponseDTO
    {
        public int commentID { get; set; }
        public int postID { get; set; }
        public string username { get; set; }
        public string fullname { get; set; }
        public string photoURL { get; set; }
        public int commenterUserID { get; set; }
        public string commentText { get; set; }
    }
}

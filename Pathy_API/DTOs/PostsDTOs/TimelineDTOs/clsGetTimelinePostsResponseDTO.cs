namespace Pathy_API.DTOs.PostsDTOs.TimelineDTOs
{
    public class clsGetTimelinePostsResponseDTO
    {
        public int postID { set; get; }
        public int accountID { set; get; }
        public string fullname { set; get; }
        public string username { set; get; }

        public string profileImageURL { set; get; }
        public DateTime creationDateTime { set; get; }
        public string textContent { set; get; }
        public string imageURL { set; get; }
        public string videoURL { set; get; }
    }
}

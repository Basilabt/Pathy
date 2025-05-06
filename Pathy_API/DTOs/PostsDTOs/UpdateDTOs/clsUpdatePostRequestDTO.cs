namespace Pathy_API.DTOs.PostsDTOs.UpdateDTOs
{
    public class clsUpdatePostRequestDTO
    {
        public int postID { get; set; }
        public string textContent { get; set; }
        public string imageURL { get; set; }
        public string videoURL { get; set; }

    }
}

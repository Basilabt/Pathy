namespace Pathy_API.DTOs.PostsDTOs.NewPostDTOs
{
    public class clsAddNewPostRequestDTO
    {
        public int accountID { get; set; }
        public string textContent { get; set; }
        public string imageURL { get; set; }
        public string videoURL { get; set; }
    }
}

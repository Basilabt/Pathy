namespace Pathy_API.DTOs.ChatDTOs
{
    public class clsGetUsersToChatWithResponseDTO
    {
        public int userID { get; set; }
        public int personID { get; set; }

        public string firstName { get; set; }

        public string lastName { get; set; }
        public string username { get; set; }
        public string photoURL { get; set; }
    }
}

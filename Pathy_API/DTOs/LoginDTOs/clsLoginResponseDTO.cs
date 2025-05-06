namespace Pathy_API.DTOs.LoginDTOs
{
    public class clsLoginResponseDTO
    {
        public int userID { get; set; }
        public int personID { get; set; } 
        public int accountID { get; set; }
        public string username { get; set; }
        public string photoURL { get; set; } 
        public string jwt { set; get; }
    }
}

namespace Pathy_API.DTOs.InformationDTOs
{
    public class clsCurrentUserInformationResponseDTO
    {
        public string username {  get; set; }

        public string photoURL { get; set; }
        public string fullName { get; set; }
        public int numberOfFollowers { get; set; }
        public int numberOfFollowings { get; set; }
        public int accountType { get; set; }
    }
}

namespace Pathy_API.DTOs.UsersDTO
{
    public class clsSuggestedUsersToFollowResponseDTO
    {
        public int suggestedUserID { set; get; }
        public string suggestedUserUsername { set; get; }
        public string suggestedUserFullname { set; get; }
        public string suggestedUserFirstname { set; get; }
        public string suggestedUserLastname { set; get; }
        public int suggestedUserNumberOfFollowers { set; get; }
        public int suggestedUserNumberOfFollowings { set; get; }
        public string suggestedUserPhotoURL {  set; get; }
    }
}

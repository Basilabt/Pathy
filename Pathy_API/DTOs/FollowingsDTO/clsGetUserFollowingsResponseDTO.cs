namespace Pathy_API.DTOs.FollowingsDTO
{
    public class clsGetUserFollowingsResponseDTO
    {
        public int userID { set; get; }
        public string username { set; get; }
        public string fullname { set; get; }
        public string firstname { set; get; }
        public string lastname { set; get; }
        public int numberOfFollowers { set; get; }
        public int numberOfFollowings { set; get; }
        public string photoURL { set; get; }
    }
}

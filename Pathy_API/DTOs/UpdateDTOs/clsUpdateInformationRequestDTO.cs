using System.Reflection.Metadata;

namespace Pathy_API.DTOs.UpdateDTOs
{
    public class clsUpdateInformationRequestDTO
    {
        public int personID {  get; set; }
        public int userID { get; set; }

        public string currentUsername { set; get; }
        public string username { set ; get; }

        public string password { set; get; }
        public string photoURL { set ; get; }
        public string firstName { set; get; }
        public string secondName {  set; get; }
        public string thirdName { set; get; }
        public string lastName { set; get; }
        public string currentEmail { set; get; }
        public string email { set; get; }   
        public string phoneNumber { set; get; }
        public int gender { set; get; }

    }
}

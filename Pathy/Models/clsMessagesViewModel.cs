using Pathy_BusinessAccessLayer;
using System.Numerics;

namespace Pathy.Models
{
    public class clsMessagesViewModel
    {
        public List<clsMessage> messages = new List<clsMessage>();

        public List<clsUser> usersToChatWith = new List<clsUser>();

        public int userToChatWithID { get; set; }
            


        public void loadMessages()
        {
            this.usersToChatWith = clsMessage.getUsersToChatWith(clsGlobal.logedInUser.userID);
        }
    }
}

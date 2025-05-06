
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Pathy_BusinessAccessLayer;
using Pathy_DataAccessLayer.DTOs;
using System.Collections.Generic;

namespace Pathy.Models
{
    public class clsChatViewModel
    {
        public int userToChatWithID { get; set; }

        public int messageID { get; set; }
        public string message { get; set; }
        public clsUser userToChatWith {  get; set; }
        public clsAccount userToChatWithAccount { get; set; }

        public List<clsMessageDTO> sentMessages = new List<clsMessageDTO>();
        public List<clsMessageDTO> recievedMessages = new List<clsMessageDTO>();

        public List<clsMessageDTO> allmessages
        {
            get
            {
                var all = new List<clsMessageDTO>();
                if (sentMessages != null) all.AddRange(sentMessages);
                if (recievedMessages != null) all.AddRange(recievedMessages);
                return all.OrderBy(m => m.messageID).ToList();
            }
        }

        public void loadUserToChatWith()
        {
            this.userToChatWith = clsUser.getUserByUserID(this.userToChatWithID);
        }

        public void loadUserToChatWithAccount()
        {
            this.userToChatWithAccount = clsAccount.getAccountByUserID(this.userToChatWithID);
        }

        public void loadRecievedMessages()
        {
            this.recievedMessages = clsMessage.getMessagesSentFromToAsDTO(new clsMessageDTO { senderAccountID = this.userToChatWithAccount.accountID , recieverAccountID = clsGlobal.account.accountID });
        }
        public void loadSentMessages()
        {            
            this.sentMessages = clsMessage.getMessagesSentFromToAsDTO(new clsMessageDTO{senderAccountID = clsGlobal.account.accountID , recieverAccountID = this.userToChatWithAccount.accountID});
        }

     
    }
}

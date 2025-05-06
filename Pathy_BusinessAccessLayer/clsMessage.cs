using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pathy_DataAccessLayer;
using Pathy_DataAccessLayer.DTOs;

namespace Pathy_BusinessAccessLayer
{
    public class clsMessage
    {
        public enum enMode
        {
            AddNew = 1 , Update = 2 , Delete = 3
        }

        public int messageID { get; set; }
        public int senderAccountID { get; set; }
        public int recieverAccountID { get; set; }
        public string message { get; set; }
        public enMode mode {  get; set; }
        public clsAccount senderAccount { get; set; }
        public clsAccount recieverAccount { get; set; } 


        public clsMessage()
        {
            this.messageID = -1;
            this.senderAccountID = -1;
            this.recieverAccountID = -1;
            this.message = "";
            this.mode = enMode.AddNew;
        }

        private clsMessage(int messageID , int senderAccountID , int recieverAccountID , string message)
        {
            this.messageID = messageID;
            this.senderAccountID = senderAccountID;
            this.recieverAccountID = recieverAccountID;
            this.message = message;
            this.mode = enMode.Update;
            this.senderAccount = clsAccount.getAccountByAccountID(senderAccountID);
            this.recieverAccount = clsAccount.getAccountByAccountID (recieverAccountID);
        }

        public void loadCompositeObjects()
        {
            this.senderAccount = clsAccount.getAccountByAccountID(this.senderAccountID);
            this.senderAccount.loadCompositeObjects();

            this.recieverAccount = clsAccount.getAccountByAccountID(this.recieverAccountID);
            this.recieverAccount.loadCompositeObjects();
        }

        public bool save()
        {

            switch (this.mode)
            {

                case enMode.AddNew:
                    {   this.messageID = addNewMessage(new clsMessageDTO { messageID = -1, recieverAccountID = this.recieverAccountID, senderAccountID = this.senderAccountID, message = this.message });
                        return this.messageID != -1;
                    }

                case enMode.Update:
                    {
                        return updateMessage(new clsMessageDTO { messageID = this.messageID, recieverAccountID = this.recieverAccountID, senderAccountID = this.senderAccountID, message = this.message });
                    }

                case enMode.Delete:
                    {
                        return deleteMessageByMessageID(new clsMessageDTO { messageID = this.messageID, recieverAccountID = this.recieverAccountID, senderAccountID = this.senderAccountID, message = this.message });
                    }

            }

            return false;
        }

        // Static Methods

        public static clsMessage getMessageByMessageID(int messageID)                
        {
            clsMessageDTO messageDTO = new clsMessageDTO();
            messageDTO.messageID = messageID;

            if(clsMessageDataAccess.getMessageByMessageID(messageDTO))
            {
                return new clsMessage { messageID = messageDTO.messageID, senderAccountID = messageDTO.senderAccountID, recieverAccountID = messageDTO.recieverAccountID, message = messageDTO.message };
            }

            return null;
        }

        public static int addNewMessage(clsMessageDTO messageDTO)
        {
            return clsMessageDataAccess.addNewMessage(messageDTO);
        }

        public static bool deleteMessageByMessageID(clsMessageDTO messageDTO)
        {
            return clsMessageDataAccess.deleteMessageByMessageID(messageDTO);
        }

        public static bool updateMessage(clsMessageDTO messageDTO)
        {
            return clsMessageDataAccess.updateMessage(messageDTO);
        }

        public static List<clsUserDTO> getUsersToChatWithAsDTO(int userID)
        {
            return clsMessageDataAccess.getUsersToChatWith(new clsUserDTO { userID = userID });
        }

        public static List<clsUser> getUsersToChatWith(int userID)
        {
            List<clsUser> users = new List<clsUser>();

            foreach(var item in getUsersToChatWithAsDTO(userID))
            {
                clsUser user = new clsUser
                {
                    userID = item.userID,
                    personID = item.personID,
                    username = item.username,
                    password = item.password,
                    photoURL = item.photoURL
                };

                user.loadUserCompositeObjects();

                users.Add(user);
            }

            return users;
        }

        public static List<clsMessageDTO> getMessagesSentFromToAsDTO(clsMessageDTO messageDTO)
        {
            return clsMessageDataAccess.getMessagesSentFromTo(messageDTO);
        }

    }
}

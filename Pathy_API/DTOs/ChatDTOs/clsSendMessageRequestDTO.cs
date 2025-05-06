namespace Pathy_API.DTOs.ChatDTOs
{
    public class clsSendMessageRequestDTO
    {
        public int senderUserID { get; set; }
        public int recipientUserID { get; set; }
        public string message { get; set; }

    }
}

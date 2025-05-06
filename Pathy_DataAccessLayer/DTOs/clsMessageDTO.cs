using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathy_DataAccessLayer.DTOs
{
    public class clsMessageDTO
    {
        public int messageID {  get; set; }
        public int senderAccountID { get; set; }
        public int recieverAccountID { get; set; }
        public string message { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathy_DataAccessLayer.DTOs
{
    public class clsPostDTO
    {
        public int postID { set; get; }
        public int accountID { set; get; }
        public DateTime creationDateTime { set; get; }
        public string textContent { set; get; }
        public string imageURL { set; get; }
        public string videoURL { set; get; }
    }
}

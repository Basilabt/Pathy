using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathy_DataAccessLayer.DTOs
{
    public class clsLikeDTO
    {
        public int likeID { get; set; }        
        public int postID { get; set; }
        public int likerUserID { get; set; }
    }
}

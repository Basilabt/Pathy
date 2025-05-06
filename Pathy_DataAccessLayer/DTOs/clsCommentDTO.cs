using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathy_DataAccessLayer.DTOs
{
    public class clsCommentDTO
    {
        public int commentID { get; set; }
        public int postID { get; set; }
        public int commenterUserID { get; set; }
        public string commentText { get; set; }
    }
}

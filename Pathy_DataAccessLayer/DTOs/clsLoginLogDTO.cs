using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathy_DataAccessLayer.DTOs
{
    public class clsLoginLogDTO
    {
        public int loginLogID { get; set; }
        public int accountID { get; set; }
        public DateTime loginDateTime { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathy_DataAccessLayer.DTOs
{
    public class clsAccountDTO
    {
        public int accountID { get; set; }
        public int userID { get; set; }
        public int accountTypeID { get; set; }
        public string accountNumber { get; set; }
    }
}

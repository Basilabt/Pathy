using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Pathy_BusinessAccessLayer
{
    public class clsGlobal
    {
        public static clsUser logedInUser;
        public static clsAccount account;

        public static bool isLoggedIn()
        {
            return clsGlobal.logedInUser != null && clsGlobal.account != null;
        }
    }
}

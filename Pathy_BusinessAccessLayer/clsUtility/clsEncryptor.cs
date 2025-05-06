

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


namespace FactWebsiteApp.Models.Utility
{
    public class clsEncryptor
    {



        public static string ComputeHash(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {

                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));

                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }

    }
}

using Pathy_DataAccessLayer;
using Pathy_DataAccessLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathy_BusinessAccessLayer
{
    public class clsAccountType
    {
        public enum enMode
        {
            AddNew = 1, Update = 2 , Delete = 3
        }

        public enum enType
        {
            Normal = 1 , InvitedByDeveloper = 2 , Developer = 3
        }

        public int accountTypeID { get; set; }
        public int accountType { get; set; }
        public string description { get; set; }
        public enType type { get; set; }

        public enMode mode { get; set; }

        public clsAccountType()
        {
            this.accountTypeID = -1;
            this.accountType = -1;
            this.description = "";
            this.mode = enMode.AddNew;
            
        }

        private clsAccountType(int accountTypeID , int accountType , string description)
        {
            this.accountTypeID = accountTypeID;
            this.accountType = accountType;
            this.description = description;
            this.mode = enMode.Update;
        }

        // Methods

        public static clsAccountType getAccountTypeByAccountTypeID(int accountTypeID)
        {
            clsAccountTypeDTO accountTypeDTO = new clsAccountTypeDTO();

            if(clsAccountTypeDataAccess.getAccountTypeByAccountTypeID(accountTypeID,accountTypeDTO))
            {
                return new clsAccountType
                {
                    accountTypeID = accountTypeDTO.accountTypeID ,
                    accountType = accountTypeDTO.accountType ,
                    description = accountTypeDTO.description 
                };
            }

            return null;
        }

        public static enType getAccountTypeByUserID(int userID)
        {
            return (enType)clsAccountTypeDataAccess.getAccountTypeByUserID(userID);
        }
    }
}

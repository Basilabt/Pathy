using Pathy_DataAccessLayer;
using Pathy_DataAccessLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Pathy_BusinessAccessLayer
{
    public class clsAccount
    {
        public enum enMode
        {
            AddNew = 1 , Update = 2 , Delete = 3
        }

        public int accountID {  get; set; }
        public int userID { get; set; }
        public int accountTypeID { get; set; }
        public string accountNumber { get; set; }
        public enMode mode { get; set; }

        public clsUser user { get; set; }
        public clsAccountType accountType { get; set; }

        public clsAccount()
        {
            this.accountID = -1;
            this.userID = -1;
            this.accountTypeID = -1;
            this.accountNumber = "";
            this.mode = enMode.AddNew;
        }

        private clsAccount(int accountID , int userID , int accountTypeID , string accountNumber)
        {
            this.accountID = accountID;
            this.userID = userID;
            this.accountTypeID = accountTypeID;
            this.accountNumber = accountNumber;
            this.mode = enMode.Update;
          
            this.accountType = clsAccountType.getAccountTypeByAccountTypeID(accountTypeID);
            this.user = clsUser.getUserByUserID(userID);
            
        }


        public bool save()
        {

            switch (this.mode)
            {

                case enMode.AddNew:
                    {
                        this.accountID = addNewAccount(new clsAccountDTO {userID=this.userID,accountTypeID=this.accountTypeID,accountNumber=this.accountNumber});
                        return this.accountID != -1;
                    }

                case enMode.Update:
                    {
                        return false;
                    }

                case enMode.Delete:
                    {
                        return false;
                    }

            }

            return false;


        }

        public void loadCompositeObjects()
        {
            this.user = clsUser.getUserByUserID(this.userID);
            this.accountType = clsAccountType.getAccountTypeByAccountTypeID(this.accountTypeID);
        }

        // Static Methods

        public static clsAccount getAccountByUserID(int userID)
        {
            clsAccountDTO accountDTO = new clsAccountDTO();

            if(clsAccountDataAccess.getAccountByUserID(userID,accountDTO))
            {
                return new clsAccount
                {
                    accountID = accountDTO.accountID,
                    userID = accountDTO.userID,
                    accountTypeID = accountDTO.accountTypeID,
                    accountNumber = accountDTO.accountNumber
                };
            }

            return null;
        }

        public static clsAccount getAccountByAccountID(int accountID)
        {
            clsAccountDTO accountDTO = new clsAccountDTO();

            if (clsAccountDataAccess.getAccountByAccountID(accountID, accountDTO))
            {
                return new clsAccount
                {
                    accountID = accountDTO.accountID,
                    userID = accountDTO.userID,
                    accountTypeID = accountDTO.accountTypeID,
                    accountNumber = accountDTO.accountNumber
                };
            }

            return null;
        }

        public static int addNewAccount(clsAccountDTO accountDTO)
        {
            return clsAccountDataAccess.addNewAccount(accountDTO);
        }

    }

}

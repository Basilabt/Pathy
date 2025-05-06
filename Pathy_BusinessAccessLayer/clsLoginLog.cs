using Pathy_DataAccessLayer;
using Pathy_DataAccessLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathy_BusinessAccessLayer
{
    public  class clsLoginLog
    {
        public enum enMode
        {
            AddNew = 1 , Update = 2 , Delete = 3
        }

        public int loginLogID {  get; set; }    
        public int accountID { get; set; }
        public DateTime loginDateTime { get; set; }
        public enMode mode {  get; set; }

        public clsLoginLog()
        {
            this.loginLogID = -1;
            this.accountID = -1;
            this.loginDateTime = DateTime.MinValue;
            this.mode = enMode.AddNew;
        }

        private clsLoginLog(int loginLogID , int accountID , DateTime loginDateTime)
        {
            this.loginLogID = loginLogID;
            this.accountID = accountID;
            this.loginDateTime= loginDateTime;
            this.mode = enMode.Update;
        }

        public bool save()
        {
            switch(this.mode)
            {
                case enMode.AddNew:
                    {
                        this.loginLogID = clsLoginLogDataAccess.addNewLoginLog(new clsLoginLogDTO {accountID=this.accountID,loginDateTime=this.loginDateTime});
                        return this.loginLogID != -1;
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

        // Static methods

    }
}

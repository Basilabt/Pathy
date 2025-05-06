using Microsoft.Data.SqlClient;
using Pathy_DataAccessLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathy_DataAccessLayer
{
    public  class clsLoginLogDataAccess
    {
        public static int addNewLoginLog(clsLoginLogDTO loginLogDTO)
        {
            int newLoginLogID= -1;

            try
            {
                using(SqlConnection connection = new SqlConnection(clsDataAccessSettings.getConnectionString()))
                {
                    connection.Open();

                    string cmd = "SP_AddNewLoginLog";
                    using (SqlCommand command = new SqlCommand(cmd, connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue(@"AccountID",loginLogDTO.accountID);
                        command.Parameters.AddWithValue(@"LoginDateTime", loginLogDTO.loginDateTime);

                        object result = command.ExecuteScalar();

                        if(result != null && int.TryParse(result.ToString(), out int id))
                        {
                            newLoginLogID = id;
                        }
                    }                     
                }

            } catch(Exception exception)
            {
                Console.WriteLine($"DEBUG: {exception.Message}");
            }

            return newLoginLogID;
        }



    }
}

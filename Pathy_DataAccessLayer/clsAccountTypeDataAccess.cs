using Microsoft.Data.SqlClient;
using Pathy_DataAccessLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathy_DataAccessLayer
{
    public class clsAccountTypeDataAccess
    {
        public static bool getAccountTypeByAccountTypeID(int accountTypeID , clsAccountTypeDTO accountTypeDTO)
        {
            bool isFound = false;

            try
            {
                using(SqlConnection connection = new SqlConnection(clsDataAccessSettings.getConnectionString()))
                {
                    connection.Open();

                    string cmd = "SP_GetAccountTypeByAccountTypeID";
                    using(SqlCommand command = new SqlCommand(cmd, connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue(@"AccountTypeID",accountTypeID);
                        
                        using(SqlDataReader reader = command.ExecuteReader())
                        {
                            if(reader.Read())
                            {
                                isFound = true;

                                accountTypeDTO.accountTypeID = reader.GetInt32(reader.GetOrdinal("AccountTypeID"));
                                accountTypeDTO.accountType = reader.GetInt32(reader.GetOrdinal("AccountType"));
                                accountTypeDTO.description = reader.GetString(reader.GetOrdinal("Description"));

                            }
                        }

                    }
                }

            } catch(Exception exception)
            {
                Console.WriteLine($"DEBUG: {exception.Message}");
            }


            return isFound;
        }

        public static int getAccountTypeByUserID(int userID)
        {
            int accountType = -1;

            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.getConnectionString()))
                {
                    connection.Open();

                    string cmd = "SP_GetAccountTypeByUserID";
                    using(SqlCommand command = new SqlCommand(cmd,connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue(@"UserID",userID);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if(reader.Read())
                            {
                                accountType = reader.GetInt32(reader.GetOrdinal("AccountType"));
                            }
                        }
                    }

                }

            } catch(Exception exception)
            {
                Console.WriteLine($"DEBUG: {exception.Message}");
            }


            return accountType;
        }

    }
}

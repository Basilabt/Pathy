using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using Pathy_DataAccessLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathy_DataAccessLayer
{
    public class clsAccountDataAccess
    {
        public static bool getAccountByUserID(int userID , clsAccountDTO accountDTO)
        {
            bool isFound = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.getConnectionString()))
                {
                    connection.Open();

                    string cmd = "SP_GetAccountByUserID";
                    using(SqlCommand command = new SqlCommand(cmd,connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue(@"UserID",userID);

                        using(SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                isFound = true;

                                accountDTO.accountID = reader.GetInt32(reader.GetOrdinal("AccountID"));
                                accountDTO.userID = reader.GetInt32(reader.GetOrdinal("UserID"));
                                accountDTO.accountTypeID = reader.GetInt32(reader.GetOrdinal("AccountTypeID"));
                                accountDTO.accountNumber = reader.GetString(reader.GetOrdinal("AccountNumber"));
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

        public static bool getAccountByAccountID(int accountID , clsAccountDTO accountDTO)
        {
            bool isFound = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.getConnectionString()))
                {
                    connection.Open();

                    string cmd = "SP_GetAccountByAccountID";
                    using (SqlCommand command = new SqlCommand(cmd, connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue(@"AccountID", accountID);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                isFound = true;

                                accountDTO.accountID = accountID;
                                accountDTO.accountID = reader.GetInt32(reader.GetOrdinal("AccountID"));
                                accountDTO.userID = reader.GetInt32(reader.GetOrdinal("UserID"));
                                accountDTO.accountTypeID = reader.GetInt32(reader.GetOrdinal("AccountTypeID"));
                                accountDTO.accountNumber = reader.GetString(reader.GetOrdinal("AccountNumber"));
                            }
                        }
                    }
                }



            }
            catch (Exception exception)
            {
                Console.WriteLine($"DEBUG: {exception.Message}");
            }



            return isFound;
        }

        public static int addNewAccount(clsAccountDTO accountDTO)
        {
            int newAccountID = -1;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.getConnectionString()))
                {
                    connection.Open();

                    string cmd = "SP_AddNewAccount";
                    using(SqlCommand command = new SqlCommand(cmd,connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue(@"UserID",accountDTO.userID);
                        command.Parameters.AddWithValue(@"AccountTypeID",accountDTO.accountTypeID);
                        command.Parameters.AddWithValue(@"AccountNumber",accountDTO.accountNumber);


                        object result = command.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int newID))
                        {
                            newAccountID = newID;
                        }

                    }
                }



            } catch(Exception exception)
            {
                Console.WriteLine($"DEBUG: {exception.Message}");
            }

            return newAccountID;
        }

    }
}

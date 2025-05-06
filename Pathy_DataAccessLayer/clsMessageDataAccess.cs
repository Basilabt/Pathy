using Microsoft.Data.SqlClient;
using Pathy_DataAccessLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathy_DataAccessLayer
{
    public class clsMessageDataAccess
    {
        public static bool getMessageByMessageID(clsMessageDTO messageDTO)
        {
            bool isFound = false;

            try
            {
                using(SqlConnection connection = new SqlConnection(clsDataAccessSettings.getConnectionString()))
                {
                    connection.Open();

                    string cmd = "SP_GetMessageByMessageID";
                    using(SqlCommand command = new SqlCommand(cmd,connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue(@"MessageID",messageDTO.messageID);

                        using(SqlDataReader reader = command.ExecuteReader())
                        {
                            if(reader.Read())
                            {
                                isFound = true;

                                messageDTO.message = reader.GetString(reader.GetOrdinal("Message"));
                                messageDTO.senderAccountID = reader.GetInt32(reader.GetOrdinal("SenderAccountID"));
                                messageDTO.recieverAccountID = reader.GetInt32(reader.GetOrdinal("RecieverAccountID"));
                               
                            }

                        }

                    }

                }



            } catch (Exception exception)
            {
                Console.WriteLine($"DEBUG: {exception.Message}");
            }



            return isFound;
        }

        public static int addNewMessage(clsMessageDTO messageDTO)
        {
            int newMessageID = -1;


            try
            {
                using(SqlConnection connection = new SqlConnection((clsDataAccessSettings.getConnectionString())))
                {
                    connection.Open();

                    string cmd = "SP_AddNewMessage";
                    using (SqlCommand command = new SqlCommand(cmd, connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;

                        command.Parameters.AddWithValue(@"SenderAccountID",messageDTO.senderAccountID);
                        command.Parameters.AddWithValue(@"RecieverAccountID", messageDTO.recieverAccountID);
                        command.Parameters.AddWithValue(@"Message",messageDTO.message);

                        object result = command.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int id))
                        {
                            newMessageID = id;
                        }

                    }

                }

            } catch(Exception exception)
            {
                Console.WriteLine($"DEBUG: {exception.Message}");
            }



            return newMessageID;
        }

        public static bool deleteMessageByMessageID(clsMessageDTO messageDTO)
        {
            int numberOfAffectedRows = 0;

            try
            {
                using(SqlConnection connection = new SqlConnection(clsDataAccessSettings.getConnectionString()))
                {
                    connection.Open();

                    string cmd = "SP_DeleteMessageByMessageID";
                    using(SqlCommand command = new SqlCommand(cmd,connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue(@"MessageID",messageDTO.messageID);

                        numberOfAffectedRows = command.ExecuteNonQuery();

                    }

                }


            } catch(Exception exception)
            {
                Console.WriteLine($"DEBUG: {exception.Message}");
            }



            return numberOfAffectedRows >= 1;
        }

        public static bool updateMessage(clsMessageDTO messageDTO)
        {
            int numberOfAffectedRows = 0;

            try
            {
                using(SqlConnection connection = new SqlConnection(clsDataAccessSettings.getConnectionString()))
                {
                    connection.Open();

                    string cmd = "SP_UpdateMessage";
                    using (SqlCommand command = new SqlCommand(cmd, connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue(@"MessageID",messageDTO.messageID);
                        command.Parameters.AddWithValue(@"Message",messageDTO.message);

                        numberOfAffectedRows = command.ExecuteNonQuery();

                    }

                }


            } catch(Exception exception)
            {
                Console.WriteLine($"DEBUG: {exception.Message}");
            }


            return numberOfAffectedRows >= 1;
        }

        public static List<clsUserDTO> getUsersToChatWith(clsUserDTO userDTO)
        {
            List<clsUserDTO> users  = new List<clsUserDTO>();

            try
            {
                using(SqlConnection connection = new SqlConnection(clsDataAccessSettings.getConnectionString()))
                {
                    connection.Open();

                    string cmd = "SP_GetUsersToChatWith";
                    using(SqlCommand command = new SqlCommand(cmd,connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue(@"UserID",userDTO.userID);

                        using(SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                clsUserDTO dto = new clsUserDTO 
                                {
                                    userID = reader.GetInt32(reader.GetOrdinal("UserID")) ,
                                    personID = reader.GetInt32(reader.GetOrdinal("PersonID")) ,
                                    username = reader.GetString(reader.GetOrdinal("Username")) ,
                                    password = reader.GetString (reader.GetOrdinal("Password")) ,
                                    photoURL = reader.GetString(reader.GetOrdinal("PhotoURL"))
                                };

                                users.Add(dto);
                            }

                        }

                    }

                }

            } catch(Exception exception)
            {
                Console.WriteLine($"DEBUG: {exception.Message}");
            }


            return users;
        }

        public static List<clsMessageDTO> getMessagesSentFromTo(clsMessageDTO messageDTO)
        {
            List<clsMessageDTO > messages = new List<clsMessageDTO>();

            try
            {
                using(SqlConnection connection = new SqlConnection(clsDataAccessSettings.getConnectionString()))
                {
                    connection.Open();

                    string cmd = "SP_GetMessagesSentFromUserToUser";
                    using (SqlCommand command = new SqlCommand(cmd, connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue(@"SenderAccountID",messageDTO.senderAccountID);
                        command.Parameters.AddWithValue(@"RecieverAccountID",messageDTO.recieverAccountID);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                clsMessageDTO dto = new clsMessageDTO
                                {
                                    messageID = reader.GetInt32(reader.GetOrdinal("MessageID")),
                                    senderAccountID = reader.GetInt32(reader.GetOrdinal("SenderAccountID")) ,
                                    recieverAccountID = reader.GetInt32(reader.GetOrdinal("RecieverAccountID")),
                                    message = reader.GetString(reader.GetOrdinal("Message"))

                                };

                                messages.Add(dto);
                            }
                        }

                    }
                }




            } catch(Exception exception)
            {
                Console.WriteLine($"DEBUG: {exception.Message}");
            }

            return messages;
        }

    }
}

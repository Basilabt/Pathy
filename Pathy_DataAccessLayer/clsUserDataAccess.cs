using Microsoft.Data.SqlClient;
using Pathy_DataAccessLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathy_DataAccessLayer
{
    public class clsUserDataAccess
    {
        public static bool getUserByUserID(int userID , clsUserDTO userDTO)
        {
            bool isFound = false;

            try
            {
                using(SqlConnection connection = new SqlConnection(clsDataAccessSettings.getConnectionString()))
                {
                    connection.Open();

                    string cmd = "SP_GetUserByUserID";
                    using(SqlCommand command = new SqlCommand(cmd, connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue(@"UserID",userID);
                        
                        using(SqlDataReader reader = command.ExecuteReader())
                        {
                            
                            
                            while (reader.Read())
                            {
                                isFound = true;

                                userDTO.userID = reader.GetInt32(reader.GetOrdinal("UserID"));
                                userDTO.personID = reader.GetInt32(reader.GetOrdinal("PersonID"));
                                userDTO.username = reader.GetString(reader.GetOrdinal("Username"));
                                userDTO.password = reader.GetString(reader.GetOrdinal("Password"));
                                userDTO.photoURL = reader.GetString(reader.GetOrdinal("PhotoURL"));
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

        public static bool doesUserExistByUsername(string username)
        {
            bool isFound = false;

            try
            {
                using(SqlConnection connection = new SqlConnection(clsDataAccessSettings.getConnectionString()))
                {
                    connection.Open();

                    string cmd = "SP_DoesUserExistByUsername";
                    using(SqlCommand command = new SqlCommand(cmd,connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue(@"Username",username);

                        using(SqlDataReader reader = command.ExecuteReader())
                        {
                            if(reader.HasRows)
                            {
                                isFound = true;
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

        public static bool loginByUsernameAndPassword(string username , string password , clsUserDTO userDTO)
        {
            bool isLoggedIn = false;

            try
            {
                using(SqlConnection connection = new SqlConnection(clsDataAccessSettings.getConnectionString()))
                {
                    connection.Open();

                    string cmd = "SP_LoginByUsernameAndPassword";
                    using(SqlCommand command = new SqlCommand(cmd,connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue(@"Username",username);
                        command.Parameters.AddWithValue(@"Password",password);

                        using(SqlDataReader reader = command.ExecuteReader())
                        {
                            if(reader.Read())
                            {
                                isLoggedIn = true;

                                userDTO.userID = reader.GetInt32(reader.GetOrdinal("UserID"));
                                userDTO.personID = reader.GetInt32(reader.GetOrdinal("PersonID"));
                                userDTO.username = reader.GetString(reader.GetOrdinal("Username"));
                                userDTO.password = reader.GetString(reader.GetOrdinal("Password"));
                                userDTO.photoURL = reader.GetString(reader.GetOrdinal("PhotoURL"));
                            }
                        }
                    }
                }
             
            } catch(Exception exception)
            {
                Console.WriteLine($"DEBUG: {exception.Message}");
            }

            return isLoggedIn;
        }

        public static int addNewUser(clsUserDTO userDTO)
        {
            int newUserID = -1;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.getConnectionString()))
                {
                    connection.Open();

                    string cmd = "SP_AddNewUser";
                    using (SqlCommand command = new SqlCommand(cmd,connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue(@"PersonID",userDTO.personID);
                        command.Parameters.AddWithValue(@"Username", userDTO.username);
                        command.Parameters.AddWithValue(@"Password", userDTO.password);
                        command.Parameters.AddWithValue(@"PhotoURL", userDTO.photoURL);


                        object result = command.ExecuteScalar();    

                        if(result != null && int.TryParse(result.ToString(),out int newID))
                        {
                            newUserID = newID;
                        }

                    }
                }



            } catch(Exception exception)
            {
                Console.WriteLine($"DEBUG: {exception.Message}");
            }




            return newUserID;
        }

        public static int getNumberOfUserFollowersByUserID(int userID)
        {
            int result = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.getConnectionString()))
                {
                    connection.Open();

                    string cmd = "SP_GetNumberOfUserFollowersByUserID";
                    using(SqlCommand command = new SqlCommand(cmd,connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue(@"UserID",userID);

                        using(SqlDataReader reader = command.ExecuteReader())
                        {
                            while(reader.Read())
                            {
                                result = reader.GetInt32(reader.GetOrdinal("NumberOfFollowers"));
                            }
                        }
                    }
                }

            } catch (Exception exception)
            {
                Console.WriteLine($"DEBUG: {exception.Message}");
            }

            return result;
        }

        public static int getNumberOfUserFollowingsByUserID(int userID)
        {
            int result = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.getConnectionString()))
                {
                    connection.Open();

                    string cmd = "SP_GetNumberOfUserFollowingsByUserID";
                    using (SqlCommand command = new SqlCommand(cmd, connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue(@"UserID", userID);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                result = reader.GetInt32(reader.GetOrdinal("NumberOfFollowings"));
                            }
                        }
                    }
                }

            }
            catch (Exception exception)
            {
                Console.WriteLine($"DEBUG: {exception.Message}");
            }

            return result;
        }

        public static bool updateUserByUserID(clsUserDTO userDTO)
        {
            int numberOfAffectedRows = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.getConnectionString()))
                {
                    connection.Open();

                    string cmd = "SP_UpdateUserByUserID";
                    using (SqlCommand command = new SqlCommand(cmd, connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue(@"UserID",userDTO.userID);
                        command.Parameters.AddWithValue(@"PersonID", userDTO.personID);
                        command.Parameters.AddWithValue(@"Username", userDTO.username);
                        command.Parameters.AddWithValue(@"Password",userDTO.password);
                        command.Parameters.AddWithValue(@"PhotoURL",(userDTO.photoURL == "") ? DBNull.Value : userDTO.photoURL);

                        numberOfAffectedRows = command.ExecuteNonQuery();

                    }
                }

            } catch (Exception exception)
            {
                Console.WriteLine($"DEBUG: {exception.Message}");
            }





            return numberOfAffectedRows >= 1;
        }

        public static List<clsUserDTO> getSuggestedUsersToFollowDTOList(int userID , int numberOfUsers)
        {

            List<clsUserDTO> usersList = new List<clsUserDTO>();

            try
            {
                using(SqlConnection connection = new SqlConnection(clsDataAccessSettings.getConnectionString()))
                {
                    connection.Open();

                    string cmd = "SP_GetSuggestedUsersToFollow";
                    using(SqlCommand command = new SqlCommand(cmd,connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue(@"UserID",userID);
                        command.Parameters.AddWithValue(@"NumberOfUsers",numberOfUsers);


                        using(SqlDataReader reader = command.ExecuteReader())
                        {
                            while(reader.Read())
                            {
                                clsUserDTO user = new clsUserDTO 
                                {
                                    userID = reader.GetInt32(reader.GetOrdinal("UserID")),
                                    personID = reader.GetInt32(reader.GetOrdinal("PersonID")),
                                    username = reader.GetString(reader.GetOrdinal("Username")) ,
                                    password = reader.GetString(reader.GetOrdinal("Password")) ,
                                    photoURL = reader.GetString(reader.GetOrdinal("PhotoURL"))
                                };

                                usersList.Add(user);
                            }
                        }
                    }
                }

            } catch(Exception exception)
            {
                Console.WriteLine($"DEBUG: {exception.Message}");
            }


            return usersList;
        }

        public static List<clsUserDTO> getUserFollowers(int userID)
        {
            List<clsUserDTO> list = new List<clsUserDTO>();

            try
            {
                using(SqlConnection connection = new SqlConnection(clsDataAccessSettings.getConnectionString()))
                {
                    connection.Open();

                    string cmd = "SP_GetUserFollowers";
                    using (SqlCommand command = new SqlCommand(cmd, connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue(@"UserID",userID);

                        using(SqlDataReader reader = command.ExecuteReader())
                        {                            
                            while(reader.Read())
                            {
                                clsUserDTO user = new clsUserDTO
                                {
                                    userID = reader.GetInt32(reader.GetOrdinal("UserID")) , 
                                    personID = reader.GetInt32(reader.GetOrdinal("PersonID")) ,
                                    username = reader.GetString(reader.GetOrdinal("Username")) , 
                                    password = reader.GetString(reader.GetOrdinal("Password")) ,
                                    photoURL = reader.GetString(reader.GetOrdinal("PhotoURL"))
                                };

                                list.Add(user);
                            }
                        }
                    }
                }

            } catch(Exception exception)
            {
                Console.WriteLine($"DEBUG: {exception.Message}");
            }

            return list;
        }

        public static List<clsUserDTO> getUserFollowings(int userID)
        {
            List<clsUserDTO> list = new List<clsUserDTO>();

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.getConnectionString()))
                {
                    connection.Open();

                    string cmd = "SP_GetUserFollowings";
                    using (SqlCommand command = new SqlCommand(cmd, connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue(@"UserID", userID);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                clsUserDTO user = new clsUserDTO
                                {
                                    userID = reader.GetInt32(reader.GetOrdinal("UserID")),
                                    personID = reader.GetInt32(reader.GetOrdinal("PersonID")),
                                    username = reader.GetString(reader.GetOrdinal("Username")),
                                    password = reader.GetString(reader.GetOrdinal("Password")),
                                    photoURL = reader.GetString(reader.GetOrdinal("PhotoURL"))
                                };

                                list.Add(user);
                            }
                        }
                    }
                }

            }
            catch (Exception exception)
            {
                Console.WriteLine($"DEBUG: {exception.Message}");
            }

            return list;
        }

        public static bool doesUserExistByUserID(int userID)
        {
            bool isFound = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.getConnectionString()))
                {
                    connection.Open();

                    string cmd = "SP_DoesUserExistByUserID";
                    using (SqlCommand command = new SqlCommand(cmd,connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue(@"UserID",userID);

                        using(SqlDataReader reader = command.ExecuteReader())
                        {
                            if(reader.Read())
                            {
                                isFound = true;
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


    }
}

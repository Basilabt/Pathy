using Microsoft.Data.SqlClient;
using Pathy_DataAccessLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathy_DataAccessLayer
{
    public class clsLikeDataAccess
    {
        public static int getNumberOfPostLikes(int postID)
        {
            int numberOfLikes = 0;

            try
            {
                using(SqlConnection connection = new SqlConnection(clsDataAccessSettings.getConnectionString()))
                {
                    connection.Open();

                    string cmd = "SP_GetNumberOfPostLikes";
                    using (SqlCommand command = new SqlCommand(cmd,connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue(@"PostID",postID);

                        using(SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                numberOfLikes = reader.GetInt32(reader.GetOrdinal("NumberOfPostLikes"));
                            }
                            
                        }

                    }
                }
                 
            } catch(Exception exception)
            {
                Console.WriteLine($"DEBUG: {exception.Message}");
            }




            return numberOfLikes;
        }

        public static int addNewLike(clsLikeDTO likeDTO)
        {
            int newLikeID = -1;

            try
            {
                using(SqlConnection connection = new SqlConnection(clsDataAccessSettings.getConnectionString()))
                {
                    connection.Open();

                    string cmd = "SP_AddNewLike";
                    using(SqlCommand command = new SqlCommand(cmd,connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue(@"PostID",likeDTO.postID);
                        command.Parameters.AddWithValue(@"LikerUserID",likeDTO.likerUserID);


                        object result = command.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int id))
                        {
                            newLikeID = id;
                        }

                    }
                }


            } catch(Exception exception)
            {
                Console.WriteLine($"DEBUG: {exception.Message}");
            }


            return newLikeID;
        }

        public static bool deleteUserLikeByLikeID(clsLikeDTO likeDTO)
        {
            int numberOfAffectedRows = 0;

            try
            {
                using(SqlConnection connection = new SqlConnection(clsDataAccessSettings.getConnectionString()))
                {
                    connection.Open();

                    string cmd = "SP_DeleteUserLikeByLikeID";
                    using(SqlCommand command = new SqlCommand(cmd,connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue(@"LikeID",likeDTO.likeID);

                        numberOfAffectedRows = command.ExecuteNonQuery();

                    }
                }

            } catch(Exception exception)
            {
                Console.WriteLine($"DEBUG: {exception.Message}");
            }

            return numberOfAffectedRows >= 1;
        }

        public static bool deleteUserLikeByUserIDAndPostID(clsLikeDTO likeDTO)
        {
            int numberOfAffectedRows = 0;

            try
            {

                using(SqlConnection connection = new SqlConnection(clsDataAccessSettings.getConnectionString()))
                {
                    connection.Open();

                    string cmd = "SP_DeletePostLikeByPostIDAndUserID";
                    using(SqlCommand command = new SqlCommand(cmd,connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue(@"UserID",likeDTO.likerUserID);
                        command.Parameters.AddWithValue(@"PostID",likeDTO.postID);

                        numberOfAffectedRows = command.ExecuteNonQuery();
                    }

                }



            } catch(Exception exception)
            {
                Console.WriteLine($"DEBUG: {exception.Message}");
            }

            return numberOfAffectedRows >= 1;
        }

        public static bool findLikeByPostIDAndUserLikerID(clsLikeDTO likeDTO)
        {
            bool isFound = false;

            try
            {
                using(SqlConnection connection = new SqlConnection(clsDataAccessSettings.getConnectionString()))
                {
                    connection.Open();

                    string cmd = "SP_FindLikeByPostIDAndLikerUserID";
                    using(SqlCommand command = new SqlCommand(cmd,connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue(@"PostID",likeDTO.postID);
                        command.Parameters.AddWithValue(@"LikerUserID",likeDTO.likerUserID);
                            
                        using(SqlDataReader reader = command.ExecuteReader())
                        {
                            
                            if(reader.Read())
                            {
                                isFound = true;
                                likeDTO.likeID = reader.GetInt32(reader.GetOrdinal("LikeID"));                                
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

        public static List<clsUserDTO>  getUsersWhoLikePostByPostIDAsDTOList(clsLikeDTO likeDTO)
        {
            List<clsUserDTO> users = new List<clsUserDTO>();

            try
            {
                using(SqlConnection connection = new SqlConnection(clsDataAccessSettings.getConnectionString()))
                {
                    connection.Open();

                    string cmd = "SP_GetUsersWhoLikePostByPostID";
                    using (SqlCommand command = new SqlCommand(cmd, connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue(@"PostID",likeDTO.postID);

                        using(SqlDataReader reader = command.ExecuteReader())
                        {
                            while(reader.Read())
                            {                               
                                clsUserDTO userDTO = new clsUserDTO 
                                { 
                                   userID   = reader.GetInt32(reader.GetOrdinal("UserID")),
                                   personID = reader.GetInt32(reader.GetOrdinal("PersonID")) ,
                                   username = reader.GetString(reader.GetOrdinal("Username")),
                                   password = reader.GetString (reader.GetOrdinal("Password")) ,
                                   photoURL = reader.GetString(reader.GetOrdinal("PhotoURL"))  
                                };

                                users.Add(userDTO);
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

    }
}

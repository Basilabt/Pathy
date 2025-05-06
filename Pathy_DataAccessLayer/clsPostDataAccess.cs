using Microsoft.Data.SqlClient;
using Pathy_DataAccessLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathy_DataAccessLayer
{
    public class clsPostDataAccess
    {
        public static int addNewPost(clsPostDTO postDTO)
        {
            int newPostID = -1;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.getConnectionString()))
                {
                    connection.Open();

                    string cmd = "SP_AddNewPost";
                    using(SqlCommand command = new SqlCommand(cmd, connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;

                        command.Parameters.AddWithValue(@"AccountID",postDTO.accountID);
                        command.Parameters.AddWithValue(@"CreationDateTime",postDTO.creationDateTime);
                        command.Parameters.AddWithValue(@"TextContent",postDTO.textContent);
                        command.Parameters.AddWithValue(@"ImageURL",postDTO.imageURL);
                        command.Parameters.AddWithValue(@"VideoURL",postDTO.videoURL);

                        object result = command.ExecuteScalar();

                        if(result != null && int.TryParse(result.ToString(),out int id))
                        {
                            newPostID = id;
                        }

                    }
                }


            } catch(Exception excception)
            {
                Console.WriteLine($"DEBUG: {excception.Message}");
            }

            return newPostID;
        }

        public static List<clsPostDTO> getAllPosts(int userID)
        {
            List<clsPostDTO> posts = new List<clsPostDTO>();

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.getConnectionString()))
                {
                    connection.Open();

                    string cmd = "SP_GetAllPosts";
                    using(SqlCommand command = new SqlCommand(cmd,connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue(@"UserID",userID);
                        
                        using(SqlDataReader reader = command.ExecuteReader())
                        {
                            while(reader.Read())
                            {
                                clsPostDTO postDTO = new clsPostDTO
                                {
                                    postID = reader.GetInt32(reader.GetOrdinal("PostID")) , 
                                    accountID = reader.GetInt32(reader.GetOrdinal("AccountID")) ,
                                    creationDateTime = reader.GetDateTime(reader.GetOrdinal("CreationDateTime")) ,
                                    textContent = reader.GetString(reader.GetOrdinal("TextContent")) ,
                                    imageURL = reader.GetString(reader.GetOrdinal("ImageURL")) , 
                                    videoURL = reader.GetString(reader.GetOrdinal("VideoURL"))

                                };

                                posts.Add(postDTO);
                            }
                        }
                    }
                }

            } catch (Exception exception)
            {
                Console.WriteLine($"DEBUG: {exception.Message}");
            }

            return posts;
        }

        public static List<clsPostDTO> getPostsByUserID(int userID)
        {
            List<clsPostDTO> posts = new List<clsPostDTO>();

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.getConnectionString()))
                {
                    connection.Open();

                    string cmd = "SP_GetPostsByUserID";
                    using (SqlCommand command = new SqlCommand(cmd, connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue(@"UserID",userID);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                clsPostDTO postDTO = new clsPostDTO
                                {
                                    postID = reader.GetInt32(reader.GetOrdinal("PostID")),
                                    accountID = reader.GetInt32(reader.GetOrdinal("AccountID")),
                                    creationDateTime = reader.GetDateTime(reader.GetOrdinal("CreationDateTime")),
                                    textContent = reader.GetString(reader.GetOrdinal("TextContent")),
                                    imageURL = reader.GetString(reader.GetOrdinal("ImageURL")),
                                    videoURL = reader.GetString(reader.GetOrdinal("VideoURL"))

                                };

                                posts.Add(postDTO);
                            }
                        }
                    }
                }

            }
            catch (Exception exception)
            {
                Console.WriteLine($"DEBUG: {exception.Message}");
            }

            return posts;
        }

        public static bool getPostByPostID(int postID , clsPostDTO postDTO)
        {
            bool isFound = false;
           
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.getConnectionString()))
                {
                    connection.Open();

                    Console.WriteLine($"DEBUG: --> PostID = {postID}");

                    string cmd = "SP_GetPostByPostID";
                    using (SqlCommand command = new SqlCommand(cmd, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue(@"PostID",postID);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            
                            if (reader.Read())
                            {
                                
                                isFound = true;

                                postDTO.postID = reader.GetInt32(reader.GetOrdinal("PostID"));
                                postDTO.accountID = reader.GetInt32(reader.GetOrdinal("AccountID"));
                                postDTO.creationDateTime = reader.GetDateTime(reader.GetOrdinal("CreationDateTime"));
                                postDTO.textContent = reader.GetString(reader.GetOrdinal("TextContent"));
                                postDTO.imageURL = reader.GetString(reader.GetOrdinal("ImageURL"));
                                postDTO.videoURL = reader.GetString(reader.GetOrdinal("VideoURL"));
                               

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

        public static bool deletePostByPostID(int postID)
        {
            int numberOfAffectedRows = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.getConnectionString()))
                {
                    connection.Open();

                    string cmd = "SP_DeletePostByPostID";
                    using(SqlCommand command = new SqlCommand(cmd,connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue(@"PostID",postID);

                        numberOfAffectedRows = command.ExecuteNonQuery();

                    }
                }

            } catch (Exception exception)
            {
                Console.WriteLine($"DEBUG: {exception.Message}");
            }

            return numberOfAffectedRows >= 1;
        }

        public static bool updatePostByPostID(clsPostDTO postDTO)
        {
            int numberOfAffectedRows = 0;

            try
            {
                using(SqlConnection connection = new SqlConnection(clsDataAccessSettings.getConnectionString()))
                {
                    connection.Open();

                    string cmd = "SP_UpdatePostByPostID";
                    using(SqlCommand command = new SqlCommand (cmd,connection))
                    {
                       
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue(@"PostID",postDTO.postID);
                        command.Parameters.AddWithValue(@"AccountID",postDTO.accountID);
                        command.Parameters.AddWithValue(@"CreationDateTime",postDTO.creationDateTime);
                        command.Parameters.AddWithValue(@"TextContent",postDTO.textContent);
                        command.Parameters.AddWithValue(@"ImageURL", string.IsNullOrEmpty(postDTO.imageURL) ? (object)DBNull.Value : postDTO.imageURL);
                        command.Parameters.AddWithValue(@"VideoURL", string.IsNullOrEmpty(postDTO.videoURL) ? (object)DBNull.Value : postDTO.videoURL);

                        numberOfAffectedRows = command.ExecuteNonQuery();

                    }

                }

            } catch (Exception exception)
            {
                Console.WriteLine($"DEBUG: {exception.Message}");
            }

            return numberOfAffectedRows >= 1;
        }

        public static bool isPostLikedByUser(int postID , int likerUserID)
        {
            bool isPostLiked = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.getConnectionString()))
                {
                    connection.Open();

                    string cmd = "SP_IsPostLikedByUser";
                    using(SqlCommand command = new SqlCommand(cmd, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue(@"PostID",postID);
                        command.Parameters.AddWithValue(@"LikerUserID",likerUserID);
                        
                        using(SqlDataReader reader = command.ExecuteReader())
                        {
                            if(reader.HasRows)
                            {
                                isPostLiked = true;   
                            }
                        }
                    }
                }

            } catch(Exception exception)
            {
                Console.WriteLine($"DEBUG: {exception.Message}");
            }


            return isPostLiked;
        }

    }
}

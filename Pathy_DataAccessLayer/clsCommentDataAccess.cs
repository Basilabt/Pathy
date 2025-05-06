using Microsoft.Data.SqlClient;
using Pathy_DataAccessLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathy_DataAccessLayer
{
    public class clsCommentDataAccess
    {
        public static List<clsCommentDTO> getPostCommentsAsDTOList(clsCommentDTO commentDTO)
        {
            List<clsCommentDTO> list = new List<clsCommentDTO>();

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.getConnectionString()))
                {
                    connection.Open();

                    string cmd = "SP_GetPostCommentsByPostID";
                    using (SqlCommand command = new SqlCommand(cmd, connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue(@"PostID",commentDTO.postID);

                        using(SqlDataReader reader = command.ExecuteReader())
                        {
                            while(reader.Read())
                            {
                                clsCommentDTO comment = new clsCommentDTO
                                {
                                    commentID = reader.GetInt32(reader.GetOrdinal("CommentID")) , 
                                    postID = reader.GetInt32(reader.GetOrdinal("PostID")) ,
                                    commenterUserID = reader.GetInt32(reader.GetOrdinal("CommenterUserID")),
                                    commentText = reader.GetString(reader.GetOrdinal("CommentText"))
                                };

                                list.Add(comment);
                            }


                        }
                    }

                }


            } catch (Exception exception)
            {
                Console.WriteLine($"DEBUG: {exception.Message}");
            }


            return list;
        }

        public static bool getCommentByCommentID(clsCommentDTO commentDTO)
        {
            bool isFound = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.getConnectionString()))
                {
                    connection.Open();

                    string cmd = "SP_GetCommentByCommentID";
                    using(SqlCommand command = new SqlCommand(cmd,connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue(@"CommentID",commentDTO.commentID);

                        using(SqlDataReader reader = command.ExecuteReader())
                        {
                            if(reader.Read())
                            {
                                isFound = true;

                                commentDTO.postID = reader.GetInt32(reader.GetOrdinal("PostID"));
                                commentDTO.commenterUserID = reader.GetInt32(reader.GetOrdinal("CommenterUserID"));
                                commentDTO.commentText = reader.GetString(reader.GetOrdinal("CommentText"));                               
                            }
                        }
                    }
                }



            } catch (Exception exception)
            {
                Console.WriteLine($"DEBUG {exception.Message}");
            }

            return isFound;
        }

        public static int getNumberOfPostComments(int postID)
        {
            int numberOfComments = 0;

            try
            {
                using(SqlConnection connection = new SqlConnection(clsDataAccessSettings.getConnectionString()))
                {
                    connection.Open();

                    string cmd = "SP_GetNumberOfPostComments";
                    using(SqlCommand command = new SqlCommand(cmd, connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue(@"PostID",postID);

                        using(SqlDataReader reader = command.ExecuteReader())
                        {
                            if(reader.Read())
                            {
                                numberOfComments = reader.GetInt32(reader.GetOrdinal("NumberOfPostComments"));
                            }

                        }
                    }
                }

            } catch(Exception exception)
            {
                Console.WriteLine($"DEBUG: {exception.Message}");
            }



            return numberOfComments;
        }

        public static int addNewComment(clsCommentDTO commentDTO)
        {
            int newCommentID = -1;

            try
            {
                using(SqlConnection connection = new SqlConnection(clsDataAccessSettings.getConnectionString()))
                {
                    connection.Open();

                    string cmd = "SP_AddNewComment";
                    using (SqlCommand command = new SqlCommand(cmd,connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue(@"PostID",commentDTO.postID);
                        command.Parameters.AddWithValue(@"CommenterUserID",commentDTO.commenterUserID);
                        command.Parameters.AddWithValue(@"CommentText",commentDTO.commentText);

                        object result = command.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int id))
                        {
                            newCommentID = id;
                        }
                    }
                }

            } catch(Exception exception)
            {
                Console.WriteLine($"DEBUG: {exception.Message}");
            }



            return newCommentID;
        }

        public static bool deleteCommentByCommentID(clsCommentDTO commentDTO)
        {
            int numberOfAffectedRows = 0;

            try
            {
                using(SqlConnection connection = new SqlConnection(clsDataAccessSettings.getConnectionString()))
                {
                    connection.Open();


                    string cmd = "SP_DeleteCommentByCommentID";
                    using (SqlCommand command = new SqlCommand(cmd,connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue(@"CommentID",commentDTO.commentID);    
                        
                        numberOfAffectedRows = command.ExecuteNonQuery();
                    }
                    
                }

            } catch(Exception exception)
            {
                Console.WriteLine($"DEBUG: {exception.Message}");
            }


            return numberOfAffectedRows >= 1;
        }
        
        public static bool updateComment(clsCommentDTO commentDTO)
        {
            int numberOfAffectedRows = 0;

            try
            {
                using(SqlConnection connection = new SqlConnection(clsDataAccessSettings.getConnectionString()))
                {
                    connection.Open();

                    string cmd = "SP_UpdateComment";
                    using(SqlCommand command = new SqlCommand(cmd, connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue(@"CommentID",commentDTO.commentID);
                        command.Parameters.AddWithValue(@"PostID",commentDTO.postID);
                        command.Parameters.AddWithValue(@"CommenterUserID",commentDTO.commenterUserID);
                        command.Parameters.AddWithValue("@CommentText",commentDTO.commentText);

                        numberOfAffectedRows = command.ExecuteNonQuery();
                    }
                }



            } catch(Exception exception)
            {
                Console.WriteLine($"DEBUG: {exception.Message}");
            }

            return numberOfAffectedRows >= 1;
        }
        
    }
}

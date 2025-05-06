using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathy_DataAccessLayer
{
    public class clsFollowersDataAccess
    {
        public static bool followUser(int followerUserID , int followedUserID)
        {
            int newFollowingID = -1;

            try
            {
                using(SqlConnection connection = new SqlConnection(clsDataAccessSettings.getConnectionString()))
                {
                    connection.Open();

                    string cmd = "SP_AddNewFollowingRelation";
                    using (SqlCommand command = new SqlCommand(cmd, connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue(@"FollowerUserID",followerUserID);
                        command.Parameters.AddWithValue(@"FollowedUserID", followedUserID);

                        object result = command.ExecuteScalar();

                        if(result != null && int.TryParse(result.ToString() , out int id))
                        {
                            newFollowingID = id;
                        }

                    }
                }


            } catch(Exception exception)
            {
                Console.WriteLine($"DEBUG: {exception.Message}");    
            }

            return newFollowingID >= 1;
        }

        public static bool unfollowUser(int followerUserID , int followedUserID)
        {
            int numberOfAffectedRows = 0;

            try
            {
                using(SqlConnection connection = new SqlConnection(clsDataAccessSettings.getConnectionString()))
                {
                    connection.Open();


                    string cmd = "SP_RemoveUserFollowing";
                    using(SqlCommand command = new SqlCommand(cmd,connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue(@"FollowerUserID",followerUserID);
                        command.Parameters.AddWithValue(@"FollowedUserID",followedUserID);

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

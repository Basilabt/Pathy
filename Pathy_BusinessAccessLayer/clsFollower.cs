using Azure.Core;
using Pathy_DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathy_BusinessAccessLayer
{
    public class clsFollowers
    {
        public static bool followUser(int followerUserID , int followedUserID)
        {
            return clsFollowersDataAccess.followUser(followerUserID, followedUserID);
        }

        public static bool unfollowUser(int followerUserID , int followedUserID)
        {
            return clsFollowersDataAccess.unfollowUser(followerUserID, followedUserID);
        }
    }
}

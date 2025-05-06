using Pathy_DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Pathy_DataAccessLayer;
using Pathy_DataAccessLayer.DTOs;

namespace Pathy_BusinessAccessLayer
{
    public class clsLike
    {
        public enum enMode
        {
            Addnew = 1 , Update = 2 , Delete = 3
        }

        public int likeID { get; set; }
        public int postID { get; set; }
        public int likerUserID { get; set; }
        public enMode mode { get; set; }
        public clsPost post {  get; set; }
        public clsUser likerUser { get; set; }

        public clsLike()
        {
            this.likeID = -1;
            this.postID = -1;
            this.likerUserID = -1;
            this.mode = enMode.Addnew;
        }

        private clsLike(int likeID , int postID , int likerUserID)
        {
            this.likeID = likeID;   
            this.postID = postID;
            this.likerUserID = likerUserID;
            this.mode = enMode.Update;
            this.post = clsPost.getPostByPostID(postID);
            this.likerUser = clsUser.getUserByUserID(likerUserID);
        }

        public bool save()
        {
            switch(this.mode)
            {
                case enMode.Addnew:
                    {
                        this.likeID = addNewLike(new clsLikeDTO {likeID=-1,postID = this.postID , likerUserID = this.likerUserID});
                        return this.likeID != -1;
                    }

                case enMode.Update:
                    {
                        return false;
                    }

                case enMode.Delete:
                    {
                        return deleteUserLikeByLikeID(new clsLikeDTO {likeID = this.likeID,postID=this.postID,likerUserID=this.likerUserID});
                    }

            }

            return false;
        }

        // Static Methods

        public static int getNumberOfPostLikes(int postID)
        {
            return clsLikeDataAccess.getNumberOfPostLikes(postID);
        }

        public static int addNewLike(clsLikeDTO likeDTO)
        {
            return clsLikeDataAccess.addNewLike(likeDTO);
        }

        public static bool deleteUserLikeByLikeID(clsLikeDTO likeDTO)
        {
            return clsLikeDataAccess.deleteUserLikeByLikeID(likeDTO);
        }

        public static bool deleteUserLikeByUserIDAndPostID(clsLikeDTO likeDTO)
        {
            return clsLikeDataAccess.deleteUserLikeByUserIDAndPostID(likeDTO);
        }

        public static clsLike findLikeByPostIDAndLikerUserID(int postID , int likerUserID)
        {
            clsLikeDTO likeDTO = new clsLikeDTO();
            likeDTO.postID = postID;
            likeDTO.likerUserID = likerUserID;

            if(clsLikeDataAccess.findLikeByPostIDAndUserLikerID(likeDTO))
            {
                return new clsLike {likeID=likeDTO.likeID,postID=likeDTO.postID,likerUserID=likeDTO.likerUserID};
            }

            return null;
        }

        public static List<clsUserDTO> getUsersWhoLikePostByPostIDAsDTOList(int postID)
        {
            return clsLikeDataAccess.getUsersWhoLikePostByPostIDAsDTOList(new clsLikeDTO {postID=postID});
        }

        public static List<clsUser> getUsersWhoLikePostByPostID(int postID)
        {
            List<clsUserDTO> usersDTOList = getUsersWhoLikePostByPostIDAsDTOList(postID);
        
            List<clsUser> usersList = new List<clsUser>();

            foreach(var userDTO in usersDTOList)
            {
                clsUser user = new clsUser
                {
                    userID = userDTO.userID,
                    personID = userDTO.personID,
                    username = userDTO.username,
                    password = userDTO.password,
                    photoURL = userDTO.photoURL
                };

                usersList.Add(user);    
            }

            return usersList;
        }
    }
}

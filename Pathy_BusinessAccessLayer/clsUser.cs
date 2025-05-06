using FactWebsiteApp.Models.Utility;
using Pathy_DataAccessLayer;
using Pathy_DataAccessLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathy_BusinessAccessLayer
{
    public class clsUser
    {
        public enum enMode
        {
            AddNew = 1, Update = 2, Delete = 3
        }

        public int userID { get; set; }
        public int personID { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string photoURL { get; set; }

        public clsPerson person { get; set; }

        public enMode mode { get; set; }

        public int numberOfFollowers
        {
            get
            {
                return getNumberOfUserFollowersByUserID(this.userID);  
            }
        }

        public int numberOfFollowings
        {
            get
            {
                return getNumberOfUserFollowingsByUserID(this.userID);
            }
        }

        public clsAccountType.enType accountType
        {
            get
            {
                return clsAccountType.getAccountTypeByUserID(this.userID);
            }
        }


        public clsUser()
        {
            this.userID = -1;
            this.username = "";
            this.password = "";
            this.photoURL = "";
            this.mode = enMode.AddNew;
        }

        private clsUser(int userID, int personID, string username, string password, string photoURL)
        {
            this.userID = userID;
            this.personID = personID;
            this.username = username;
            this.password = password;
            this.photoURL = photoURL;
            this.mode = enMode.Update;
            this.person = clsPerson.getPersonByPersonID(personID);

        }

        public bool save()
        {
            switch (this.mode)
            {

                case enMode.AddNew:
                    {
                        this.userID = addNewUser(new clsUserDTO {personID=this.personID,username=this.username,password=clsEncryptor.ComputeHash(this.password),photoURL=this.photoURL});
                        return this.userID != -1;
                    }

                case enMode.Update:
                    {
                        return updateUser(new clsUserDTO {userID=this.userID,personID=this.personID,username=this.username,password=clsEncryptor.ComputeHash(this.password),photoURL=this.photoURL});
                    }

                case enMode.Delete:
                    {
                        return false;
                    }

            }

            return false;
        }


        public void loadUserCompositeObjects()
        {
            this.person = clsPerson.getPersonByPersonID(this.personID);            
        }

        // Static Methods

        public static clsUser getUserByUserID(int userID)
        {
            clsUserDTO userDTO = new clsUserDTO();

            if (clsUserDataAccess.getUserByUserID(userID, userDTO))
            {
                return new clsUser
                {
                    userID = userDTO.userID,
                    personID = userDTO.personID,
                    username = userDTO.username,
                    password = userDTO.password,
                    photoURL = userDTO.photoURL,
                };
            }

            return null;
        }

        public static bool doesUserExistByUsername(string username)
        {
            return clsUserDataAccess.doesUserExistByUsername(username);
        }

        public static clsUser loginByUsernameAndPassword(string username, string password)
        {
            clsUserDTO userDTO = new clsUserDTO();

            if (clsUserDataAccess.loginByUsernameAndPassword(username, clsEncryptor.ComputeHash(password), userDTO))
            {
                return new clsUser
                {
                    userID = userDTO.userID,
                    personID = userDTO.personID,
                    username = userDTO.username,
                    password = userDTO.password,
                    photoURL = userDTO.photoURL
                };
            }

            return null;
        }

        public static int addNewUser(clsUserDTO userDTO)
        {
            return clsUserDataAccess.addNewUser(userDTO);
        }

        public static int getNumberOfUserFollowersByUserID(int userID)
        {
            return clsUserDataAccess.getNumberOfUserFollowersByUserID(userID);
        }

        public static int getNumberOfUserFollowingsByUserID(int userID)
        {
            return clsUserDataAccess.getNumberOfUserFollowingsByUserID(userID);
        }
        
        public static bool updateUser(clsUserDTO userDTO)
        {
            return clsUserDataAccess.updateUserByUserID(userDTO);
        }



        public static List<clsUserDTO> getSuggestedUsersToFollowDTOList(int userID, int numberOfUsers)
        {
            return clsUserDataAccess.getSuggestedUsersToFollowDTOList(userID,numberOfUsers);
        }

        public static List<clsUser> getSuggestedUsersToFollowList(int userID , int numberOfUsers)
        {
            List<clsUser> usersList = new List<clsUser> ();

            foreach(var userDTO in getSuggestedUsersToFollowDTOList(userID,numberOfUsers))
            {
                clsUser user = new clsUser
                {
                    userID = userDTO.userID,
                    personID = userDTO.personID,
                    username = userDTO.username,
                    password = userDTO.password,
                    photoURL = userDTO.photoURL
                };

                user.loadUserCompositeObjects();

                usersList.Add(user);
            }


            return usersList;
        }




        public static List<clsUserDTO> getUserFollowersDTOList(int userID)
        {
            return clsUserDataAccess.getUserFollowers(userID);
        }

        public static List<clsUser> getUserFollowers(int userID)
        {
            List<clsUser> usersList = new List<clsUser>();

            foreach (var userDTO in getUserFollowersDTOList(userID))
            {
                clsUser user = new clsUser
                {
                    userID = userDTO.userID,
                    personID = userDTO.personID,
                    username = userDTO.username,
                    password = userDTO.password,
                    photoURL = userDTO.photoURL
                };

                user.loadUserCompositeObjects();

                usersList.Add(user);
            }


            return usersList;
        }




        public static List<clsUserDTO> getUserFollowingsDTOList(int userID)
        {
            return clsUserDataAccess.getUserFollowings(userID);
        }

        public static List<clsUser> getUserFollowings(int userID)
        {
            List<clsUser> usersList = new List<clsUser>();

            foreach (var userDTO in getUserFollowingsDTOList(userID))
            {
                clsUser user = new clsUser
                {
                    userID = userDTO.userID,
                    personID = userDTO.personID,
                    username = userDTO.username,
                    password = userDTO.password,
                    photoURL = userDTO.photoURL
                };

                user.loadUserCompositeObjects();

                usersList.Add(user);
            }


            return usersList;
        }

        public static bool doesUserExistByUserID(int userID)
        {
            return clsUserDataAccess.doesUserExistByUserID(userID);
        }


    }
}

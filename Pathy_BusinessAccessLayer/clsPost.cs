using Pathy_DataAccessLayer;
using Pathy_DataAccessLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Pathy_BusinessAccessLayer
{
    public class clsPost
    {
        public enum enMode
        {
            AddNew = 1 , Update = 2 , Delete = 3
        }

        public int postID { set; get; }
        public int accountID { set; get; }
        public DateTime creationDateTime { set; get; }
        public string textContent { set; get; }
        public string imageURL { set; get; }
        public string videoURL { set; get; }
        public enMode mode { set; get; }
        public clsAccount account {  set; get; }

        public int numberOfLikes
        {
            get
            {
                return clsLike.getNumberOfPostLikes(this.postID);
            }
        }

        public int numberOfComments
        {
            get
            {
                return clsComment.getNumberOfPostComments(this.postID);
            }
        }

        public clsPost()
        {
            this.postID = -1;
            this.accountID = -1;
            this.creationDateTime = DateTime.MinValue;
            this.textContent = "";
            this.imageURL = "";
            this.videoURL = "";
            this.mode = enMode.AddNew;

        }

        private clsPost(int postID , int accountID , DateTime creationDateTime , string textContent , string imageURL , string videoURL)
        {
            this.postID = postID;
            this.accountID = accountID;
            this.creationDateTime = creationDateTime;
            this.textContent = textContent;
            this.imageURL = imageURL;
            this.videoURL = videoURL;
            this.mode = enMode.Update;
            this.account = clsAccount.getAccountByAccountID(accountID);
        }

        public bool save()
        {
            switch(this.mode)
            {
                case enMode.AddNew:
                    {
                        this.postID = addNewPost(new clsPostDTO {accountID=this.accountID,creationDateTime=this.creationDateTime,textContent=this.textContent,imageURL=this.imageURL,videoURL=this.videoURL});
                        return this.postID != -1;                        
                    }

                case enMode.Update:
                    {
                        return updatePostByPostID(new clsPostDTO {postID=this.postID,accountID=this.accountID,creationDateTime=this.creationDateTime,textContent=this.textContent,imageURL=this.imageURL,videoURL=this.videoURL});                        
                    }

                case enMode.Delete:
                    {
                        return deletePostByPostID(this.postID);                        
                    }

            }

            return false;
        }

        public void loadCompositeObjects()
        {
            this.account = clsAccount.getAccountByAccountID(this.accountID);
        }

        // static methods.

        public static int addNewPost(clsPostDTO postDTO)
        {
            return clsPostDataAccess.addNewPost(postDTO);
        }

        public static List<clsPost> getAllPosts(int userID)
        {
           List<clsPost> posts = new List<clsPost>();

           List<clsPostDTO> postsDTO = clsPostDataAccess.getAllPosts(userID);

            foreach(var postDTO in postsDTO)
            {
                clsPost post = new clsPost 
                { 
                    postID = postDTO.postID,
                    accountID = postDTO.accountID,
                    textContent = postDTO.textContent,
                    creationDateTime = postDTO.creationDateTime,
                    imageURL = postDTO.imageURL,
                    videoURL = postDTO.videoURL,
                };

                post.loadCompositeObjects();
                post.account.loadCompositeObjects();
                post.account.user.loadUserCompositeObjects();


                posts.Add(post);
            }

            return posts;
        }

        public static List<clsPostDTO> getAllPostsAsDTO(int userID)
        {
            return clsPostDataAccess.getAllPosts(userID);
        }

        public static List<clsPost> getPostsByUserID(int userID)
        {
            List<clsPost> posts = new List<clsPost>();

            List<clsPostDTO> postsDTO = clsPostDataAccess.getPostsByUserID(userID);

            foreach (var postDTO in postsDTO)
            {
                clsPost post = new clsPost
                {
                    postID = postDTO.postID,
                    accountID = postDTO.accountID,
                    textContent = postDTO.textContent,
                    creationDateTime = postDTO.creationDateTime,
                    imageURL = postDTO.imageURL,
                    videoURL = postDTO.videoURL,
                };

                post.loadCompositeObjects();
                post.account.loadCompositeObjects();
                post.account.user.loadUserCompositeObjects();


                posts.Add(post);
            }

            return posts;
        }

        public static List<clsPostDTO> getPostsByUserIDAsDTO(int userID)
        {
            return clsPostDataAccess.getPostsByUserID(userID);
        }
        
        public static clsPost getPostByPostID(int postID)
        {
            clsPostDTO postDTO = new clsPostDTO();

            if(clsPostDataAccess.getPostByPostID(postID,postDTO))
            {
                       
                return new clsPost
                {
                    postID = postDTO.postID,
                    accountID = postDTO.accountID,
                    creationDateTime = postDTO.creationDateTime,
                    textContent = postDTO.textContent,
                    imageURL = postDTO.imageURL,
                    videoURL = postDTO.videoURL
                };
            }

            return null;
        }

        public static bool deletePostByPostID(int postID)
        {
            return clsPostDataAccess.deletePostByPostID(postID);
        }

        public static bool updatePostByPostID(clsPostDTO postDTO)
        {
            return clsPostDataAccess.updatePostByPostID(postDTO);
        }

        public static bool isPostLikedByUser(int postID , int likerUserID)
        {
            return clsPostDataAccess.isPostLikedByUser(postID, likerUserID);
        }

    }
}

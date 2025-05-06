using Pathy_DataAccessLayer;
using Pathy_DataAccessLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathy_BusinessAccessLayer
{
    public class clsComment
    {
        public enum enMode
        {
            AddNew = 1 , Update = 2 , Delete = 3
        }

        public int commentID { get; set; }         
        public int postID { get; set; }
        public int commenterUserID { get; set; }
        public string commentText { get; set; }
        public enMode mode { get; set; }
        public clsPost post { get; set; }
        public clsUser commenterUser { get; set; }

        public clsComment()
        {
            this.commentID = -1;
            this.postID = -1;
            this.commenterUserID = -1;
            this.commentText = "";
            this.mode = clsComment.enMode.AddNew;
            
        }

        private clsComment(int postID , int commenterUserID , string commentText)
        {
            this.postID = postID;
            this.commenterUserID = commenterUserID;
            this.commentText = commentText;
            this.mode = clsComment.enMode.Update;
            this.post = clsPost.getPostByPostID(postID);
            this.commenterUser = clsUser.getUserByUserID(commenterUserID);
        }

        public bool save()
        {
            switch(this.mode)
            {
                case enMode.AddNew:
                    {
                        this.commentID = addNewComment(new clsCommentDTO {commentID=this.commentID,postID = this.postID,commenterUserID=this.commenterUserID,commentText=this.commentText});
                        return this.commentID != -1;
                    }

                case enMode.Update:
                    {
                        return updateComment(new clsCommentDTO {commentID=this.commentID,postID = this.postID, commenterUserID = this.commenterUserID, commentText = this.commentText });
                    }

                case enMode.Delete:
                    {                        
                        return deleteCommentByCommentID(new clsCommentDTO {commentID=this.commentID,postID = this.postID, commenterUserID = this.commenterUserID, commentText = this.commentText });
                    }
            }


            return false;
        }

        public void loadCompositeObjects()
        {
            this.commenterUser = clsUser.getUserByUserID(this.commenterUserID);
            this.post = clsPost.getPostByPostID(this.postID);
            this.commenterUser.loadUserCompositeObjects();
        }

        // Static Methods

        public static List<clsCommentDTO> getPostCommentsAsDTOList(int postID)
        {
            return clsCommentDataAccess.getPostCommentsAsDTOList(new clsCommentDTO {postID = postID});
        }

        public static List<clsComment> getPostComments(int postID)
        {
            List<clsComment> list = new List<clsComment>();

            foreach(clsCommentDTO commentDTO in getPostCommentsAsDTOList(postID))
            {
                clsComment comment = new clsComment 
                { 
                    commentID = commentDTO.commentID,
                    postID = commentDTO.postID,
                    commenterUserID = commentDTO.commenterUserID,
                    commentText = commentDTO.commentText
                };

                comment.loadCompositeObjects();

                list.Add(comment);
                    
            }

            return list;
        }

        public static int getNumberOfPostComments(int postID)
        {
            return clsCommentDataAccess.getNumberOfPostComments(postID);
        }

        public static int addNewComment(clsCommentDTO commentDTO)
        {
            return clsCommentDataAccess.addNewComment(commentDTO);
        }

        public static bool deleteCommentByCommentID(clsCommentDTO commentDTO)
        {
            return clsCommentDataAccess.deleteCommentByCommentID(commentDTO);
        }

        public static bool updateComment(clsCommentDTO commentDTO)
        {
            return clsCommentDataAccess.updateComment(commentDTO);
        }

        public static clsComment getCommentByCommentID(int commentID)
        {
            clsCommentDTO commentDTO = new clsCommentDTO();
            commentDTO.commentID = commentID;

            if(clsCommentDataAccess.getCommentByCommentID(commentDTO))
            {
                return new clsComment
                {
                    commentID = commentDTO.commentID,
                    postID = commentDTO.postID ,
                    commenterUserID = commentDTO.commenterUserID,
                    commentText = commentDTO.commentText
                };
            }

            return null;
        }
    }
}

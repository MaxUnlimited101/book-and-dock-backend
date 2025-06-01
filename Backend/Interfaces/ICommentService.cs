using System.Collections.Generic;
using Backend.DTO;
using Backend.Models;

namespace Backend.Interfaces
{
    public interface ICommentService
    {
        IEnumerable<Comment> GetAllComments();
        Comment? GetCommentById(int id);
        void AddComment(CommentDto comment);
        void UpdateComment(CommentDto comment);
        void DeleteComment(int id);
        IEnumerable<Comment> GetCommentsByGuideId(int guideId);
    }
}
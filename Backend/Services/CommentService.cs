using Backend.Models;
using Backend.Repositories;
using Backend.Interfaces;
using Backend.DTO;
using Backend.Exceptions;

namespace Backend.Services;

public class CommentService : ICommentService
{
    private readonly ICommentRepository _commentRepository;
    private readonly IUserRepository _userRepository;
    private readonly IGuideRepository _guideRepository;

    public CommentService(ICommentRepository commentRepository, IUserRepository userRepository, IGuideRepository guideRepository)
    {
        _userRepository = userRepository;
        _guideRepository = guideRepository;
        _commentRepository = commentRepository;
    }

    public void AddComment(CommentDto comment)
    {
        Comment newComment = new();
        var user = _userRepository.GetUserById(comment.CreatedBy);
        if (user == null)
        {
            throw new ModelInvalidException("User not found");
        }
        var guide = _guideRepository.GetGuideById(comment.GuideId);
        if (guide == null)
        {
            throw new ModelInvalidException("Guide not found");
        }
        newComment.CreatedBy = comment.CreatedBy;
        newComment.GuideId = comment.GuideId;
        newComment.Content = comment.Content;
        newComment.CreatedOn = DateTime.UtcNow;
        newComment.CreatedByNavigation = user;
        newComment.Guide = guide;
        _commentRepository.Add(newComment);
    }

    public void DeleteComment(int id)
    {
        var comment = _commentRepository.GetById(id);
        if (comment == null)
        {
            throw new ModelInvalidException("Comment not found");
        }
        _commentRepository.Delete(id);
    }

    public IEnumerable<Comment> GetAllComments()
    {
        return _commentRepository.GetAll();
    }

    public Comment? GetCommentById(int id)
    {
        var comment = _commentRepository.GetById(id);
        if (comment == null)
        {
            return null;
        }
        return comment;
    }

    public IEnumerable<Comment> GetCommentsByGuideId(int guideId)
    {
        var guide = _guideRepository.GetGuideById(guideId);
        if (guide == null)
        {
            throw new ModelInvalidException("Guide not found");
        }
        var comments = _commentRepository.GetAll().Where(c => c.GuideId == guideId);
        return comments;
    }

    public void UpdateComment(CommentDto comment)
    {
        var existingComment = _commentRepository.GetById(comment.Id);
        if (existingComment == null)
        {
            throw new ModelInvalidException("Comment not found");
        }
        var user = _userRepository.GetUserById(comment.CreatedBy);
        if (user == null)
        {
            throw new ModelInvalidException("User not found");
        }
        var guide = _guideRepository.GetGuideById(comment.GuideId);
        if (guide == null)
        {
            throw new ModelInvalidException("Guide not found");
        }
        existingComment.CreatedBy = comment.CreatedBy;
        existingComment.CreatedByNavigation = user;
        existingComment.GuideId = comment.GuideId;
        existingComment.Guide = guide;
        existingComment.Content = comment.Content;

        _commentRepository.Update(existingComment);
    }
}
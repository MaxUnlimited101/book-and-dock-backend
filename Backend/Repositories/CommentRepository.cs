using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Models;
using Backend.Interfaces;
using Backend.Data;

namespace Backend.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly BookAndDockContext _context;

        public CommentRepository(BookAndDockContext context)
        {
            _context = context;
        }

        public void Add(Comment comment)
        {
            _context.Comments.Add(comment);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var comment = _context.Comments.Find(id);
            if (comment != null)
            {
                _context.Comments.Remove(comment);
                _context.SaveChanges();
            }
        }

        public IEnumerable<Comment> GetAll()
        {
            return _context.Comments;
        }

        public Comment? GetById(int id)
        {
            return _context.Comments.Find(id);
        }

        public void Update(Comment comment)
        {
            _context.Comments.Update(comment);
            _context.SaveChanges();
        }
    }
}
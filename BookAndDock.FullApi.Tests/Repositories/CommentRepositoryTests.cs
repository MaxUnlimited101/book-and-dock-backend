using System;
using System.Linq;
using Backend.Data;
using Backend.Models;
using Backend.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Backend.Tests.Repositories
{
    public class CommentRepositoryTests
    {
        private readonly BookAndDockContext _context;
        private readonly CommentRepository _repository;

        public CommentRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<BookAndDockContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new BookAndDockContext(options);
            _repository = new CommentRepository(_context);
        }

        [Fact]
        public void Add_AddsCommentToDatabase()
        {
            var comment = new Comment { Id = 1, CreatedBy = 1, GuideId = 1, Content = "Nice guide!", CreatedOn = DateTime.UtcNow };

            _repository.Add(comment);

            Assert.Equal(1, _context.Comments.Count());
            Assert.Equal("Nice guide!", _context.Comments.First().Content);
        }

        [Fact]
        public void Delete_RemovesCommentFromDatabase()
        {
            var comment = new Comment { Id = 2, CreatedBy = 1, GuideId = 1, Content = "To be deleted", CreatedOn = DateTime.UtcNow };
            _context.Comments.Add(comment);
            _context.SaveChanges();

            _repository.Delete(2);

            Assert.Empty(_context.Comments);
        }

        [Fact]
        public void GetAll_ReturnsAllComments()
        {
            _context.Comments.Add(new Comment { Id = 1, CreatedBy = 1, GuideId = 1, Content = "First", CreatedOn = DateTime.UtcNow });
            _context.Comments.Add(new Comment { Id = 2, CreatedBy = 2, GuideId = 1, Content = "Second", CreatedOn = DateTime.UtcNow });
            _context.SaveChanges();

            var result = _repository.GetAll();

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public void GetById_ReturnsCorrectComment()
        {
            var comment = new Comment { Id = 3, CreatedBy = 1, GuideId = 1, Content = "Specific", CreatedOn = DateTime.UtcNow };
            _context.Comments.Add(comment);
            _context.SaveChanges();

            var result = _repository.GetById(3);

            Assert.NotNull(result);
            Assert.Equal("Specific", result.Content);
        }

        [Fact]
        public void Update_UpdatesCommentInDatabase()
        {
            var comment = new Comment { Id = 4, CreatedBy = 1, GuideId = 1, Content = "Before", CreatedOn = DateTime.UtcNow };
            _context.Comments.Add(comment);
            _context.SaveChanges();

            comment.Content = "After";
            _repository.Update(comment);

            var updated = _context.Comments.Find(4);
            Assert.Equal("After", updated.Content);
        }
    }
}

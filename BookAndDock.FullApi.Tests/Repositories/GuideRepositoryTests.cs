using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Data;
using Backend.Models;
using Backend.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Backend.Tests.Repositories
{
    public class GuideRepositoryTests
    {
        private readonly BookAndDockContext _context;
        private readonly GuideRepository _repository;

        public GuideRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<BookAndDockContext>()
                .UseInMemoryDatabase(databaseName: "GuideTestDb")
                .Options;
            _context = new BookAndDockContext(options);
            _repository = new GuideRepository(_context);
        }

        [Fact]
        public void Add_AddsGuideToDatabase()
        {
            var guide = new Guide { Id = 1, Title = "Guide Title", Content = "Some content", CreatedBy = 1, IsApproved = true };

            _repository.Add(guide);

            var result = _context.Guides.Find(1);
            Assert.NotNull(result);
            Assert.Equal("Guide Title", result.Title);
        }


        [Fact]
        public void GetGuideById_ReturnsCorrectGuide()
        {
            var guide = new Guide { Id = 4, Title = "Specific Guide", Content = "Details", CreatedBy = 3, IsApproved = true };
            _context.Guides.Add(guide);
            _context.SaveChanges();

            var result = _repository.GetGuideById(4);

            Assert.NotNull(result);
            Assert.Equal("Specific Guide", result.Title);
        }

        [Fact]
        public void Update_UpdatesGuide()
        {
            var guide = new Guide { Id = 5, Title = "Old Title", Content = "Old Content", CreatedBy = 4, IsApproved = false };
            _context.Guides.Add(guide);
            _context.SaveChanges();

            guide.Title = "Updated Title";
            _repository.Update(guide);

            var updated = _context.Guides.Find(5);
            Assert.Equal("Updated Title", updated.Title);
        }

        [Fact]
        public void Delete_RemovesGuide()
        {
            var guide = new Guide { Id = 6, Title = "To Be Deleted", Content = "Gone", CreatedBy = 5, IsApproved = true };
            _context.Guides.Add(guide);
            _context.SaveChanges();

            _repository.Delete(6);

            var deleted = _context.Guides.Find(6);
            Assert.Null(deleted);
        }
    }
}

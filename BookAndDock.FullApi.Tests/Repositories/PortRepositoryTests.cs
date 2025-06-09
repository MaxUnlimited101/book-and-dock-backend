using System;
using System.Collections.Generic;
using System.Linq;
using Backend.Data;
using Backend.Models;
using Backend.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Backend.Tests.Repositories
{
    public class PortRepositoryTests
    {
        private readonly DbContextOptions<BookAndDockContext> _dbOptions;

        public PortRepositoryTests()
        {
            _dbOptions = new DbContextOptionsBuilder<BookAndDockContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
        }

        private Port CreateSamplePort(int id = 0)
        {
            return new Port
            {
                Id = id,
                Name = "Sample Port",
                Description = "Description",
                OwnerId = 1,
                IsApproved = true,
                CreatedOn = DateTime.UtcNow
            };
        }

        [Fact]
        public void Create_AddsPortToDatabaseAndReturnsId()
        {
            using var context = new BookAndDockContext(_dbOptions);
            var repo = new PortRepository(context);
            var port = CreateSamplePort();

            var id = repo.Create(port);

            Assert.True(id > 0);
            var fromDb = context.Ports.Find(id);
            Assert.NotNull(fromDb);
        }

        

        [Fact]
        public void GetById_ReturnsNullIfNotFound()
        {
            using var context = new BookAndDockContext(_dbOptions);
            var repo = new PortRepository(context);

            var result = repo.GetById(123);
            Assert.Null(result);
        }

        

        [Fact]
        public void Update_ModifiesPort()
        {
            using var context = new BookAndDockContext(_dbOptions);
            var port = CreateSamplePort(10);
            context.Ports.Add(port);
            context.SaveChanges();

            port.Name = "Updated Name";

            var repo = new PortRepository(context);
            repo.Update(port);

            var updated = context.Ports.Find(10);
            Assert.Equal("Updated Name", updated!.Name);
        }

        [Fact]
        public void Delete_RemovesPortIfExists()
        {
            using var context = new BookAndDockContext(_dbOptions);
            var port = CreateSamplePort(20);
            context.Ports.Add(port);
            context.SaveChanges();

            var repo = new PortRepository(context);
            repo.Delete(20);

            var deleted = context.Ports.Find(20);
            Assert.Null(deleted);
        }

        [Fact]
        public void Delete_NoOpIfNotFound()
        {
            using var context = new BookAndDockContext(_dbOptions);
            var repo = new PortRepository(context);

            var ex = Record.Exception(() => repo.Delete(999));
            Assert.Null(ex);
        }

        [Fact]
        public void CheckIfExistsById_ReturnsTrueIfExists()
        {
            using var context = new BookAndDockContext(_dbOptions);
            var port = CreateSamplePort(33);
            context.Ports.Add(port);
            context.SaveChanges();

            var repo = new PortRepository(context);
            var exists = repo.CheckIfExistsById(33);

            Assert.True(exists);
        }

        [Fact]
        public void CheckIfExistsById_ReturnsFalseIfNotExists()
        {
            using var context = new BookAndDockContext(_dbOptions);
            var repo = new PortRepository(context);

            var exists = repo.CheckIfExistsById(999);
            Assert.False(exists);
        }
    }
}

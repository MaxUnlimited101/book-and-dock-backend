using Backend.Data;
using Backend.Models;
using Backend.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Backend.Tests.Repositories
{
    public class UserRepositoryTests
    {
        private DbContextOptions<BookAndDockContext> _options;

        public UserRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<BookAndDockContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;
        }

        [Fact]
        public async Task CreateUser_AddsUserAndReturnsId()
        {
            using var context = new BookAndDockContext(_options);
            var repository = new UserRepository(context);

            var role = new Role { Id = 1, Name = "User" };
            context.Roles.Add(role);
            context.SaveChanges();

            var user = new User { Name = "Test", Email = "test@example.com", Password = "123456", Surname = "Smith", Role = role };

            var id = await repository.CreateUserAsync(user);

            Assert.True(id > 0);
        }

        [Fact]
        public async Task GetUserById_ReturnsCorrectUser()
        {
            using var context = new BookAndDockContext(_options);
            var repository = new UserRepository(context);

            var role = new Role { Id = 2, Name = "Admin" };
            context.Roles.Add(role);

            var user = new User { Name = "AdminUser", Email = "admin@example.com", Password = "secure123", Surname = "Adminson", Role = role };
            context.Users.Add(user);
            context.SaveChanges();

            var result = await repository.GetUserByIdAsync(user.Id);

            Assert.NotNull(result);
            Assert.Equal(user.Email, result.Email);
        }

        [Fact]
        public async Task DeleteUser_RemovesUser()
        {
            using var context = new BookAndDockContext(_options);
            var repository = new UserRepository(context);

            var role = new Role { Id = 3, Name = "Editor" };
            context.Roles.Add(role);

            var user = new User { Name = "EditorUser", Email = "editor@example.com", Password = "pass123", Surname = "Editor", Role = role };
            context.Users.Add(user);
            context.SaveChanges();

            await repository.DeleteUserAsync(user.Id);

            var result = await repository.GetUserByIdAsync(user.Id);
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateUser_ChangesUserFields()
        {
            using var context = new BookAndDockContext(_options);
            var repository = new UserRepository(context);

            var role = new Role { Id = 4, Name = "Viewer" };
            context.Roles.Add(role);

            var user = new User { Name = "ViewerUser", Email = "viewer@example.com", Password = "view123", Surname = "Viewer", Role = role };
            context.Users.Add(user);
            context.SaveChanges();

            user.Surname = "Changed";
            repository.UpdateUser(user);

            var result = repository.GetUserById(user.Id);
            Assert.Equal("Changed", result.Surname);
        }


        [Fact]
        public void GetUserByEmail_ReturnsUser()
        {
            using var context = new BookAndDockContext(_options);
            var repository = new UserRepository(context);

            var role = new Role { Id = 6, Name = "Manager" };
            context.Roles.Add(role);

            var user = new User { Name = "ManagerName", Email = "manager@example.com", Password = "mng123", Surname = "Manager", Role = role };
            context.Users.Add(user);
            context.SaveChanges();

            var result = repository.GetUserByEmail(user.Email);
            Assert.NotNull(result);
            Assert.Equal(user.Email, result.Email);
        }

        [Fact]
        public void CheckIfUserExistsById_ReturnsTrue()
        {
            using var context = new BookAndDockContext(_options);
            var repository = new UserRepository(context);

            var role = new Role { Id = 7, Name = "Test" };
            context.Roles.Add(role);

            var user = new User { Name = "ExistUser", Email = "exist@example.com", Password = "existpass", Surname = "Exist", Role = role };
            context.Users.Add(user);
            context.SaveChanges();

            var exists = repository.CheckIfUserExistsById(user.Id);
            Assert.True(exists);
        }

        [Fact]
        public void GetUsersByRole_ReturnsMatchingUsers()
        {
            using var context = new BookAndDockContext(_options);
            var repository = new UserRepository(context);

            var role = new Role { Id = 8, Name = "Special" };
            context.Roles.Add(role);

            var users = new List<User>
            {
                new User { Name = "SpecialOne", Email = "special1@example.com", Password = "sp1", Surname = "One", Role = role },
                new User { Name = "SpecialTwo", Email = "special2@example.com", Password = "sp2", Surname = "Two", Role = role }
            };

            context.Users.AddRange(users);
            context.SaveChanges();

            var result = repository.GetUsersByRole(role);
            Assert.Equal(2, result.Count);
        }
    }
}

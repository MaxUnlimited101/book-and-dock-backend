using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Backend.Data;
using Backend.DTO;
using Backend.Models;
using Backend.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Backend.Tests.Repositories
{
    public class RoleRepositoryTests
    {
        private readonly DbContextOptions<BookAndDockContext> _options;

        public RoleRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<BookAndDockContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        private Role CreateSampleRole(int id = 0, string name = "Admin")
        {
            return new Role
            {
                Id = id,
                Name = name,
                CreatedOn = DateTime.UtcNow
            };
        }

        [Fact]
        public async Task GetAllRolesAsync_ReturnsAllRoles()
        {
            using var context = new BookAndDockContext(_options);
            context.Roles.AddRange(
                CreateSampleRole(1, "Admin"),
                CreateSampleRole(2, "User")
            );
            await context.SaveChangesAsync();

            var repo = new RoleRepository(context);
            var result = (await repo.GetAllRolesAsync()).ToList();

            Assert.Equal(2, result.Count);
            Assert.Contains(result, r => r.Name == "Admin");
            Assert.Contains(result, r => r.Name == "User");
        }

        [Fact]
        public async Task GetRoleByIdAsync_FindsCorrectRole()
        {
            using var context = new BookAndDockContext(_options);
            context.Roles.Add(CreateSampleRole(10, "Moderator"));
            await context.SaveChangesAsync();

            var repo = new RoleRepository(context);
            var result = await repo.GetRoleByIdAsync(10);

            Assert.NotNull(result);
            Assert.Equal(10, result!.Id);
            Assert.Equal("Moderator", result.Name);
        }

        [Fact]
        public async Task GetRoleByIdAsync_ReturnsNullIfNotFound()
        {
            using var context = new BookAndDockContext(_options);
            var repo = new RoleRepository(context);

            var result = await repo.GetRoleByIdAsync(999);

            Assert.Null(result);
        }

        [Fact]
        public async Task CreateRoleAsync_AddsRoleAndReturnsId()
        {
            using var context = new BookAndDockContext(_options);
            var repo = new RoleRepository(context);
            var dto = new RoleDTO(0, "Guest", DateTime.MinValue);

            var id = await repo.CreateRoleAsync(dto);

            var created = await context.Roles.FindAsync(id);
            Assert.NotNull(created);
            Assert.Equal("Guest", created!.Name);
        }

        [Fact]
        public async Task UpdateRoleAsync_UpdatesExistingRole()
        {
            using var context = new BookAndDockContext(_options);
            var original = CreateSampleRole(20, "Staff");
            context.Roles.Add(original);
            await context.SaveChangesAsync();

            var repo = new RoleRepository(context);
            var updatedDto = new RoleDTO(20, "UpdatedStaff", DateTime.UtcNow);

            var result = await repo.UpdateRoleAsync(updatedDto);
            var updated = await context.Roles.FindAsync(20);

            Assert.True(result);
            Assert.Equal("UpdatedStaff", updated!.Name);
        }

        [Fact]
        public async Task UpdateRoleAsync_ReturnsFalseIfNotFound()
        {
            using var context = new BookAndDockContext(_options);
            var repo = new RoleRepository(context);
            var nonExisting = new RoleDTO(999, "Ghost", DateTime.UtcNow);

            var result = await repo.UpdateRoleAsync(nonExisting);

            Assert.False(result);
        }

        [Fact]
        public async Task DeleteRoleAsync_RemovesIfExists()
        {
            using var context = new BookAndDockContext(_options);
            context.Roles.Add(CreateSampleRole(30, "Temp"));
            await context.SaveChangesAsync();

            var repo = new RoleRepository(context);
            await repo.DeleteRoleAsync(30);

            Assert.Null(await context.Roles.FindAsync(30));
        }

        [Fact]
        public async Task DeleteRoleAsync_NoExceptionIfNotFound()
        {
            using var context = new BookAndDockContext(_options);
            var repo = new RoleRepository(context);

            var ex = await Record.ExceptionAsync(() => repo.DeleteRoleAsync(999));

            Assert.Null(ex); // should not throw
        }
    }
}

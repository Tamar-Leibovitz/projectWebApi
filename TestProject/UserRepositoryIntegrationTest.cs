using DTOs;
using Entities;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject
{
    public class UserRepositoryIntegrationTest : IClassFixture<DatabaseFixture>
    {
        private readonly Shop214928673Context _dbContext;
        private readonly UserRepository _userRepository;

        public UserRepositoryIntegrationTest(DatabaseFixture databaseFixture)
        {
            _dbContext = databaseFixture.Context;
            _userRepository = new UserRepository(_dbContext);
        }


        [Fact]
        public async Task Login_ValifCredentials_ReturnsUser()
        {
            var email = "test@gmail.com";
            var password = "password";
            var user = new User { Email = email, Password = password, FirstName = "test", LastName = "test22" };
            var userLogin = new User { Email = email, Password = password, FirstName = null, LastName = null };

            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            var result = await _userRepository.Login(userLogin);

            Assert.NotNull(result);
        }


        [Fact]
        public async Task Register_ValifCredentials_ReturnsUser()
        {
            var email = "aaa111@gmail.com";
            var password = "password";
            var user = new User { Email = email, Password = password, FirstName = "test", LastName = "test22" };
            
            var result = await _userRepository.Register(user);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task UpdateUser_ShouldUpdateUserDetails()
        {
            // Arrange
            var originalUser = new User { FirstName = "old first Name", LastName = "old last name", Email = "old@example.com", Password = "lh2458!11" };
            await _dbContext.Users.AddAsync(originalUser);
            await _dbContext.SaveChangesAsync();

            // Retrieve the newly generated UserId
            int userId = originalUser.UserId;

            User newUser = new User { FirstName = "new first Name", LastName = "new last name", Email = "new@example.com", Password = "lh2458!22" };

            // Act
            await _userRepository.updateUser(userId, newUser);

            // Verify the update in the database
            var dbUser = await _dbContext.Users.FindAsync(userId);
            Assert.NotNull(dbUser);


        }
    }
}

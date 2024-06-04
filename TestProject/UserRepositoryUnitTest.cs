using Entities;
using Moq;
using Moq.EntityFrameworkCore;
using Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject
{
    public class UserRepositoryUnitTest
    {
        [Fact]
        public async Task Login_ValidCredentials_ReturnsUser()
        {
            var user = new User { Email= "t@exaaample.com",Password = "password"};
            var mockContext = new Mock<Shop214928673Context>();
            var users = new List<User>() { user };

            mockContext.Setup(x => x.Users).ReturnsDbSet(users);

            var userRepository = new UserRepository(mockContext.Object);
            //Act
            var result = await userRepository.Login(user);
            Assert.Equal(user, result);
        }
        [Fact]
        public async Task Login_ValidCredentials_ReturnsNull()
        {
            var user = new User { Email = "t@exaaample.com", Password = "password" };
            var user1 = new User { Email = "t1@exaaample.com", Password = "password1" };
            var mockContext = new Mock<Shop214928673Context>();
            var users = new List<User>() { user };

            mockContext.Setup(x => x.Users).ReturnsDbSet(users);

            var userRepository = new UserRepository(mockContext.Object);
            //Act
            var result = await userRepository.Login(user1);
            Assert.Null(result);
        }


        [Fact]
        public async Task UpdateUser_ValidUser_UpdatesUserInDbContext()
        {
            // Arrange
            var userId = 1;
            var existingUser = new User { UserId = userId, Email = "old@example.com", Password = "oldpassword" };
            var newUser = new User { Email = "new@example.com", Password = "newpassword" };

            // Create a mock database context
            var mockContext = new Mock<Shop214928673Context>();

            // Setup the Users DbSet with initial data
            var users = new List<User> { existingUser };
            mockContext.Setup(x => x.Users).ReturnsDbSet(users);

            var userRepository = new UserRepository(mockContext.Object);

            // Act
            var result = await userRepository.updateUser(userId, newUser);

            // Assert
           
            var updatedUser = users.Find(u => u.UserId == userId && u.Email == newUser.Email && u.Password == newUser.Password);
            Assert.NotNull(updatedUser);
            Assert.Equal("new@example.com", updatedUser.Email);
            Assert.Equal("newpassword", updatedUser.Password);
        }


        [Fact]
        public async Task Register_ValidCredentials_ReturnsUser()
        {
            var user = new User { Email = "t@exaaample.com", Password = "password" };
            var mockContext = new Mock<Shop214928673Context>();
            var users = new List<User>() { user };

            mockContext.Setup(x => x.Users).ReturnsDbSet(users);

            var userRepository = new UserRepository(mockContext.Object);
            //Act
            var result = await userRepository.Register(user);
            Assert.Null(result);
        }

    }
}

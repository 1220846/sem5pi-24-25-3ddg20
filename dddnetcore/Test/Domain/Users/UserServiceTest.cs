using System;
using System.Threading.Tasks;
using Xunit;
using Moq;
using DDDSample1.Domain.Users;
using DDDSample1.Domain.Shared;

namespace DDDSample1.Tests.Domain.Users
{
    public class UserServiceTests
    {
        private readonly UserService _service;
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;

        public UserServiceTests()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _service = new UserService(_mockUnitOfWork.Object, _mockUserRepository.Object);
        }

        [Fact]
        public async Task GetByIdAsyncExistingUserReturnsUserDto()
        {
            //var username = "D240003";
            var email = "user@example.com";
            var role = Role.NURSE;
            var username = Username.Create(role,"D240003");
            var user = new User(username, new Email(email), role);

            _mockUserRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Username>()))
                .ReturnsAsync(user);

            var result = await _service.GetByIdAsync(username.Name);

            Assert.NotNull(result);
            Assert.Equal(username.Name, result.Username);
            Assert.Equal(email, result.Email);
            Assert.Equal(role.ToString(), result.Role);
        }

        [Fact]
        public async Task GetByIdAsyncNonExistingUserReturnsNull()
        {
            _mockUserRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Username>()))
                .ReturnsAsync((User)null);

            var result = await _service.GetByIdAsync("NonExistingUser@gmail.com");

            Assert.Null(result);
        }

       [Fact]
        public async Task AddAsyncValidUserAddsUserSuccessfully()
        {
            var dto = new CreatingUserDto
            {
                Email = "newuser@example.com",
                Role = "NURSE"
            };

            _mockUserRepository.Setup(repo => repo.CountBackofficeUsersAsync())
                .ReturnsAsync(100);

             _mockUserRepository.Setup(repo => repo.AddAsync(It.IsAny<User>()))
                .Callback<User>(user => {Assert.NotNull(user);});

            var result = await _service.addBackofficeUserAsync(dto);

            Assert.NotNull(result);
            Assert.Equal(dto.Email, result.Email);
            Assert.Equal(dto.Role.ToUpper(), result.Role);
            
            _mockUserRepository.Verify(repo => repo.AddAsync(It.IsAny<User>()), Times.Once);
            
            _mockUnitOfWork.Verify(uow => uow.CommitAsync(), Times.Once);
        }

    
    }
}
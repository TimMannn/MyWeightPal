using Microsoft.VisualStudio.TestTools.UnitTesting;
using BLL;
using BLL.Models;
using Moq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Testen.UnitTesten
{
    [TestClass]
    public class UserServiceTests
    {
        private Mock<IUserData> _userDataMock;
        private Mock<UserManager<IdentityUser>> _userManagerMock;
        private Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private UserService _userService;

        private const string TestUserId = "test-user-id";

        [TestInitialize]
        public void Setup()
        {
            _userDataMock = new Mock<IUserData>();

            var userStoreMock = new Mock<IUserStore<IdentityUser>>();
            _userManagerMock = new Mock<UserManager<IdentityUser>>(
                userStoreMock.Object, null, null, null, null, null, null, null, null);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, TestUserId)
            };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            var httpContextMock = new DefaultHttpContext { User = claimsPrincipal };

            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _httpContextAccessorMock.Setup(h => h.HttpContext).Returns(httpContextMock);

            _userService = new UserService(_userDataMock.Object, _userManagerMock.Object, _httpContextAccessorMock.Object);
        }

        [TestMethod]
        public async Task GetAllUsers_ReturnsList()
        {
            var users = new List<UserDetails>
            {
                new UserDetails { Id = "1", UserName = "User1" },
                new UserDetails { Id = "2", UserName = "User2" }
            };

            _userDataMock.Setup(u => u.GetAllUsers()).ReturnsAsync(users);

            var result = await _userService.GetAllUsers();

            Assert.AreEqual(2, result.Count);
        }

        [TestMethod]
        public async Task GetCurrentUser_ReturnsUserDetails()
        {
            var user = new UserDetails { Id = TestUserId, UserName = "TestUser" };

            _userDataMock.Setup(u => u.GetCurrentUser(TestUserId)).ReturnsAsync(user);

            var result = await _userService.GetCurrentUser();

            Assert.AreEqual("TestUser", result.UserName);
        }

        [TestMethod]
        public async Task CreateUser_CallsDataLayer()
        {
            await _userService.CreateUser("NewUser", null);

            _userDataMock.Verify(u => u.CreateUser(TestUserId, "NewUser", null), Times.Once);
        }

        [TestMethod]
        public async Task EditUser_CallsDataLayer()
        {
            await _userService.EditUser("EditedUser", null);

            _userDataMock.Verify(u => u.EditUser(TestUserId, "EditedUser", null), Times.Once);
        }

        // Alt flow
        
        [TestMethod]
        public async Task GetCurrentUser_ReturnsNull_WhenUserNotFound()
        {
            _userDataMock.Setup(u => u.GetCurrentUser(TestUserId)).ReturnsAsync((UserDetails)null);

            var result = await _userService.GetCurrentUser();

            Assert.IsNull(result);
        }
    }
}

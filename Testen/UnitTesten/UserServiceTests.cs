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

            var httpContextMock = new DefaultHttpContext
            {
                User = claimsPrincipal
            };

            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _httpContextAccessorMock.Setup(h => h.HttpContext).Returns(httpContextMock);

            _userService = new UserService(_userDataMock.Object, _userManagerMock.Object, _httpContextAccessorMock.Object);
        }

        [TestMethod]
        public async Task GetAllUsers_CallsDataLayerAndReturnsList()
        {
            var expectedList = new List<UserDetails>
            {
                new UserDetails { Id = "1", UserName = "User1" },
                new UserDetails { Id = "2", UserName = "User2" }
            };

            _userDataMock.Setup(u => u.GetAllUsers()).ReturnsAsync(expectedList);

            var result = await _userService.GetAllUsers();

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("User1", result[0].UserName);
            Assert.AreEqual("User2", result[1].UserName);
            _userDataMock.Verify(u => u.GetAllUsers(), Times.Once);
        }

        [TestMethod]
        public async Task GetCurrentUser_CallsDataLayerWithUserId()
        {
            var expected = new UserDetails { Id = TestUserId, UserName = "TestUser" };

            _userDataMock.Setup(u => u.GetCurrentUser(TestUserId)).ReturnsAsync(expected);

            var result = await _userService.GetCurrentUser();

            Assert.AreEqual(expected.Id, result.Id);
            Assert.AreEqual(expected.UserName, result.UserName);
            _userDataMock.Verify(u => u.GetCurrentUser(TestUserId), Times.Once);
        }

        [TestMethod]
        public async Task CreateUser_CallsDataLayerWithCorrectParameters()
        {
            string username = "NewUser";

            await _userService.CreateUser(username, null);

            _userDataMock.Verify(u => u.CreateUser(TestUserId, username, null), Times.Once);
        }

        [TestMethod]
        public async Task EditUser_CallsDataLayerWithCorrectParameters()
        {
            string username = "EditedUser";

            await _userService.EditUser(username, null);

            _userDataMock.Verify(u => u.EditUser(TestUserId, username, null), Times.Once);
        }

        // Alt flows

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task GetCurrentUser_ThrowsException_WhenHttpContextIsNull()
        {
            _httpContextAccessorMock.Setup(h => h.HttpContext).Returns((HttpContext)null);
            var userService = new UserService(_userDataMock.Object, _userManagerMock.Object, _httpContextAccessorMock.Object);

            await userService.GetCurrentUser();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task GetCurrentUser_ThrowsException_WhenUserNotAuthenticated()
        {
            var identity = new ClaimsIdentity();
            var claimsPrincipal = new ClaimsPrincipal(identity);

            var httpContext = new DefaultHttpContext
            {
                User = claimsPrincipal
            };

            _httpContextAccessorMock.Setup(h => h.HttpContext).Returns(httpContext);
            var userService = new UserService(_userDataMock.Object, _userManagerMock.Object, _httpContextAccessorMock.Object);

            await userService.GetCurrentUser();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task GetCurrentUser_ThrowsException_WhenUserIdClaimMissing()
        {
            var identity = new ClaimsIdentity(new List<Claim>(), "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            var httpContext = new DefaultHttpContext
            {
                User = claimsPrincipal
            };

            _httpContextAccessorMock.Setup(h => h.HttpContext).Returns(httpContext);
            var userService = new UserService(_userDataMock.Object, _userManagerMock.Object, _httpContextAccessorMock.Object);

            await userService.GetCurrentUser();
        }

        [TestMethod]
        public async Task GetAllUsers_ReturnsEmptyList_WhenNoUsers()
        {
            _userDataMock.Setup(u => u.GetAllUsers()).ReturnsAsync(new List<UserDetails>());

            var result = await _userService.GetAllUsers();

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }
    }
}

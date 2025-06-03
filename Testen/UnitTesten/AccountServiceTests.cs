using Microsoft.VisualStudio.TestTools.UnitTesting;
using BLL;
using BLL.Models;
using Moq;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Testen.UnitTesten
{
    [TestClass]
    public class AccountServiceTests
    {
        private Mock<IAccountData> _accountDataMock;
        private AccountService _accountService;

        [TestInitialize]
        public void Setup()
        {
            _accountDataMock = new Mock<IAccountData>();
            _accountService = new AccountService(_accountDataMock.Object);
        }

        [TestMethod]
        public async Task RegisterAsync_ReturnsSuccessMessage_WhenRegistrationSucceeds()
        {
            var model = new RegisterModel
            {
                UserName = "testuser",
                Email = "test@example.com",
                Password = "Test123!"
            };

            var identityResult = IdentityResult.Success;

            _accountDataMock
                .Setup(a => a.CreateUserAsync(It.IsAny<IdentityUser>(), model.Password))
                .ReturnsAsync(identityResult);

            var result = await _accountService.RegisterAsync(model);

            Assert.AreEqual("Registration successful", result);
        }

        [TestMethod]
        public async Task RegisterAsync_ReturnsErrorMessages_WhenRegistrationFails()
        {
            var model = new RegisterModel
            {
                UserName = "testuser",
                Email = "test@example.com",
                Password = "Test123!"
            };

            var identityResult = IdentityResult.Failed(new IdentityError { Description = "Email is invalid" });

            _accountDataMock
                .Setup(a => a.CreateUserAsync(It.IsAny<IdentityUser>(), model.Password))
                .ReturnsAsync(identityResult);

            var result = await _accountService.RegisterAsync(model);

            Assert.AreEqual("Email is invalid", result);
        }

        [TestMethod]
        public async Task LoginAsync_ReturnsJwtToken_WhenLoginSucceeds()
        {
            var model = new LoginModel
            {
                UserName = "testuser",
                Password = "Test123!"
            };

            var user = new IdentityUser
            {
                UserName = model.UserName,
                Id = "user-id-123"
            };

            _accountDataMock.Setup(a => a.FindByUserNameAsync(model.UserName)).ReturnsAsync(user);
            _accountDataMock.Setup(a => a.LoginAsync(model)).ReturnsAsync(SignInResult.Success);

            var result = await _accountService.LoginAsync(model);

            Assert.IsFalse(string.IsNullOrWhiteSpace(result));
            Assert.IsTrue(result.Contains("."));
        }

        // Alt flows

        [TestMethod]
        public async Task LoginAsync_ReturnsError_WhenUserNotFound()
        {
            var model = new LoginModel
            {
                UserName = "notfound",
                Password = "irrelevant"
            };

            _accountDataMock.Setup(a => a.FindByUserNameAsync(model.UserName)).ReturnsAsync((IdentityUser)null);

            var result = await _accountService.LoginAsync(model);

            Assert.AreEqual("Invalid login attempt", result);
        }

        [TestMethod]
        public async Task LoginAsync_ReturnsError_WhenLoginFails()
        {
            var model = new LoginModel
            {
                UserName = "testuser",
                Password = "wrongpass"
            };

            var user = new IdentityUser { UserName = model.UserName, Id = "user-id-123" };

            _accountDataMock.Setup(a => a.FindByUserNameAsync(model.UserName)).ReturnsAsync(user);
            _accountDataMock.Setup(a => a.LoginAsync(model)).ReturnsAsync(SignInResult.Failed);

            var result = await _accountService.LoginAsync(model);

            Assert.AreEqual("Invalid login attempt", result);
        }

        [TestMethod]
        public async Task LogoutAsync_CallsLogoutMethod()
        {
            await _accountService.LogoutAsync();

            _accountDataMock.Verify(a => a.LogoutAsync(), Times.Once);
        }
    }
}

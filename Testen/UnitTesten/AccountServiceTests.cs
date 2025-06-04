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
        public async Task RegisterAsync_ReturnsSuccessMessage()
        {
            var model = new RegisterModel
            {
                UserName = "testuser",
                Email = "test@example.com",
                Password = "Test123!"
            };

            _accountDataMock
                .Setup(a => a.CreateUserAsync(It.IsAny<IdentityUser>(), model.Password))
                .ReturnsAsync(IdentityResult.Success);

            var result = await _accountService.RegisterAsync(model);

            Assert.AreEqual("Registration successful", result);
        }

        [TestMethod]
        public async Task LoginAsync_ReturnsToken_WhenSuccessful()
        {
            var model = new LoginModel
            {
                UserName = "testuser",
                Password = "Test123!"
            };

            var user = new IdentityUser { UserName = model.UserName };

            _accountDataMock.Setup(a => a.FindByUserNameAsync(model.UserName)).ReturnsAsync(user);
            _accountDataMock.Setup(a => a.LoginAsync(model)).ReturnsAsync(SignInResult.Success);

            var result = await _accountService.LoginAsync(model);

            Assert.IsFalse(string.IsNullOrWhiteSpace(result));
        }

        [TestMethod]
        public async Task LogoutAsync_CallsLogout()
        {
            await _accountService.LogoutAsync();

            _accountDataMock.Verify(a => a.LogoutAsync(), Times.Once);
        }
        
        // Alt flow
        
        [TestMethod]
        public async Task LoginAsync_ReturnsError_WhenUserNotFound()
        {
            var model = new LoginModel
            {
                UserName = "unknown",
                Password = "irrelevant"
            };

            _accountDataMock.Setup(a => a.FindByUserNameAsync(model.UserName)).ReturnsAsync((IdentityUser)null);

            var result = await _accountService.LoginAsync(model);

            Assert.AreEqual("Invalid login attempt", result);
        }
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using BLL;
using Moq;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;


namespace Testen.UnitTesten
{
    [TestClass]
    public class GewichtServiceTests
    {
        private Mock<IGewichtData> _gewichtDataMock;
        private Mock<UserManager<IdentityUser>> _userManagerMock;
        private Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private GewichtService _gewichtService;

        private const string TestUserId = "test-user-id";

        [TestInitialize]
        public void Setup()
        {
            _gewichtDataMock = new Mock<IGewichtData>();

            // Mock van UserManager is wat complexer, hier een basic dummy:
            var userStoreMock = new Mock<IUserStore<IdentityUser>>();
            _userManagerMock = new Mock<UserManager<IdentityUser>>(userStoreMock.Object, null, null, null, null, null, null, null, null);

            // Setup IHttpContextAccessor mock met een HttpContext met authenticated user met userId claim
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

            _gewichtService = new GewichtService(_gewichtDataMock.Object, _userManagerMock.Object, _httpContextAccessorMock.Object);
        }

        [TestMethod]
        public async Task SetGewicht_CallsDataLayerWithCorrectValueAndUserId()
        {
            double Gewicht = 75.5;

            await _gewichtService.SetGewicht(Gewicht);

            _gewichtDataMock.Verify(g => g.SetGewicht(Gewicht, TestUserId), Times.Once);
        }

        [TestMethod]
        public async Task GetGewicht_CallsDataLayerWithUserIdAndReturnsList()
        {
            var expectedList = new List<GewichtDetails>
            {
                new GewichtDetails { Gewicht = 75.5, Id = 1 },
                new GewichtDetails { Gewicht = 80.0, Id = 2 }
            };

            _gewichtDataMock.Setup(g => g.GetGewicht(TestUserId)).ReturnsAsync(expectedList);

            var result = await _gewichtService.GetGewicht();

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(75.5, result[0].Gewicht);
            Assert.AreEqual(80.0, result[1].Gewicht);
            _gewichtDataMock.Verify(g => g.GetGewicht(TestUserId), Times.Once);
        }

        [TestMethod]
        public async Task GetGewicht_ById_CallsDataLayerWithCorrectIdAndUserId()
        {
            int id = 1;
            var expected = new GewichtDetails { Gewicht = 75.5, Id = id };

            _gewichtDataMock.Setup(g => g.GetGewicht(id, TestUserId)).ReturnsAsync(expected);

            var result = await _gewichtService.GetGewicht(id);

            Assert.AreEqual(expected.Id, result.Id);
            Assert.AreEqual(expected.Gewicht, result.Gewicht);
            _gewichtDataMock.Verify(g => g.GetGewicht(id, TestUserId), Times.Once);
        }

        [TestMethod]
        public async Task EditGewicht_CallsDataLayerWithCorrectParametersAndUserId()
        {
            int id = 1;
            double gewicht = 70.0;

            await _gewichtService.EditGewicht(id, gewicht);

            _gewichtDataMock.Verify(g => g.EditGewicht(id, gewicht, TestUserId), Times.Once);
        }

        [TestMethod]
        public async Task DeleteGewicht_CallsDataLayerWithCorrectIdAndUserId()
        {
            int id = 5;

            await _gewichtService.DeleteGewicht(id);

            _gewichtDataMock.Verify(g => g.DeleteGewicht(id, TestUserId), Times.Once);
        }

        [TestMethod]
        public async Task GetDoelGewicht_CallsDataLayerWithUserIdAndReturnsList()
        {
            var expectedList = new List<DoelGewichtDetails>
            {
                new DoelGewichtDetails { Doelgewicht = 70.0, Id = 1 },
                new DoelGewichtDetails { Doelgewicht = 68.0, Id = 2 }
            };

            _gewichtDataMock.Setup(g => g.GetDoelGewicht(TestUserId)).ReturnsAsync(expectedList);

            var result = await _gewichtService.GetDoelGewicht();

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(70.0, result[0].Doelgewicht);
            Assert.AreEqual(68.0, result[1].Doelgewicht);
            _gewichtDataMock.Verify(g => g.GetDoelGewicht(TestUserId), Times.Once);
        }

        [TestMethod]
        public async Task SetDoelGewicht_CallsDataLayerWithCorrectValueAndUserId()
        {
            double doelGewicht = 68.5;

            await _gewichtService.SetDoelGewicht(doelGewicht);

            _gewichtDataMock.Verify(g => g.SetDoelGewicht(doelGewicht, TestUserId), Times.Once);
        }

        [TestMethod]
        public async Task EditDoelGewicht_CallsDataLayerWithCorrectParametersAndUserId()
        {
            int id = 1;
            double? doelGewicht = 67.0;
            DateTime? datumBehaald = DateTime.Today;

            await _gewichtService.EditDoelGewicht(id, doelGewicht, datumBehaald);

            _gewichtDataMock.Verify(g => g.EditDoelGewicht(id, doelGewicht, datumBehaald, TestUserId), Times.Once);
        }

        [TestMethod]
        public async Task DeleteDoelGewicht_CallsDataLayerWithCorrectIdAndUserId()
        {
            int id = 3;

            await _gewichtService.DeleteDoelGewicht(id);

            _gewichtDataMock.Verify(g => g.DeleteDoelGewicht(id, TestUserId), Times.Once);
        }
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using BLL;
using Moq;

namespace Testen.UnitTesten;

[TestClass]
public class GewichtServiceTests
{
    private Mock<IGewichtData> _gewichtDataMock;
    private GewichtService _gewichtService;

    [TestInitialize]
    public void Setup()
    {
        _gewichtDataMock = new Mock<IGewichtData>();
        _gewichtService = new GewichtService(_gewichtDataMock.Object);
    }

    [TestMethod]
    public async Task SetGewicht_CallsDataLayerWithCorrectValue()
    {
        // Arrange
        double Gewicht = 75.5;

        // Act
        await _gewichtService.SetGewicht(Gewicht);

        // Assert
        _gewichtDataMock.Verify(g => g.SetGewicht(Gewicht), Times.Once);
    }
    
    [TestMethod]
    public async Task GetGewicht_CallsDataLayerAndReturnsList()
    {
        // Arrange
        var expectedList = new List<GewichtDetails>
        {
            new GewichtDetails { Gewicht = 75.5, Id = 1 },
            new GewichtDetails { Gewicht = 80.0, Id = 2 }
        };
        _gewichtDataMock.Setup(g => g.GetGewicht()).ReturnsAsync(expectedList);

        // Act
        var result = await _gewichtService.GetGewicht();

        // Assert
        Assert.AreEqual(2, result.Count);
        Assert.AreEqual(75.5, result[0].Gewicht);
        Assert.AreEqual(80.0, result[1].Gewicht);
        _gewichtDataMock.Verify(g => g.GetGewicht(), Times.Once);
    }

    [TestMethod]
    public async Task GetGewicht_ById_CallsDataLayerWithCorrectId()
    {
        // Arrange
        int id = 1;
        var expected = new GewichtDetails { Gewicht = 75.5, Id = id };
        _gewichtDataMock.Setup(g => g.GetGewicht(id)).ReturnsAsync(expected);

        // Act
        var result = await _gewichtService.GetGewicht(id);

        // Assert
        Assert.AreEqual(expected.Id, result.Id);
        Assert.AreEqual(expected.Gewicht, result.Gewicht);
        _gewichtDataMock.Verify(g => g.GetGewicht(id), Times.Once);
    }

    [TestMethod]
    public async Task EditGewicht_CallsDataLayerWithCorrectParameters()
    {
        // Arrange
        int id = 1;
        double gewicht = 70.0;

        // Act
        await _gewichtService.EditGewicht(id, gewicht);

        // Assert
        _gewichtDataMock.Verify(g => g.EditGewicht(id, gewicht), Times.Once);
    }

    [TestMethod]
    public async Task DeleteGewicht_CallsDataLayerWithCorrectId()
    {
        // Arrange
        int id = 5;

        // Act
        await _gewichtService.DeleteGewicht(id);

        // Assert
        _gewichtDataMock.Verify(g => g.DeleteGewicht(id), Times.Once);
    }

    [TestMethod]
    public async Task GetDoelGewicht_CallsDataLayerAndReturnsList()
    {
        // Arrange
        var expectedList = new List<DoelGewichtDetails>
        {
            new DoelGewichtDetails { Doelgewicht = 70.0, Id = 1 },
            new DoelGewichtDetails { Doelgewicht = 68.0, Id = 2 }
        };
        _gewichtDataMock.Setup(g => g.GetDoelGewicht()).ReturnsAsync(expectedList);

        // Act
        var result = await _gewichtService.GetDoelGewicht();

        // Assert
        Assert.AreEqual(2, result.Count);
        Assert.AreEqual(70.0, result[0].Doelgewicht);
        Assert.AreEqual(68.0, result[1].Doelgewicht);
        _gewichtDataMock.Verify(g => g.GetDoelGewicht(), Times.Once);
    }

    [TestMethod]
    public async Task SetDoelGewicht_CallsDataLayerWithCorrectValue()
    {
        // Arrange
        double doelGewicht = 68.5;

        // Act
        await _gewichtService.SetDoelGewicht(doelGewicht);

        // Assert
        _gewichtDataMock.Verify(g => g.SetDoelGewicht(doelGewicht), Times.Once);
    }

    [TestMethod]
    public async Task EditDoelGewicht_CallsDataLayerWithCorrectParameters()
    {
        // Arrange
        int id = 1;
        double? doelGewicht = 67.0;
        DateTime? datumBehaald = DateTime.Today;

        // Act
        await _gewichtService.EditDoelGewicht(id, doelGewicht, datumBehaald);

        // Assert
        _gewichtDataMock.Verify(g => g.EditDoelGewicht(id, doelGewicht, datumBehaald), Times.Once);
    }

    [TestMethod]
    public async Task DeleteDoelGewicht_CallsDataLayerWithCorrectId()
    {
        // Arrange
        int id = 3;

        // Act
        await _gewichtService.DeleteDoelGewicht(id);

        // Assert
        _gewichtDataMock.Verify(g => g.DeleteDoelGewicht(id), Times.Once);
    }
}
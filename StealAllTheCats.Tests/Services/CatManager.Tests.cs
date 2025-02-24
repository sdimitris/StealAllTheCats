using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using StealAllTheCats.Application.Interfaces;
using StealAllTheCats.Application.Services;
using StealAllTheCats.Domain.Common.Result;
using StealAllTheCats.Domain.Entities;

namespace StealAllTheCats.Tests.Services;

public class CatManagerTests
{
    private readonly Mock<ICatService> _catServiceMock = new();

    public CatManagerTests()
    {
    }

    [Fact]
    public async Task FetchCatsAsync_ShouldReturnSuccess_WhenCatsAreFetched()
    {
        // Arrange
        var catApiResponse = new CatApiResponse
        {
            Id = "1",
            Breeds = new List<Breed>
            {
                new Breed
                {
                    Temperament = "Affectionate, Intelligent, Curious, Social, Playful"
                }
            },
            Height = 555,
            Width = 400,
            Url = "https://cdn2.thecatapi.com/images/1.jpg"
        };

        var catApiResponseList = new List<CatApiResponse> { catApiResponse };

        var catsApiHttpServiceMock = new Mock<ICatsApiHttpService>();
        catsApiHttpServiceMock.Setup(x => x.GetCatsAsync(1, 25))
            .ReturnsAsync(Result<IEnumerable<CatApiResponse>>.Ok(catApiResponseList));
        catsApiHttpServiceMock.Setup(x => x.DownloadImageFromApiAsync(It.IsAny<string>()))
            .ReturnsAsync(Result<byte[]>.Ok(new byte[1]));

        _catServiceMock.Setup(x => x.GetCatByCatIdAsync("1")).ReturnsAsync(Result<CatEntity?>.Ok(null));
        _catServiceMock.Setup(x => x.AddCatAsync(It.IsAny<CatEntity>())).ReturnsAsync(Result.Ok);
        _catServiceMock.Setup(service => service.UpdateCatAsync(It.IsAny<CatEntity>()))
            .ReturnsAsync(Result<bool>.Ok(true));
        
        var serviceProvider = new Mock<IServiceProvider>();
        var serviceScope = new Mock<IServiceScope>();
        serviceProvider.Setup(x => x.GetService(typeof(ICatService))).Returns(_catServiceMock.Object);
        serviceScope.Setup(x => x.ServiceProvider).Returns(serviceProvider.Object);

        var serviceScopeFactory = new Mock<IServiceScopeFactory>();
        serviceScopeFactory
            .Setup(x => x.CreateScope())
            .Returns(serviceScope.Object);

        
        catsApiHttpServiceMock
            .Setup(service => service.DownloadImageFromApiAsync(It.IsAny<string>()))
            .ReturnsAsync(Result<byte[]>.Ok(new byte[]{1}));
        
        var breedServiceMock = new Mock<IBreedService>();
        breedServiceMock.Setup(x => x.ConstructBreeds("Affectionate, Intelligent, Curious, Social, Playful"))
            .ReturnsAsync(Result<IEnumerable<CatTag>>.Ok(new List<CatTag>()));

        var loggerMock = new Mock<ILogger<CatManager>>();
        var catManager = new CatManager(_catServiceMock.Object, breedServiceMock.Object, catsApiHttpServiceMock.Object,
            loggerMock.Object, serviceScopeFactory.Object);

        // Act
        var result = await catManager.FetchCatsAsync();

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task FetchCatsAsync_ShouldNotSaveToDb_WhenCatsExist()
    {
        // Arrange
        var catApiResponse = new CatApiResponse
        {
            Id = "1",
            Breeds = new List<Breed>
            {
                new Breed
                {
                    Temperament = "Affectionate, Intelligent, Curious, Social, Playful"
                }
            },
            Height = 555,
            Width = 400,
            Url = "https://cdn2.thecatapi.com/images/1.jpg"
        };

        var catApiResponseList = new List<CatApiResponse> { catApiResponse };

        var catsApiHttpServiceMock = new Mock<ICatsApiHttpService>();
        catsApiHttpServiceMock.Setup(x => x.GetCatsAsync(1, 25))
            .ReturnsAsync(Result<IEnumerable<CatApiResponse>>.Ok(catApiResponseList));
        catsApiHttpServiceMock.Setup(x => x.DownloadImageFromApiAsync(It.IsAny<string>()))
            .ReturnsAsync(Result<byte[]>.Ok(new byte[1]));

        var c = new CatEntity
        {
            Id = 1,
            CatId = "1",
            ImageData = new byte[1],
            Width = 200,
            Height = 200,
        };

        _catServiceMock.Setup(x => x.GetCatByCatIdAsync("1")).ReturnsAsync(Result<CatEntity?>.Ok(c));
        _catServiceMock.Setup(service => service.UpdateCatAsync(It.IsAny<CatEntity>()))
            .ReturnsAsync(Result<bool>.Ok(true));

        var breedServiceMock = new Mock<IBreedService>();
        breedServiceMock.Setup(x => x.ConstructBreeds("Affectionate, Intelligent, Curious, Social, Playful"))
            .ReturnsAsync(Result<IEnumerable<CatTag>>.Ok(new List<CatTag>()));

        var loggerMock = new Mock<ILogger<CatManager>>();

        var serviceProvider = new Mock<IServiceProvider>();
        var serviceScope = new Mock<IServiceScope>();
        serviceScope.Setup(x => x.ServiceProvider).Returns(serviceProvider.Object);
        serviceProvider.Setup(x => x.GetService(typeof(ICatService))).Returns(_catServiceMock.Object);
        var serviceScopeFactory = new Mock<IServiceScopeFactory>();
        serviceScopeFactory
            .Setup(x => x.CreateScope())
            .Returns(serviceScope.Object);
        
        catsApiHttpServiceMock
            .Setup(service => service.DownloadImageFromApiAsync(It.IsAny<string>()))
            .ReturnsAsync(Result<byte[]>.Ok(new byte[]{1}));

        var catManager = new CatManager(
            _catServiceMock.Object,
            breedServiceMock.Object,
            catsApiHttpServiceMock.Object,
            loggerMock.Object,
            serviceScopeFactory.Object
        );

        // Act
        var result = await catManager.FetchCatsAsync();

        // Assert
        _catServiceMock.Verify(x => x.AddCatAsync(It.IsAny<CatEntity>()), Times.Never);
        Assert.True(result.IsSuccess);
    }
}
using Microsoft.Extensions.Logging;
using Moq;
using StealAllTheCats.Application.Interfaces;
using StealAllTheCats.Application.Services;
using StealAllTheCats.Domain.Common.Result;
using StealAllTheCats.Domain.Dtos;
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

        _catServiceMock.Setup(x => x.GetCatByCatIdAsync("1")).ReturnsAsync(Result<CatDto?>.Ok(null));
        _catServiceMock.Setup(x => x.AddCatAsync(It.IsAny<CatEntity>())).ReturnsAsync(Result.Ok);
        
        var breedServiceMock = new Mock<IBreedService>();
        breedServiceMock.Setup(x => x.ConstructBreeds("Affectionate, Intelligent, Curious, Social, Playful"))
            .ReturnsAsync(Result<IEnumerable<CatTag>>.Ok(new List<CatTag>()));

        var loggerMock = new Mock<ILogger<CatManager>>();
        var catManager = new CatManager(_catServiceMock.Object, breedServiceMock.Object, catsApiHttpServiceMock.Object,
            loggerMock.Object);

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

        var c = new CatDto()
        {
            CatId = "1",
            ImageUrl = "someurl",
            Width = 200,
            Height = 200,
        };

        _catServiceMock.Setup(x => x.GetCatByCatIdAsync("1")).ReturnsAsync(Result<CatDto?>.Ok(c));

        var breedServiceMock = new Mock<IBreedService>();
        breedServiceMock.Setup(x => x.ConstructBreeds("Affectionate, Intelligent, Curious, Social, Playful"))
            .ReturnsAsync(Result<IEnumerable<CatTag>>.Ok(new List<CatTag>()));

        var loggerMock = new Mock<ILogger<CatManager>>();
        var catManager = new CatManager(
            _catServiceMock.Object,
            breedServiceMock.Object,
            catsApiHttpServiceMock.Object,
            loggerMock.Object
        );

        // Act
        var result = await catManager.FetchCatsAsync();

        // Assert
        _catServiceMock.Verify(x => x.AddCatAsync(It.IsAny<CatEntity>()), Times.Never);
        Assert.True(result.IsSuccess);
    }
}
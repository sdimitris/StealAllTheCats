using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using StealAllTheCats.Application.Services;
using StealAllTheCats.Domain.Common.Result;
using StealAllTheCats.Domain.Entities;
using StealAllTheCats.Domain.Repositories;

namespace StealAllTheCats.Tests.Services;

public class BreedServiceTests
{
    
    [Fact]
    public async Task ConstructBreeds_WhenCalled_ReturnsOkResult_IfTagExist()
    {
        // Arrange
        var catTagRepository = new Mock<ICatTagRepository>();
        var logger = new Mock<ILogger<BreedService>>();
        var friendlyTag = new TagEntity { Name = "calm", Id = 1 };
        var calmTag = new TagEntity() { Name = "friendly", Id = 2 };
        catTagRepository.Setup(x => x.GetCatTagByNameAsync("calm"))!
            .ReturnsAsync(Result<TagEntity>.Ok(calmTag));
        catTagRepository.Setup(x => x.GetCatTagByNameAsync("friendly"))!
            .ReturnsAsync(Result<TagEntity>.Ok(friendlyTag));
        var breedService = new BreedService(catTagRepository.Object, logger.Object);

        // Act
        var result = await breedService.ConstructBreeds("calm,friendly");
        
        var expectedTags = new List<CatTag>
        {
            new CatTag { Tag = calmTag },
            new CatTag { Tag = friendlyTag }
        };

        Assert.Equal(JsonConvert.SerializeObject(expectedTags), JsonConvert.SerializeObject(result.Value));
        // Assert
        
        Assert.True(result.IsSuccess);
    }
    
    [Fact]
    public async Task ConstructBreeds_WhenCalled_ReturnsOkResult_IfTagNotExist()
    {
        // Arrange
        var catTagRepository = new Mock<ICatTagRepository>();
        var logger = new Mock<ILogger<BreedService>>();
        catTagRepository.Setup(x => x.GetCatTagByNameAsync("calm"))!
            .ReturnsAsync(Result<TagEntity>.Ok(null));
        catTagRepository.Setup(x => x.GetCatTagByNameAsync("friendly"))!
            .ReturnsAsync(Result<TagEntity>.Ok(null));
        catTagRepository.Setup(x => x.SaveChangesAsync())!
            .ReturnsAsync(Result.Ok());
        var breedService = new BreedService(catTagRepository.Object, logger.Object);

        
        catTagRepository.Setup(x => x.AddCatTagAsync(It.IsAny<TagEntity>()))
            .ReturnsAsync(Result.Ok());
        
        // Act
        var result = await breedService.ConstructBreeds("calm,friendly");
        // Assert
        

        
        catTagRepository.Verify(x => x.AddCatTagAsync(It.IsAny<TagEntity>()), Times.Exactly(2));
        Assert.Equal(2,result.Value.Count());
        Assert.True(result.IsSuccess);
    }
}
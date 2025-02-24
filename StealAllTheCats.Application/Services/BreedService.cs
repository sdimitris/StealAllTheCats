using Microsoft.Extensions.Logging;
using StealAllTheCats.Application.Interfaces;
using StealAllTheCats.Domain.Common.Result;
using StealAllTheCats.Domain.Entities;
using StealAllTheCats.Domain.Repositories;

namespace StealAllTheCats.Application.Services;

public class BreedService : IBreedService
{
    private readonly ICatTagRepository _catTagRepository;
    private readonly ILogger<BreedService> _logger;
    public BreedService(ICatTagRepository catTagRepository, ILogger<BreedService> logger)
    {
        ArgumentNullException.ThrowIfNull(_catTagRepository = catTagRepository);
        ArgumentNullException.ThrowIfNull(_logger = logger);

    }

    public async Task<Result<IEnumerable<CatTag>>> ConstructBreeds(string temperament)
    {
            List<CatTag> catTags = new();
            foreach (var breedString in temperament.Replace(" ", "").Split(","))
            {
                var tagResult = await GetTagByNameAsync(breedString);
                if (tagResult.IsFailure) 
                    return Result<IEnumerable<CatTag>>.FromFailure(tagResult);

                if (tagResult.Value == null)
                {
                    var newTagEntity = new TagEntity
                    {
                        Name = breedString,
                        Created = DateTime.UtcNow, //should moved to db
                    };

                    var addTagResult = await AddTagAsync(newTagEntity);
                    if(addTagResult.IsFailure)
                        return Result<IEnumerable<CatTag>>.Failure(addTagResult.Error);
                    
                    catTags.Add(new CatTag { Tag = newTagEntity });
                }
                else
                {
                    _logger.LogWarning("Tag: {TagName} already exists", breedString);
                    catTags.Add(new CatTag { Tag = tagResult.Value });
                }
            }

            return Result<IEnumerable<CatTag>>.Ok(catTags);
    }
    
    private async Task<Result<TagEntity?>> GetTagByNameAsync(string tagName)
    {
        //should add caching here and store the tag ids that we already have
        var catResult = await _catTagRepository.GetCatTagByNameAsync(tagName);
        if (catResult.IsFailure)
        {
            return Result<TagEntity?>.FromFailure(catResult);
        }

        return catResult;
    }

    private async Task<Result> AddTagAsync(TagEntity tag)
    {
        var addResult = await _catTagRepository.AddCatTagAsync(tag);
        if (addResult.IsFailure)
        {
            return Result.FromFailure(addResult);
        }
        
        var saveResult = await _catTagRepository.SaveChangesAsync();
        if (saveResult.IsFailure)
        {
            return Result.FromFailure(saveResult);
        }
        
        return Result.Ok();
    }
}
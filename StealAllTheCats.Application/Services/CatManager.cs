using Microsoft.Extensions.Logging;
using StealAllTheCats.Application.Interfaces;
using StealAllTheCats.Domain.Common.Result;
using StealAllTheCats.Domain.Entities;

namespace StealAllTheCats.Application.Services;

public class CatManager : ICatManager
{
    private readonly ICatService _catService;
    private readonly IBreedService _breedService;
    private readonly ICatsApiHttpService _catsApiHttpService;
    private readonly ILogger<CatManager> _logger;

    public CatManager(ICatService catService, IBreedService breedService, ICatsApiHttpService catsApiHttpService, ILogger<CatManager> logger)
    {
        ArgumentNullException.ThrowIfNull(_catService = catService);
        ArgumentNullException.ThrowIfNull(_breedService = breedService);
        ArgumentNullException.ThrowIfNull(_catsApiHttpService = catsApiHttpService);
        ArgumentNullException.ThrowIfNull(_logger = logger);
    }

    public async Task<Result> FetchCatsAsync()
    {
        var catsResult = await _catsApiHttpService.GetCatsAsync(1, 25);
        if (catsResult.IsFailure)
        {
            return Result.FromFailure(catsResult);
        }
        
        foreach (var cat in catsResult.Value)
        {
            List<CatTag> catTags = new();

            //should add caching here and store the ids of the cats that we already have
            var catFromDb = await _catService.GetCatByCatIdAsync(cat.Id);
            if (catFromDb.IsFailure )
            {
                return Result.FromFailure(catFromDb);
            }

            if (catFromDb.Value is not null)
            {
                _logger.LogWarning("Skipping cat: {CatId} because already exists", cat.Id);
                continue;
            }

            foreach (var breed in cat.Breeds)
            {
                var catTagsResult = await _breedService.ConstructBreeds(breed.Temperament);
                if (catTagsResult.IsFailure)
                {
                    return Result.FromFailure(catTagsResult);
                }

                catTags.AddRange(catTagsResult.Value);
            }

            var catEntity = new CatEntity
            {
                CatId = cat.Id,
                Width = cat.Width,
                Height = cat.Height,
                ImageUrl = cat.Url,
                CatTags = catTags
            };

            var addCatResult = await _catService.AddCatAsync(catEntity);
            if (addCatResult.IsFailure)
            {
                return Result.FromFailure(addCatResult);
            }
        }
        
        return Result.Ok();
    }

}
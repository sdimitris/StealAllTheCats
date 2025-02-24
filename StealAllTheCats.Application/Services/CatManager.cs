using Microsoft.Extensions.DependencyInjection;
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
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public CatManager(ICatService catService, IBreedService breedService, ICatsApiHttpService catsApiHttpService, ILogger<CatManager> logger, IServiceScopeFactory scopeFactory)
    {
        ArgumentNullException.ThrowIfNull(_catService = catService);
        ArgumentNullException.ThrowIfNull(_breedService = breedService);
        ArgumentNullException.ThrowIfNull(_catsApiHttpService = catsApiHttpService);
        ArgumentNullException.ThrowIfNull(_logger = logger);
        ArgumentNullException.ThrowIfNull(_serviceScopeFactory = scopeFactory);
    }

    public async Task<Result> FetchCatsAsync()
    {
        var catsResult = await _catsApiHttpService.GetCatsAsync(1, 25);
        if (catsResult.IsFailure)
        {
            return Result.FromFailure(catsResult);
        }

        var tasks = new List<Task>();

        foreach (var cat in catsResult.Value)
        {
            List<CatTag> catTags = new();

            var catFromDb = await _catService.GetCatByCatIdAsync(cat.Id);
            if (catFromDb.IsFailure)
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
                CatTags = catTags
            };

            var addCatResult = await _catService.AddCatAsync(catEntity);
            if (addCatResult.IsFailure)
            {
                return Result.FromFailure(addCatResult);
            }

            var imageDownloadTask = DownloadImageAndUpdateCatAsync(catEntity, cat.Url);
            tasks.Add(imageDownloadTask);
        }

        await Task.WhenAll(tasks);

        return Result.Ok();
    }

    private async Task<Result> DownloadImageAndUpdateCatAsync(CatEntity catEntity, string imageUrl)
    {
        var imageData = await _catsApiHttpService.DownloadImageFromApiAsync(new Uri(imageUrl).ToString());
        if (imageData.IsFailure)
        {
            return Result.FromFailure(imageData);
        }

        catEntity.ImageData = imageData.Value;

        using var scope = _serviceScopeFactory.CreateScope();
        var catService = scope.ServiceProvider.GetRequiredService<ICatService>();
        var updateCatResult = await catService.UpdateCatAsync(catEntity);

        if (updateCatResult.IsFailure)
        {
            return Result.FromFailure(updateCatResult);
        }

        return Result.Ok();
    }

}
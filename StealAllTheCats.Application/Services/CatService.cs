using StealAllTheCats.Application.Interfaces;
using StealAllTheCats.Domain.Common.Enums;
using StealAllTheCats.Domain.Common.Result;
using StealAllTheCats.Domain.Entities;
using StealAllTheCats.Domain.Repositories;
namespace StealAllTheCats.Application.Services;

public class CatService : ICatService
{
    private readonly ICatRepository _catRepository;

    public CatService(ICatRepository catRepository)
    {
        ArgumentNullException.ThrowIfNull(_catRepository = catRepository);
    }

    public async Task<Result<IEnumerable<CatEntity>>> GetCatsAsync(int page, int pageSize)
    {
        
        var catsResult =  await _catRepository.GetCatsAsync(page, pageSize);
        if (catsResult.IsFailure)
        {
            return Result<IEnumerable<CatEntity>>.FromFailure(catsResult);
        }
        
        return Result<IEnumerable<CatEntity>>.Ok(catsResult.Value);
    }
    
    public async Task<Result<CatEntity?>> GetCatByCatIdAsync(string catId)
    {
        var catResult = await _catRepository.GetCatByCatIdAsync(catId);
        if (catResult.IsFailure)
        {
            return Result<CatEntity?>.FromFailure(catResult);
        }

        return Result<CatEntity?>.Ok(catResult.Value);;
    }
    
    public async Task<Result<IEnumerable<CatEntity>>> GetCatsByTagAsync(string tag, int page, int pageSize)
    {
        try
        {
            var cats = await _catRepository.GetCatsByTagAsync(tag, page, pageSize);
            if (cats.IsFailure)
            {
                return Result<IEnumerable<CatEntity>>.FromFailure(cats);
            }
            
            return Result<IEnumerable<CatEntity>>.Ok(cats.Value);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<CatEntity>>.Failure(Error.New("An error occurred while fetching the cats from the database", ex, KnownApplicationErrorEnum.SqlGenericError));
        }
    }
    
    public async Task<Result> AddCatAsync(CatEntity catEntity)
    {
        var addCatResult = await _catRepository.AddCatAsync(catEntity);
        
        if (addCatResult.IsFailure)
        {
            return Result.FromFailure(addCatResult);
        }

        var saveResult = await _catRepository.SaveChangesAsync();
        
        if (saveResult.IsFailure)
        {
            return Result.FromFailure(addCatResult);
        }
        
        return Result.Ok();
    }
}
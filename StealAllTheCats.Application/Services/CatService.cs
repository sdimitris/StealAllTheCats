using StealAllTheCats.Application.Interfaces;
using StealAllTheCats.Domain.Common.Enums;
using StealAllTheCats.Domain.Common.Result;
using StealAllTheCats.Domain.Dtos;
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

    public async Task<Result<IEnumerable<CatDto>>> GetCatsAsync(int page, int pageSize)
    {
        var catsResult = await _catRepository.GetCatsAsync(page, pageSize);
        if (catsResult.IsFailure)
        {
            return Result<IEnumerable<CatDto>>.FromFailure(catsResult);
        }


        return Result<IEnumerable<CatDto>>.Ok(ConvertToDtoListFromCatEntityList(catsResult.Value));
    }

    public async Task<Result<CatDto?>> GetCatByCatIdAsync(string catId)
    {
        var catResult = await _catRepository.GetCatByCatIdAsync(catId);
        if (catResult.IsFailure)
        {
            return Result<CatDto?>.FromFailure(catResult);
        }

        return Result<CatDto?>.Ok(ConvertToCatDto(catResult.Value));
    }

    public async Task<Result<IEnumerable<CatDto>>> GetCatsByTagAsync(string tag, int page, int pageSize)
    {
        try
        {
            var cats = await _catRepository.GetCatsByTagAsync(tag, page, pageSize);
            if (cats.IsFailure)
            {
                return Result<IEnumerable<CatDto>>.FromFailure(cats);
            }


            return Result<IEnumerable<CatDto>>.Ok(ConvertToDtoListFromCatEntityList(cats.Value));
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<CatDto>>.Failure(Error.New(
                "An error occurred while fetching the cats from the database", ex,
                KnownApplicationErrorEnum.SqlGenericError));
        }
    }

    public async Task<Result<bool>> UpdateCatAsync(CatEntity catEntity)
    {
        var existingCat = await _catRepository.GetCatByCatIdAsync(catEntity.CatId);
        if (existingCat.IsFailure)
        {
            return Result<bool>.FromFailure(existingCat);
        }

        if (existingCat.Value is null)
        {
            return Result<bool>.Failure(Error.New($"Cat with id: {catEntity.CatId} not found", null,
                KnownApplicationErrorEnum.CatNotFound));
        }

        var updateResult = await _catRepository.UpdateCatΙmageDataAsync(existingCat.Value, catEntity);
        if (updateResult.IsFailure)
        {
            return Result<bool>.FromFailure(updateResult);
        }

        return Result<bool>.Ok(true);
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

    private IEnumerable<CatDto> ConvertToDtoListFromCatEntityList(IEnumerable<CatEntity> list)
    {
        var dtoList = new List<CatDto>();
        foreach (var catEntity in list)
        {
            
            var catDto = ConvertToCatDto(catEntity);
            if (catDto is null)
                continue;
            dtoList.Add(catDto);
        }

        return dtoList;
    }

    private CatDto? ConvertToCatDto(CatEntity? catEntity) => catEntity is null
        ? null
        : new CatDto
        {
            CatId = catEntity.CatId,
            Width = catEntity.Width,
            Height = catEntity.Height,
            ImageData = catEntity.ImageData,
            Tags = catEntity.CatTags.Select(x => x.Tag.Name).ToList()
        };
}
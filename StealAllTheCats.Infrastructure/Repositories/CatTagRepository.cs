using Microsoft.EntityFrameworkCore;
using StealAllTheCats.Domain.Common.Enums;
using StealAllTheCats.Domain.Common.Result;
using StealAllTheCats.Domain.Entities;
using StealAllTheCats.Domain.Repositories;
using StealAllTheCats.Infrastructure.Database;

namespace StealAllTheCats.Infrastructure.Repositories;

public class CatTagRepository : ICatTagRepository
{
    private readonly AppDbContext _context;
    
    public CatTagRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Result> AddCatTagAsync(TagEntity catTag)
    {
        try
        {
            await _context.Tags.AddAsync(catTag);
            return Result.Ok();
        }
        catch (Exception e)
        {
            return Result.Failure(Error.New("An error occurred while adding the tag to the database", e, KnownApplicationErrorEnum.SqlGenericError));
        }    
    }
    
    public async Task<Result<TagEntity?>> GetCatTagByNameAsync(string tag)
    {
        try
        {
            var tagEntity = await _context.Tags.FirstOrDefaultAsync(x => x.Name == tag);
            return Result<TagEntity?>.Ok(tagEntity);
        }
        catch (Exception e)
        {
            return Result<TagEntity?>.Failure(Error.New("An error occurred while fetching the tag from the database", e, KnownApplicationErrorEnum.SqlGenericError));
        }
    }
    
    public async Task<Result> SaveChangesAsync()
    {
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            return Result.Failure(Error.New("An error occurred while saving changes to the database. Please try again.", ex, KnownApplicationErrorEnum.SqlGenericError));
        }

        return Result.Ok();
    }
}
using Microsoft.EntityFrameworkCore;
using StealAllTheCats.Domain.Common.Enums;
using StealAllTheCats.Domain.Common.Result;
using StealAllTheCats.Domain.Entities;
using StealAllTheCats.Domain.Repositories;
using StealAllTheCats.Infrastructure.Database;

namespace StealAllTheCats.Infrastructure.Repositories;

public class CatRepository : ICatRepository
{
    private readonly AppDbContext _context;
    
    public CatRepository(AppDbContext context)
    {
           ArgumentNullException.ThrowIfNull(_context = context);
    }

    public async Task<Result<IEnumerable<CatEntity>>> GetCatsAsync(int page, int pageSize)
    {
        try
        {
            var cats = await _context.Cats.OrderBy(x => x.Created)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return Result<IEnumerable<CatEntity>>.Ok(cats);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<CatEntity>>.Failure(Error.New("An error occurred while fetching the cats from the database", ex, KnownApplicationErrorEnum.SqlGenericError));
        }
    }

    public async Task<Result<IEnumerable<CatEntity>>> GetCatsByTagAsync(string tag, int page, int pageSize)
    {
        try
        {
            var cats = await _context.Cats
                .Where(c => c.CatTags.Any(t => t.Tag.Name == tag))
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            
            return Result<IEnumerable<CatEntity>>.Ok(cats);
        }

        catch (Exception ex)
        {
            return Result<IEnumerable<CatEntity>>.Failure(Error.New("An error occurred while fetching the cats from the database", ex, KnownApplicationErrorEnum.SqlGenericError));
        }
        
    }

    public async Task<Result<CatEntity?>> GetCatByCatIdAsync(string catId)
    {
        try
        {
            var cat = await _context.Cats.FirstOrDefaultAsync(x => x.CatId == catId);
            return Result<CatEntity?>.Ok(cat);
        }
        catch (Exception e)
        {
            return Result<CatEntity?>.Failure(Error.New("An error occurred while adding the cat to the database", e, KnownApplicationErrorEnum.SqlGenericError));
        }
    }

    public async  Task<Result>  AddCatAsync(CatEntity cat)
    {
        try
        {
            await _context.Cats.AddAsync(cat);
            return Result.Ok();
        }
        catch (Exception e)
        {
            return Result.Failure(Error.New("An error occurred while adding the cat to the database", e, KnownApplicationErrorEnum.SqlGenericError));
        }
    }
    
    public async Task<Result<bool>> UpdateCatAsync(CatEntity existingCat, CatEntity catEntity)
    {
        try
        {
            existingCat.ImageData = catEntity.ImageData;
            existingCat.CatTags = catEntity.CatTags;

            existingCat.Width = catEntity.Width;
            existingCat.Height = catEntity.Height;
            
            await _context.SaveChangesAsync();
            return Result<bool>.Ok(true);
        }
        catch (Exception e)
        {
            return Result<bool>.Failure(Error.New($"An error occurred while updating the cat: {existingCat.CatId} to the database", e, KnownApplicationErrorEnum.SqlGenericError));
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
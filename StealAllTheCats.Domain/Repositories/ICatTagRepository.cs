using StealAllTheCats.Domain.Common.Result;
using StealAllTheCats.Domain.Entities;

namespace StealAllTheCats.Domain.Repositories;

public interface ICatTagRepository
{
    Task<Result> AddCatTagAsync(TagEntity catTag);
    
    Task<Result<TagEntity?>> GetCatTagByNameAsync(string tag);
    
    Task<Result> SaveChangesAsync();
}
using StealAllTheCats.Domain.Common.Result;
using StealAllTheCats.Domain.Entities;

namespace StealAllTheCats.Domain.Repositories;

public interface ICatRepository
{
    Task<Result<IEnumerable<CatEntity>>> GetCatsAsync(int page, int pageSize);
    Task<Result<IEnumerable<CatEntity>>> GetCatsByTagAsync(string tag, int page, int pageSize);
    Task<Result<CatEntity?>> GetCatByCatIdAsync(string catId);
    Task<Result> AddCatAsync(CatEntity cat);
    Task<Result<bool>> UpdateCatAsync(CatEntity existingCat, CatEntity catEntity);
    Task<Result> SaveChangesAsync();
}
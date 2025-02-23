using StealAllTheCats.Domain.Common.Result;
using StealAllTheCats.Domain.Entities;

namespace StealAllTheCats.Application.Interfaces;

public interface ICatService
{
    Task<Result<IEnumerable<CatEntity>>> GetCatsAsync(int page, int pageSize);
    Task<Result<CatEntity?>> GetCatByCatIdAsync(string catId);
    Task<Result> AddCatAsync(CatEntity catEntity);
    
    Task<Result<IEnumerable<CatEntity>>> GetCatsByTagAsync(string tag, int page, int pageSize);

}
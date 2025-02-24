using StealAllTheCats.Domain.Common.Result;
using StealAllTheCats.Domain.Dtos;
using StealAllTheCats.Domain.Entities;

namespace StealAllTheCats.Application.Interfaces;

public interface ICatService
{
    Task<Result<IEnumerable<CatDto>>> GetCatsAsync(int page, int pageSize);
    Task<Result<CatDto?>> GetCatByCatIdAsync(string catId);
    Task<Result> AddCatAsync(CatEntity catEntity);
    
    Task<Result<IEnumerable<CatDto>>> GetCatsByTagAsync(string tag, int page, int pageSize);
    
    Task<Result<bool>> UpdateCatAsync(CatEntity catEntity);

}
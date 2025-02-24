using StealAllTheCats.Domain.Common.Result;
using StealAllTheCats.Domain.Entities;

namespace StealAllTheCats.Application.Interfaces;

public interface ICatsApiHttpService
{
    public Task<Result<IEnumerable<CatApiResponse>>> GetCatsAsync(int page, int pageSize);
}
using StealAllTheCats.Domain.Common.Result;

namespace StealAllTheCats.Application.Interfaces;

public interface ICatManager
{
    public Task<Result> FetchCatsAsync();
}
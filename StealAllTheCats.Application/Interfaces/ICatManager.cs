using StealAllTheCats.Domain.Common.Result;

namespace StealAllTheCats.Application.Interfaces;

public interface ICatManager
{
    /// <summary>
    /// Fetch cats from the API and stores the cats and its tags in the database
    /// </summary>
    /// <returns></returns>
    public Task<Result> FetchCatsAsync();
}
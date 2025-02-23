using StealAllTheCats.Domain.Common.Result;
using StealAllTheCats.Domain.Entities;

namespace StealAllTheCats.Application.Interfaces;

public interface IBreedService
{
    Task<Result<IEnumerable<CatTag>>> ConstructBreeds(string temperament); 
}
using StealAllTheCats.Domain.Entities;

namespace StealAllTheCats.Application.Interfaces;

public interface ICatService
{
    Task FetchAndStoreCatsAsync();
    Task<IEnumerable<CatEntity>> GetCatsAsync(int page, int pageSize);
    Task<CatEntity> GetCatByIdAsync(int id);
}
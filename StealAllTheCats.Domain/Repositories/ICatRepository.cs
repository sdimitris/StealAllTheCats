using StealAllTheCats.Domain.Entities;

namespace StealAllTheCats.Domain.Repositories;

public interface ICatRepository
{
    Task<IEnumerable<CatEntity>> GetCatsAsync(int page, int pageSize);
    Task<CatEntity> GetCatByIdAsync(int id);
    Task AddCatAsync(CatEntity cat);
    Task<bool> CatExistsAsync(string catId);
    Task SaveChangesAsync();
}
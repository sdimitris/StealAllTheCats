using Newtonsoft.Json;
using StealAllTheCats.Application.Interfaces;
using StealAllTheCats.Domain.Entities;
using StealAllTheCats.Domain.Repositories;
namespace StealAllTheCats.Application.Services;

public class CatService : ICatService
{
    private readonly ICatRepository _catRepository;
    private readonly IHttpClientFactory  _httpClientFactory;

    public CatService(ICatRepository catRepository, IHttpClientFactory httpClientFactory)
    {
        _catRepository = catRepository;
        _httpClientFactory = httpClientFactory;
    }

    public async Task FetchAndStoreCatsAsync()
    {
        using var httpClient = _httpClientFactory.CreateClient();
        var response = await httpClient.GetStringAsync("https://api.thecatapi.com/v1/images/search?limit=25&has_breeds=1");
        var cats = JsonConvert.DeserializeObject<List<CatApiResponse>>(response);
        
        foreach (var cat in cats)
        {
            if (await _catRepository.CatExistsAsync(cat.Id)) continue;

            var newCat = new CatEntity
            {
                CatId = cat.Id,
                Width = cat.Width,
                Height = cat.Height,
                ImageUrl = cat.Url
            };
            await _catRepository.AddCatAsync(newCat);
        }
        await _catRepository.SaveChangesAsync();
    }

    public Task<IEnumerable<CatEntity>> GetCatsAsync(int page, int pageSize) => _catRepository.GetCatsAsync(page, pageSize);

    public Task<CatEntity> GetCatByIdAsync(int id) => _catRepository.GetCatByIdAsync(id);
}
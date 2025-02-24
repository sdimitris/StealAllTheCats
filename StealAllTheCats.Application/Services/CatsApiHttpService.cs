using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using StealAllTheCats.Application.Interfaces;
using StealAllTheCats.Domain.Common.Enums;
using StealAllTheCats.Domain.Common.Result;
using StealAllTheCats.Domain.Configuration;
using StealAllTheCats.Domain.Entities;

namespace StealAllTheCats.Application.Services;

public class CatsApiHttpService : ICatsApiHttpService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly CatsApiSettings _catsApiSettings;
    public CatsApiHttpService(IOptions<CatsApiSettings> catsApiSettings, IHttpClientFactory httpClientFactory)
    {
        ArgumentNullException.ThrowIfNull(_httpClientFactory = httpClientFactory);
        ArgumentNullException.ThrowIfNull(_catsApiSettings = catsApiSettings.Value);

    }

    public async Task<Result<IEnumerable<CatApiResponse>>> GetCatsAsync(int page, int pageSize)
    {
        using var httpClient = _httpClientFactory.CreateClient();
        var uri = new Uri(_catsApiSettings.BaseUrl).ToString().TrimEnd('/') +$"?limit={pageSize}&page={page}";
        httpClient.DefaultRequestHeaders.Add("x-api-key", _catsApiSettings.ApiKey); // Set API Key in headers
        List<CatApiResponse> cats;

        try
        {
            var response = await httpClient.GetAsync(uri);
            if (!response.IsSuccessStatusCode)
            {
                return Result<IEnumerable<CatApiResponse>>.Failure(Error.New($"Failed to fetch cats from API. ErrorCode:  {response.StatusCode}", null,
                    KnownApplicationErrorEnum.CatsApiError));
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            cats = JsonConvert.DeserializeObject<List<CatApiResponse>>(jsonResponse);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<CatApiResponse>>.Failure(Error.New("Failed to fetch cats from API.", ex, KnownApplicationErrorEnum.CatsApiError));
        }

        return Result<IEnumerable<CatApiResponse>>.Ok(cats);
    }
    
    public async Task<Result<byte[]>> DownloadImageFromApiAsync(string imageUrl)
    {
        using var httpClient = new HttpClient();
        try
        {
            var imageData = await httpClient.GetByteArrayAsync(imageUrl);
            return Result<byte[]>.Ok(imageData);
        }
        catch (Exception e)
        {
            return Result<byte[]>.Failure(Error.New($"Failed to download image: {imageUrl} from API", e, KnownApplicationErrorEnum.CatsApiError));
        }  
    }
}
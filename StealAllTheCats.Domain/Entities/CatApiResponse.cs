using System.Text.Json.Serialization;

namespace StealAllTheCats.Domain.Entities;

public class CatApiResponse
{
    [JsonPropertyName("id")]
    public string Id { get; set; }
    
    [JsonPropertyName("width")]
    public int Width { get; set; }
    
    [JsonPropertyName("url")]
    public int Height { get; set; }
    
    [JsonPropertyName("url")]
    public string Url { get; set; }
    public List<Breed> Breeds { get; set; } = new List<Breed>();
}

public class Breed
{
    public Weight Weight { get; set; } = new();
    
    [JsonPropertyName("id")]    
    public string Id { get; set; }
    
    [JsonPropertyName("name")]
    public string Name { get; set; }
    
    [JsonPropertyName("temperament")]
    public string Temperament { get; set; }
    
    [JsonPropertyName("origin")]
    public string Origin { get; set; }
    
    [JsonPropertyName("country_codes")]
    public string CountryCodes { get; set; }
    
    [JsonPropertyName("country_code")]
    public string CountryCode { get; set; }
    
    [JsonPropertyName("life_span")]
    public string LifeSpan { get; set; }
    
    [JsonPropertyName("wikipedia_url")]
    public string WikipediaUrl { get; set; }
}

public class Weight
{
    [JsonPropertyName("imperial")]
    public string Imperial { get; set; }
    
    [JsonPropertyName("metric")]
    public string Metric { get; set; }
}
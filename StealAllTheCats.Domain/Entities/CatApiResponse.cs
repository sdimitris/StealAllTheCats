namespace StealAllTheCats.Domain.Entities;

public class CatApiResponse
{
    public string Id { get; set; }
    public string Url { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public List<string> Breeds { get; set; }
    public Favourite Favourite { get; set; }
}

public class Favourite
{
    // You can define properties of 'Favourite' here based on actual response structure, 
    // or leave it empty if it's not necessary for your use case.
}
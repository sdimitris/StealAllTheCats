namespace StealAllTheCats.Domain.Dtos;

public class CatDto
{
    public string CatId { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public string ImageUrl { get; set; } = String.Empty;
    
    public List<string> Tags { get; set; } = [];
}
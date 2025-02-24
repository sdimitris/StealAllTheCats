namespace StealAllTheCats.Domain.Dtos;

public class CatDto
{
    public string CatId { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public byte[] ImageData { get; set; }
    
    public List<string> Tags { get; set; } = [];
}
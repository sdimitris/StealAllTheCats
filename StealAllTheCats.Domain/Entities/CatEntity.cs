namespace StealAllTheCats.Domain.Entities;

public class CatEntity
{
    public int Id { get; set; }
    public string CatId { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public string ImageUrl { get; set; }
    public DateTime Created { get; set; } = DateTime.UtcNow;
    public List<CatTag> CatTags { get; set; } = new();
}

namespace StealAllTheCats.Domain.Entities;

public class CatEntity
{
    public int Id { get; set; }
    
    /// <summary>
    ///   The unique identifier for the cat
    /// </summary>
    public string CatId { get; set; }
    
    /// <summary>
    /// Width of the cat image
    /// </summary>
    public int Width { get; set; }
    
    /// <summary>
    /// Height of the cat image
    /// </summary>
    public int Height { get; set; }

    /// <summary>
    /// Image url
    /// </summary>
    public string ImageUrl { get; set; } = String.Empty;
    public DateTime Created { get; set; } = DateTime.UtcNow;
    public List<CatTag> CatTags { get; set; } = new();
}

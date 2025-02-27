namespace StealAllTheCats.Domain.Dtos;

public class CatDto
{
    /// <summary>
    /// The unique Id of the cat
    /// </summary>
    public string CatId { get; set; }
    
    /// <summary>
    /// Width of the image
    /// </summary>
    public int Width { get; set; }
    
    /// <summary>
    /// Height of the image
    /// </summary>
    public int Height { get; set; }
    
    /// <summary>
    /// Image url
    /// </summary>
    public string ImageUrl { get; set; } = String.Empty;
    
    
    /// <summary>
    /// List of cat tags.
    /// </summary>
    public List<string> Tags { get; set; } = [];
}
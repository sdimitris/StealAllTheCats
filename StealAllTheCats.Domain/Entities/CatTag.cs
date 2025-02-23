namespace StealAllTheCats.Domain.Entities;

/// <summary>
/// Represents a cat entity.
/// </summary>
public class CatTag
{
    /// <summary>
    /// The unique identifier for the cat
    /// </summary>
    public int CatId { get; set; }
    
    public CatEntity Cat { get; set; }
    
    /// <summary>
    /// The tag id of the cat
    /// </summary>
    public int TagId { get; set; }
    public TagEntity Tag { get; set; }
}
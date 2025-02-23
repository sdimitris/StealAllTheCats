namespace StealAllTheCats.Domain.Entities;

public class TagEntity
{
    /// <summary>
    /// Id of the tag
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// The name of the tag
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// Created date of the tag
    /// </summary>
    public DateTime Created { get; set; } = DateTime.UtcNow;
    
    /// <summary>
    /// List of cats that have this tag
    /// </summary>
    public List<CatTag> CatTags { get; set; } = new();
}
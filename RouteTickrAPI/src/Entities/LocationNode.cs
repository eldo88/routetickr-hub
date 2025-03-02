using System.ComponentModel.DataAnnotations;

namespace RouteTickrAPI.Entities;

public class LocationNode
{
    [Key]
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public int? ParentId { get; set; }
    public LocationNode? Parent { get; set; }
    public List<LocationNode> Children { get; set; } = [];
    
    public LocationNode(string name)
    {
        Name = name;
    }
}
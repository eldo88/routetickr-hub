using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using RouteTickrAPI.Enums;

namespace RouteTickrAPI.Entities;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "ClimbType")]
[JsonDerivedType(typeof(SportRoute), typeDiscriminator: "Sport")]
[JsonDerivedType(typeof(TradRoute), typeDiscriminator: "Trad")]
[JsonDerivedType(typeof(Boulder), typeDiscriminator: "Boulder")]
[JsonDerivedType(typeof(IceClimb), typeDiscriminator: "Ice")]
[JsonDerivedType(typeof(AlpineRockRoute), typeDiscriminator: "Alpine")]
public abstract class Climb
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Rating { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public decimal Height { get; set; }
    public ClimbDangerRating DangerRating { get; set; }
    public ClimbType ClimbType { get; set; }
}
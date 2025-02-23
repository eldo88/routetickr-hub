using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using RouteTickrAPI.Enums;

namespace RouteTickrAPI.Entities;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "climbType")]
[JsonDerivedType(typeof(SportRoute), typeDiscriminator: "Sport")]
[JsonDerivedType(typeof(TradRoute), typeDiscriminator: "Trad")]
[JsonDerivedType(typeof(Boulder), typeDiscriminator: "Boulder")]
[JsonDerivedType(typeof(IceClimb), typeDiscriminator: "Ice")]
[JsonDerivedType(typeof(AlpineRockRoute), typeDiscriminator: "Alpine")]
public abstract class Climb
{
    public ClimbType ClimbType { get; init; } // type discriminator, has to be first property in json
    [Key]
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Rating { get; init; } = string.Empty;
    public string Location { get; init; } = string.Empty;
    public string Url { get; init; } = string.Empty;
    public decimal Height { get; init; }
    public ClimbDangerRating DangerRating { get; init; }
}
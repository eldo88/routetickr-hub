using System.Diagnostics.CodeAnalysis;
using RouteTickr.Entities;
using RouteTickr.Messages.Enums;

namespace RouteTickrAPI.Builders;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public abstract class ClimbBuilderBase<TBuilder, TClimb>
 where TBuilder : ClimbBuilderBase<TBuilder, TClimb>
 where TClimb : Climb // TODO Add common fields and add validation
{
    protected int _id;
    protected string _name = string.Empty;
    protected string _rating = string.Empty;
    protected string _location = string.Empty;
    protected string _url = string.Empty;
    protected double? _height;
    protected ClimbDangerRating _dangerRating;
    protected ClimbType _climbType;

    public TBuilder WithId(int id)
    {
        _id = id;

        return (TBuilder)this;
    }

    public TBuilder WithName(string name)
    {
        _name = name;

         return (TBuilder)this;
    }

    public TBuilder WithRating(string rating)
    {
        _rating = rating;

        return (TBuilder)this;
    }

    public TBuilder WithLocation(string location)
    {
        _location = location;

        return (TBuilder)this;
    }

    public TBuilder WithUrl(string url)
    {
        _url = url;

        return (TBuilder)this;
    }

    public TBuilder WithHeight(double? height)
    {
        _height = height ?? 0;

        return (TBuilder)this;
    }

    public TBuilder WithDangerRating(string dangerRating)
    {
        if (!Enum.TryParse(dangerRating, out _dangerRating))
        {
            _dangerRating = ClimbDangerRating.Unknown;
        }

        return (TBuilder)this;
    }

    public TBuilder WithClimbType(string climbType)
    {
        if (!Enum.TryParse(climbType, out _climbType))
        {
            _climbType = ClimbType.Unknown;
        }

        return (TBuilder)this;
    }

    public abstract TClimb Build();
}
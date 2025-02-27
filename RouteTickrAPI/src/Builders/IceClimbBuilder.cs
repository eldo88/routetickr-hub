using RouteTickrAPI.Entities;

namespace RouteTickrAPI.Builders;

public class IceClimbBuilder : ClimbBuilderBase<IceClimbBuilder, IceClimb>
{
    private string _iceType = string.Empty;
    private int _numberOfPitches;

    public IceClimbBuilder WithIceType(string iceType)
    {
        _iceType = iceType;

        return this;
    }

    public IceClimbBuilder WithNumberOfPitches(int numberOfPitches)
    {
        _numberOfPitches = numberOfPitches;

        return this;
    }
    
    public override IceClimb Build()
    {
        return new IceClimb()
        {
            Id = _id,
            Name = _name,
            Rating = _rating,
            Location = _location,
            Url = _url,
            Height = _height,
            DangerRating = _dangerRating,
            ClimbType = _climbType,
            IceType = _iceType,
            NumberOfPitches = _numberOfPitches
        };
    }
}
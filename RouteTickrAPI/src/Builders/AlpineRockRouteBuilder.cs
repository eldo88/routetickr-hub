using RouteTickr.Entities;

namespace RouteTickrAPI.Builders;

public class AlpineRockRouteBuilder : ClimbBuilderBase<AlpineRockRouteBuilder, AlpineRockRoute>
{
    private int _numberOfPitches;
    private decimal _approachDistance;
    private string _gearNeeded = string.Empty;

    public AlpineRockRouteBuilder WithNumberOfPitches(int numberOfPitches)
    {
        _numberOfPitches = numberOfPitches;

        return this;
    }

    public AlpineRockRouteBuilder WithApproachDistance(decimal approachDistance)
    {
        _approachDistance = approachDistance;

        return this;
    }

    public AlpineRockRouteBuilder WithGearNeeded(string gearNeeded)
    {
        _gearNeeded = gearNeeded;

        return this;
    }
    
    public override AlpineRockRoute Build()
    {
        return new AlpineRockRoute()
        {
            Id = _id,
            Name = _name,
            Rating = _rating,
            Location = _location,
            Url = _url,
            Height = _height ?? 0,
            DangerRating = _dangerRating,
            ClimbType = _climbType,
            NumberOfPitches = _numberOfPitches,
            ApproachDistance = _approachDistance,
            GearNeeded = _gearNeeded
        };
    }
}
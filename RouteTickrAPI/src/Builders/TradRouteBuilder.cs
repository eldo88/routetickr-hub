using System.Runtime.CompilerServices;
using RouteTickrAPI.Entities;

namespace RouteTickrAPI.Builders;

public class TradRouteBuilder : ClimbBuilderBase<TradRouteBuilder, TradRoute>
{
    private int _numberOfPitches;
    private string _gearNeeded = string.Empty;

    public TradRouteBuilder WithNumberOfPitches(int numberOfPitches)
    {
        _numberOfPitches = numberOfPitches;

        return this;
    }

    public TradRouteBuilder WithGearNeeded(string gearNeeded)
    {
        _gearNeeded = gearNeeded;
        
        return this;
    }
    public override TradRoute Build()
    {
        return new TradRoute()
        {
            Id = _id,
            Name = _name,
            Rating = _rating,
            Location = _location,
            Url = _url,
            Height = _height,
            DangerRating = _dangerRating,
            ClimbType = _climbType,
            NumberOfPitches = _numberOfPitches,
            GearNeeded = _gearNeeded
        };
    }
}
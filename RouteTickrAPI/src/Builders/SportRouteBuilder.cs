using RouteTickr.Entities;

namespace RouteTickrAPI.Builders;

public class SportRouteBuilder : ClimbBuilderBase<SportRouteBuilder, SportRoute>
{
    private int _numberOfBolts;
    private int _numberOfPitches;

    public SportRouteBuilder WithNumberOfBolts(int numberOfBolts)
    {
        _numberOfBolts = numberOfBolts;

        return this;
    }
    
    public SportRouteBuilder WithNumberOfPitches(int numberOfPitches)
    {
        _numberOfPitches = numberOfPitches;

        return this;
    }
    
    
    public override SportRoute Build()
    {
        return new SportRoute()
        {
            Id = _id,
            Name = _name,
            Rating = _rating,
            Location = _location,
            Url = _url,
            Height = _height ?? 0,
            DangerRating = _dangerRating,
            ClimbType = _climbType,
            NumberOfBolts = _numberOfBolts,
            NumberOfPitches = _numberOfPitches
        };
    }
}
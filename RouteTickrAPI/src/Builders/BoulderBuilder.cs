using RouteTickrAPI.Entities;

namespace RouteTickrAPI.Builders;

public class BoulderBuilder : ClimbBuilderBase<BoulderBuilder, Boulder>
{
    private bool _hasTopOut;
    private int _numberOfCrashPadsNeeded;
    private bool _isTraverse;

    public BoulderBuilder WithHasTopOut(bool hasTopOut)
    {
        _hasTopOut = hasTopOut;

        return this;
    }

    public BoulderBuilder WithNumberOfCrashPadsNeeded(int numberOfCrashPads)
    {
        _numberOfCrashPadsNeeded = numberOfCrashPads;

        return this;
    }

    public BoulderBuilder WithIsTraverse(bool isTraverse)
    {
        _isTraverse = isTraverse;

        return this;
    }
    
    public override Boulder Build()
    {
        return new Boulder()
        {
            Id = _id,
            Name = _name,
            Rating = _rating,
            Location = _location,
            Url = _url,
            Height = _height,
            DangerRating = _dangerRating,
            ClimbType = _climbType,
            HasTopOut = _hasTopOut,
            NumberOfCrashPadsNeeded = _numberOfCrashPadsNeeded,
            IsTraverse = _isTraverse
        };
    }
}
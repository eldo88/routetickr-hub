using RouteTickrAPI.Enums;

namespace RouteTickr.Entities;

public class Boulder : Climb
{
    public bool HasTopOut { get; init; }
    public int NumberOfCrashPadsNeeded { get; init; }
    public bool IsTraverse { get; init; }

    public Boulder() => ClimbType = ClimbType.Boulder;
}
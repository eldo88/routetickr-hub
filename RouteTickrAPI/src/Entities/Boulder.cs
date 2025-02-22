namespace RouteTickrAPI.Entities;

public class Boulder : Climb
{
    public bool HasTopOut { get; set; }
    public int NumberOfCrashPadsNeeded { get; set; }
    public bool IsTraverse { get; set; }
}
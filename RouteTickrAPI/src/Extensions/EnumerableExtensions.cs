namespace RouteTickrAPI.Extensions;

public static class EnumerableExtensions
{
    public static IEnumerable<T> Paginate<T>(this IEnumerable<T> source, int pageNumber, int pageSize)
    {
        return source.Skip((pageNumber - 1) * pageSize).Take(pageSize);
    }
}
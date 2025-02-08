using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Mvc.TagHelpers.Cache;

namespace RouteTickrAPI.Extensions;

public static class DictionaryExtensions
{
    public static bool TryIncrementCount<TKey>(this Dictionary<TKey, int>? dict, TKey key)
        where TKey : notnull
    {
        if (dict is null) return false;

        ref var valueRef = ref CollectionsMarshal.GetValueRefOrAddDefault(dict, key, out var exists);
        
        if (!exists)
        {
            valueRef = 1;
        }
        else
        {
            valueRef++;
        }
        
        return true;
    }
    
}
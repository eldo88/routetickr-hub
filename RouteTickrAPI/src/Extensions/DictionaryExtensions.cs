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

    public static void InvokeAction<TKey, TValue>( //experimenting
        this Dictionary<TKey, TValue>? dict, 
        Action<Dictionary<TKey, TValue>> action)
        where TKey : notnull
    {
        if (dict is null) return;


        action(dict);
    }

    public static void AddToCollection<TKey, TValue, TCollection>(
        this Dictionary<TKey, TCollection> dict, 
        TKey key, 
        TValue value) 
        where TKey : notnull 
        where TCollection : ICollection<TValue>, new()
    {
        if (!dict.TryGetValue(key, out var collection))
        {
            collection = new TCollection();
            dict[key] = collection;
        }
        collection.Add(value);
    }
    
}
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Mvc.TagHelpers.Cache;

namespace RouteTickrAPI.Extensions;

public static class DictionaryExtensions
{
    
    /// <summary>
    /// Increments the count of a specified key in a dictionary.  
    /// If the key does not exist, it is added with an initial count of 1.  
    /// </summary>
    /// <param name="dict">The dictionary that stores key counts. Must not be null and should be empty when first used.</param>
    /// <param name="key">The key whose count should be incremented. Must not be null.</param>
    /// <typeparam name="TKey">The type of dictionary key. Must not be null.</typeparam>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="dict"/> is null.</exception>
    public static void IncrementCount<TKey>(this Dictionary<TKey, int> dict, TKey key)
        where TKey : notnull
    {
        ArgumentNullException.ThrowIfNull(dict, nameof(dict));

        ref var valueRef = ref CollectionsMarshal.GetValueRefOrAddDefault(dict, key, out var exists);
        
        if (!exists)
        {
            valueRef = 1;
        }
        else
        {
            valueRef++;
        }
    }

    
    /// <summary>
    /// Adds a value to a collection stored in a dictionary under the specified key.  
    /// If the key does not exist, a new collection is created and added to the dictionary before storing the value.  
    /// </summary>
    /// <param name="dict">The dictionary where the collection is stored.</param>
    /// <param name="key">The key associated with the collection. Must not be null.</param>
    /// <param name="value">The value to add to the collection.</param>
    /// <typeparam name="TKey">The type of the dictionary key. Must not be null.</typeparam>
    /// <typeparam name="TValue">The type of values stored in the collection.</typeparam>
    /// <typeparam name="TCollection">The type of collection that holds values. Must implement <see cref="ICollection{TValue}"/> and have a parameterless constructor.</typeparam>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="dict"/> is null.</exception>
    public static void AddToCollection<TKey, TValue, TCollection>(
        this Dictionary<TKey, TCollection> dict, 
        TKey key, 
        TValue value) 
        where TKey : notnull 
        where TCollection : ICollection<TValue>, new()
    {
        ArgumentNullException.ThrowIfNull(dict, nameof(dict));
        
        if (!dict.TryGetValue(key, out var collection))
        {
            collection = new TCollection();
            dict[key] = collection;
        }
        collection.Add(value);
    }


    public static void PerformAction<TKey, TValue>(this Dictionary<TKey, TValue> dict, Action<TValue> action)
    where TKey: notnull // likely not needed
    {
        ArgumentNullException.ThrowIfNull(dict);

        foreach (var kvp in dict)
        {
            action(kvp.Value);
        }
    }
    
}
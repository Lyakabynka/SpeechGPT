namespace SpeechGPT.Application.Interfaces;

public interface IRedisCache {
    
    /// <summary>
    /// Get Data using key
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <returns></returns>
    Task<T?> GetCacheData<T> (string key);
    
    /// <summary>
    /// Set Data with Value and Expiration Time of Key
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <param name="expirationTime"></param>
    /// <returns></returns>
    Task<bool> SetCacheData<T> (string key, T value, DateTime expirationTime = default);
    
    /// <summary>
    /// Remove Data
    /// </summary>
    /// <param name="key">Key of the Key-Value pair</param>
    /// <returns></returns>
    Task<bool> RemoveData(string key);

    /// <summary>
    /// Appends Data to the Specific List
    /// </summary>
    /// <param name="key">Name of the list</param>
    /// <param name="value">Value to append</param>
    /// <param name="expirationTime">Time when Key-Value pair expires</param>
    /// <returns></returns>
    Task<long> ListAppend<T>(string key, T value, DateTime expirationTime = default);

    /// <summary>
    /// Gets all data from list
    /// </summary>
    /// <param name="key">Name of the list</param>
    /// <returns></returns>
    Task<List<T>?> GetList<T>(string key);
}
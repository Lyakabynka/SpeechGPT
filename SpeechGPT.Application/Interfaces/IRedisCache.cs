﻿namespace SpeechGPT.Application.Interfaces;

public interface IRedisCache {
    
    /// <summary>
    /// Get Data using key
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <returns></returns>
    Task<T> GetCacheData<T> (string key);
    
    /// <summary>
    /// Set Data with Value and Expiration Time of Key
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <param name="expirationTime"></param>
    /// <returns></returns>
    Task<bool> SetCacheData<T> (string key, T value, DateTimeOffset expirationTime);
    
    /// <summary>
    /// Remove Data
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    Task<bool> RemoveData(string key);
}

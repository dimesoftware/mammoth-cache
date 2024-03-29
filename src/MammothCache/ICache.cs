﻿namespace MammothCache
{
    /// <summary>
    /// Contracts of the capabilities of the cache
    /// </summary>
    public interface ICache : IReadCache, IWriteCache
    {
    }
}
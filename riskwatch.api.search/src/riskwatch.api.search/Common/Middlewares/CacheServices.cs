using Dapper;
using Microsoft.Extensions.Caching.Distributed;
using MySql.Data.MySqlClient;
using riskwatch.api.search.Database;
using Serilog;

namespace riskwatch.api.search.Common.Middlewares
{
    #region RidesCache
    public static class CacheHelper
    {
        public static async Task<IEnumerable<TDto>> GetOrSetCacheAsync<TDto>(
      IDistributedCache cache,
      string key,
      Func<Task<IEnumerable<TDto>>> getData,
      DistributedCacheEntryOptions options,
      CancellationToken token,
      string tableName = null)
        {
            // Try to get data from cache
            string cachedData = await cache.GetStringAsync(key);

            if (cachedData != null)
            {
                Log.Information($"Cache hit: {key}");
                return System.Text.Json.JsonSerializer.Deserialize<IEnumerable<TDto>>(cachedData);
            }

            Log.Information("Cache miss. Fetching...");

            // If cache miss, fetch data
            IEnumerable<TDto> data;
            if (!string.IsNullOrEmpty(tableName))
            {
                // Construct query based on tableName
                string query = $"SELECT * FROM {tableName}";

                // Execute query to fetch data
                var connection = new MySqlConnection(GlobalDbConnection.MysqlConnection);
                data = await connection.QueryAsync<TDto>(query);
            }
            else
            {
                // Execute provided getData function to fetch data
                data = await getData();
            }

            // Only update cache if there's data to cache
            if (data != null)
            {
                await cache.SetStringAsync(key, System.Text.Json.JsonSerializer.Serialize(data), options, token);
            }

            return data;
        }


        //Remove cache function
        public static async Task RemoveCacheAsync(IDistributedCache cache, string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key), "Cache key cannot be null or empty.");
            }

            await cache.RemoveAsync(key);
            Log.Information($"Cache removed: {key}");
        }

    }
    #endregion
}

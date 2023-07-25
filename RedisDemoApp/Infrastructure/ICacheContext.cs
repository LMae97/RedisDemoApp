namespace RedisDemoApp.Infrastructure
{
    public interface ICacheContext
    {
        Task<T> GetRecordAsync<T>(string recordId);
        Task SetRecordAsync<T>(string recordId, T data, TimeSpan? absoluteExpireTime = null, TimeSpan? unusedExpireTime = null);
        Task RemoveRecordAsync(string recordId);
    }
}
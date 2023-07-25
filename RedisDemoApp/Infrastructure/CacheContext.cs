using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace RedisDemoApp.Infrastructure;

public class CacheContext : ICacheContext
{
    private readonly IDistributedCache _cache;

    //Redis è un DB chiave valore. Lavora con stringhe, quindi è necessario serializzare e deserializzare i dati
    //(non restituisce oggetti / modelli ma stringhe --> sono necessari metodi Get e Set)

    public CacheContext(IDistributedCache cache)
    {
        _cache = cache;
    }

    public async Task<T> GetRecordAsync<T>(string recordId)
    {
        var jsonData = await _cache.GetStringAsync(recordId); //Recupera la stringa JSON dalla cache

        if (jsonData is null) //Se non è presente in cache
        {
            return default(T); //Restituisce il valore di default per il tipo T
        }

        return JsonSerializer.Deserialize<T>(jsonData); //Deserializza la stringa JSON in un oggetto di tipo T
    }

    public async Task SetRecordAsync<T>(string recordId, T data, TimeSpan? absoluteExpireTime = null, TimeSpan? unusedExpireTime = null)
    {
        var options = new DistributedCacheEntryOptions(); //Permette di definire delle opzioni per il record che si sta salvando in cache

        options.AbsoluteExpirationRelativeToNow = absoluteExpireTime ?? TimeSpan.FromSeconds(60); //Tempo di permanenza in cache: default 60 secondi
        options.SlidingExpiration = unusedExpireTime; //Tempo di inutilizzo prima di eliminare il record dalla cache

        var jsonData = JsonSerializer.Serialize(data); //Serializza l'oggetto in una stringa JSON

        await _cache.SetStringAsync(recordId, jsonData, options); //Salva la stringa JSON in cache --> chiave: recordId, valore: jsonData
    }

    public async Task RemoveRecordAsync(string recordId)
    {
        await _cache.RemoveAsync(recordId); //Rimuove il record dalla cache
    }
}

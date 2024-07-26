using DistributedCache.Services;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace DistributedCache.Controllers;

[ApiController]
[Route("dataPureRedisController")]
public class DataPureRedisController : ControllerBase
{
    private readonly DataService _dataService;
    private readonly IDatabase _redis;

    public DataPureRedisController(DataService dataService, IConnectionMultiplexer multiplexer)
    {
        _dataService = dataService;
        _redis = multiplexer.GetDatabase();
    }

    [HttpPatch]
    public IActionResult UpdateData(string data)
    {
        _dataService.Data = data;
        _redis.KeyDeleteAsync("GetDataWithCash");

        return Ok();
    }

    [HttpGet("GetDataWithOutCash")]
    public async Task<IActionResult> GetDataWithOutCash()
    {
        var data = await _dataService.GetData();

        await Console.Out.WriteLineAsync(data);

        return Ok("Data without cash");
    }

    [HttpGet("GetDataWithCash")]
    public async Task<IActionResult> GetDataWithCash()
    {
        var data = await _redis.StringGetAsync("GetDataWithCash");

        if (data.IsNull)
        {
            data = await _dataService.GetData();
            _ = _redis.StringSetAsync("GetDataWithCash", data, TimeSpan.FromSeconds(10));
        }

        await Console.Out.WriteLineAsync("1 " + data);

        return Ok("Data with cash");
    }

    //https://www.nuget.org/packages/CachingFramework.Redis/
    //https://github.com/thepirat000/CachingFramework.Redis/blob/master/src/CachingFramework.Redis/RedisObjects/RedisList.cs

    //https://www.nuget.org/packages/ServiceStack.Redis
    //https://github.com/ServiceStack/ServiceStack/blob/main/ServiceStack.Redis/src/ServiceStack.Redis/RedisClientList.cs

}

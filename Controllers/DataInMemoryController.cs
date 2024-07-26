using DistributedCache.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace DistributedCache.Controllers;

[ApiController]
[Route("dataInMemory")]
public class DataInMemoryController : ControllerBase
{
    private readonly DataService _dataService;
    private readonly IMemoryCache _memoryCache;

    public DataInMemoryController(DataService dataService, IMemoryCache memoryCache)
    {
        _dataService = dataService;
        _memoryCache = memoryCache;
    }

    [HttpPatch]
    public IActionResult UpdateData(string data)
    {
        _dataService.Data = data;
        _memoryCache.Remove("GetDataWithCash1");
        _memoryCache.Remove("GetDataWithCash2");

        return Ok();
    }


    [HttpGet("GetDataWithOutCash")]
    public async Task<IActionResult> GetDataWithOutCash()
    {
        var data = await _dataService.GetData();

        await Console.Out.WriteLineAsync(data);

        return Ok("Data without cash");
    }

    [HttpGet("GetDataWithCash1")]
    public async Task<IActionResult> GetDataWithCash1()
    {
        var data = await _memoryCache.
            GetOrCreateAsync("GetDataWithCash1", async entry =>
            {
                //entry.AbsoluteExpiration = DateTimeOffset.UtcNow.AddSeconds(10);
                //entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(10);
                //entry.Priority = CacheItemPriority.Normal;

                entry.SlidingExpiration = TimeSpan.FromSeconds(10);
                return await _dataService.GetData();
            });

        await Console.Out.WriteLineAsync("1 " + data);

        return Ok("Data with cash");
    }

    [HttpGet("GetDataWithCash2")]
    public async Task<IActionResult> GetDataWithCash2()
    {
        if (_memoryCache.TryGetValue("GetDataWithCash2", out string? data) == false)
        {
            data = await _dataService.GetData();
            _memoryCache.Set("GetDataWithCash2", data,
                new MemoryCacheEntryOptions() { SlidingExpiration = TimeSpan.FromSeconds(10) });
        }

        await Console.Out.WriteLineAsync("2 " + data);

        return Ok("Data with cash");
    }

}

using DistributedCache.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace DistributedCache.Controllers;

[ApiController]
[Route("dataInDistributed")]
public class DataInDistributedController : ControllerBase
{
    private readonly DataService _dataService;
    private readonly IDistributedCache _distributedCache;

    public DataInDistributedController(DataService dataService, IDistributedCache memoryCache)
    {
        _dataService = dataService;
        _distributedCache = memoryCache;
    }

    [HttpPatch]
    public IActionResult UpdateData(string data)
    {
        _dataService.Data = data;
        _distributedCache.RefreshAsync("GetDataWithCash");

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
        var data = await _distributedCache.GetStringAsync("GetDataWithCash");
        if (data == null)
        {
            data = await _dataService.GetData();
            _ = _distributedCache.SetStringAsync("GetDataWithCash", data,
                new DistributedCacheEntryOptions() { SlidingExpiration = TimeSpan.FromSeconds(10) });
        }

        await Console.Out.WriteLineAsync("1 " + data);

        return Ok("Data with cash");
    }

}

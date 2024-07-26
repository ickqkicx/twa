namespace DistributedCache.Services;

public class DataService
{
    public string Data { get; set; } = "Data 1";

    public async Task<string> GetData()
    {
        await Task.Delay(3 * 1000);

        return Data;
    }

}

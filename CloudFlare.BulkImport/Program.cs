using CloudFlare.BulkDelete;
using CloudFlare.BulkImport;
using System.Net.Http.Json;
using System.Text.Json;
using System.Web;

AppConfig? cfg = JsonSerializer.Deserialize<AppConfig>(File.ReadAllText($"{Directory.GetCurrentDirectory()}\\appsettings.json"));
if (cfg is null)
{
    Console.WriteLine("Config couldn't be loaded!");
    return;
}

var api = new CfApi(cfg.AuthEmail, cfg.AuthKey);

var account = new Account(cfg.AccountId);

Console.WriteLine($"Starting to get zones..");
var deleteTasks = new List<Task>();
foreach (var domainName in cfg.DomainNames)
{
    deleteTasks.Add(Task.Run(async () =>
    {
        Console.WriteLine($"Creating zones for: {domainName}");
        await api.CreateZone(domainName, account, cfg.JumpStart, cfg.Type);
        Console.WriteLine($"Finished creating zones for: {domainName}");
    }));

}

await Task.WhenAll(deleteTasks);
Console.WriteLine($"Finished, all done :)");


public class CfApi
{
    private readonly HttpClient http;

    public CfApi(string authEmail, string authKey)
    {
        http = new HttpClient()
        {
            BaseAddress = new Uri("https://api.cloudflare.com/client/v4/")
        };

        http.DefaultRequestHeaders.Add("X-Auth-Email", authEmail);
        http.DefaultRequestHeaders.Add("X-Auth-Key", authKey);

    }

    public async Task CreateZone(string domainName, Account account, bool jumpStart, string type)
    {
        var resp = await http.PostAsJsonAsync("zones", new CreateZoneRequest(domainName, account, jumpStart, type));
        if (!resp.IsSuccessStatusCode)
        {
            string json = await resp.Content.ReadAsStringAsync();
            Console.WriteLine($"Domain failed: {domainName}\n{json}");
        }
    }

}
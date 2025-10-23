using System.Net.Http.Headers;
using System.Net.Http.Json;
using DisBot_Helper_Bot.Models;

namespace DisBot_Helper_Bot.Services;

public class DocsService
{
    private readonly ConfigService ConfigService;

    public DocsService(ConfigService configService)
    {
        ConfigService = configService;
    }

    public async Task<DocsSearchModel?> GetDocsPostAsync(string query, int limit, int? offset = 0)
    {
        var client = new HttpClient();
        var request = new HttpRequestMessage()
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri($"{ConfigService.Get().SupportHelper.DocsAPIUrl}/documents.search"),
            Headers =
            {
                { "Authorization", $"Bearer {ConfigService.Get().SupportHelper.DocsAPIToken}" },
            },
            Content = new StringContent(
                $"{{\"offset\":{offset},\"limit\":{limit},\"query\":\"{query}\",\"collectionId\":\"{ConfigService.Get().SupportHelper.DocsId}\"}}")
            {
                Headers =
                {
                    ContentType = new MediaTypeHeaderValue("application/json")
                }
            }
        };
        using var response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<DocsSearchModel>();
    }
}
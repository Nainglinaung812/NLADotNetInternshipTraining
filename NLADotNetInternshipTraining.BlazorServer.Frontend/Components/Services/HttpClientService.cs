using Newtonsoft.Json;
using System.Text;

namespace NLADotNetInternshipTraining.BlazorServer.Frontend.Services;

public class HttpClientService
{
    
    private readonly IHttpClientFactory _httpClientFactory;

    public HttpClientService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<T> ExecuteAsync<T>(string url, EnumHttpMethod method, object? obj = null)
    {
        HttpResponseMessage? responseMessage = null;
        HttpContent? content = null;

        if (obj != null)
        {
         
            content = new StringContent(obj.ToJson(), Encoding.UTF8, "application/json");
        }

     
        var client = _httpClientFactory.CreateClient("BlogApi");

        switch (method)
        {
            case EnumHttpMethod.Get:
                responseMessage = await client.GetAsync(url);
                break;
            case EnumHttpMethod.Post:
                responseMessage = await client.PostAsync(url, content);
                break;
            case EnumHttpMethod.Put:
                responseMessage = await client.PutAsync(url, content);
                break;
            case EnumHttpMethod.Patch:
                responseMessage = await client.PatchAsync(url, content);
                break;
            case EnumHttpMethod.Delete:
                responseMessage = await client.DeleteAsync(url);
                break;
            case EnumHttpMethod.None:
            default:
                throw new Exception("PLM Invalid HTTP method.");
        }

        if (responseMessage.IsSuccessStatusCode)
        {
            var jsonStr = await responseMessage.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(jsonStr)!;
        }

        throw new Exception("PLM Error occurred while executing the HTTP request.");
    }
}

public enum EnumHttpMethod
{
    None,
    Get,
    Post,
    Put,
    Patch,
    Delete
}
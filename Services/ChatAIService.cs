using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using your_auction_api.Models.Dto;
using your_auction_api.Services.IServices;

namespace your_auction_api.Services
{
    public class ChatAIService : IChatAIService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private const string OpenAiUrl = "https://api.openai.com/v1/chat/completions";

        public ChatAIService(IConfiguration configuration)
        {
            _httpClient = new HttpClient();
            _apiKey = configuration["OpenAI:ApiKey"];
        }

        public async Task<ChatResponseDto> AskAsync(ChatRequestDto request)
        {
            var payload = new
            {
                model = "gpt-3.5-turbo",
                messages = new[]
                {
                    new { role = "user", content = request.Message }
                },
                max_tokens = 256,
                temperature = 0.7
            };

            var httpRequest = new HttpRequestMessage(HttpMethod.Post, OpenAiUrl)
            {
                Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json")
            };
            httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

            var response = await _httpClient.SendAsync(httpRequest);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(responseString);
            var content = doc.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();

            return new ChatResponseDto { Response = content };
        }
    }
}

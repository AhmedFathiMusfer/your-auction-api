using System.Threading.Tasks;
using your_auction_api.Models.Dto;

namespace your_auction_api.Services.IServices
{
    public interface IChatAIService
    {
        Task<ChatResponseDto> AskAsync(ChatRequestDto request);
    }
}
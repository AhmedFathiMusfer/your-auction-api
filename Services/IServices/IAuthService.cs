

using ErrorOr;
using your_auction_api.Models.Dto;

namespace your_auction_api.Services.IServices
{
    public interface IAuthService
    {

        Task<ErrorOr<TokenDTO>> Login(LoginRequestDTO loginRequestDTO);
        Task<ErrorOr<TokenDTO>> Register(RegisterationRequestDTO regitsterationRequestDTO);
        /*   Task<TokenDTO> RefreshAccessToken(TokenDTO tokenDTO);
           Task RevokeAccessToken(TokenDTO tokenDTO);
           Task<ForgetPassworedDTO> ForgetPasswored(ForgetPassworedDTO forgetPassworedDTO);
           Task<ForgetPassworedConfirmationDTO> ForgetPassworedConfirmation(ForgetPassworedConfirmationDTO forgetPassworedConfirmationDTO);
           Task<bool> ResetPasswored(ResetPassworedDTO resetPassworedDTO);*/
    }
}
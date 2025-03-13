using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ErrorOr;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using your_auction_api.Data;
using your_auction_api.Models;
using your_auction_api.Models.Dto;
using your_auction_api.Services.IServices;

namespace your_auction_api.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly IEmailSender _emailSender;
        private string SecertKey;
        private IValidator<LoginRequestDTO> _loginValidator;
        private IValidator<RegisterationRequestDTO> _registerValidator;
        public AuthService(ApplicationDbContext db, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration
            , IEmailSender emailSender, IMapper mapper, IValidator<LoginRequestDTO> loginValidator, IValidator<RegisterationRequestDTO> registerValidator)
        {
            _db = db;
            SecertKey = configuration.GetValue<string>("ApiSettings:Secert");
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _emailSender = emailSender;
            _loginValidator = loginValidator;
            _registerValidator = registerValidator;
        }

        private bool IsUniqueUser(string username)
        {
            var user = _db.Users.FirstOrDefault(u => u.UserName.ToLower() == username.ToLower());
            if (user == null)
            {
                return true;
            }
            return false;
        }

        public async Task<ErrorOr<TokenDTO>> Login(LoginRequestDTO loginRequestDTO)
        {
            var resultValidator = _loginValidator.Validate(loginRequestDTO);
            if (!resultValidator.IsValid)
            {
                var error = resultValidator.Errors.ConvertAll(error => Error.Validation(code: error.PropertyName, description: error.ErrorMessage)).ToList();
                return error;
            }
            TokenDTO TokenDTO = new()
            {

                AccessToken = ""
            };

            var user = await _db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.UserName.ToLower() == loginRequestDTO.UserName.ToLower());
            var checkPassword = await _userManager.CheckPasswordAsync(user, loginRequestDTO.Password);

            if (user == null || checkPassword == false)
            {
                return Error.NotFound(description: "the email or password is wrong");
            }

            var JWTTokenId = $"JTI{Guid.NewGuid()}";
            //    var Refreshtoken = await CreateNewRefreshToken(user.Id, JWTTokenId);
            var JWTToken = await GetAccessTokenAsync(user, JWTTokenId);
            TokenDTO.AccessToken = JWTToken;
            //TokenDTO.RefreshToken = Refreshtoken;
            //  TokenDTO = await RefreshAccessToken(TokenDTO);






            return TokenDTO;
        }

        public async Task<ErrorOr<UserDTO>> Register(RegisterationRequestDTO regitsterationRequestDTO)
        {
            var resultValidator = _registerValidator.Validate(regitsterationRequestDTO);
            if (!resultValidator.IsValid)
            {
                var error = resultValidator.Errors.ConvertAll(error => Error.Validation(code: error.PropertyName, description: error.ErrorMessage)).ToList();
                return error;
            }
            ApplicationUser user = new ApplicationUser()
            {
                UserName = regitsterationRequestDTO.UserName,
                Name = regitsterationRequestDTO.Name,
                Email = regitsterationRequestDTO.UserName,
                NormalizedEmail = regitsterationRequestDTO.UserName.ToUpper()

            };
            try
            {
                var IsUserAlready = IsUniqueUser(regitsterationRequestDTO.UserName);
                if (!IsUserAlready)
                {
                    return Error.Validation(description: "this user is already");
                }
                var result = await _userManager.CreateAsync(user, regitsterationRequestDTO.Password);

                if (result.Succeeded)
                {
                    if (!_roleManager.RoleExistsAsync("admin").GetAwaiter().GetResult())
                    {
                        await _roleManager.CreateAsync(new IdentityRole("admin"));
                        await _roleManager.CreateAsync(new IdentityRole("customer"));
                    }
                    if (!_roleManager.RoleExistsAsync(regitsterationRequestDTO.Role).GetAwaiter().GetResult())
                    {
                        await _userManager.AddToRoleAsync(user, "customer");
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(user, regitsterationRequestDTO.Role);
                    }


                    var UserToReturn = _db.Users.FirstOrDefault(u => u.UserName == regitsterationRequestDTO.UserName);


                    var UserDTO = _mapper.Map<UserDTO>(UserToReturn);
                    return UserDTO;
                }
                var errors = result.Errors.Select(e => Error.Validation(description: e.Description)).ToList();
                return errors;
            }
            catch (Exception e)
            {
                return Error.Unexpected(description: e.Message);

            }
            // return new UserDTO();
        }
        private async Task<string> GetAccessTokenAsync(ApplicationUser user, string JWTTokenId)
        {
            var role = await _userManager.GetRolesAsync(user);
            //Generate Token
            var tokenhandeler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(SecertKey);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserName.ToString()),
                    new Claim(ClaimTypes.Role, role.FirstOrDefault()),
                    new Claim(JwtRegisteredClaimNames.Jti,JWTTokenId),
                    new Claim(JwtRegisteredClaimNames.Sub,user.Id)


                }),
                Expires = DateTime.UtcNow.AddMinutes(3),

                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var userDTO = _mapper.Map<UserDTO>(user);

            var token = tokenhandeler.CreateToken(tokenDescriptor);

            return tokenhandeler.WriteToken(token);
        }

        private bool GetAccessTokenData(string AccessToken, string excectedUserId, string excectedTokenId)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwt = tokenHandler.ReadJwtToken(AccessToken);
                var userId = jwt.Claims.FirstOrDefault(t => t.Type == JwtRegisteredClaimNames.Sub).Value;
                var tokenId = jwt.Claims.FirstOrDefault(t => t.Type == JwtRegisteredClaimNames.Jti).Value;
                if (excectedUserId != userId || excectedTokenId != tokenId)
                {
                    return false;
                }
                return true;
            }
            catch
            {
                return false;
            }

        }



        /*  public async Task RevokeAccessToken(TokenDTO tokenDTO)
 {
     var existingRefreshToken = await _db.refreshTokens.FirstOrDefaultAsync(r => r.Refresh_Token == tokenDTO.RefreshToken);
     if (existingRefreshToken == null)
     {
         return;
     }
     var accessTokenData = GetAccessTokenData(tokenDTO.AccessToken, existingRefreshToken.UserId, existingRefreshToken.JwtTokenId);
     if (!accessTokenData)
     {
         return;
     }
     await MarkAllTokenInChainAsInvalid(existingRefreshToken.UserId, existingRefreshToken.JwtTokenId);
 }
 public async Task<TokenDTO> RefreshAccessToken(TokenDTO tokenDTO)
 {
     var existingRefreshToken = await _db.refreshTokens.FirstOrDefaultAsync(r => r.Refresh_Token == tokenDTO.RefreshToken);
     if (existingRefreshToken == null)
     {
         return new TokenDTO();
     }
     if (!existingRefreshToken.Is_Valid)
     {
         await MarkAllTokenInChainAsInvalid(existingRefreshToken.UserId, existingRefreshToken.JwtTokenId);
         return new TokenDTO();
     }
     var accessTokenData = GetAccessTokenData(tokenDTO.AccessToken, existingRefreshToken.UserId, existingRefreshToken.JwtTokenId);
     if (!accessTokenData)
     {

         await MarkTokenAsInvalid(existingRefreshToken);

         return new TokenDTO();
     }
     if (existingRefreshToken.ExpieresAt < DateTime.UtcNow)
     {
         await MarkTokenAsInvalid(existingRefreshToken);
         return new TokenDTO();
     }

     var newRefreshAccessToken = await CreateNewRefreshToken(existingRefreshToken.UserId, existingRefreshToken.JwtTokenId);

     await MarkTokenAsInvalid(existingRefreshToken);

     var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == existingRefreshToken.UserId);
     if (user == null)
     {
         return new TokenDTO();
     }
     var newAccessToken = await GetAccessTokenAsync(user, existingRefreshToken.JwtTokenId);

     return new TokenDTO { RefreshToken = newRefreshAccessToken, AccessToken = newAccessToken };

     throw new NotImplementedException();

 }
 private async Task<string> CreateNewRefreshToken(string userId, string tokenId)
 {
     RefreshToken refreshToken = new RefreshToken()
     {
         UserId = userId,
         JwtTokenId = tokenId,
         ExpieresAt = DateTime.UtcNow.AddMinutes(3),
         Is_Valid = true,
         Refresh_Token = $"{Guid.NewGuid()}-{Guid.NewGuid()}"
     };
     await _db.refreshTokens.AddAsync(refreshToken);
     await _db.SaveChangesAsync();
     return refreshToken.Refresh_Token;
 }
 private async Task MarkAllTokenInChainAsInvalid(string UserId, string tokenId)
 {
     var refreshTokens = _db.refreshTokens.Where(r => r.UserId == UserId && r.JwtTokenId == tokenId).ToList();
     foreach (var refreshToken in refreshTokens)
     {
         refreshToken.Is_Valid = false;
     }
     _db.refreshTokens.UpdateRange(refreshTokens);
     await _db.SaveChangesAsync();
 }
 private Task MarkTokenAsInvalid(RefreshToken refreshToken)
 {
     refreshToken.Is_Valid = false;
     return _db.SaveChangesAsync();
 }

 public async Task<ForgetPassworedDTO> ForgetPasswored(ForgetPassworedDTO forgetPassworedDTO)
 {
     var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == forgetPassworedDTO.Email);
     if (user == null)
     {
         return null;
     }
     try
     {
         var Token = await _userManager.GeneratePasswordResetTokenAsync(user);
         var code = GenerateRandomNumber();

         ForgetPasswored forgetPasswored = new ForgetPasswored()
         {
             code = code,
             Email = user.Email,
             Token = Token,
             ExpieresAt = DateTime.UtcNow.AddMinutes(2),
             Is_Valid = true
         };
         await _db.fogetPassworeds.AddAsync(forgetPasswored);
         await _db.SaveChangesAsync();
         await _emailSender.SendEmailAsync(user.Email,
              "Reset Password",
              $"Verification codes: <h4>{code}</h4>.");

         return forgetPassworedDTO;
     }
     catch (Exception ex)
     {
         return null;
     }


 }

 public async Task<ForgetPassworedConfirmationDTO> ForgetPassworedConfirmation(ForgetPassworedConfirmationDTO forgetPassworedConfirmationDTO)
 {
     var GetToken = await _db.fogetPassworeds.FirstOrDefaultAsync(t => t.Email == forgetPassworedConfirmationDTO.Email
     && t.code == forgetPassworedConfirmationDTO.Code);
     if (GetToken == null)
     {
         return null;
     }
     if (!GetToken.Is_Valid)
     {
         return null;
     }
     if (GetToken.ExpieresAt < DateTime.UtcNow)
     {
         GetToken.Is_Valid = false;
         await _db.SaveChangesAsync();
         return null;
     }

     ForgetPassworedConfirmationDTO forgetPassworedDTO = new ForgetPassworedConfirmationDTO()
     {
         Email = forgetPassworedConfirmationDTO.Email,
         Code = forgetPassworedConfirmationDTO.Code,
     };
     return forgetPassworedDTO;
 }
 private int GenerateRandomNumber()
 {
     Random random = new Random();
     return random.Next(100000, 1000000);
 }

 public async Task<bool> ResetPasswored(ResetPassworedDTO resetPassworedDTO)
 {
     var user = await _db.ApplicationUsers.FirstOrDefaultAsync(u => u.Email == resetPassworedDTO.Email);
     if (user == null)
     {
         return false;
     }

     var Token = await _db.fogetPassworeds.FirstOrDefaultAsync(t => t.code == resetPassworedDTO.Code && t.Is_Valid == true && t.Email == resetPassworedDTO.Email);
     if (Token == null)
     {

         return false;
     }
     var result = await _userManager.ResetPasswordAsync(user, Token.Token, resetPassworedDTO.Password);
     if (result.Succeeded)
     {
         Token.Is_Valid = false;
         await _db.SaveChangesAsync();
         return true;
     }
     return false;


 }*/
    }
}
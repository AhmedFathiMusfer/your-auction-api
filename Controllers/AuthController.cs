using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using your_auction_api.Models.Dto;
using your_auction_api.Services.IServices;

namespace your_auction_api.Controllers
{

    [Route("api/[controller]")]
    public class AuthController : ApiController
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpPost("login")]
        public Task<IActionResult> login(LoginRequestDTO loginRequest)
        {
            var authResult = _authService.Login(loginRequest);
            return authResult.Match(
           authResult => Ok(authResult),
           Problem);

        }
        [HttpPost("register")]
        public Task<IActionResult> register(RegisterationRequestDTO registerationRequest)
        {
            var authResult = _authService.Register(registerationRequest);
            return authResult.Match(
           authResult => Ok(authResult),
           Problem);

        }

    }
}
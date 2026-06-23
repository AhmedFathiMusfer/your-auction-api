using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using your_auction_api.Models.Specifications;
using your_auction_api.Services.IServices;
using your_auction_api.Utility;

namespace your_auction_api.Controllers
{

    [Route("api/[controller]")]
    [Authorize(Roles = Roles.Admin)]
    public class UserController : ApiController
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet("Count")]
        public async Task<IActionResult> GetUsersCount()
        {
            var result = await _userService.GetCountUsers();
            return result.Match(
               Count => Ok(new { count = Count })
                , Problem);
        }
        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery] UserSpecification spec)
        {

            var result = await _userService.GetUsers(spec);
            return result.Match(
                users => Ok(users),
                Problem);
        }
    }
}
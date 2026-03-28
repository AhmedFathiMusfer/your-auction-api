using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using your_auction_api.Models.Dto;
using your_auction_api.Services.IServices;

namespace your_auction_api.Controllers
{

    [Route("api/[controller]")]
    public class DashboardController : ApiController
    {
        private readonly IUserService _userService;
        private readonly IProductService _productService;
        private readonly IAuctionService _auctionService;

        public DashboardController(IUserService userService, IProductService productService, IAuctionService auctionService)
        {
            _userService = userService;
            _productService = productService;
            _auctionService = auctionService;
        }

        [HttpGet("Summary")]
        public async Task<IActionResult> GetDashboardCounts()
        {
            var userCount = await _userService.GetCountUsers();
            var productCount = await _productService.GetCountProducts();
            var auctionCount = await _auctionService.GetCountAuctions();
            if (userCount.IsError || productCount.IsError || auctionCount.IsError)
            {
                return Problem(error: (userCount.Errors ?? productCount.Errors ?? auctionCount.Errors).FirstOrDefault());
            }

            return Ok(new DashboardDataDto
            {
                CountUsers = userCount.Value,
                CountProducts = productCount.Value,
                CountAuctions = auctionCount.Value
            });
        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ErrorOr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using your_auction_api.Models;
using your_auction_api.Models.Dto;
using your_auction_api.Services.IServices;


namespace your_auction_api.Controllers
{

    [Route("api/[controller]")]
    [Authorize]
    public class AuctionController : ApiController
    {


        private readonly IAuctionService _auctionService;
        public AuctionController(IAuctionService auctionService)
        {
            _auctionService = auctionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAuctions()
        {
            var result = await _auctionService.GetAuctions();
            return result.Match(
                auctions => Ok(auctions),
                 Problem
            );
        }

        [HttpGet("{auctionId}")]
        public async Task<IActionResult> getAuctionById(int auctionId)
        {
            var result = await _auctionService.getAuctionById(auctionId);
            return result.Match(
                auction => Ok(auction),
                Problem
            );
        }

        [HttpPost]
        public async Task<IActionResult> AddAuction([FromBody] AuctionDto auctionDto)
        {
            var result = await _auctionService.AddAuction(auctionDto);
            return result.Match(
                success => Ok(success),
                Problem
            );
        }
        [HttpPost("{auctionId}/AddAuctionUser")]
        public async Task<IActionResult> AddAuctionUser([FromBody] AuctionUser auctionUser)
        {
            var result = await _auctionService.AddAuctionUser(auctionUser);
            return result.Match(
                success => Ok(success),
                Problem
            );
        }
        [HttpPut("{auctionId}")]
        public async Task<IActionResult> UpdateAuction(int auctionId, [FromBody] AuctionDto auctionDto)
        {
            var result = await _auctionService.UpdateAuction(auctionId, auctionDto);
            return result.Match(
                success => Ok(success),
                Problem
            );
        }
        [HttpPut("{auctionId}/Canceled")]
        public async Task<IActionResult> CanceledAuction(int auctionId)
        {
            var result = await _auctionService.CanceledAuction(auctionId);
            return result.Match(
                success => Ok(success),
                Problem
            );
        }
        [HttpDelete("{auctionId}")]
        public async Task<IActionResult> DeleteAuction(int auctionId)
        {
            var result = await _auctionService.DeleteAuction(auctionId);
            return result.Match(
                deleted => Ok(deleted),
                Problem
            );
        }
        [HttpGet("{auctionId}/details")]
        public async Task<IActionResult> detailsAuction(int auctionId)
        {
            var result = await _auctionService.detailsAuction(auctionId);
            return result.Match(
                auctionDetails => Ok(auctionDetails),
                Problem
            );
        }
        [HttpGet("{auctionId}/users")]
        public async Task<IActionResult> getAuctionUsers(int auctionId)
        {
            var result = await _auctionService.getAuctionUsers(auctionId);
            return result.Match(
                auctionUsers => Ok(auctionUsers),
                Problem
            );
        }


    }
}
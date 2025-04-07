using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using your_auction_api.Services.IServices;

namespace your_auction_api.Controllers
{

    [Route("api/[controller]")]
    [Authorize]
    public class FileController : ApiController
    {
        private readonly IFileService _fileService;
        public FileController(IFileService fileService)
        {
            _fileService = fileService;
        }
        [HttpPost("UploadImageToTemp")]
        public async Task<IActionResult> UploadImageToTemp(IFormFile file)
        {
            var result = await _fileService.UploadImageToTemp(file);
            return result.Match(
                result => Ok(new { url = result }),
                Problem);
        }
        [HttpPost("AddImageToProduct/{ProductId}")]
        public async Task<IActionResult> AddImageToProduct(IFormFile file, int ProductId)
        {
            var result = await _fileService.UploadImageToProduct(file, ProductId);
            return result.Match(
                result => Ok(new { url = result }),
                Problem);
        }
        [HttpDelete("DeleteImageFromProduct/{ProductId}")]
        public async Task<IActionResult> DeleteImageFromProduct(int ProductId, string ImageUrl)
        {
            var result = await _fileService.DeleteImageFromProduct(ProductId, ImageUrl);
            return result.Match(
                result => NoContent(),
                Problem);
        }
        [HttpDelete("DeleteImageFromTemp")]
        public async Task<IActionResult> DeleteImageFromTemp(string ImageUrl)
        {
            var result = await _fileService.DeleteImageFromTemp(ImageUrl);
            return result.Match(
                result => NoContent(),
                Problem);
        }


    }
}
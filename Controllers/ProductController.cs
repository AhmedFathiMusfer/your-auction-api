using System.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using your_auction_api.Services;
using your_auction_api.Services.IServices;
using your_auction_api.Models;
using Microsoft.AspNetCore.Authorization;
using your_auction_api.Models.Dto;

namespace your_auction_api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]

    public class ProductController : ApiController
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> getAll()
        {
            var result = await _productService.GetProducts();

            return result.Match(
               result => Ok(result),
               Problem
              );
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> get(int id)
        {
            var result = await _productService.getProductById(id);

            return result.Match(
               result => Ok(result),
               Problem
              );
        }
        [HttpPost]
        public async Task<IActionResult> Add(ProductDto productDto)
        {
            var result = await _productService.AddProduct(productDto);

            return result.Match(
               result => Ok(result),
               Problem
              );
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int Id, ProductDto productDto)
        {
            var result = await _productService.UpdateProduct(Id, productDto);

            return result.Match(
               result => Ok(result),
               Problem
              );
        }


        [HttpDelete("id:int")]
        public async Task<IActionResult> Delete(int productId)
        {
            var result = await _productService.DeleteProduct(productId);

            return result.Match(
               result => Ok(result),
               Problem
              );
        }




    }
}
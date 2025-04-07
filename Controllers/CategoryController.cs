using System.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using your_auction_api.Services.IServices;
using your_auction_api.Models;

namespace your_auction_api.Controllers
{

    [Route("api/[controller]")]
    [Authorize]
    public class CategoryController : ApiController
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        [HttpGet]
        [ProducesResponseType(typeof(List<Category>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetCategories()
        {
            var result = await _categoryService.GetCategories();
            return result.Match(
                     categories => Ok(categories),
                     Problem
                 );

        }
        [HttpGet("{categoryId:int}")]

        public async Task<IActionResult> GetCategoryById(int categoryId)
        {
            var result = await _categoryService.getCategoryById(categoryId);
            return result.Match(
                     category => Ok(category),
                     Problem
                 );
        }
        [HttpPost]

        public async Task<IActionResult> AddCategory([FromBody] Category category)
        {
            var result = await _categoryService.AddCategory(category);
            return result.Match(
                     result => Created(),
                     Problem
                 );
        }
        [HttpPut("{categoryId:int}")]
        public async Task<IActionResult> UpdateCategory(int categoryId, [FromBody] Category category)
        {
            var result = await _categoryService.UpdateCategory(categoryId, category);
            return result.Match(
                     result => NoContent(),
                     Problem
                 );
        }
        [HttpDelete("{categoryId:int}")]
        public async Task<IActionResult> DeleteCategory(int categoryId)
        {
            var result = await _categoryService.DeleteCategory(categoryId);
            return result.Match(
                     category => NoContent(),
                     Problem
                 );
        }
    }
}
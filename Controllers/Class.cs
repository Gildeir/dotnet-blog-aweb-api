using BlogWebApi.Data;
using BlogWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace BlogWebApi.Controllers
{
    [ApiController]
    [Route("")]
    public class CategoryController: ControllerBase 
    {
        [HttpGet("v1/categories")]
        public async Task<IActionResult> GetAsync
            (
                [FromServices] BlogDataContext context
            )
        {
            var categories = await context.Categories.ToListAsync();

            return Ok(categories);
        }

        [HttpGet("v1/categories/{id:int}")]
        public async Task<IActionResult> GetByIdAsync
            (
                [FromRoute] int id,
                [FromServices] BlogDataContext context
            )
        {
            var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (category is null) return NotFound();

            return Ok(category);
        }   
        
        [HttpPost("v1/categories/create")]
        public async Task<IActionResult> PostAsync
            (
                [FromBody] Category categoryModel,
                [FromServices] BlogDataContext context
            )
        {
            await context.Categories.AddAsync(categoryModel);
            await context.SaveChangesAsync();

            return Created($"categories/{categoryModel.Id}", categoryModel);
        }        
        
        [HttpPut("v1/categories/edit")]
        public async Task<IActionResult> PutAsync
            (
                [FromRoute] int id,
                [FromBody] Category categoryModel,
                [FromServices] BlogDataContext context
            )
        {
            var editedCategoty = new List<Category>();

            var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);

            if (category is null) return NotFound();

            category.Name = categoryModel.Name;
            category.Slug = categoryModel.Slug;

            context.Categories.Update(category);
            
            await context.SaveChangesAsync();

            return Ok(categoryModel); ;
        }        
        
        [HttpDelete("v1/categories/edit")]
        public async Task<IActionResult> DeleteAsync
            (
                [FromRoute] int id,
                [FromServices] BlogDataContext context
            )
        {

            var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);

            if (category is null) return NotFound();


            context.Categories.Remove(category);
            
            await context.SaveChangesAsync();

            return Ok(category); ;
        }
    }
}

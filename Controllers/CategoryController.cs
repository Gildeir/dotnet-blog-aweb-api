using BlogWebApi.Data;
using BlogWebApi.Models;
using BlogWebApi.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace BlogWebApi.Controllers
{
    [ApiController]
    [Route("")]
    public class CategoryController : ControllerBase
    {
        [HttpGet("v1/categories")]
        public async Task<IActionResult> GetAsync
            (
                [FromServices] BlogDataContext context
            )
        {
            try
            {
                var categories = await context.Categories.ToListAsync();

                return Ok(categories);
            
    }
            catch (Exception e)
            {
                return StatusCode(500, $"Falha interna no servidor - {e.Message}");
            }
        }

        [HttpGet("v1/categories/{id:int}")]
        public async Task<IActionResult> GetByIdAsync
            (
                [FromRoute] int id,
                [FromServices] BlogDataContext context
            )
        {
            try
            {
                var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);
                if (category is null) return NotFound();

                return Ok(category);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Falha interna no servidor - {e.Message}");
            }
        }

        [HttpPost("v1/categories/create")]
        public async Task<IActionResult> PostAsync
            (
                [FromBody] EditorCategoryViewModel viewModel,
                [FromServices] BlogDataContext context
            )
        {
            try
            {
                var category = new Category
                {
                    Name = viewModel.Name,
                    Slug = viewModel.Slug,
                };

                await context.Categories.AddAsync(category);
                await context.SaveChangesAsync();

                return Created($"categories/{category.Name}", viewModel);
            }
            catch (DbUpdateException ex)
            { 
                return StatusCode(500, $"Não foi possível incluir a categoria{ex.Message}");
            }
            
            catch (Exception e)
            {
                return StatusCode(500, "Falha interna no servidor");
            }
        }

        [HttpPut("v1/categories/{id:int}")]
        public async Task<IActionResult> PutAsync
            (
                [FromRoute] int id,
                [FromBody] Category categoryModel,
                [FromServices] BlogDataContext context
            )
        {
            try
            {
                var editedCategoty = new List<Category>();

                var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);

                if (category is null) return NotFound();

                category.Name = categoryModel.Name;
                category.Slug = categoryModel.Slug;

                context.Categories.Update(category);

                await context.SaveChangesAsync();

                return Ok(categoryModel);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Falha interna no servidor");
            }
        }

        [HttpDelete("v1/categories/{id:int}")]
        public async Task<IActionResult> DeleteAsync
            (
                [FromRoute] int id,
                [FromServices] BlogDataContext context
            )
        {
            try
            {
                var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);

                if (category is null) return NotFound();

                context.Categories.Remove(category);

                await context.SaveChangesAsync();

                return Ok(category); ;
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Não foi possível excluir a categoria{ex.Message}");
            }

            catch (Exception e)
            {
                return StatusCode(500, "Falha interna no servidor");
            }
        }
    }
}

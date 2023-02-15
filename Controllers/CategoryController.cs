using BlogWebApi.Data;
using BlogWebApi.Extensions;
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
                
                 var result = new ResultViewModel<List<Category>>(categories);
                
                return Ok(result);
            
    }
            catch (Exception e)
            {
                var result = new ResultViewModel<List<Category>>($"Falha interna no servidor - {e.Message}");
                return StatusCode(500, result);
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
                
                var result = new ResultViewModel<Category>(category);
               
                if (category is null) return NotFound(new ResultViewModel<List<Category>>($"Nothing to show :("));

                return Ok(result);
            }
            catch (Exception e)
            {
                var result = new ResultViewModel<List<Category>>($"Falha interna no servidor - {e.Message}");
                return StatusCode(500, result);
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

                if (!ModelState.IsValid)
                {
                    return BadRequest(new ResultViewModel<Category>(ModelState.GetErros()));
                }

                var category = new Category
                {
                    Name = viewModel.Name,
                    Slug = viewModel.Slug,
                };

                await context.Categories.AddAsync(category);
                await context.SaveChangesAsync();

                return Created($"categories/{category.Name}", new ResultViewModel<Category>(category));
            }
            catch (DbUpdateException ex)
            {
                var result = new ResultViewModel<List<Category>>($"Falha interna no servidor - {e.Message}");
                return StatusCode(500, result);
            }
            
            catch (Exception e)
            {
                var result = new ResultViewModel<List<Category>>($"Falha interna no servidor - {e.Message}");
                return StatusCode(500, result);
            }
        }

        [HttpPut("v1/categories/{id:int}")]
        public async Task<IActionResult> PutAsync
            (
                [FromRoute] int id,
                [FromBody] EditorCategoryViewModel viewModel,
                [FromServices] BlogDataContext context
            )
        {
            try
            {
                var editedCategoty = new List<Category>();

                var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);

                if (category is null) return NotFound();

                category.Name = viewModel.Name;
                category.Slug = viewModel.Slug;

                context.Categories.Update(category);

                await context.SaveChangesAsync();

                return Ok(category);
            }
            catch (Exception e)
            {
                var result = new ResultViewModel<List<Category>>($"Falha interna no servidor - {e.Message}");
                return StatusCode(500, result);
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
                var result = new ResultViewModel<List<Category>>($"Não foi possível excluir categoria - {ex.Message}");
                return StatusCode(500, result);
            }

            catch (Exception e)
            {
                var result = new ResultViewModel<List<Category>>($"Falha interna no servidor - {e.Message}");
                return StatusCode(500, result);
            }
        }
    }
}

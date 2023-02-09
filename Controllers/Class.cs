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
            var categoria = await context.Categories.ToListAsync();

            return Ok(categoria);
        }
    }
}

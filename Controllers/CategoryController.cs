using CatalogoApi.Data;
using CatalogoApi.Models;
using CatalogoApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CatalogoApi.Controllers
{
    [ApiController]
    [Route("api/v1/[Controller]")]
    public class CategoryController : ControllerBase
    {
        private CategoryService _categoryService;

        public CategoryController(AppDbContext dbContext, CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("produtos")]
        public ActionResult<IEnumerable<Category>> GetAllWithProducts([FromQuery] int page)
        {
            if (page < 0) page = 0;
            IEnumerable<Category> categories = _categoryService.FindAllWithProducts(page);
            return Ok(categories);
        }

        [HttpGet]
        public ActionResult<IEnumerable<Category>> GetAll([FromQuery] int page)
        {
            if (page < 0) page = 0;
            IEnumerable<Category> categories = _categoryService.FindAll(page);
            return Ok(categories);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Category category)
        {
            _categoryService.Create(category);

            return CreatedAtAction(nameof(GetById), category.CategoryId);
        }

        [HttpGet("{id:int:min(1)}")]
        public IActionResult GetById(int id)
        {
            Category category = _categoryService.FindById(id);
            return Ok(category);
        }

        [HttpPut("{id:int}")]
        public IActionResult Update(int id, Category category)
        {
            _categoryService.Update(id, category);
            return Ok(category);
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            _categoryService.Delete(id);
            return NoContent();
        }

    }
}

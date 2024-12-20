using AutoMapper;
using CatalogoApi.Data;
using CatalogoApi.Dtos;
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
        private readonly CategoryService _categoryService;
        private readonly IMapper _mapper;

        public CategoryController(CategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }

        [HttpGet("products")]
        public ActionResult<IEnumerable<Category>> GetAllWithProducts([FromQuery] int page)
        {
            if (page < 0) page = 0;
            IEnumerable<CategoryResponseDto> categories = _mapper.Map<IEnumerable<CategoryResponseDto>>(_categoryService.FindAllWithProducts(page));
            return Ok(categories);
        }

        [HttpGet]
        public ActionResult<IEnumerable<Category>> GetAll([FromQuery] int page)
        {
            if (page < 0) page = 0;
            IEnumerable<CategoryResponseDto> categories =  _mapper.Map<IEnumerable<CategoryResponseDto>>(_categoryService.FindAll(page));
            return Ok(categories);
        }

        [HttpPost]
        public IActionResult Create([FromBody] CategoryRequestDto categoryRequestDto)
        {
            Category category = _categoryService.Create(_mapper.Map<Category>(categoryRequestDto));
            CategoryResponseDto categoryResponseDto = _mapper.Map<CategoryResponseDto>(category);
            
            return CreatedAtAction(nameof(GetById), new { id = categoryResponseDto.CategoryId }, categoryResponseDto);
        }

        [HttpGet("{id:int:min(1)}")]
        public ActionResult<CategoryResponseDto> GetById(int id)
        {
            Category category = _categoryService.FindById(id);
            return Ok(category);
        }

        [HttpPut("{id:int}")]
        public ActionResult<CategoryResponseDto> Update(int id, [FromBody] CategoryRequestDto categoryRequestDto)
        {
            Category category = _categoryService.Update(id, _mapper.Map<Category>(categoryRequestDto));
            return Ok(_mapper.Map<CategoryResponseDto>(category));
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            _categoryService.Delete(id);
            return NoContent();
        }

    }
}

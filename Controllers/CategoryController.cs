using Asp.Versioning;
using AutoMapper;
using CatalogoApi.Data;
using CatalogoApi.Dtos;
using CatalogoApi.Models;
using CatalogoApi.Pagination;
using CatalogoApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CatalogoApi.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[Controller]")]
    [ApiVersion("1.0")]
    public class CategoryController : ControllerBase
    {
        private readonly CategoryService _categoryService;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(CategoryService categoryService, IMapper mapper, ILogger<CategoryController> logger)
        {
            _categoryService = categoryService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("products")]
        public async Task<ActionResult<PageListResponseDto<CategoryResponseDto>>> GetAllWithProductsAsync([FromQuery] int page)
        {
            if (page < 0) page = 0;
            IEnumerable<CategoryResponseDto> categories = _mapper.Map<IEnumerable<CategoryResponseDto>>(await _categoryService.FindAllWithProductsAsync(page));
            return Ok(PageList<CategoryResponseDto>.ToPagedList(categories, page).ToPageListResponse());
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        public async Task<ActionResult<PageListResponseDto<CategoryResponseDto>>> GetAllAsync([FromQuery] int page)
        {
            if (page < 0) page = 0;
            IEnumerable<CategoryResponseDto> categories = _mapper.Map<IEnumerable<CategoryResponseDto>>(await _categoryService.FindAllAsync(page));
            return Ok(PageList<CategoryResponseDto>.ToPagedList(categories, page).ToPageListResponse());
        }

        [HttpGet("name")]
        public async Task<ActionResult<PageListResponseDto<CategoryResponseDto>>> GetAllByNameAsync([FromQuery] int page, [FromQuery] string name = "")
        {
            if (page < 0) page = 0;
            IEnumerable<CategoryResponseDto> categories = _mapper.Map<IEnumerable<CategoryResponseDto>>(await _categoryService.FindAllFilteredByNameAsync(page, name));
            return Ok(PageList<CategoryResponseDto>.ToPagedList(categories, page).ToPageListResponse());
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "AdminOnly")]
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CategoryRequestDto categoryRequestDto)
        {
            Category category = await _categoryService.CreateAsync(_mapper.Map<Category>(categoryRequestDto));
            CategoryResponseDto categoryResponseDto = _mapper.Map<CategoryResponseDto>(category);

            return Created($"https://localhost:7123/api/v1/category/{category.CategoryId}", categoryResponseDto);
            // return CreatedAtAction(nameof(GetByIdAsync), new { id = categoryResponseDto.CategoryId }, categoryResponseDto);
        }

        [HttpGet("{id:int:min(1)}")]
        public async Task<ActionResult<CategoryResponseDto>> GetByIdAsync(int id)
        {
            Category category = await _categoryService.FindByIdAsync(id);
            return Ok(_mapper.Map<CategoryResponseDto>(category));
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<Dtos.CategoryResponseDto>> UpdateAsync(int id, [FromBody] CategoryRequestDto categoryRequestDto)
        {
            Category category = await _categoryService.UpdateAsync(id, _mapper.Map<Category>(categoryRequestDto));
            return base.Ok(_mapper.Map<CategoryResponseDto>(category));
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            await _categoryService.Delete(id);
            return NoContent();
        }

    }
}

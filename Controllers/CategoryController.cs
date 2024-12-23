using AutoMapper;
using CatalogoApi.Data;
using CatalogoApi.Dtos;
using CatalogoApi.Models;
using CatalogoApi.Pagination;
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
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(CategoryService categoryService, IMapper mapper, ILogger<CategoryController> logger)
        {
            _categoryService = categoryService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("products")]
        public ActionResult<PageListResponseDto<CategoryResponseDto>> GetAllWithProducts([FromQuery] int page)
        {
            if (page < 0) page = 0;
            IEnumerable<CategoryResponseDto> categories = _mapper.Map<IEnumerable<CategoryResponseDto>>(_categoryService.FindAllWithProducts(page));
            return Ok(PageList<CategoryResponseDto>.ToPagedList(categories, page).ToPageListResponse());
        }

        [HttpGet]
        public ActionResult<PageListResponseDto<CategoryResponseDto>> GetAll([FromQuery] int page)
        {
            if (page < 0) page = 0;
            IEnumerable<CategoryResponseDto> categories = _mapper.Map<IEnumerable<CategoryResponseDto>>(_categoryService.FindAll(page));
            return Ok(PageList<CategoryResponseDto>.ToPagedList(categories, page).ToPageListResponse());
        }

        [HttpGet("/name")]
        public ActionResult<PageListResponseDto<CategoryResponseDto>> GetAllByName([FromQuery] int page, [FromQuery] string name = "")
        {
            if (page < 0) page = 0;
            IEnumerable<CategoryResponseDto> categories = _mapper.Map<IEnumerable<CategoryResponseDto>>(_categoryService.FindAllFilteredByName(page, name));
            return Ok(PageList<CategoryResponseDto>.ToPagedList(categories, page).ToPageListResponse());
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
            return Ok(_mapper.Map<CategoryResponseDto>(category));
        }

        [HttpPut("{id:int}")]
        public ActionResult<Dtos.CategoryResponseDto> Update(int id, [FromBody] CategoryRequestDto categoryRequestDto)
        {
            Category category = _categoryService.Update(id, _mapper.Map<Category>(categoryRequestDto));
            return base.Ok(_mapper.Map<CategoryResponseDto>(category));
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            _categoryService.Delete(id);
            return NoContent();
        }

    }
}

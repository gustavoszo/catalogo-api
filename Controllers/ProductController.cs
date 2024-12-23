using System.Runtime.CompilerServices;
using AutoMapper;
using CatalogoApi.Data;
using CatalogoApi.Dtos;
using CatalogoApi.Models;
using CatalogoApi.Pagination;
using CatalogoApi.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CatalogoApi.Controllers
{
    [ApiController]
    [Route("api/v1/[Controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _productService;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductController> _logger;

        public ProductController(ProductService productService, IMapper mapper, ILogger<ProductController> logger)
        {
            _productService = productService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<PageListResponseDto<ProductResponseDto>>> GetAllAsync([FromQuery] int page)
        {
            if (page < 0) page = 0;
            IEnumerable<ProductResponseDto> products = _mapper.Map<IEnumerable<ProductResponseDto>>(await _productService.FindAllAsync(page));
            return Ok(PageList<ProductResponseDto>.ToPagedList(products, page).ToPageListResponse());
        }

        [HttpGet("price")]
        public async Task<ActionResult<PageListResponseDto<ProductResponseDto>>> GetAllByPriceAsync([FromQuery] int page, 
            [FromQuery] double min, [FromQuery] double max)
        {
            if (page < 0) page = 0;
            if (min < 0) min = 0;
            if (max < 0) max = 0;
            if (min > max) min = max;


            IEnumerable<ProductResponseDto> products = _mapper.Map<IEnumerable<ProductResponseDto>>(await _productService.FindAllByPriceAsync(page, min, max));
            return Ok(PageList<ProductResponseDto>.ToPagedList(products, page).ToPageListResponse());
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] ProductRequestDto productRequestDto)
        {
            Product product = await _productService.CreateAsync(_mapper.Map<Product>(productRequestDto));
            ProductResponseDto productResponseDto = _mapper.Map<ProductResponseDto>(product);

            return CreatedAtAction(nameof(GetByIdAsync), new { id = productResponseDto.ProductId }, productResponseDto);
        }

        [HttpGet("{id:int:min(1)}")]
        public async Task<ActionResult<ProductResponseDto>> GetByIdAsync(int id)
        {
            Product product = await _productService.FindByIdAsync(id);
            return Ok(_mapper.Map<ProductResponseDto>(product));
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<ProductResponseDto>> UpdateAsync(int id, [FromBody] ProductRequestDto productRequestDto)
        {
            Product product = await _productService.UpdateAsync(id, _mapper.Map<Product>(productRequestDto));
            return Ok(_mapper.Map<ProductResponseDto>(product));
        }


        [HttpPatch("{id:int}/partialUpdate")]
        public async Task<IActionResult> PartialUpdateAsync(int id, [FromBody] JsonPatchDocument<ProductUpdateDto> productUpdateDto)
        {
            Product product = await _productService.PartialUpdateAsync(id, productUpdateDto);

            if (!TryValidateModel(product))
            {
                return BadRequest(ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            await _productService.DeleteAsync(id);
            return NoContent();
        }

    }
}

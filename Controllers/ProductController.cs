using System.Runtime.CompilerServices;
using Asp.Versioning;
using AutoMapper;
using CatalogoApi.Data;
using CatalogoApi.Dtos;
using CatalogoApi.Models;
using CatalogoApi.Pagination;
using CatalogoApi.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CatalogoApi.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[Controller]")]
    [ApiVersion("1.0")]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _productService;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductController> _logger;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ProductController(ProductService productService, IMapper mapper, ILogger<ProductController> logger, RoleManager<IdentityRole> roleManager)
        {
            _productService = productService;
            _mapper = mapper;
            _logger = logger;
            _roleManager = roleManager;
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
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateAsync([FromBody] ProductRequestDto productRequestDto)
        {
            Product product = await _productService.CreateAsync(_mapper.Map<Product>(productRequestDto));
            ProductResponseDto productResponseDto = _mapper.Map<ProductResponseDto>(product);

            return Created($"https://localhost:7123/api/v1/product/{product.ProductId}", productResponseDto);
            // return CreatedAtAction(nameof(GetByIdAsync), new { id = productResponseDto.ProductId });
        }

        [HttpGet("{id:int:min(1)}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductResponseDto>> GetByIdAsync(int id)
        {
            Product product = await _productService.FindByIdAsync(id);
            return Ok(_mapper.Map<ProductResponseDto>(product));
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ProductResponseDto>> UpdateAsync(int id, [FromBody] ProductRequestDto productRequestDto)
        {
            Product product = await _productService.UpdateAsync(id, _mapper.Map<Product>(productRequestDto));
            return Ok(_mapper.Map<ProductResponseDto>(product));
        }


        [HttpPatch("{id:int}/partialUpdate")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            await _productService.DeleteAsync(id);
            return NoContent();
        }

    }
}

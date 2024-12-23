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
        public ActionResult<PageListResponseDto<ProductResponseDto>> GetAll([FromQuery] int page)
        {
            if (page < 0) page = 0;
            IEnumerable<ProductResponseDto> products = _mapper.Map<IEnumerable<ProductResponseDto>>(_productService.FindAll(page));
            return Ok(PageList<ProductResponseDto>.ToPagedList(products, page).ToPageListResponse());
        }

        [HttpGet("price")]
        public ActionResult<PageListResponseDto<ProductResponseDto>> GetAllByPrice([FromQuery] int page, 
            [FromQuery] double min, [FromQuery] double max)
        {
            if (page < 0) page = 0;
            if (min < 0) min = 0;
            if (max < 0) max = 0;
            if (min > max) min = max;


            IEnumerable<ProductResponseDto> products = _mapper.Map<IEnumerable<ProductResponseDto>>(_productService.FindAllByPrice(page, min, max));
            return Ok(PageList<ProductResponseDto>.ToPagedList(products, page).ToPageListResponse());
        }

        [HttpPost]
        public IActionResult Create([FromBody] ProductRequestDto productRequestDto)
        {
            Product product = _productService.Create(_mapper.Map<Product>(productRequestDto));
            ProductResponseDto productResponseDto = _mapper.Map<ProductResponseDto>(product);

            return CreatedAtAction(nameof(GetById), new { id = productResponseDto.ProductId }, productResponseDto);
        }

        [HttpGet("{id:int:min(1)}")]
        public ActionResult<ProductResponseDto> GetById(int id)
        {
            Product product = _productService.FindById(id);
            return Ok(_mapper.Map<ProductResponseDto>(product));
        }

        [HttpPut("{id:int}")]
        public ActionResult<ProductResponseDto> Update(int id, [FromBody] ProductRequestDto productRequestDto)
        {
            Product product = _productService.Update(id, _mapper.Map<Product>(productRequestDto));
            return Ok(_mapper.Map<ProductResponseDto>(product));
        }


        [HttpPatch("{id:int}/partialUpdate")]
        public IActionResult PartialUpdate(int id, [FromBody] JsonPatchDocument<ProductUpdateDto> productUpdateDto)
        {
            Product product = _productService.PartialUpdate(id, productUpdateDto);

            if (!TryValidateModel(product))
            {
                return BadRequest(ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            _productService.Delete(id);
            return NoContent();
        }

    }
}

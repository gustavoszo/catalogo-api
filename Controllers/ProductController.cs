using AutoMapper;
using CatalogoApi.Data;
using CatalogoApi.Dtos;
using CatalogoApi.Models;
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


        public ProductController(ProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ProductResponseDto>> GetAll([FromQuery] int page)
        {
            if (page < 0) page = 0;
            IEnumerable<ProductResponseDto> products = _mapper.Map<IEnumerable<ProductResponseDto>>(_productService.FindAll(page));
            return Ok(products);
        }

        [HttpPost]
        public IActionResult Create([FromBody] ProductRequestDto productRequestDto)
        {
            Product product = _productService.Create(_mapper.Map<Product>(productRequestDto));
            ProductResponseDto productResponseDto = _mapper.Map<ProductResponseDto>(product);

            return CreatedAtAction(nameof(GetById), new { id = productResponseDto.ProductId }, product);
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

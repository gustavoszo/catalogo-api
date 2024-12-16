using CatalogoApi.Data;
using CatalogoApi.Models;
using CatalogoApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CatalogoApi.Controllers
{
    [ApiController]
    [Route("api/v1/[Controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Product>> GetAll([FromQuery] int page)
        {
            if (page < 0) page = 0;
            IEnumerable<Product> categories = _productService.FindAll(page);
            return Ok(categories);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Product product)
        {
            product = _productService.Create(product);

            return CreatedAtAction(nameof(GetById), new { id = product.ProductId }, product);
        }

        [HttpGet("{id:int:min(1)}")]
        public IActionResult GetById(int id)
        {
            Product product = _productService.FindById(id);
            return Ok(product);
        }

        [HttpPut("{id:int}")]
        public IActionResult Update(int id, Product product)
        {
            product = _productService.Update(id, product);
            return Ok(product);
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            _productService.Delete(id);
            return NoContent();
        }

    }
}

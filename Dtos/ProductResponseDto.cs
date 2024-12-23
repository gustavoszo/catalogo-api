using CatalogoApi.Models;
using CatalogoApi.Validations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CatalogoApi.Dtos
{
    public class ProductResponseDto
    {
        public int ProductId { get; set; }

        public string Name { get; set; }

        public string? Description { get; set; }

        public double? Price { get; set; }

        public string ImageUrl { get; set; }

        // public CategoryResponseDto Category { get; set; }

        public int CategoryId { get; set; }
    }
}

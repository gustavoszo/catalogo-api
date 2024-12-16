using CatalogoApi.Validations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CatalogoApi.Dtos
{
    public class ProductRequestDto
    {

        [Required]
        [StringLength(150)]
        [MaiuscFirstLetter]
        public string Name { get; set; }

        [StringLength(255)]
        public string? Description { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        public int QuantityAvaiable { get; set; }

        [StringLength(255)]
        public string? ImageUrl { get; set; }

        public DateTime DateRegister { get; set; } = DateTime.UtcNow;

        [Required]
        public int CategoryId { get; set; }

    }
}

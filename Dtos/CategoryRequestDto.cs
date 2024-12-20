using System.ComponentModel.DataAnnotations;

namespace CatalogoApi.Dtos
{
    public class CategoryRequestDto
    {

        [Required]
        [StringLength(150)]
        public string Name { get; set; }

        [StringLength(255)]
        public string? ImageUrl { get; set; }

    }
}

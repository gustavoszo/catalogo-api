using System.ComponentModel.DataAnnotations;

namespace CatalogoApi.Dtos
{
    public class CategoryResponseDto
    {

        public int CategoryId { get; set; }

        public string Name { get; set; }

        public string ImageUrl { get; set; }

    }
}

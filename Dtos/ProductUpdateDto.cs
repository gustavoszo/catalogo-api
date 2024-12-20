using CatalogoApi.Exceptions;
using System.ComponentModel.DataAnnotations;

namespace CatalogoApi.Dtos
{
    public class ProductUpdateDto
    {
        [Required]
        [Range(1, 999, ErrorMessage = "QuantityAvaiable deve estar entre 1 e 999")]
        public int QuantityAvaiable { get; set; }
    }
}

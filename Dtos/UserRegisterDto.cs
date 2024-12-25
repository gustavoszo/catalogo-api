using System.ComponentModel.DataAnnotations;

namespace CatalogoApi.Dtos
{
    public class UserRegisterDto
    {
        [Required(ErrorMessage = "Informe o 'Username'")]
        public string Username { get; set; }

        [EmailAddress(ErrorMessage = "Endereço de e-mail inválido")]
        [Required(ErrorMessage = "Informe o 'Email'")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Informe a 'Password'")]       
        public string Password { get; set; }
    }
}

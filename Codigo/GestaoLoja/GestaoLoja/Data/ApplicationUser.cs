using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace GestaoLoja.Data
{
    
    public class ApplicationUser : IdentityUser
    {
        public string? Nome { get; set; }
        public string? Apelido { get; set; }
        public long? NIF { get; set; }
        public string? Rua { get; set; }
        public string? Localidade1 { get; set; }
        public string? Localidade2 { get; set; }
        public string? Pais { get; set; }
        public byte[]? Fotografia { get; set; }
        [Required(ErrorMessage = "O estado é obrigatório.")]
        public string? Estado { get; set; } // Pendente ou Ativo

        public List<int> produtosFavoritos { get; set; }
    }

}

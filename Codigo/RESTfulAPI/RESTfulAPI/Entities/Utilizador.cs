using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RESTfulAPI.Entities;

public class Utilizador
{
    public string? Nome { get; set; }
    public string? Apelido { get; set; }
    public string? Email { get; set; }
    public string Password { get; set; }
    public string? ConfirmPassword { get; set; }
    public long? NIF { get; set; }
    public string? Rua { get; set; }
    public string? Localidade1 { get; set; }
    public string? Localidade2 { get; set; }
    public string? Pais { get; set; }

    [NotMapped]
    public IFormFile? ImageFile { get; set; }
    public byte[]? Fotografia { get; set; }
    public string? UrlImagem { get; set; }
    
}

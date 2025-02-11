using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RCLAPI.DTO;

public class Utilizador
{
    public string? Nome { get; set; }
    public string? Apelido { get; set; }

    [Required(ErrorMessage = "O email � obrigat�rio")]
    [EmailAddress(ErrorMessage = "Endere�o de Email Inv�lido")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "A indica��o da Password � obrigat�ria!")]
    public string Password { get; set; }

    //[Required(ErrorMessage = "A confirma��o da Password � obrigat�ria!")]
    //[Compare("Password", ErrorMessage = "A Password e a Confirma��o da Password n�o coincidem")]
    //public string ConfirmPassword { get; set; }


    [ValidarNIF(ErrorMessage = "NIF inv�lido!")]
    public long? NIF { get; set; }
    public string? Rua { get; set; }
    public string? Localidade1 { get; set; }
    public string? Localidade2 { get; set; }
    public string? Pais { get; set; }

    [NotMapped]
    public IFormFile? ImageFile { get; set; }
    public byte[]? Fotografia { get; set; }
    public string? UrlImagem { get; set; }

    // Valida��o customizada para o NIF
    public class ValidarNIF: ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            // Inserir o c�digo que est� no site das Finan�as
            if (value is long nif)
            {
                return nif > 100;
            }
            return false;
        }
    }
}


//// Valida��o customizada para o NIF
//public class ValidarNIF : ValidationAttribute
//{
//    public override bool IsValid(object value)
//    {
//        if (value is not long nif) return false;

//        string nifStr = nif.ToString();

//        // Verifica se o NIF tem exatamente 9 d�gitos
//        if (nifStr.Length != 9) return false;

//        // Verifica o primeiro d�gito
//        int primeiroDigito = int.Parse(nifStr[0].ToString());
//        if (!(primeiroDigito == 1 || primeiroDigito == 2 || primeiroDigito == 3 ||
//              primeiroDigito == 5 || primeiroDigito == 6 || primeiroDigito == 7 ||
//              primeiroDigito == 8 || primeiroDigito == 9))
//        {
//            return false;
//        }

//        // Calcula o d�gito de controlo
//        int soma = 0;
//        for (int i = 0; i < 8; i++)
//        {
//            soma += (nifStr[i] - '0') * (9 - i);
//        }

//        int resto = soma % 11;
//        int digitoControlo = (resto == 0 || resto == 1) ? 0 : 11 - resto;

//        // Verifica se o �ltimo d�gito coincide
//        return digitoControlo == (nifStr[8] - '0');
//    }
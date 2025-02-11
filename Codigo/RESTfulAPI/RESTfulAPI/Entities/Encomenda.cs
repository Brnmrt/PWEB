
using RESTfulAPI.Data;
using RESTfulAPI.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RESTfulAPI.Entities;

public class Encomenda
{
    public int Id { get; set; }

    
    public string UserId { get; set; }

    [JsonIgnore]
    public ApplicationUser? User { get; set; }

    
    public int ProdutoId { get; set; }
    [JsonIgnore]
    public Produto? Produto { get; set; }


    public decimal Preco { get; set; }

    public int Quantidade { get; set; }
    public bool Enviada { get; set; }

    public string Estado { get; set; } // Pendente, Aceite ou Rejeitada
    public DateTime DataCriacao { get; set; }
    public DateTime? DataFinalizacao { get; set; }
    public bool EmStock { get; set; }
    public bool Paga { get; set; }



}

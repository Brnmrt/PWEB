using GestaoLoja.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace GestaoLoja.Entities
{
    public class Encomenda
    {
        public int Id { get; set; }

        [JsonIgnore]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        [JsonIgnore]
        public int ProdutoId { get; set; }
        public Produto Produto { get; set; }

        public decimal Preco { get; set; }

        public int Quantidade { get; set; }
        public bool Enviada { get; set; }

        public string Estado { get; set; } // Pendente, Aceite ou Rejeitada
        public DateTime DataCriacao { get; set; }
        public DateTime? DataFinalizacao { get; set; }


        public bool EmStock { get; set; }
        public bool Paga { get; set; }

    }
}

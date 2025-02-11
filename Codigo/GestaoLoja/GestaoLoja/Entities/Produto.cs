﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace GestaoLoja.Entities
{
    public class Produto
    {
        public int Id { get; set; }

        [StringLength(100)]
        [Required]
        public string? Nome { get; set; }

        [StringLength(200)]
        [Required]
        public string? Detalhe { get; set; }

        [StringLength(200)]
        public string? UrlImagem { get; set; }

        public byte[] Imagem { get; set; }
        [Required]
        [Column(TypeName = "decimal(10, 2)")]

        public decimal Preco { get; set; }

        public bool Promocao { get; set; }
        public bool MaisVendido { get; set; }
        public int EmStock { get; set; }
        public bool Disponivel { get; set; }
        public string? Origem { get; set; }
        public int CategoriaId { get; set; } // O utilizador precisa de saber o id da categoria (a app deve poder filtrar por categoria)
        public Categoria categoria { get; set; }

        public int? ClassificacaoId { get; set; }

        public Classificacao classificacao { get; set; }

        public int? MarcaId { get; set; }   
        
        public Marca marca { get; set; }


        [NotMapped] // Não é mapeado para a base de dados
        public IFormFile? ImageFile { get; set; }


    }
}

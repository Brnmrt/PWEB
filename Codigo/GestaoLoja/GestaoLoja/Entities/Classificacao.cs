﻿using System.ComponentModel.DataAnnotations.Schema;

namespace GestaoLoja.Entities
{
    public class Classificacao
    {
        public int Id { get; set; }
        public string? Nome { get; set; }
        public int? Ordem { get; set; } // Ordem em que estão ordenados no frontend
        public string? UrlImagem { get; set; }

        public byte[]? Imagem { get; set; }

        [NotMapped]
        public IFormFile? ImageFile { get; set; }
    }
}

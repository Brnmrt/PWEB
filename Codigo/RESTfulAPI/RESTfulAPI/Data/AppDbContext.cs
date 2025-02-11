using RESTfulAPI.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RESTfulAPI.Data;
using System.Collections.Generic;

namespace RESTfulAPI.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {

        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Classificacao> Classificacoes { get; set; }
        public DbSet<Marca> Marcas { get; set; }
        public DbSet<Encomenda> Encomendas { get; set; }
    }
}



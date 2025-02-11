using Microsoft.EntityFrameworkCore;
using RESTfulAPI.Data;
using RESTfulAPI.Entities;
using RESTfulAPI.Repositories.IRepositories;

namespace RESTfulAPI.Repositories.Services
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ProdutoRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Produto>> ObterProdutosPorCategoriaAsync(int categoriaId)
        {
            return await _dbContext.Produtos
                .Where(p => p.CategoriaId == categoriaId)
                .Where(x => x.Imagem.Length > 0)
                .Include("marca")
                .Include("classificacao")
                .Include("categoria")
                .OrderBy(o => o.Nome)
                .ToListAsync();
        }

        public async Task<IEnumerable<Produto>> ObterProdutosPorMarcaAsync(int marcaId)
        {
            return await _dbContext.Produtos
                .Where(p => p.MarcaId == marcaId)
                .Where(x => x.Imagem.Length > 0)
                .Include("marca")
                .Include("classificacao")
                .Include("categoria")
                .OrderBy(o => o.Nome)
                .ToListAsync();
        }

        public async Task<IEnumerable<Produto>> ObterProdutosPorClassificacaoAsync(int classificacaoId)
        {
            return await _dbContext.Produtos
                .Where(p => p.ClassificacaoId == classificacaoId)
                .Where(x => x.Imagem.Length > 0)
                .Include("marca")
                .Include("classificacao")
                .Include("categoria")
                .OrderBy(o => o.Nome)
                .ToListAsync();
        }

        public async Task<IEnumerable<Produto>> ObterProdutosPromocaoAsync()
        {
            return await _dbContext.Produtos
                .Where(p => p.Promocao == true)
                .Where(x => x.Imagem.Length > 0)
                .Include("marca")
                .Include("classificacao")
                .Include("categoria")
                .OrderBy(p => p.categoria.Ordem)
                .ThenBy(p => p.Nome)
                .ToListAsync();
        }

        public async Task<IEnumerable<Produto>> ObterProdutosMaisVendidosAsync()
        {
            return await _dbContext.Produtos
                .Where(p => p.MaisVendido)
                .Where(x => x.Imagem.Length > 0)
                .Include("marca")
                .Include("classificacao")
                .Include("categoria")
                .OrderBy(p => p.categoria.Ordem)
                .ThenBy(p => p.Nome)
                .ToListAsync();
        }



        public async Task<IEnumerable<Produto>> ObterTodosProdutosAsync()
        {
            var produtos = await _dbContext.Produtos
                .Where(x => x.Imagem.Length > 0)
                .Include("marca")
                .Include("classificacao")
                .Include("categoria")
                .OrderBy(p => p.categoria.Ordem)
                .ThenBy(p => p.Nome)
                .ToListAsync();
            return produtos;
        }

        public async Task<Produto> ObterDetalheProdutoAsync(int id)
        {
            var detalheProduto = await _dbContext.Produtos
            .Where(p => p.Id == id)
            .Where(x => x.Imagem.Length > 0)
            .Include("marca")
            .Include("classificacao")
            .Include("categoria")
            .FirstOrDefaultAsync(p => p.Id == id);

            if (detalheProduto is null)
            {
                throw new InvalidOperationException();
            }

            return detalheProduto;
        }

        public async Task<IEnumerable<Produto>> ObterProdutosFavoritosUserAsync(string userid)
        {
           
            ApplicationUser? user = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Id == userid);

            if(user is null)
            {
                throw new InvalidOperationException();
            }

            var produtos = await _dbContext.Produtos
                .Where(p => user.produtosFavoritos.Contains(p.Id))
                .Where(x => x.Imagem.Length > 0)
                .Include("marca")
                .Include("classificacao")
                .Include("categoria")
                .OrderBy(p => p.categoria.Ordem)
                .ThenBy(p => p.Nome)
                .ToListAsync();

            return produtos;
        }

        public async Task<bool> AdicionaFavorito(string userId, int produtoId)
        {
            try
            {
                ApplicationUser? user = await _dbContext.Users
                    .FirstOrDefaultAsync(u => u.Id == userId);

                if (user is null)
                {
                    throw new InvalidOperationException();
                }

                if (user.produtosFavoritos.Contains(produtoId)) // Se ja tem o produto como favorito, fallback
                {
                    return false;
                }

                Produto? produto = await _dbContext.Produtos
                    .FirstOrDefaultAsync(p => p.Id == produtoId);

                if (produto is null)
                {
                    return false;
                }

                user.produtosFavoritos.Add(produtoId);

                await _dbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao adicionar favorito: {ex.Message}");
                return false; 
            }

        }

        public async Task<bool> RemoveFavorito(string userId, int produtoId)
        {
            ApplicationUser? user = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user is null)
            {
                throw new InvalidOperationException();
            }

            if (user.produtosFavoritos.Contains(produtoId)) // Se ja tem o produto como favorito, fallback
            {
                user.produtosFavoritos.Remove(produtoId);
                await _dbContext.SaveChangesAsync();
            }
            return true;
        }
    }
}

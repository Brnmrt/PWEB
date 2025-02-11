using RESTfulAPI.Entities;

namespace RESTfulAPI.Repositories.IRepositories
{
    public interface IProdutoRepository
    {
        Task<IEnumerable<Produto>> ObterProdutosPorCategoriaAsync(int categoriaId);
        Task<IEnumerable<Produto>> ObterProdutosPorMarcaAsync(int marcaId);
        Task<IEnumerable<Produto>> ObterProdutosPorClassificacaoAsync(int classificacaoId);

        Task<IEnumerable<Produto>> ObterProdutosPromocaoAsync();
        Task<IEnumerable<Produto>> ObterProdutosMaisVendidosAsync();
        Task<Produto> ObterDetalheProdutoAsync(int id);
        Task<IEnumerable<Produto>> ObterTodosProdutosAsync();
        Task<IEnumerable<Produto>> ObterProdutosFavoritosUserAsync(string userid);
        Task<bool> AdicionaFavorito(string userId, int produtoId);
        Task<bool> RemoveFavorito(string userId, int produtoId);
    }
}

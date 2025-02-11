using RCLAPI.DTO;

namespace RCLAPI.Services;

public interface IApiServices
{
    public Task<List<ProdutoDTO>> GetProdutosEspecificos(string produtoTipo, int? IdCategoria);
    public Task<(T? Data, string? ErrorMessage)> GetAsync<T>(string endpoint);
    public Task<List<Categoria>> GetCategorias();
    public Task<(bool Data, string? ErrorMessage)> ActualizaFavorito(string acao,int produtoId);
    public Task<List<ProdutoDTO>> GetFavoritos();
    public Task<ApiResponse<bool>> RegistarUtilizador(Utilizador novoUtilizador);
    public Task<ApiResponse<bool>> Login(LoginModel login);
    public Task<bool> IsUserLoggedIn();
    public Task<ApiResponse<bool>> CriaEncomenda(ItemCarrinhoCompra itemCarrinhoCompra);
    public Task<ApiResponse<bool>> PagaEncomenda(int encomendaId);
    public Task<List<EncomendaDTO>> GetEncomendasByUser();
    public Task<string> GetUserId();
    public Task<string> GetUserEstado();
    public Task Logout();
}

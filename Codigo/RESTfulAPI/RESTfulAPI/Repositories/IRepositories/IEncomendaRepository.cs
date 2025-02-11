using RESTfulAPI.Entities;

namespace RESTfulAPI.Repositories.IRepositories;

public interface IEncomendaRepository
{
    public Task<IEnumerable<Encomenda>> GetEncomendas();
    public Task AddEncomenda(Encomenda encomenda);
    public Task<IEnumerable<Encomenda>> GetEncomendasByUserId(string userId);
    public Task PagaEncomenda(int encomendaId);
}

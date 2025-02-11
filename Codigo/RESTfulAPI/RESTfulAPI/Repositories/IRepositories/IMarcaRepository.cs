using RESTfulAPI.Entities;

namespace RESTfulAPI.Repositories.IRepositories
{
    public interface IMarcaRepository
    {
        Task<IEnumerable<Marca>> GetMarcas();
    }
}

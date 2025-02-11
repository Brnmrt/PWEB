using RESTfulAPI.Entities;

namespace RESTfulAPI.Repositories.IRepositories
{
    public interface ICategoriaRepository
    {
        Task<IEnumerable<Categoria>> GetCategorias();   
    }
}

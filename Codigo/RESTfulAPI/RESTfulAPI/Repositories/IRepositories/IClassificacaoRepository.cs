using RESTfulAPI.Entities;

namespace RESTfulAPI.Repositories.IRepositories
{
    public interface IClassificacaoRepository
    {
        Task<IEnumerable<Classificacao>> GetClassificacao();

    }
}

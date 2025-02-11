using Microsoft.EntityFrameworkCore;
using RESTfulAPI.Data;
using RESTfulAPI.Entities;
using RESTfulAPI.Repositories.IRepositories;


namespace RESTfulAPI.Repositories.Services;

public class ClassificacaoRepository : IClassificacaoRepository
{


    private readonly ApplicationDbContext dbContext;

    public ClassificacaoRepository(ApplicationDbContext context)
    {
        this.dbContext = context;
    }
    public async Task<IEnumerable<Classificacao>> GetClassificacao()
    {
        return await dbContext.Classificacoes
            .Where(x => x.Imagem.Length > 0)
            .OrderBy(O => O.Ordem)
            .ThenBy(p => p.Nome)
            .ToListAsync();
    }
}



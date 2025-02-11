using Microsoft.EntityFrameworkCore;
using RESTfulAPI.Data;
using RESTfulAPI.Entities;
using RESTfulAPI.Repositories.IRepositories;

namespace RESTfulAPI.Repositories.Services
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly ApplicationDbContext dbContext;

        public CategoriaRepository(ApplicationDbContext context)
        {
            this.dbContext = context;
        }
        public async Task<IEnumerable<Categoria>> GetCategorias()
        {
            return await dbContext.Categorias
                .Where(x=> x.Imagem.Length > 0)
                .OrderBy(O => O.Ordem)
                .ThenBy(p => p.Nome)
                .ToListAsync();
        }
    }
}

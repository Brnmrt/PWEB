using Microsoft.AspNetCore.Mvc;
using RESTfulAPI.Entities;
using RESTfulAPI.Repositories.IRepositories;
using RESTfulAPI.Repositories.Services;
using Microsoft.AspNetCore.Authorization;


namespace RESTfulAPI.Controllers;

[Route("api/[controller]")]
[ApiController]


//[Authorize]
public class CategoriasController : ControllerBase
{
    private readonly ICategoriaRepository categoriaRepository;

    public CategoriasController(ICategoriaRepository categoriaRepository)
    {
        this.categoriaRepository = categoriaRepository;
    }


    [HttpGet]
    public async Task<IActionResult> Get()
    {
        IEnumerable<Categoria> categorias = await categoriaRepository.GetCategorias();
        return Ok(categorias);
    }
   
}

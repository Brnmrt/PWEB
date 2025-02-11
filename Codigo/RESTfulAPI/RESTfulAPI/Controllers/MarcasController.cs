using Microsoft.AspNetCore.Mvc;
using RESTfulAPI.Entities;
using RESTfulAPI.Repositories.IRepositories;
using RESTfulAPI.Repositories.Services;
using Microsoft.AspNetCore.Authorization;


namespace RESTfulAPI.Controllers;

[Route("api/[controller]")]
[ApiController]


//[Authorize]
public class MarcasController : ControllerBase
{
    private readonly IMarcaRepository marcaRepository;

    public MarcasController(IMarcaRepository marcaRepository)
    {
        this.marcaRepository = marcaRepository;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        IEnumerable<Marca> marcas = await marcaRepository.GetMarcas();
        return Ok(marcas);
    }
}


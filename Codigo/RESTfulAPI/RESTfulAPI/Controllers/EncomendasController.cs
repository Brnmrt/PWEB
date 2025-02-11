
using Microsoft.AspNetCore.Mvc;
using RESTfulAPI.Entities;
using RESTfulAPI.Repositories.IRepositories;
using RESTfulAPI.Repositories.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace RESTfulAPI.Controllers;

[Route("api/[controller]")]
[ApiController]


public class EncomendasController : ControllerBase
{
    private readonly IEncomendaRepository encomendaRepository;


    public EncomendasController(IEncomendaRepository rep)
    {
        encomendaRepository = rep;
    }


    [HttpGet]
    public async Task<IActionResult> Get()
    {
        IEnumerable<Encomenda> encomendas = await encomendaRepository.GetEncomendas();
        return Ok(encomendas);
    }

    [HttpGet("user")]
    [Authorize(Roles =("Administrador,Funcionario,Cliente"), AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetByUserId()
    {
        
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("Email not found in token.");
        }

        
        var encomendas = await encomendaRepository.GetEncomendasByUserId(userId);

        if (!encomendas.Any())
        {
            return NotFound($"No encomendas found for user with ID {userId}");
        }

        return Ok(encomendas);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Encomenda encomenda)
    {
        if (encomenda == null)
        {
            return BadRequest("Encomenda is null.");
        }

        try
        {
            // Remover o campo User para evitar que o cliente envie dados incorretos
            encomenda.User = null;

            await encomendaRepository.AddEncomenda(encomenda);
            return StatusCode(StatusCodes.Status201Created);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPut("pagaencomenda")]
    public async Task<IActionResult> PagaEncomenda(int encomendaId)
    {
        try
        {
            await encomendaRepository.PagaEncomenda(encomendaId);
            return Ok("Encomenda paga com sucesso.");
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
 }

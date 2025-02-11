using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

using RCLAPI.DTO;
using System;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Data.Entity.Core.Objects;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Net.NetworkInformation;

using System.Security.Claims;

namespace RCLAPI.Services;
public class ApiService : IApiServices
{
    private static Token _token;
    private readonly ILogger<ApiService> _logger;
    private readonly HttpClient _httpClient = new();

    

    JsonSerializerOptions _serializerOptions;

    private List<ProdutoDTO> produtos;
    private ProdutoDTO produto;

    private List<Categoria> categorias;

    private ProdutoDTO _detalhesProduto;
        public ApiService(ILogger<ApiService> logger)
        {
        

        _logger = logger;
        _serializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        _detalhesProduto = new ProdutoDTO();
        categorias = new List<Categoria>();
    }

    public void SetToken(Token token)
    {
        _token = token;
    }



    private void AddAuthorizationHeader()
    {
        
        if (_token is not null && !string.IsNullOrEmpty(_token.accesstoken))
        {
            _httpClient.DefaultRequestHeaders.Authorization =
           new AuthenticationHeaderValue("Bearer", _token.accesstoken);
       }
    }

    


    // ********************* Categorias  **********
    public async Task<List<Categoria>> GetCategorias()
    {
        string endpoint = $"api/Categorias";

        try
        {
            HttpResponseMessage httpResponseMessage = 
                await _httpClient.GetAsync($"{AppConfig.BaseUrl}{endpoint}");

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                string content = "";

                content = await httpResponseMessage.Content.ReadAsStringAsync();
                categorias = JsonSerializer.Deserialize<List<Categoria>>(content, _serializerOptions)!;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Erro ao obter categorias");
            Console.WriteLine(ex.Message);
            return null;
        }

        return categorias;
    }


    // ********************* Marcas  **********

    // ********************* Classificacao  **********

    // ********************* Produtos  **********
    public async Task<List<ProdutoDTO>> GetProdutosEspecificos(string produtoTipo,int? IdCategoria)
    {
        string endpoint = "";

        if (produtoTipo == "categoria" && IdCategoria != null)
        {
            endpoint = $"api/Produtos?tipoProduto=categoria&searchId={IdCategoria}";
        }
        else if (produtoTipo == "todos")
        {
            endpoint = $"api/Produtos?tipoProduto=todos";
        }
        else
        {
            return null;
        }
        try
        {
            HttpResponseMessage httpResponseMessage = 
                await _httpClient.GetAsync($"{AppConfig.BaseUrl}{endpoint}");
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                string content = "";
                content = await httpResponseMessage.Content.ReadAsStringAsync();
                produtos = JsonSerializer.Deserialize<List<ProdutoDTO>>(content, _serializerOptions)!;           
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
        return produtos;
    }

    public async Task<ProdutoDTO> GetProdutoById(int produtoId)
    {
        try
        {
            // Construir o endpoint para buscar o produto pelo ID
            var endpoint = $"api/Produtos/{produtoId}";

            // Chamar o método GetAsync para buscar o produto
            HttpResponseMessage httpResponseMessage =
                await _httpClient.GetAsync($"{AppConfig.BaseUrl}{endpoint}");

            // Verificar se houve erro na requisição
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                string content = "";
                content = await httpResponseMessage.Content.ReadAsStringAsync();
                produto = JsonSerializer.Deserialize<ProdutoDTO>(content, _serializerOptions)!;  
                
            }

            
            
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
        return produto;
    }

    public async Task<(T?Data, string?ErrorMessage)>GetAsync<T>(string endpoint)
    {
        try
        {
            AddAuthorizationHeader();
            var response = await _httpClient.GetAsync(AppConfig.BaseUrl + endpoint);
            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<T>(responseString, _serializerOptions);
                return (data ?? Activator.CreateInstance<T>(), null);
            }
            else
            {
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    string errorMessage = "Unauthorized";
                    _logger.LogWarning(errorMessage);
                    return (default, errorMessage);
                }
                string generalErrorMessage = $"Erro na requisição: {response.ReasonPhrase}";
                _logger.LogError(generalErrorMessage);
                return (default, generalErrorMessage);
            }
        }
        catch (HttpRequestException ex)
        {
            string errrMessage = $"Erro de requisição HTTP: {ex.Message}";
            _logger.LogError(errrMessage);
            return (default, errrMessage);
        }
        catch (JsonException ex)
        {
            string errorMessage = $"Erro de desserialização JSON: {ex.Message}";
            _logger.LogError(ex.Message);
            return (default, errorMessage);
        }
        catch (Exception ex)
        {
            string errorMessage = $"Erro inesperado: {ex.Message}";
            _logger.LogError(ex.Message);
            return (default, errorMessage);
        }
    }

    // ***************** Compras ******************
    public async Task<ApiResponse<bool>> CriaEncomenda(ItemCarrinhoCompra itemCarrinhoCompra)
    {
        try
        {
            var userID = _token.utilizadorid;
            _logger.LogInformation($"Criar encomenda para o cliente {userID}");
            // Definir o UserId
            itemCarrinhoCompra.UserId = userID;

           

            // Criar a encomenda
            EncomendaDTO encomenda = new()
            {
                UserId = userID,
                ProdutoId = itemCarrinhoCompra.ProdutoId,
                Preco = itemCarrinhoCompra.PrecoUnitario,
                Quantidade = itemCarrinhoCompra.Quantidade,
                Enviada = false,
                Estado = "Pendente",
                DataCriacao = DateTime.UtcNow, // Usar UTC para consistência
                DataFinalizacao = null,
                
            };

            // Serializar a encomenda para JSON
            var json = JsonSerializer.Serialize(encomenda, _serializerOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            _logger.LogInformation($"Encomenda enviada {json}");

            // Enviar a requisição POST para a API
            var response = await PostRequest("api/Encomendas", content);

            // Verificar se a requisição foi bem-sucedida
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError($"Erro ao enviar requisição HTTP: {response.StatusCode}. Detalhes: {errorContent}");
                return new ApiResponse<bool>
                {
                    ErrorMessage = $"Erro ao enviar requisição HTTP: {response.StatusCode}. Detalhes: {errorContent}"
                };
            }

            // Retornar sucesso
            return new ApiResponse<bool> { Data = true };
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError($"Erro de requisição HTTP: {ex.Message}");
            return new ApiResponse<bool> { ErrorMessage = $"Erro de requisição HTTP: {ex.Message}" };
        }
        catch (JsonException ex)
        {
            _logger.LogError($"Erro de desserialização JSON: {ex.Message}");
            return new ApiResponse<bool> { ErrorMessage = $"Erro de desserialização JSON: {ex.Message}" };
        }
        catch (Exception ex)
        {
            _logger.LogError($"Erro inesperado: {ex.Message}");
            return new ApiResponse<bool> { ErrorMessage = $"Erro inesperado: {ex.Message}" };
        }
    }



    public async Task<List<EncomendaDTO>> GetEncomendasByUser()
    {
        string endpoint = $"api/Encomendas/user";

        try
        {
            AddAuthorizationHeader();
            var response = await _httpClient.GetAsync($"{AppConfig.BaseUrl}{endpoint}");

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var encomendas = JsonSerializer.Deserialize<List<EncomendaDTO>>(responseContent, _serializerOptions);
                return encomendas ?? new List<EncomendaDTO>();
            }
            else
            {
                _logger.LogError($"Erro ao obter encomendas do utilizador: {response.ReasonPhrase}");
                return new List<EncomendaDTO>();
            }
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError($"Erro de requisição HTTP ao obter encomendas do utilizador: {ex.Message}");
            return new List<EncomendaDTO>();
        }
        catch (JsonException ex)
        {
            _logger.LogError($"Erro de desserialização JSON ao obter encomendas do utilizador: {ex.Message}");
            return new List<EncomendaDTO>();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Erro inesperado ao obter encomendas do utilizador: {ex.Message}");
            return new List<EncomendaDTO>();
        }
    }

    public async Task<ApiResponse<bool>> PagaEncomenda(int encomendaId)
    {
        string endpoint = $"api/Encomendas/pagaencomenda?encomendaId={encomendaId}";

        try
        {
            AddAuthorizationHeader();
            var response = await _httpClient.PutAsync($"{AppConfig.BaseUrl}{endpoint}", null);

            if (response.IsSuccessStatusCode)
            {
                return new ApiResponse<bool> { Data = true };
            }
            else
            {

                return new ApiResponse<bool> { Data = false, ErrorMessage = $"Erro ao pagar encomenda: {response.ReasonPhrase}" };
            }
        }
        catch(Exception ex)
        {
            _logger.LogError($"Erro ao pagar a encomenda: {ex.Message}");
            return new ApiResponse<bool> { Data = false, ErrorMessage = $"Erro ao pagar a encomenda: {ex.Message}" };
        }
    }


    // ****************** Utilizadores ********************
    public async Task<ApiResponse<bool>> RegistarUtilizador(Utilizador novoUtilizador)
    {
        try
        {
            var json = JsonSerializer.Serialize(novoUtilizador, _serializerOptions);
            _logger.LogInformation($"JSON enviado: {json}");



            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await PostRequest("api/Utilizadores/RegistarUser", content);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"Erro ao enviar requisitos Http: {response.StatusCode}");
                return new ApiResponse<bool>
                {
                    ErrorMessage = $"Erro ao enviar requisição HTTP: {response.StatusCode}"
                };
            }

            return new ApiResponse<bool> { Data = true };
        }
        catch (Exception ex)
        {
            _logger.LogError($"Erro ao registar o utiizador: {ex.Message}");
            return new ApiResponse<bool> { ErrorMessage = ex.Message };
        }
    }
    public async Task<bool> IsUserLoggedIn()
    {

        return _token is not null;
    }



    public async Task<ApiResponse<bool>> Login(LoginModel login)
    {
        try
        {
            string endpoint = "api/Utilizadores/LoginUser";

            var json = JsonSerializer.Serialize(login, _serializerOptions);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await PostRequest($"{endpoint}", content);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"Erro ao enviar requisição Http: {response.StatusCode}");

            }
            var jsonResult = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<Token>(jsonResult, _serializerOptions);

            

            if (result != null)
            {
                SetToken(result); // Configurar o token no serviço
            }

           

            return new ApiResponse<bool> { Data = true };
        }
        catch (Exception ex)
        {
            string errorMessage = $"Erro inesperado: {ex.Message}";
            _logger.LogError(ex.Message);
            return (default);
        }

    }
    private async Task<HttpResponseMessage> PostRequest(string enderecoURL, HttpContent content)
    {
        try
        {
            AddAuthorizationHeader();
            var result = await _httpClient.PostAsync($"{AppConfig.BaseUrl}{enderecoURL}", content);
            return result;
        }
        catch (Exception ex)
        {
            // Log o erro ou trata conforme necessario
            _logger.LogError($"Erro ao enviar requisição POST para enderecoURL: {ex.Message}");
            return new HttpResponseMessage(HttpStatusCode.BadRequest);
        }
    }

    

    // *************** Gerir Favoritos ******************

    public async Task<List<ProdutoDTO>> GetFavoritos()
    {
        string endpoint = $"api/Produtos/GetFavoritosUser";
        try  
        {
            AddAuthorizationHeader();
            var response = await _httpClient.GetAsync($"{AppConfig.BaseUrl}{endpoint}");

            var responseString = await response.Content.ReadAsStringAsync();
            List<ProdutoDTO> data = JsonSerializer.Deserialize<List<ProdutoDTO>>(responseString, _serializerOptions);

            
            return data; 
        }
        catch (Exception ex)
        {
            
            Console.WriteLine($"Erro ao obter favoritos: {ex.Message}");
            return new List<ProdutoDTO>();
        }

    }
    public async Task<(bool Data, string? ErrorMessage)> ActualizaFavorito(string acao, int produtoId)
    {
        try
        {
            
            if (_token is null)
            {
                return (false, "Utilizador não autenticado");
            }
           
            var content = new StringContent(string.Empty, Encoding.UTF8, "application/json");
            string endpoint = "";
            if(acao == "adiciona")
            {
                endpoint = $"api/Produtos/AdicionaFavoritoUser?produtoId={produtoId}";
            }
            else if(acao == "remove")
            {
                endpoint = $"api/Produtos/RemoveFavoritoUser?produtoId={produtoId}";
            }

            AddAuthorizationHeader();
            var response = await _httpClient.PutAsync($"{AppConfig.BaseUrl}{endpoint}", content);
            
            
            if (response.IsSuccessStatusCode)
            {
                return (true, null);
            }
            else
            {
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    string errorMessage = "Unauthorized";
                    _logger.LogWarning(errorMessage);
                    return (false, errorMessage);
                }
                string generalErrorMessage = $"Erro na requisição: {response.ReasonPhrase}";
                _logger.LogError(generalErrorMessage);
                return (false, generalErrorMessage);
            }
        }
        catch (HttpRequestException ex)
        {
            string errorMessage = $"Erro de requisição HTTP: {ex.Message}";
            _logger.LogError(errorMessage);
            return (false, errorMessage);
        }
        catch (Exception ex)
        {
            string errorMessage = $"Erro inesperado: {ex.Message}";
            _logger.LogError(errorMessage);
            return (false, errorMessage);
        }
    }

    private async Task<HttpResponseMessage> FavoritosPutRequest(string uri, HttpContent content)
    {
        var enderecoUrl = AppConfig.BaseUrl + uri;
        try
        {
            AddAuthorizationHeader();
            var result = await _httpClient.PutAsync(enderecoUrl, content);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Erro ao enviar requisição PUT para {uri}: {ex.Message}");
            return new HttpResponseMessage(HttpStatusCode.BadRequest);
        }
    }
    public async Task<string> GetUserId()
    {
        if (_token is null)
            return null;
        return _token.utilizadorid;
    }
    
    public async Task<string> GetUserEstado()
    {
        if (_token is null)
            return null;
        return _token.utilizadorestado;
    }

    public async Task Logout()
    {
        _token = null; 
        _httpClient.DefaultRequestHeaders.Authorization = null;
        
    }
}

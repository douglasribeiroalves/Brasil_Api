using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using API.Model;
using System.Collections.Generic;
using Microsoft.AspNetCore.Cors;

namespace API.Controllers
{
    [ApiController]
    public class ClimaController : ControllerBase
    {
        private readonly IBrasilApiService _brasilApiService;
        private readonly ILogger<ClimaController> _logger;

        public ClimaController(IBrasilApiService brasilApiService, ILogger<ClimaController> logger)
        {
            _brasilApiService = brasilApiService;
            _logger = logger;
        }

        
        // GET CondicoesCidades
        /// <summary>
        /// Retorna as condições de uma Cidade pelo seu Id
        /// </summary>
        /// <param name="IdCidade">Insira aqui o código da Cidade</param>
        /// <returns>Retorna as condições de uma Cidade</returns>
        /// <response code="200">Retorna as condições de uma Cidade</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Falha</response>
        [HttpGet]
        [Route("api/CondicoesCidades/{IdCidade}")]
        public async Task<ActionResult<dynamic>> CondicoesCidades(int IdCidade)
        {
            Console.WriteLine("");
            _logger.LogInformation($"Inicio da rota 'CondicoesCidades'.");

            return await _brasilApiService.CondicoesCidades(IdCidade);
        }

        /// GET ListaCidades
        /// <summary>
        /// Retorna uma lista de cidades do Brasil. Pode-se utilizar parte do nome da cidade como filtro.
        /// </summary>
        /// <param name="Nome_Cidade">Parte do nome da cidade a ser buscada</param>
        /// <returns>Retorna uma lista de cidades do Brasil</returns>
        /// <response code="200">Retorna uma lista de cidades do Brasil</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Falha</response>
        [HttpGet]
        [Route("api/ListadeCidades")]
        public async Task<dynamic> ListarCidades([FromQuery] string Nome_Cidade)
        {
            Console.WriteLine("");
            _logger.LogInformation($"Inicio da rota 'ListadeCidades'.");

            return await _brasilApiService.ListarCidades(Nome_Cidade);
        }

        /// GET CondicoesAeroportos
        /// <summary>
        /// Retorna as condições de um Aeroporto pelo codigo Icao.
        /// </summary>
        /// <param name="Codigo_Icao"></param>
        /// <returns>Retorna as condições de um aeroporto</returns>
        /// <response code="200">Condições do Aeroporto</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Falha</response>
        [HttpGet]
        [Route("api/CondicoesAeroportos")]
        public async Task<dynamic> CondicoesAeroportos([FromQuery] string Codigo_Icao)
        {
            Console.WriteLine("");
            _logger.LogInformation($"Inicio da rota 'CondicoesAeroportos'.");

            return await _brasilApiService.Aeroportos(Codigo_Icao);
        }

        // GET ListadeAeroportos
        /// <summary>
        /// Retorna uma lista de Aeroportos do Brasil. Pode-se utilizar as siglas dos estados como filtro.
        /// </summary>
        /// <param name="Sigla_Estado"></param>
        /// <returns>Retorna uma lista de Aeroportos do Brasil</returns>
        /// <response code="200">Retorna uma lista de Aeroportos do Brasil</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Falha</response>
        [HttpGet]
        [Route("api/ListadeAeroportos")]
        public List<ListaAeroportosResponse> ListaAeroportos([FromQuery] string Sigla_Estado)
        {
            Console.WriteLine("");
            _logger.LogInformation($"Inicio da rota 'ListadeAeroportos'.");

            return _brasilApiService.ListarAeroportos(Sigla_Estado);
        }
    }
}

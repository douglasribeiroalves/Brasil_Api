using API.Infrastructure;
using API.Interfaces;
using API.Uteis;
using Azure;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RestSharp;
using System;
using System.Threading.Tasks;

namespace API.Services.Apis
{
    public class ApiBrasilApiService
    {
        private readonly IGravaLogService _gravaLog;
        private readonly ILogger<BrasilApiService> _logger;
        private readonly DadosBrasilApi _dadosBrasilApi;

        public ApiBrasilApiService(IGravaLogService gravaLog, ILogger<BrasilApiService> apiLogger,
            IOptions<DadosBrasilApi> optionsBrasilApi)
        {
            _gravaLog = gravaLog;
            _logger = apiLogger;
            _dadosBrasilApi = optionsBrasilApi.Value;
        }

        public async Task<RestResponse> ClimaCidades(int idCidades)
        {
            _logger.LogInformation("Iniciando integrações no Endpoint 'ClimaCidades'.");

            RestResponse response = null;

            try
            {
                var options = new RestClientOptions(_dadosBrasilApi.BaseUrl)
                {
                    MaxTimeout = -1,
                };
                var client = new RestClient(options);

                var request = new RestRequest($"/api/cptec/v1/clima/previsao/{idCidades}")
                    .AddHeader("Accept", "application/json; charset=utf-8");

                response = await client.ExecuteGetAsync(request);

                _logger.LogInformation("Status Code: " + (int)response.StatusCode + " " + response.StatusCode.ToString());

                GravaRequisicoes("ClimaCidades", response, "");

            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro no endpoit 'ClimaCidades': {ex.Message}");
                GravaRequisicoes("ClimaCidades", response, "falha");
            }

            return response;
        }

        public async Task<RestResponse> ListarCidades(string nomeCidade)
        {
            _logger.LogInformation("Iniciando integrações no Endpoint 'ListarCidades'.");

            RestResponse response = null;
            try
            {
                var options = new RestClientOptions(_dadosBrasilApi.BaseUrl)
                {
                    MaxTimeout = -1,
                };
                var client = new RestClient(options);

                var request = new RestRequest($"/api/cptec/v1/cidade/{nomeCidade}")
                    .AddHeader("Accept", "application/json; charset=utf-8");

                response = await client.ExecuteGetAsync(request);

                _logger.LogInformation("Status Code: " + (int)response.StatusCode + " " + response.StatusCode.ToString());

                GravaRequisicoes("ListarCidades", response, "");

            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro no endpoit 'ListarCidades': {ex.Message}");
                GravaRequisicoes("ListarCidades", response, "falha");
            }

            return response;
        }

        public async Task<RestResponse> Aeroportos(string codigoIcao)
        {
            _logger.LogInformation("Iniciando integrações no Endpoint 'ListarAeroportos'.");

            RestResponse response = null;

            try
            {
                var options = new RestClientOptions(_dadosBrasilApi.BaseUrl)
                {
                    MaxTimeout = -1,
                };
                var client = new RestClient(options);

                var request = new RestRequest($"/api/cptec/v1/clima/aeroporto/{codigoIcao}")
                    .AddHeader("Accept", "application/json; charset=utf-8");

                response = await client.ExecuteGetAsync(request);

                _logger.LogInformation("Status Code: " + (int)response.StatusCode + " " + response.StatusCode.ToString());

                GravaRequisicoes("Aeroportos", response, "");

            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro no endpoit 'ListarAeroportos': {ex.Message}");
                GravaRequisicoes("Aeroportos", response, "falha");
            }

            return response;
        }

        private void GravaRequisicoes(string metodo, RestResponse response, string tipo)
        {
            if (tipo == "falha")
                _gravaLog.GravarLogRequisicao(metodo, response, NivelLog.FALHA);
            else
                _gravaLog.GravarLogRequisicao(metodo, response, NivelLog.INFORMACAO);
        }
    }
}

using API.Infrastructure;
using API.Interfaces;
using API.Model;
using API.Services.Apis;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace API.Services
{
    public class BrasilApiService : IBrasilApiService
    {
        private readonly ILogger<BrasilApiService> _logger;
        private readonly IGravaLogService _gravaLog;
        private readonly ApiBrasilApiService _apiService;
        protected readonly string _json;
        protected readonly List<ListaAeroportosResponse> _listaAeroportos;

        public BrasilApiService(IGravaLogService gravaLog, ILogger<BrasilApiService> apiLogger, 
            IOptions<DadosBrasilApi> optionsBrasilApi)
        {
            _logger = apiLogger;
            _gravaLog = gravaLog;
            _apiService = new ApiBrasilApiService(gravaLog, apiLogger, optionsBrasilApi);
            _json = File.ReadAllText("Aeroportos.json");
            _listaAeroportos = JsonConvert.DeserializeObject<List<ListaAeroportosResponse>>(_json);
        }

        /// <summary>
        /// Este método retorna as condições de uma cidade. Deve-se utilizar o seu código para o filtro e este é obrigatório.
        /// </summary>
        /// <param name="idCidades"></param>
        /// <returns></returns>
        public async Task<dynamic> CondicoesCidades(int idCidades)
        {
            var result = await _apiService.ClimaCidades(idCidades);
            
            try
            {
                if (result.StatusCode == HttpStatusCode.OK)
                {
                    CidadeResponse retorno = JsonConvert.DeserializeObject<CidadeResponse>(result.Content.ToString());

                    IDictionary<string, string> map = new Dictionary<string, string>
                    {
                        { "Condição: ", retorno.Clima[0].Condicao == "pn" ? "Predominantemente Normal" : retorno.Clima[0].Condicao },
                        { "Temperatura Minima: ", retorno.Clima[0].Min.ToString() + "ºC" },
                        { "Temperatura Maxima: ", retorno.Clima[0].Max + "ºC" },
                        { "Indice UV: ", retorno.Clima[0].IndiceUv.ToString() },
                        { "Condição Desc: ", retorno.Clima[0].CondicaoDesc.ToString() }
                    };

                    _logger.LogInformation($"As condições na Cidade {retorno.Cidade} em {retorno.Clima[0].Data} são:");

                    _gravaLog.GravarLogCidades(retorno);

                    PrintLog(map);

                    return retorno;

                }
                else
                {
                    ErrorResponse retorno = JsonConvert.DeserializeObject<ErrorResponse>(result.Content.ToString());
                    retorno.StatusCode = result.StatusCode;

                    MontaLogError(retorno, $"com o Id '{idCidades}'");

                    _gravaLog.GravarLogError(retorno, idCidades.ToString());

                    return retorno;
                }
            }
            catch (Exception ex)
            {
                var retorno = new ErrorResponse(HttpStatusCode.InternalServerError, ex.Message.ToString(), "request_error", ex.Message.ToString());
                MontaLogError(retorno, $"com o Id '{idCidades}'");
                _gravaLog.GravarLogError(retorno, idCidades.ToString());
                return retorno;
            }

        }

        /// <summary>
        /// Este método retorna uma lista de cidades brasileiras com o seu código. Pode-se filtrar pelo nome da cdade ou parte dele.
        /// </summary>
        /// <param name="nomeCidade"></param>
        /// <returns></returns>
        /// 
        public async Task<dynamic> ListarCidades(string nomeCidade)
        {
            try
            {
                var result = await _apiService.ListarCidades(nomeCidade);

                if (result.StatusCode == HttpStatusCode.OK)
                {
                    List<ListaCidadesResponse> cidadesRetorno = JsonConvert.DeserializeObject<List<ListaCidadesResponse>>(result.Content.ToString());

                    _logger.LogInformation($"Foram encontradas {cidadesRetorno.Count} cidades com o filtro '{nomeCidade}'.");
                    
                    return cidadesRetorno;
                }
                else
                {
                    ErrorResponse cidadesRetorno = JsonConvert.DeserializeObject<ErrorResponse>(result.Content.ToString());
                    cidadesRetorno.StatusCode = result.StatusCode;

                    MontaLogError(cidadesRetorno, $"com o filtro '{nomeCidade}'");

                    return cidadesRetorno;
                }
            }
            catch(Exception ex)
            {
                var retorno = new ErrorResponse(HttpStatusCode.InternalServerError, ex.Message.ToString(), "request_error", ex.Message.ToString());
                MontaLogError(retorno, $"com o filtro '{nomeCidade}'");
                return retorno;
            }

        }

        /// <summary>
        /// Este método retorna as condições metereológicas de um aeroporto. Pode-se utilizar o IdIcao ou o próprio nome do Aeroporto.
        /// </summary>
        /// <param name="codigoIcao"></param>
        /// <returns></returns>
        public async Task<dynamic> Aeroportos(string codigoIcao)
        {
            try
            {
                string nomeAeroporto = string.Empty;

                if (string.IsNullOrEmpty(codigoIcao))
                {
                    var retorno = new ErrorResponse(HttpStatusCode.BadRequest, "Codigo Icao não pode ser vazio", "request_error", "INVALID_ICAO_CODE");
                    MontaLogError(retorno, "");

                    return retorno;
                }

                foreach (var item in _listaAeroportos)
                {
                    if (item.Icao == codigoIcao)
                    {
                        nomeAeroporto = item.Aeroporto;
                    }
                }

                var result = await _apiService.Aeroportos(codigoIcao);
                if (result.Content.Contains("<!")) { _logger.LogError(result.Content.ToString()); return result; }
                
                if (result.StatusCode == HttpStatusCode.OK)
                {
                    var retorno = JsonConvert.DeserializeObject<AeroportoResponse>(result.Content.ToString());
                    retorno.StatusCode = result.StatusCode;
                    retorno.Nome_Aeroporto = nomeAeroporto;

                    if (retorno.Umidade == "undefined")
                    {
                        _logger.LogInformation($"Não há informações para o aeroporto {nomeAeroporto} na data e hora {DateTime.Now}.");
                        return retorno;
                    }

                    IDictionary<string, string> map = new Dictionary<string, string>
                    {
                        { "Umidade: ", retorno.Umidade.ToString() + "%" },
                        { "Visibilidade: ", retorno.Visibilidade + "Km" },
                        { "Pressao Atmosferica: ", retorno.Pressao_Atmosferica.ToString() + "hPa" },
                        { "Vento: ", retorno.Vento.ToString() + "Km/h" },
                        { "Direcao_vento: ", retorno.Direcao_Vento.ToString() + "º" },
                        { "Condicao: ", retorno.Condicao },
                        { "Condicao_desc: ", retorno.Condicao_Desc },
                        { "Temperatura: ", retorno.Temp.ToString() + "ºC" }
                    };

                    _logger.LogInformation($"As condições para o aeroporto {nomeAeroporto} em {retorno.Atualizado_Em} são:");

                    PrintLog(map);

                    return retorno;
                }
                else
                {
                    var retorno = JsonConvert.DeserializeObject<ErrorResponse>(result.Content.ToString());
                    retorno.StatusCode = result.StatusCode;

                    MontaLogError(retorno, "");

                    _gravaLog.GravarLogError(retorno, "");

                    return retorno;

                }
                
            }
            catch (Exception ex)
            {
                var retorno = new ErrorResponse(HttpStatusCode.InternalServerError, ex.Message.ToString(), "request_error", ex.Message.ToString());
                MontaLogError(retorno, "");

                return retorno;
            }
        }

        /// <summary>
        /// Retorna uma Lista de Aeroportos que estão pre-definidas no arquivo Aeroportos.jsoon. Pode-se filtrar pela sigla do estado. Ex.: MG, SP, RJ...
        /// </summary>
        /// <param name="siglaEstado"></param>
        /// <returns></returns>
        public List<ListaAeroportosResponse> ListarAeroportos(string siglaEstado)
        {
            try
            {
                var retorno = new List<ListaAeroportosResponse>();
                if (string.IsNullOrEmpty(siglaEstado))
                {
                    _logger.LogInformation($"{_listaAeroportos.Count} Aeroportos foram encontrados!");
                    return _listaAeroportos;
                }
                else
                {
                    siglaEstado = siglaEstado.ToUpper().Trim();
                    List<ListaAeroportosResponse> listaRetorno = new();

                    foreach (var item in _listaAeroportos)
                    {
                        if (item.Estado.Equals(siglaEstado))
                        {
                            listaRetorno.Add(item);
                        }
                    }

                    _logger.LogInformation($"{listaRetorno.Count} Aeroportos foram encontrados para o filtro '{siglaEstado}'!");

                    return listaRetorno;

                }
            }
            catch
            {
                return null;
            }
        }

        private void MontaLogError(ErrorResponse retorno, string mensagem)
        {
            IDictionary<string, string> log = new Dictionary<string, string>
            {
                { "Status Code: ", (int)retorno.StatusCode + " " + retorno.StatusCode.ToString() },
                { "Mensagem: ", retorno.Message + " " + mensagem},
                { "Tipo: ", retorno.Type },
                { "Nome: ", retorno.Name },
            };

            PrintLog(log);
        }

        private void PrintLog(IDictionary<string, string> map)
        {
            foreach (var item in map)
                _logger.LogInformation("{0} {1}", item.Key, item.Value);
        }
    }
}

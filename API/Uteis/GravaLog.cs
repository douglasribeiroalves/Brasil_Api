using API.Infrastructure;
using API.Interfaces;
using API.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Uteis
{
    public class GravaLog
    {
        private readonly ILogger<GravaLog> _logger;
        private readonly DadosBrasilApi _dadosBrasilApi;
        private readonly DataAccess _dataAccess;
        private readonly string _stringConexao;

        public GravaLog(ILogger<GravaLog> logger, IOptions<DadosBrasilApi> options)
        {
            _logger = logger;
            _dadosBrasilApi = options.Value;
            _dataAccess = new DataAccess(_dadosBrasilApi.StringConnection);
        }

        public async Task<Boolean> GravarLogRequisicao(string metodo, RestResponse payload, NivelLog nivel)
        {
            try
            {
                string statusCode = payload != null ? payload.StatusCode.ToString() : "InternalServerError";
                string content = payload != null ? payload.Content.ToString() : "Erro na Requisição";

                string commandText = $"Insert into tLogIntegracao values (GetDate(), '{metodo}', '{statusCode}', '{content}', '{nivel}')";
                bool resposta = await _dataAccess.ExecuteNonQuery(commandText);

                return resposta;
            }
            catch
            {
                return false;
            }
        }

        public async Task<Boolean> GravarLogAeroportos(string metodo, RestResponse payload, string ucid, NivelLog nivel)
        {
            try
            {
                string statusCode = payload != null ? payload.StatusCode.ToString() : string.Empty;
                string content = payload != null ? payload.Content.ToString() : string.Empty;

                string commandText = $"Insert into tLogIntegracao values (GetDate(), '{ucid}', '{metodo}', '{statusCode}', '{content}', '{nivel}')";
                bool resposta = await _dataAccess.ExecuteNonQuery(commandText);

                return resposta;
            }
            catch
            {
                return false;
            }
        }

        public async Task<Boolean> GravarLogCidades(CidadeResponse cidade)
        {
            try
            {
                string commandText = $"Insert into tLogCidade values (GetDate(), '{cidade.Cidade}', '{cidade.Clima[0].Condicao}', '{cidade.Clima[0].Min}', '{cidade.Clima[0].Max}', '{cidade.Clima[0].IndiceUv}', '{cidade.Clima[0].CondicaoDesc}')";
                bool resposta = await _dataAccess.ExecuteNonQuery(commandText);

                return resposta;
            }
            catch
            {
                return false;
            }
        }

        public async Task<Boolean> GravarLogError(ErrorResponse error, string mensagem)
        {
            //try
            //{
            //    string commandText = $"Insert into tLogIntegracao values (GetDate(), '{ucid}', '{metodo}', '{statusCode}', '{content}', '{nivel}')";
            //    bool resposta = await _dataAccess.ExecuteNonQuery(commandText);

            //    return resposta;
            //}
            //catch
            //{
            //    return false;
            //}

            return false;
        }
    }
}

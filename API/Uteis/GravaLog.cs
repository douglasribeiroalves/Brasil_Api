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

        public Boolean GravarLogRequisicao(string metodo, RestResponse payload, NivelLog nivel)
        {
            try
            {
                string statusCode = payload != null ? payload.StatusCode.ToString() : "InternalServerError";
                string content = payload != null ? payload.Content.ToString() : "Erro na Requisição";

                string commandText = $"Insert into tLogIntegracao values (GetDate(), '{metodo}', '{statusCode}', '{content}', '{nivel}')";
                bool resposta = _dataAccess.ExecuteNonQuery(commandText);

                return resposta;
            }
            catch
            {
                return false;
            }
        }

        public Boolean GravarLogAeroportos(AeroportoResponse aeroporto)
        {
            try
            {
                string commandText = $"Insert into tLogAeroporto values (GetDate(), '{aeroporto.Nome_Aeroporto}', '{aeroporto.Umidade}', '{aeroporto.Visibilidade}', " +
                    $"'{aeroporto.Pressao_Atmosferica}', '{aeroporto.Vento}', '{aeroporto.Direcao_Vento}', '{aeroporto.Condicao}, '{aeroporto.Condicao_Desc}, '{aeroporto.Temp}')";
                bool resposta = _dataAccess.ExecuteNonQuery(commandText);

                return resposta;
            }
            catch
            {
                return false;
            }
        }

        public Boolean GravarLogCidades(CidadeResponse cidade)
        {
            try
            {
                string commandText = $"Insert into tLogCidade values (GetDate(), '{cidade.Cidade}', '{cidade.Clima[0].Condicao}', " +
                    $"'{cidade.Clima[0].Min}', '{cidade.Clima[0].Max}', '{cidade.Clima[0].IndiceUv}', '{cidade.Clima[0].CondicaoDesc}')";
                bool resposta = _dataAccess.ExecuteNonQuery(commandText);

                return resposta;
            }
            catch
            {
                return false;
            }
        }

        public Boolean GravarLogError(ErrorResponse error, string mensagem)
        {
            try
            {
                string commandText = $"Insert into tLogError values (GetDate(), '{error.StatusCode}', '{error.Message} {mensagem}', '{error.Type}', '{error.Name}')";
                bool resposta = _dataAccess.ExecuteNonQuery(commandText);

                return resposta;
            }
            catch
            {
                return false;
            }
        }
    }
}

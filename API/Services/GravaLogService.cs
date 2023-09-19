
using API.Infrastructure;
using API.Interfaces;
using API.Model;
using API.Uteis;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RestSharp;
using System;
using System.Threading.Tasks;

namespace API.Services
{
    public class GravaLogService : IGravaLogService
    {
        private readonly GravaLog _gravaLog;

        public GravaLogService(ILogger<GravaLog> logger, IOptions<DadosBrasilApi> options)
        {
            _gravaLog = new GravaLog(logger, options);
        }

        public Boolean GravarLogRequisicao(string metodo, RestResponse payload, NivelLog nivel)
        {
            return _gravaLog.GravarLogRequisicao(metodo, payload, nivel);
        }

        public Boolean GravarLogCidades(CidadeResponse cidade)
        {
            return _gravaLog.GravarLogCidades(cidade);
        }

        public Boolean GravarLogAeroportos(AeroportoResponse aeroporto)
        {
            return _gravaLog.GravarLogAeroportos(aeroporto);
        }

        public Boolean GravarLogError(ErrorResponse error, string mensagem)
        {
            return _gravaLog.GravarLogError(error, mensagem);
        }
    }
}

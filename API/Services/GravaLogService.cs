
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

        public async Task<Boolean> GravarLogRequisicao(string metodo, RestResponse payload, NivelLog nivel)
        {
            return await _gravaLog.GravarLogRequisicao(metodo, payload, nivel);
        }

        public async Task<Boolean> GravarLogCidades(CidadeResponse cidade)
        {
            return await _gravaLog.GravarLogCidades(cidade);
        }

        public async Task<Boolean> GravarLogAeroportos(string metodo, RestResponse payload, string ucid, NivelLog nivel)
        {
            return await _gravaLog.GravarLogAeroportos(metodo, payload, ucid, nivel);
        }

        public async Task<Boolean> GravarLogError(ErrorResponse error, string mensagem)
        {
            return await _gravaLog.GravarLogError(error, mensagem);
        }
    }
}

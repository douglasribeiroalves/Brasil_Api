
using API.Model;
using RestSharp;
using System;
using System.Threading.Tasks;

namespace API.Interfaces
{
    public enum NivelLog
    {
        FALHA = 1,
        FALHA_EMINENTE = 2,
        ATENCAO = 3,
        INFORMACAO = 4
    }

    public interface IGravaLogService
    {
        Task<Boolean> GravarLogRequisicao(string metodo, RestResponse payload, NivelLog nivel);

        Task<Boolean> GravarLogCidades(CidadeResponse cidade);

        Task<Boolean> GravarLogAeroportos(string metodo, RestResponse payload, string ucid, NivelLog nivel);

        Task<Boolean> GravarLogError(ErrorResponse error, string mensagem);
    }
}

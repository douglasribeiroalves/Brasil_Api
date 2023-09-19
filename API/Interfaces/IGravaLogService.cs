
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
        Boolean GravarLogRequisicao(string metodo, RestResponse payload, NivelLog nivel);

        Boolean GravarLogCidades(CidadeResponse cidade);

        Boolean GravarLogAeroportos(AeroportoResponse aeroporto);

        Boolean GravarLogError(ErrorResponse error, string mensagem);
    }
}

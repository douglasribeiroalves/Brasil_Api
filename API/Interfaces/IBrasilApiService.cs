using API.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IBrasilApiService
    {
        Task<dynamic> CondicoesCidades(int idCidades);
        Task<dynamic> ListarCidades(string nomeCidade);
        Task<dynamic> Aeroportos(string codigoIcao);
        List<ListaAeroportosResponse> ListarAeroportos(string siglaEstado);
    }
}

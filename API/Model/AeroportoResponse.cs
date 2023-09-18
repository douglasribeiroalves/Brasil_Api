
using System.Net;

namespace API.Model
{
    public class AeroportoResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Umidade { get; set; }
        public string Visibilidade { get; set; }
        public string Codigo_Icao { get; set; }
        public string Nome_Aeroporto { get; set; }
        public string Pressao_Atmosferica { get; set; }
        public string Vento { get; set; }
        public string Direcao_Vento { get; set; }
        public string Condicao { get; set; }
        public string Condicao_Desc { get; set; }
        public string Temp { get; set; }
        public string Atualizado_Em { get; set; }

    }

}

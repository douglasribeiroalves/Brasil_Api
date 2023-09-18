using System.Collections.Generic;

namespace API.Model
{
    public class Clima
    {
        public string Data { get; set; }
        public string Condicao { get; set; }
        public string Min { get; set; }
        public string Max { get; set; }
        public string IndiceUv { get; set; }
        public string CondicaoDesc { get; set; }

        public Clima()
        {
            Data = string.Empty;
            Condicao = string.Empty;
            Min = string.Empty;
            Max = string.Empty;
            IndiceUv = string.Empty;
            CondicaoDesc = string.Empty;
        }
    }

    public class CidadeResponse
{
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public string AtualizadoEm { get; set; }
        public List<Clima> Clima { get; set; }

        public CidadeResponse()
        {
            Cidade = string.Empty;
            Estado = string.Empty;
            AtualizadoEm = string.Empty;
        }
    }
}

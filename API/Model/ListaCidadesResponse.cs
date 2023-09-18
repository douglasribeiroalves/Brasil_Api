using System.Net;

namespace API.Model
{
    /// <summary>
    /// ListaCidadesResponse myDeserializedClass = JsonConvert.DeserializeObject[List[ListaCidadesResponse]](myJsonResponse)
    /// </summary>
    public class ListaCidadesResponse
    {
        public string Nome { get; set; }
        public int? Id { get; set; }
        public string Estado { get; set; }


    }

}

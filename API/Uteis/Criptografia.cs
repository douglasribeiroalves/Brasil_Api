using System.Text;
using System;

namespace API.Uteis
{
    public class Criptografia
    {
        public string Decriptografar(string dados)
        {
            string empty = string.Empty;
            try
            {
                byte[] bytes = Convert.FromBase64String(dados);
                return Encoding.ASCII.GetString(bytes);
            }
            catch (Exception ex)
            {
                return "ERRO DECRIPTOGRAFAR: " + ex.Message.ToString();
            }
        }
    }
}

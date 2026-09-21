using MauiAppTempoAgora.Models;
using Newtonsoft.Json.Linq;
using System.Net;

namespace MauiAppTempoAgora.Services
{
    public class DataService
    {
        public static async Task<Tempo?> GetPrevisao(string cidade)
        {
            // 1. Verifica se há conexão com a internet
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
            {
                throw new Exception("SemConexao");
            }

            Tempo? t = null;

            string chave = "a0ecb4f95ad45c1c36aa97d794d5b68d"; 

            string url = $"https://api.openweathermap.org/data/2.5/weather?" +
                $"q={cidade}&units=metric&appid={chave}";

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage resp = await client.GetAsync(url);
                
                // 2. Se a cidade for encontrada (Sucesso - Status 200)
                if (resp.IsSuccessStatusCode)
                {
                    string json = await resp.Content.ReadAsStringAsync();
                    
                    var rascunho = JObject.Parse(json);

                    DateTime time = new();
                    DateTime sunrise = time.AddSeconds((double)rascunho["sys"]["sunrise"]).ToLocalTime();
                    DateTime sunset = time.AddSeconds((double)rascunho["sys"]["sunset"]).ToLocalTime();

                    t = new()
                    {
                        lat = (double)rascunho["coord"]["lat"],
                        lon = (double)rascunho["coord"]["lon"],
                        description = (string)rascunho["weather"][0]["description"],
                        main = (string)rascunho["weather"][0]["main"],
                        temp_min = (double)rascunho["main"]["temp_min"],
                        temp_max = (double)rascunho["main"]["temp_max"],
                        speed = (double)rascunho["wind"]["speed"],
                        visibility = (int)rascunho["visibility"],
                        sunrise = sunrise.ToString("HH:mm:ss"),
                        sunset = sunset.ToString("HH:mm:ss")
                    };// fecha objeto do tempo.
                } // fecha if se o status code for sucesso.

                // 3. Se a API retornar erro 404 (Cidade Não Encontrada)
                else if (resp.StatusCode == HttpStatusCode.NotFound)
                {
                    throw new Exception("CidadeNaoEncontrada");
                }
                // 4. Outros erros de API (como chave inválida)
                else
                {
                    throw new Exception($"Erro na API: {resp.StatusCode}");
                }
            } //fecha laço do using do HttpClient.



            return t;
        }
    }
}

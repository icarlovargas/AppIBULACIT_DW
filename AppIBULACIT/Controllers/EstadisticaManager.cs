using AppIBULACIT.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace AppIBULACIT.Controllers
{
    public class EstadisticaController
    {
        String UrlBase = "http://localhost:49220/api/Estadistica/";
        HttpClient GetClient(string token)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", token);
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");



            return httpClient;
        }

        public async Task<Estadistica> ObtenerEstadistica(string codigo)
        {
            HttpClient httpClient = new HttpClient();

            var reponse = await httpClient.GetStringAsync(string.Concat(UrlBase, codigo));

            return JsonConvert.DeserializeObject<Estadistica>(reponse);
        }

        public async Task<IEnumerable<Estadistica>> ObtenerEstadisticas()
        {
            HttpClient httpClient = new HttpClient();

            var reponse = await httpClient.GetStringAsync(string.Concat(UrlBase));

            return JsonConvert.DeserializeObject<IEnumerable<Estadistica>>(reponse);
        }


        public async Task<Estadistica> Ingresar(Estadistica estadistica)
        {
            HttpClient httpClient = new HttpClient();

            var reponse = await httpClient.PostAsync(UrlBase,
                new StringContent(JsonConvert.SerializeObject(estadistica), Encoding.UTF8, "application/json"));

            return JsonConvert.DeserializeObject<Estadistica>(await reponse.Content.ReadAsStringAsync());
        }
    }
}
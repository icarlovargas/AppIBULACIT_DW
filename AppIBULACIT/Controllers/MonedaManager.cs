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
    public class MonedaController
    {
        String UrlBase = "http://localhost:49220/api/Moneda/";
        HttpClient GetClient(string token)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", token);
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");



            return httpClient;
        }

        public async Task<Moneda> ObtenerMoneda(string token, string codigo)
        {
            HttpClient httpClient = GetClient(token);

            var reponse = await httpClient.GetStringAsync(string.Concat(UrlBase, codigo));

            return JsonConvert.DeserializeObject<Moneda>(reponse);
        }

        public async Task<IEnumerable<Moneda>> ObtenerMonedas(string token, string codigo)
        {
            HttpClient httpClient = GetClient(token);

            var reponse = await httpClient.GetStringAsync(string.Concat(UrlBase));

            return JsonConvert.DeserializeObject<IEnumerable<Moneda>>(reponse);
        }


        public async Task<Moneda> Ingresar(Moneda moneda, string token)
        {
            HttpClient httpClient = GetClient(token);

            var reponse = await httpClient.PostAsync(UrlBase,
                new StringContent(JsonConvert.SerializeObject(moneda), Encoding.UTF8, "application/json"));

            return JsonConvert.DeserializeObject<Moneda>(await reponse.Content.ReadAsStringAsync());
        }


        public async Task<Moneda> Actualizar(Moneda moneda, string token)
        {
            HttpClient httpClient = GetClient(token);

            var reponse = await httpClient.PutAsync(UrlBase,
                new StringContent(JsonConvert.SerializeObject(moneda), Encoding.UTF8, "application/json"));

            return JsonConvert.DeserializeObject<Moneda>(await reponse.Content.ReadAsStringAsync());
        }

        public async Task<string> Actualizar(string id, string token)
        {
            HttpClient httpClient = GetClient(token);

            var reponse = await httpClient.DeleteAsync(string.Concat(UrlBase, id));

            return JsonConvert.DeserializeObject<string>(await reponse.Content.ReadAsStringAsync());
        }


    }
}
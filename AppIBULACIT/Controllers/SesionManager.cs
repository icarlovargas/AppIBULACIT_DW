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
    public class SesionController
    {
        String UrlBase = "http://localhost:49220/api/Sesion/";
        HttpClient GetClient(string token)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", token);
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");



            return httpClient;
        }

        public async Task<Sesion> ObtenerSesion(string token, string codigo)
        {
            HttpClient httpClient = GetClient(token);

            var reponse = await httpClient.GetStringAsync(string.Concat(UrlBase, codigo));

            return JsonConvert.DeserializeObject<Sesion>(reponse);
        }

        public async Task<IEnumerable<Sesion>> ObtenerSesiones(string token, string codigo)
        {
            HttpClient httpClient = GetClient(token);

            var reponse = await httpClient.GetStringAsync(string.Concat(UrlBase));

            return JsonConvert.DeserializeObject<IEnumerable<Sesion>>(reponse);
        }


        public async Task<Sesion> Ingresar(Sesion sesion, string token)
        {
            HttpClient httpClient = GetClient(token);

            var reponse = await httpClient.PostAsync(UrlBase,
                new StringContent(JsonConvert.SerializeObject(sesion), Encoding.UTF8, "application/json"));

            return JsonConvert.DeserializeObject<Sesion>(await reponse.Content.ReadAsStringAsync());
        }


        public async Task<Sesion> Actualizar(Sesion sesion, string token)
        {
            HttpClient httpClient = GetClient(token);

            var reponse = await httpClient.PutAsync(UrlBase,
                new StringContent(JsonConvert.SerializeObject(sesion), Encoding.UTF8, "application/json"));

            return JsonConvert.DeserializeObject<Sesion>(await reponse.Content.ReadAsStringAsync());
        }

        public async Task<string> Eliminar(string id, string token)
        {
            HttpClient httpClient = GetClient(token);

            var reponse = await httpClient.DeleteAsync(string.Concat(UrlBase, id));

            return JsonConvert.DeserializeObject<string>(await reponse.Content.ReadAsStringAsync());
        }


    }
}
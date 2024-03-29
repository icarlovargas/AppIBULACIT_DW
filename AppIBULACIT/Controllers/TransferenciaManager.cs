﻿using AppIBULACIT.Models;
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
    public class TransferenciaManager
    {
        String UrlBase = "http://localhost:49220/api/Transferencia/";
        HttpClient GetClient(string token)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", token);
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");



            return httpClient;
        }

        public async Task<Transferencia> ObtenerTransferencia(string token, string codigo)
        {
            HttpClient httpClient = GetClient(token);

            var reponse = await httpClient.GetStringAsync(string.Concat(UrlBase, codigo));

            return JsonConvert.DeserializeObject<Transferencia>(reponse);
        }

        public async Task<IEnumerable<Transferencia>> ObtenerTransferencias(string token)
        {
            HttpClient httpClient = GetClient(token);

            var reponse = await httpClient.GetStringAsync(string.Concat(UrlBase));

            return JsonConvert.DeserializeObject<IEnumerable<Transferencia>>(reponse);
        }


        public async Task<Transferencia> Ingresar(Transferencia transferencia, string token)
        {
            HttpClient httpClient = GetClient(token);

            var reponse = await httpClient.PostAsync(UrlBase,
                new StringContent(JsonConvert.SerializeObject(transferencia), Encoding.UTF8, "application/json"));

            return JsonConvert.DeserializeObject<Transferencia>(await reponse.Content.ReadAsStringAsync());
        }


        public async Task<Transferencia> Actualizar(Transferencia transferencia, string token)
        {
            HttpClient httpClient = GetClient(token);

            var reponse = await httpClient.PutAsync(UrlBase,
                new StringContent(JsonConvert.SerializeObject(transferencia), Encoding.UTF8, "application/json"));

            return JsonConvert.DeserializeObject<Transferencia>(await reponse.Content.ReadAsStringAsync());
        }

        public async Task<string> Actualizar(string id, string token)
        {
            HttpClient httpClient = GetClient(token);

            var reponse = await httpClient.DeleteAsync(string.Concat(UrlBase, id));

            return JsonConvert.DeserializeObject<string>(await reponse.Content.ReadAsStringAsync());
        }


    }
}
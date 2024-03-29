﻿using AppIBULACIT.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;


//estas clases no tiene maneja tokens
namespace AppIBULACIT.Controllers
{
    public class ErrorManager
    {
        String UrlBase = "http://localhost:49220/api/Error/";
        HttpClient GetClient(string token)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", token);
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");



            return httpClient;
        }

        public async Task<Error> ObtenerError(string codigo)
        {
            HttpClient httpClient = new HttpClient();

            var reponse = await httpClient.GetStringAsync(string.Concat(UrlBase, codigo));

            return JsonConvert.DeserializeObject<Error>(reponse);
        }

        public async Task<IEnumerable<Error>> ObtenerErrores(string token)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.GetStringAsync(UrlBase);

            return JsonConvert.DeserializeObject<IEnumerable<Error>>(response);
        }


        public async Task<Error> Ingresar(Error error)
        {
            HttpClient httpClient = new HttpClient();

            var reponse = await httpClient.PostAsync(UrlBase,
                new StringContent(JsonConvert.SerializeObject(error), Encoding.UTF8, "application/json"));

            return JsonConvert.DeserializeObject<Error>(await reponse.Content.ReadAsStringAsync());
        }


        public async Task<Error> Actualizar(Error error)
        {
            HttpClient httpClient = new HttpClient();

            var reponse = await httpClient.PutAsync(UrlBase,
                new StringContent(JsonConvert.SerializeObject(error), Encoding.UTF8, "application/json"));

            return JsonConvert.DeserializeObject<Error>(await reponse.Content.ReadAsStringAsync());
        }

        public async Task<string> Actualizar(string id, string token)
        {
            HttpClient httpClient = new HttpClient();

            var reponse = await httpClient.DeleteAsync(string.Concat(UrlBase, id));

            return JsonConvert.DeserializeObject<string>(await reponse.Content.ReadAsStringAsync());
        }


    }
}
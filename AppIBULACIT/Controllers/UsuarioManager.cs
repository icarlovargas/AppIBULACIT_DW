using AppIBULACIT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;

namespace AppIBULACIT.Controllers
{
    public class UsuarioManager
    {
        string UrlAuthenthicate = "http://localhost:49220/api/login/authenticate/";
        string UrlRegister = "http://localhost:49220/api/login/register";
        //private Encoding encoding;

        public async Task<Usuario> Autenticar(LoginRequest loginRequest)
        {
            
            HttpClient httpClient = new HttpClient();

            var reponse = await 
                httpClient.PostAsync(UrlAuthenthicate,new StringContent(JsonConvert.SerializeObject(loginRequest),
                Encoding.UTF8, "application/json"));

            return JsonConvert.DeserializeObject<Usuario>(await reponse.Content.ReadAsStringAsync());
        }

        public async Task<Usuario> Registrar(Usuario usuario)
        {
            
            HttpClient httpClient = new HttpClient();

            var reponse = await
                httpClient.PostAsync(UrlRegister, new StringContent(JsonConvert.SerializeObject(usuario),
                Encoding.UTF8, "application/json"));

            return JsonConvert.DeserializeObject<Usuario>(await reponse.Content.ReadAsStringAsync());
        }
    }
}
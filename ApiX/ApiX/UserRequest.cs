using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
namespace ApiX
{
    public class UserRequest
    {
        private HttpClient _restclient;
        public UserRequest(HttpClient restclient){

            if (restclient != null) _restclient = restclient;
           else throw new NullReferenceException("El cliente http no puede hacer null");
             
        }
        public async Task<List<Usuario>> All(){
           

            var response = await _restclient.ExcecuteAsync<List<Usuario>>(Method.GET, $"{App.BaseUrl}/Usuario/todo");
            if (response != null) return response.Result;
           
                return new List<Usuario>();
            
        }

        public async Task<bool> Add(Usuario usuario)
        {
            var response = await _restclient.ExcecuteAsync<Usuario>(Method.POST, $"{App.BaseUrl}/Usuario/add", new Dictionary<string, object>
            {
                { "nombre", usuario.nombre},
                { "email", usuario.email},
                { "usuario", usuario.usuario},
                { "contra", usuario.contra}
            });

            return response.Result != null;
            
        }
        public async Task<bool> Update(Usuario usuario, int id)
        {
            var response = await _restclient.ExcecuteAsync<Usuario>(Method.POST, $"{App.BaseUrl}/Usuario/Update/{id}", new Dictionary<string, object>
            {
                { "nombre", usuario.nombre},
                { "email", usuario.email},
                { "usuario", usuario.usuario},
                { "contra", usuario.contra}
            });

            return response.Result != null;

        }

        public async Task<bool> Delete(int id)
        {
            var response = await _restclient.ExcecuteAsync<StatusResponse>(Method.GET, $"{App.BaseUrl}/Usuario/Delete/{id}");
            return response.Result != null && response.Result.code==401;
        }
    }
}

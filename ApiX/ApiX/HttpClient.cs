using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApiX
{
    public class HttpClient
    {
        private Dictionary<string, string> _headers;
        public HttpClient(Dictionary<string, string> headers)
        {
            if (headers != null)
            {
                _headers = headers;
            }
            else
            {
                _headers = new Dictionary<string, string>();
            }
        }

        public string this[string key]
        {
            get
            {
                return _headers[key];
            }
            set
            {
                _headers[key] = value;
            }
        }
        //HTTP GET REQUEST
        //HTTP POST REQUEST
        public async Task<HttpResponse<T>> ExcecuteAsync<T>(Method method, string baseUrl, Dictionary<string, object> formData=null)
        {
            var client = new RestSharp.RestClient(baseUrl);
            var request = new RestRequest(method);

            //asignar los headers a la peticion
            foreach(var header in _headers)
            {
                request.AddHeader(header.Key, header.Value);
            }
            if (formData != null)
            {
                foreach (var item in formData)
                {
                    request.AddParameter(item.Key, item.Value);
                }
            }
            
            IRestResponse rta=null;
            Exception ex = null;
            try
            {
                rta = await client.ExecuteAsync(request);
            }
            catch(Exception _ex)
            {
                ex = _ex;
            }
            var httpResponse = new HttpResponse<T>
            {
                Response = rta
            };
            if (rta == null)
            {
                httpResponse.Status = new StatusResponse
                {
                    code = -1,
                    message = $"No se pudo obtener una respuesta del servidor, stacktrace: {ex.StackTrace}"
                };
                return httpResponse;
            }
            
            var jsonresult = rta.Content;
            if (rta.StatusCode == System.Net.HttpStatusCode.OK)
            {
               
               var telement= JsonConvert.DeserializeObject<T>(jsonresult);
               httpResponse.Result = telement;

            }
            else if (rta.StatusCode == 0) 
            {
                httpResponse.Status = new StatusResponse
                {
                    code = 0,
                    message = $"No se pudo obtener una respuesta del servidor"
                };
            }
            else
            {
                var status = JsonConvert.DeserializeObject<StatusResponse>(jsonresult);
                httpResponse.Status = status;
            }
            
            return httpResponse;
        }

        
       

    }
}

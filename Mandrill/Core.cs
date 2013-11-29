using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oak;
using RestSharp;
using RestSharp.Deserializers;
using RestSharp.Extensions;

namespace Mandrill
{
    public partial class MandrillRestClient
    {
        // Version # of API (default: 1.0)
        public string ApiVersion { get; set; }
        /// <summary>
        /// Base URL of API (default: https://mandrillapp.com/api/)
        /// </summary>
        public string BaseUrl { get; set; }
        private string ApiKey { get; set; }

        private RestClient _client;

        public MandrillRestClient(string apiKey)
        {
            ApiVersion = "1.0";
            BaseUrl = "https://mandrillapp.com/api/";
            ApiKey = apiKey;

            _client = new RestClient { BaseUrl = string.Format("{0}{1}", BaseUrl, ApiVersion) };
            _client.AddHandler("application/json", new DynamicJsonDeserializer());
        }

        public T Execute<T>(RestRequest request, dynamic data) where T : new()
        {
            request.RequestFormat = DataFormat.Json;
            request.Method = Method.POST;

            request.OnBeforeDeserialization = (resp) =>
            {
                if (((int)resp.StatusCode) >= 400)
                {
                    request.RootElement = "";
                }
            };

            request.DateFormat = "yyyy-MM-ddTHH:mm:ssZ";

            data.key = ApiKey;

            string json = DynamicToJson.Convert(data);

            request.AddParameter("application/json; charset=utf-8", json, ParameterType.RequestBody);

            var response = _client.Execute<T>(request);
            return response.Data;
        }

    }
}

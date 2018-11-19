using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.IRepo;
using Repository.Models;
using Newtonsoft;
using Newtonsoft.Json;
using RestSharp;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Net;
using System.Xml;
using SimpleJson;

namespace Repository.Repo
{
    public class CurrencyFixerRepo : ICurrencyFixerRepo
    {
        private readonly string _apiKey;

        public CurrencyFixerRepo()
        {
            //_apiKey = "http://data.fixer.io/api/latest?access_key=98319d39a4a5ec81de1a2b0e235ec781";
            _apiKey = "http://api.nbp.pl/api/exchangerates/tables/c?format=json";
            //_apiKey = "http://api.nbp.pl/api/exchangerates/tables/A/last/?format=json";
        }
        public Response GetCurrentRates()
        {
            var response = RetriveRestResponse();

            var content = ParseResponse(response);
            return content;
        }


        public Response ParseResponse( IRestResponse response)
        {
            var content = new Response();
            
            response.Content = response.Content.Substring(1, response.Content.Length - 2);

            content.Success = JObject.Parse(response.Content).GetValue("table") != null;

            content.TimeStamp = DateTime.ParseExact(
                JObject.Parse(response.Content)
                    .GetValue("effectiveDate")
                    .ToString(),
                "yyyy-MM-dd",
                CultureInfo.InvariantCulture);


            var resultObjects = AllChildren(JObject.Parse(response.Content))
                .First(c => c.Type == JTokenType.Array && c.Path.Contains("rates"))
                .Children<JObject>();

            foreach (JObject result in resultObjects)
            {
                content.Rates.Add(JsonConvert.DeserializeObject<Rate>(result.ToString()));
            }

            return content;
        }

        public IRestResponse RetriveRestResponse()
        {
            var client = new RestClient(_apiKey);
            var request = new RestRequest(Method.GET);

            IRestResponse response = client.Execute(request);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                //throw new WebRequestFailedException { HttpStatusCode = (int)response.StatusCode };
            }

            return response;
        }

        private static IEnumerable<JToken> AllChildren(JToken json)
        {
            foreach (var c in json.Children())
            {
                yield return c;
                foreach (var cc in AllChildren(c))
                {
                    yield return cc;
                }
            }
        }

    }
}

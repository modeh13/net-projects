using RestAppDiplomaed.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;

namespace RestAppDiplomaed.Services
{
    public class RestClient
    {
        protected string BaseUrl { get; set; } = "http://restcountries.eu/rest/v1";
        public string Serialize()
        {
            List<Country> countries = new List<Country>()
            {
                new Country() { Name = "México" },
                new Country() { Name = "Costa Rica" }
            };

            string json = JsonConvert.SerializeObject(countries);

            Debug.WriteLine(json);
            return json;
        }

        public void Deserialize()
        {
            var json = Serialize();
            IEnumerable<Country> countries = JsonConvert.DeserializeObject<IEnumerable<Country>>(json);

            foreach(Country country in countries)
            {
                Debug.WriteLine(country.Name);
            }
        }

        public async Task<IEnumerable<Country>> GetCountries()
        {
            return await GetAsJson();
        }

        protected async Task<IEnumerable<Country>> GetAsJson()
        {
            var result = Enumerable.Empty<Country>();

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await httpClient.GetAsync(BaseUrl).ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                    if (!string.IsNullOrEmpty(json))
                    {
                        result = await Task.Run(() =>
                        {
                            return JsonConvert.DeserializeObject<IEnumerable<Country>>(json);

                        }).ConfigureAwait(false);
                    }
                }
            }

            return result;
        }
    }
}
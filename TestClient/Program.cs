using IdentityModel.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace TestClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                string token = await GenerateToken();
                // string token = "eyJhbGciOiJSUzI1NiIsImtpZCI6IjM1RUIyNzNBNjM5M0JDREZFRDgwRTczNTc1RTc5N0Q2IiwidHlwIjoiYXQrand0In0.eyJuYmYiOjE2MDI2MTY5NzIsImV4cCI6MTYwMjYyMDU3MiwiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NTAwMSIsImNsaWVudF9pZCI6ImNsaWVudCIsImp0aSI6IjZCMkEwOEU1QjM5NDNEOTRGNDgyMTk2RTY4RDFENTVEIiwiaWF0IjoxNjAyNjE2OTcyLCJzY29wZSI6WyJhcGkxIl19.lE7LZrilO08Z0zCUbjUgkpkJ5NHI6vcMiBal5S_DcjfJx5zai6KgDnqgsTrwS064iWpqVMh0W0F0MaAyeURJB6IHPAYMcgcnB4hS52yrv8a7c5yrsvqggAdacQcmqJ5KNNn7wH25cACIELjV-n_jj6TvlyQ3M4_7BxzPfUb0H3BkyAWOkRnMCuI4JjC7uDIcrv-PWW4IIoVavOaJ8NDASvvp9ZPABE4yX1MIYU50jxZJ8dlNavIIYIwb1tXa4xXGPgsmSp1Cq187rglYpLYwbccLuegslgl1dWUwLPtGdq3y1VjlaJdIC-CIavXzauMVKAtQzMN3ehOU_G3-fzRZUw";
                await GetDataFromAPI(token);
                Console.ReadKey();
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }

        private static async Task<string> GenerateToken()
        {
            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync("https://localhost:5001");
            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
                return "";
            }

            var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = "client",
                ClientSecret = "secret",
                Scope = "api1"
            });

            if (tokenResponse.IsError)
            {
                Console.WriteLine(disco.Error);
                return "";
            }

            Console.WriteLine(tokenResponse.Json);
            return tokenResponse.AccessToken;
        }

        private static async Task GetDataFromAPI(string token)
        {
            var apiClient = new HttpClient();
            apiClient.SetBearerToken(token);

            var response = await apiClient.GetAsync("https://localhost:6001/identity");
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(JArray.Parse(content));
            }
            else
            {
                string content = $"Status: {response.StatusCode}";
                Console.WriteLine(content);
            }
        }
    }
}

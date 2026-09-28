using JWTBearerTokenAuthentificationSample.ClientConsole.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace JWTBearerTokenAuthentificationSample.ClientConsole
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Bitte Taste drücken, wenn API verfügbar ist");
            Console.ReadKey();


            JWTBearerTokenClient client = new JWTBearerTokenClient();

            LoginCredentialsResponse tokenResult = await client.Login("MaxMuster", "Wert1234!");


            Console.WriteLine("Token sollte verfügbar sein - Weiter mit Key");
            Console.ReadKey();

            List<WeatherForecast> forecastList = await client.GetWheatherForecast(tokenResult);


            Console.WriteLine("Fertig");
            Console.ReadLine();
        }
    }

    public class JWTBearerTokenClient
    {
        private HttpClient _client = new HttpClient();



        public async Task<LoginCredentialsResponse> Login(string Name, string Passwort)
        {
            string loginURL = "https://localhost:7114/api/Auth/Login";

            //Aufbau meiner Login Parameter als Object -> Das Object wird in Http-Body bgelegt -> StringContent
            LoginCredentials login = new LoginCredentials
            {
                Username = Name,
                Password = Passwort
            };

            //Serialisiereun unsere Parameter
            string jsonText = JsonConvert.SerializeObject(login);

            //HttpBody 
            StringContent body = new StringContent(jsonText, Encoding.UTF8, "application/json");

            //Aufruf der WebAPI - Methode  -> _client.PostAsync(loginURL, body);
            HttpResponseMessage resposne = await _client.PostAsync(loginURL, body);

            //Auslesen des JSON Ergebnises
            string responseJson = resposne.Content.ReadAsStringAsync().Result;

            //Konventieren von JSON zu Ergebnis-Objekt
            LoginCredentialsResponse loginCredentialsResponse = JsonConvert.DeserializeObject<LoginCredentialsResponse>(responseJson);

            //wir haben jetzt einen Token :-) 
            return loginCredentialsResponse;
        }

        public async Task<List<WeatherForecast>> GetWheatherForecast(LoginCredentialsResponse token)
        {
            string url = "https://localhost:7114/WeatherForecast/";

            //Bearer Token Authentifizierung wird verwendet 
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Token);

            //Aufbau des Requests
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, url);

            //Aufruf von WeatherForecast Get-Methode
            HttpResponseMessage httpResponseMessage = await _client.SendAsync(httpRequestMessage);

            //Auslesen des ERgebisses als JSON 
            string resultJSON = await httpResponseMessage.Content.ReadAsStringAsync();

            //Konventieren von JSON auf List<WeatherForecast>
            List<WeatherForecast> forecastList = JsonConvert.DeserializeObject<List<WeatherForecast>>(resultJSON);


            foreach (WeatherForecast forecast in forecastList)
            {
                Console.WriteLine($"{forecast.Date} - {forecast.Summary} - {forecast.TemperatureC} - {forecast.TemperatureF}");

            }


            return forecastList;
            //httpRequestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
        }



    }
}

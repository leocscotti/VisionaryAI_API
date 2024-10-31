using Newtonsoft.Json;
using System.Text;

namespace VisionaryAI.API.Services
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;
        public AuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> AutenticarUsuario(string username, string password)
        {
            var user = new { username, password };
            var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://jsonplaceholder.typicode.com/users", content);
            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                return jsonResponse;
            }
            else
            {
                throw new Exception("Falha na autenticação.");
            }
        }
    }
}

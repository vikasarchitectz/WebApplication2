using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace WebApplication2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly HttpClient _httpClient;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _logger = logger;
        }

        [HttpGet("mainAPI")]
        public async Task<IActionResult> InvokeGetMethod()
        {
            var responseGet = await _httpClient.GetAsync("https://localhost:7076/WeatherForecast/getAPI?first_name=Vikas");   

            var jsonData = JsonSerializer.Serialize(new
            {
                last_name = "Dadlani"
            });
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var responsePost = await _httpClient.PostAsync("https://localhost:7076/WeatherForecast/postAPI", content);  
            var responseGetTask = responseGet.Content.ReadAsStringAsync();
            var responseGetString = await responseGetTask;
            var responsePostTask = responsePost.Content.ReadAsStringAsync();
            var responsePostString = await responsePostTask;

            return Ok( responseGetString + " " + responsePostString);    
        } 

        [HttpGet("getAPI")]
        public string GetAPI(string first_name)
        {
            return first_name;
        }

        [HttpPost("postAPI")]
        public string InvokePostMethod([FromBody] dynamic data)
        {
            if (data.TryGetProperty("last_name", out JsonElement parameter1Element))
            {
                if (parameter1Element.ValueKind == JsonValueKind.String)
                {
                    return parameter1Element.GetString();
                }
            }
            return "";
        }
    }
}

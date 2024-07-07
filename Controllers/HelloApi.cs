using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace HNG_Stage_1_Task.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HelloApi : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<HelloApi> _logger;
        public HelloApi(HttpClient httpClient, ILogger<HelloApi> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }


        //https://localhost:7009/api/HelloApi?visitor_name=[visitor_name]
        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] string visitor_name)
        {
            var clientIp = HttpContext.Connection.RemoteIpAddress?.ToString();
            Console.WriteLine(clientIp);

            //Google Public DNS IP for testing
            if (clientIp == "::1") clientIp = await FetchIp.GetIp(_httpClient);


            //Fetching location details using IpInfo.io
            var ipInfoClient = await _httpClient.GetStringAsync($"http://ip-api.com/json/{clientIp}");
            var option = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };  
            var response = JsonSerializer.Deserialize<JsonElement>(ipInfoClient);
            var city = response.GetProperty("city").GetString();
            Console.WriteLine(city);


            //Get weather information
            var apiKey = "08a67d4ab3494eeca76202920240507";
            var weatherResponse = await _httpClient.GetStringAsync($"https://api.weatherapi.com/v1/current.json?key={apiKey}&q={city}");
            var weatherData = JsonSerializer.Deserialize<JsonDocument>(weatherResponse, option);
            var temperature = weatherData.RootElement.GetProperty("current").GetProperty("temp_c").GetDouble();

            var returnResponse = new
            {
                client_Ip = clientIp,
                location = city,
                greeting = $"Hello, {visitor_name}!, the temperature is {temperature} degrees Celsius in {city}"
            };

            return Ok(returnResponse);
        }

    }
}

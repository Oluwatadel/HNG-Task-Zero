namespace HNG_Stage_1_Task
{
    public class FetchIp
    {
        public static async Task<string> GetIp(HttpClient httpClient)
        {
            // Make a request to a service that returns the public IP address
            HttpResponseMessage response = await httpClient.GetAsync("https://api64.ipify.org?format=json");
            response.EnsureSuccessStatusCode();

            // Read the response content
            string responseBody = await response.Content.ReadAsStringAsync();

            // Deserialize JSON response to get the IP address
            dynamic jsonResponse = Newtonsoft.Json.JsonConvert.DeserializeObject(responseBody);
            string ipAddress = jsonResponse.ip;
            return ipAddress;
        }
    }
}

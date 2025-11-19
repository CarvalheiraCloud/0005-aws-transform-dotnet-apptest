using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace JobSchedulerApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string apiUrl = "https://api.myip.com"; 
            string logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log");
            string logFilePath = Path.Combine(logDirectory, "execution-test.log");

            try
            {
                using HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode();

                string jsonResponse = await response.Content.ReadAsStringAsync();
                var ipInfo = JsonConvert.DeserializeObject<IpInfo>(jsonResponse);

                if (!Directory.Exists(logDirectory))
                    Directory.CreateDirectory(logDirectory);

                string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} | IP: {ipInfo?.Ip}, Country: {ipInfo?.Country}, CC: {ipInfo?.Cc}";
                await File.AppendAllTextAsync(logFilePath, logEntry + Environment.NewLine);

                Console.WriteLine("Log entry added successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }

    public class IpInfo
    {
        [JsonProperty("ip")]
        public required string Ip { get; set; }

        [JsonProperty("country")]
        public required string Country { get; set; }

        [JsonProperty("cc")]
        public required string Cc { get; set; }
    }
}


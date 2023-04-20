using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using POCChatGPTCore.Models;
using RestSharp;
using System.Diagnostics;
using System.Linq;
using System.Net.Mime;
using System.Text.Json;

namespace POCChatGPTCore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        private string _apiKey = String.Empty;
        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View(new RequestModel());
        }

        [HttpPost]
        public async Task<IActionResult> IndexAsync(RequestModel model)
        {
            var options = new RestClientOptions()
            {
                MaxTimeout = -1,
            };
            _apiKey = _configuration.GetValue<string>("OpenAIKey") ?? String.Empty;
            var client = new RestClient(options);
            var request = new RestRequest("https://api.openai.com/v1/completions", Method.Post);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", $"Bearer {_apiKey}");
            var body = new TextDavinci003Model()
            {
                Model = "text-davinci-003",
                Prompt = model.Prompt,
                Temperature = 0.6f,
                max_tokens = 200,
                top_p = 1.0f,
                frequency_penalty = 1.0f,
                presence_penalty = 1.0f
            };
            request.AddBody(body);
            RestResponse response = await client.ExecuteAsync(request);

            if (response.IsSuccessful)
            {
                var result = JsonConvert.DeserializeObject<ResponseModel>(response.Content);
                if (string.IsNullOrEmpty(model.output))
                {
                    model.output = result.Choices.FirstOrDefault().Text.Trim();

                }
                else
                {
                    model.output = String.Concat(model.output, result.Choices.FirstOrDefault().Text.Trim());

                }
                return View(model);
            }
            _logger.LogError(JsonConvert.SerializeObject(response.Content));
            return View(new RequestModel());
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
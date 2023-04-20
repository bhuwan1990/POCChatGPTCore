using Microsoft.AspNetCore.Mvc;
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

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
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
            var client = new RestClient(options);
            var request = new RestRequest("https://api.openai.com/v1/completions", Method.Post);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", "Bearer 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");
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

            return View(new TextDavinci003Model());
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
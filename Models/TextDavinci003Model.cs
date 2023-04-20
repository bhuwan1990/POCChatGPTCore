

using Newtonsoft.Json;

namespace POCChatGPTCore.Models
{
    public class TextDavinci003Model
    {
        [JsonProperty("model")]
        public string Model { get; set; }

        [JsonProperty("prompt")]
        public string Prompt { get; set; }

        [JsonProperty("temperature")]
        public double Temperature { get; set; }

        [JsonProperty("max_tokens")]
        public int max_tokens { get; set; }

        [JsonProperty("top_p")]
        public double top_p { get; set; }

        [JsonProperty("frequency_penalty")]
        public double frequency_penalty { get; set; }

        [JsonProperty("presence_penalty")]
        public double presence_penalty { get; set; }
    }
}

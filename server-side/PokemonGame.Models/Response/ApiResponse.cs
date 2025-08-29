using Newtonsoft.Json;

namespace PokemonGame.Core.Models.Response
{
    public class ApiResponse
    {

        public int code { get; set; }
        public string message { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public object data { get; set; }
        public ApiResponse()
        {
            code = 0;
            message = "";
            data = default;
        }
        public ApiResponse(int code, string message, object data)
        {
            this.code = code;
            this.message = message;
            this.data = data;
        }
    }
}

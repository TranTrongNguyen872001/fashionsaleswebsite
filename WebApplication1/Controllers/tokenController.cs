using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json.Nodes;

namespace UngDungBanHang.Api.Controllers
{
    [ApiController]
    [Route("/api/cmd/[controller]")]
    public class tokenController : ControllerBase
    {
        [AllowAnonymous]
        [HttpPost]
        public JsonNode Index([FromBody] JsonObject index)
        {
            Token_Input input = JsonConvert.DeserializeObject<Token_Input>(index.ToString());
            Token_Output output = new Token_Output();
            output.Query_DataInput(input);
            return JsonObject.Parse(JsonConvert.SerializeObject(output));
        }
    }
}
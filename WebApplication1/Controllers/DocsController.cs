using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace UngDungBanHang.Api.Controllers
{
    [ApiController]
    [Route("/api/query/[controller]")]
    public class DocsController : ControllerBase
    {
        [HttpPost]
        public JsonNode Index([FromBody] JsonObject index)
        {
            Docs_Input input = JsonConvert.DeserializeObject<Docs_Input>(index.ToString());
            Docs_Output output = new Docs_Output();
            output.Query_DataInput(input);
            return JsonObject.Parse(JsonConvert.SerializeObject(output));
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json.Nodes;

namespace UngDungBanHang.Api.Controllers
{
    [ApiController]
    [Route("/api/cmd/[controller]")]
    public class createController : BaseController
    {
        [Authorize]
        [HttpPost]
        public JsonNode Index([FromBody] JsonObject index)
        {
            //string Code = this.User.FindFirst("Code").Value;
            //Console.WriteLine(userId);
            Create_Output output = new Create_Output();
            if (checkAuth())
            {
                Create_Input input = JsonConvert.DeserializeObject<Create_Input>(index.ToString());
                output.Query_DataInput(input);
                return JsonObject.Parse(JsonConvert.SerializeObject(output));
            }
            output.Success = false;
            output.Message = "Token không còn hợp lệ!";
            return JsonObject.Parse(JsonConvert.SerializeObject(output));
        }
    }
}

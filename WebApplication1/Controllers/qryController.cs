using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Principal;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace UngDungBanHang.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("/api/[controller]")]
    public class qryController : BaseController
    {
        [HttpPost("docs")]
        public async Task<JsonNode> Docs([FromBody] JsonObject index)
        {
            //string Code = this.User.FindFirst("Code").Value;
            //Console.WriteLine(userId);
            Docs_Output output = new Docs_Output();
            if (await checkAuth())
            {
                Docs_Input input = JsonConvert.DeserializeObject<Docs_Input>(index.ToString());
                output.Query_DataInput(input);
                return JsonObject.Parse(JsonConvert.SerializeObject(output));
            }
            output.Success = false;
            output.Message = "Token không còn hợp lệ!";
            return JsonObject.Parse(JsonConvert.SerializeObject(output));
        }
        [HttpGet("doc/{ApplicationCode}/{id}")]
        public async Task<JsonNode> Doc(string ApplicationCode, string id)
        {
            //string Code = this.User.ToString();
            //Console.WriteLine(Code);
            Doc_Output output = new Doc_Output();
            if (await checkAuth())
            {
                Doc_Input input = new Doc_Input
                {
                    ApplicationCode = ApplicationCode,
                    id = id,
                };
                output.Query_DataInput(input);
                return JsonObject.Parse(JsonConvert.SerializeObject(output));
            }
            output.Success = false;
            output.Message = "Token không còn hợp lệ!";
            return JsonObject.Parse(JsonConvert.SerializeObject(output));
        }
    }
}

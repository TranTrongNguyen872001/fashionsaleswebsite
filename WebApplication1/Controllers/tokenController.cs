using Microsoft.AspNetCore.Authorization;
<<<<<<<< HEAD:WebApplication1/Controllers/tokenController.cs
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
========
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Principal;
>>>>>>>> f5a84a63b2d5f8c292f6acc725618ac1a288e009:WebApplication1/Controllers/DocsController.cs
using System.Text.Json.Nodes;

namespace UngDungBanHang.Api.Controllers
{
    [ApiController]
<<<<<<<< HEAD:WebApplication1/Controllers/tokenController.cs
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
========
    [Route("/api/qry/[controller]")]
    public class docsController : BaseController
    {
        [Authorize]
        [HttpPost]
        public JsonNode Index([FromBody] JsonObject index)
        {
            //string Code = this.User.FindFirst("Code").Value;
            //Console.WriteLine(userId);
            Docs_Output output = new Docs_Output();
            if (checkAuth())
            {
                Docs_Input input = JsonConvert.DeserializeObject<Docs_Input>(index.ToString());
                output.Query_DataInput(input);
                return JsonObject.Parse(JsonConvert.SerializeObject(output));
            }
            output.Success = false;
            output.Message = "Token không còn hợp lệ!";
>>>>>>>> f5a84a63b2d5f8c292f6acc725618ac1a288e009:WebApplication1/Controllers/DocsController.cs
            return JsonObject.Parse(JsonConvert.SerializeObject(output));
        }
    }
}

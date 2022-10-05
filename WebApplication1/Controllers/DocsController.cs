﻿using Microsoft.AspNetCore.Authorization;
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
            return JsonObject.Parse(JsonConvert.SerializeObject(output));
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json.Nodes;

namespace UngDungBanHang.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("/api/[controller]")]
    public class cmdController : BaseController
    {
        static public void create(Dictionary<string, object> DataSourse, string ApplicationCode, string? parentID = null)
        {
            string newid = Conection.NewId();
            string key = "";
            string values = "";
            foreach (KeyValuePair<string, object> field in DataSourse.ToList())
            {
                if (!field.Value.GetType().IsArray)
                {
                    key += "[" + field.Key + "],";
                    if (field.Value.GetType() == Type.GetType("System.String"))
                    {
                        values += "'" + field.Value + "',";
                    }
                    else
                    {
                        values += field.Value + ",";
                    }
                }
                else
                {
                    var json = JsonConvert.SerializeObject(field.Value);
                    List<Dictionary<string, object>> dictionary = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(json);
                    foreach (Dictionary<string, object> Item in dictionary)
                    {
                        create(Item, field.Key, newid);
                    }
                }
            }
            if(parentID == null)
            {
                key += "[ID]";
                values += "'" + newid + "'";
            }
            else
            {
                key += "[ID], [ParentID]";
                values += "'" + newid + "', '" + parentID + "'";
            }
            string sql = "insert into " + ApplicationCode + "(" + key + ") values (" + values + ")";
            Conection.ExecuteNonQuery(sql);
        }
        [HttpPost("create")]
        public async Task<JsonNode> create([FromBody] JsonObject index)
        {
            //string Code = this.User.FindFirst("Code").Value;
            //Console.WriteLine(userId);
            Create_Output output = new Create_Output();
            if (await checkAuth())
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

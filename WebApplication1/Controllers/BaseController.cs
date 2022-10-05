using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json.Nodes;

namespace UngDungBanHang.Api.Controllers
{
    public class BaseController : ControllerBase
    {
        protected bool checkAuth()
        {
            string Name = this.User.FindFirst("Name").Value;
            string Code = this.User.FindFirst("Code").Value;
            Conection con = new Conection();
            string QueryStringData = "select [IDUser], [ID] from LoginHistory where IDUser = '" + Name + "'";
            con.QueryToObject(QueryStringData);
            string temp = con.Sourse[0].ToList<KeyValuePair<string, object>>()[0].Value.ToString() + con.Sourse[0].ToList<KeyValuePair<string, object>>()[1].Value.ToString();
            if (Code == Converter.MD5Convert(temp))
            {
                return true;
            }
            return false;
        }
    }
}

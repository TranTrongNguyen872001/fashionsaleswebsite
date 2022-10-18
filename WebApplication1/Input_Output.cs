using Castle.Components.DictionaryAdapter;
using Microsoft.IdentityModel.Tokens;
using MySqlX.XDevAPI.Relational;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.X509;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace UngDungBanHang.Api
{
    public abstract class Input
    {
    }
    public abstract class Output
    {
        public bool Success = true;
        public string Message = "";
        public abstract void Query_DataInput(Input input);
    }
    public class Docs_Input : Input
    {
        public String ApplicationCode = "";
        public int pageIndex = 0;
        public int pageSize = 0;
    }
    public class Docs_Output : Output 
    {
        public List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
        public int TotelItemCount = 0;
        public override void Query_DataInput(Input ip)
        {
            Docs_Input input = (Docs_Input)ip;
            Conection con = new Conection();
            String QueryStringData = "select * from (Select *, ROW_NUMBER() OVER(ORDER BY id) as row$ from " + input.ApplicationCode + " ) as a";
            try
            {
                con.QueryToObject(QueryStringData);
                this.data = con.Sourse.GetRange(input.pageIndex, input.pageSize <= 0 ? con.Sourse.Count : input.pageSize);
                this.TotelItemCount = con.Sourse.Count;
            }
            catch (Exception e)
            {
                this.Success = false;
                this.Message = e.Message;
            }
        }
    }
    public class Doc_Input : Input
    {
        public string ApplicationCode = "";
        public string id = "";
    }
    public class Doc_Output : Output
    {
        public Dictionary<string, object> data = new Dictionary<string, object>();
        public override void Query_DataInput(Input ip)
        {
            Doc_Input input = (Doc_Input)ip;
            Conection con = new Conection();
            String QueryStringData = "select * from " + input.ApplicationCode + " where ID = '" + input.id + "'";
            try
            {
                con.QueryToObject(QueryStringData);
                this.data = con.Sourse.FirstOrDefault();
            }
            catch (Exception e)
            {
                this.Success = false;
                this.Message = e.Message;
            }
        }
    }
    public class Token_Input : Input
    {
        static public string key = "fashionsaleswebsite2022";
        public string Username = "";
        public string Password = "";
    }
    public class Token_Output : Output
    {
        public string Token = "";
        void ResetToken(string Name)
        {
            string sql = "delete from LoginHistory where [IDUser] = '" + Name + "' insert into LoginHistory([ID], [IDUser]) values ((select newid()), '" + Name + "')";
            Conection.ExecuteNonQuery(sql);
        }
        public override void Query_DataInput(Input ip)
        {
            Token_Input input = (Token_Input)ip;
            Conection con = new Conection();
            try
            {
                string QueryStringData = "select ID from UserManager where Username = N'" + input.Username + "' and Password = N'" + input.Password + "'";
                con.QueryToObject(QueryStringData);
                if (con.Sourse.Count == 0)
                {
                    this.Message = "Đăng nhập không hợp lệ!";
                    this.Success = false;
                }
                else
                {
                    var key = Encoding.ASCII.GetBytes(Token_Input.key);
                    string Name = con.Sourse[0].ToList<KeyValuePair<string, object>>()[0].Value.ToString();
                    ResetToken(Name);
                    QueryStringData = "select ID from LoginHistory where [IDUser] = '" + Name + "'";
                    con.QueryToObject(QueryStringData);
                    string ID = con.Sourse[0].ToList<KeyValuePair<string, object>>()[0].Value.ToString();
                    JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                    SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new Claim[]
                        {
                        new Claim("Name", Name),
                        new Claim("Code", Converter.MD5Convert(Name + ID))
                        }),
                        Expires = DateTime.Now.AddMinutes(5),
                        SigningCredentials = new SigningCredentials(
                            new SymmetricSecurityKey(key),
                            SecurityAlgorithms.HmacSha256Signature)
                    };
                    SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
                    this.Token = tokenHandler.WriteToken(token);
                }
            }
            catch(Exception e)
            {
                this.Success = false;
                this.Message = e.Message;
            }
        }
    }
    public class Create_Input : Input 
    {
        public Dictionary<string, object> DataSourse = new Dictionary<string, object>();
        public string ApplicationCode = "";
    }
    public class Create_Output : Output
    {
        public string DocumentId = "";
        public override void Query_DataInput(Input ip)
        {
            Create_Input input = (Create_Input)ip;
            try
            {
                string key = "";
                string values = "";
                foreach (KeyValuePair<string,object> field in input.DataSourse.ToList())
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
                key += "[ID]";
                values += "(select newid())";
                string sql = "insert into " + input.ApplicationCode + "(" + key + ") values (" + values + ")";
                Conection.ExecuteNonQuery(sql);
            }
            catch(Exception e)
            {
                this.Success = false;
                this.Message = e.Message;
            }
        }
    }
}

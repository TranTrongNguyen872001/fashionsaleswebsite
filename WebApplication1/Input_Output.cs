using MySqlX.XDevAPI.Relational;
using Org.BouncyCastle.Asn1.X509;
using System.Collections.Generic;
using System.Data;
using System.Text.Json;

namespace UngDungBanHang.Api
{
    public class Docs_Input
    {
        public String ApplicationCode = "";
        public int pageIndex = 0;
        public int pageSize = 0;
    }
    public class Docs_Output
    {
        public List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
        public int TotelItemCount = 0;
        public bool Success = true;
        public void Query_DataInput(Docs_Input input)
        {
            Conection_Converter con = new Conection_Converter();
            String QueryStringData = input.pageSize <= 0 ?
                "select * from (Select *, ROW_NUMBER() OVER(ORDER BY id) as row$ from " + input.ApplicationCode + " ) as a where row$ >= " + input.pageIndex :
                "select * from (Select *, ROW_NUMBER() OVER(ORDER BY id) as row$ from " + input.ApplicationCode + " ) as a where row$ BETWEEN " + input.pageIndex + " and " + (input.pageSize + input.pageIndex - 1);
            try
            {
                con.QueryToObject(QueryStringData);
                this.data = con.Sourse;
                this.TotelItemCount = con.Sourse.Count;
            }
            catch
            {
                this.Success = false;
            }
        }
    }
}

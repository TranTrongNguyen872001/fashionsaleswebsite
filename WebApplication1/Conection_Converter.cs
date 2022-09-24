using Castle.Core;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Specialized;
using System.Data;

namespace UngDungBanHang.Api
{
    public class Conection_Converter
    {
        private const string connStr = @"Data Source=THANHVINH-PC\KAITOKIDS;Initial Catalog=BanHang;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        public List<Dictionary<string, object>> Sourse = new List<Dictionary<string, object>>();
        public void QueryToObject(String sql)
        {
            DataTable dtCountry = new DataTable();
            List<Dictionary<string, object>> obj = new List<Dictionary<string, object>>();
            try
            {
                SqlConnection conn = new SqlConnection(connStr);
                conn.Open();
                SqlDataAdapter daCountry = new SqlDataAdapter(sql, conn);
                daCountry.Fill(dtCountry);
                conn.Close();
                foreach (DataRow row in dtCountry.Rows)
                {
                    Dictionary<string, object> temp = new Dictionary<string, object>();
                    foreach (DataColumn col in dtCountry.Columns)
                    {
                        temp.Add(col.ColumnName, row[col.ColumnName]);
                    }
                    obj.Add(temp);
                }
                this.Sourse = obj;
            }
            catch
            {
                return;
            }
        }
    }
}

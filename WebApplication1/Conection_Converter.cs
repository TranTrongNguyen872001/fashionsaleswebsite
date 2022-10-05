using Castle.Core;
using Microsoft.Data.SqlClient;
using System;
using System.Collections;
using System.Collections.Specialized;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace UngDungBanHang.Api
{
    public class Conection
    {
        private const string connStr = @"Data Source=THANHVINH-PC\KAITOKIDS;Initial Catalog=BanHang;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=false";
        public List<Dictionary<string, object>> Sourse = new List<Dictionary<string, object>>();
        public void QueryToObject(String sql)
        {
            DataTable dtCountry = new DataTable();
            List<Dictionary<string, object>> obj = new List<Dictionary<string, object>>();
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
        static public void ExecuteNonQuery(string str)
        {
            SqlConnection conn = new SqlConnection(connStr);
            conn.Open();
            using (var command = new SqlCommand(str, conn))
            {
                command.ExecuteNonQuery();
            }
            conn.Close();
        }
    }
    public class Converter
    {
        static public string MD5Convert(string str)
        {
            //Tạo MD5 
            MD5 mh = MD5.Create();
            //Chuyển kiểu chuổi thành kiểu byte
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(str);
            //mã hóa chuỗi đã chuyển
            byte[] hash = mh.ComputeHash(inputBytes);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }
    }
}

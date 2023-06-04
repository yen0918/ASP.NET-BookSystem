using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;


namespace WorkShop4.Models
{
    public class DropDownListData
    {
        /// <summary>
        /// 取得DB連線字串
        /// </summary>
        /// <returns></returns>
        private string GetDBConnectionString()
        {
            return
                System.Configuration.ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString.ToString();
        }
        public List<SelectListItem> GetUserEname()
        {
            List<SelectListItem> result = new List<SelectListItem>();
            DataTable dt = new DataTable();
            string sql = @"SELECT USER_ENAME, USER_ID, USER_CNAME
                           FROM dbo.MEMBER_M";
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dt);
                conn.Close();
            }
            foreach (DataRow row in dt.Rows)
            {
                result.Add(new SelectListItem()
                {
                    Text = row["USER_ENAME"].ToString()+"-"+row["USER_CNAME"].ToString(),
                    Value = row["USER_ID"].ToString()
                });
            }
            return result;
        }

        public List<SelectListItem> GetCodeName()
        {
            List<SelectListItem> result = new List<SelectListItem>();
            DataTable dt = new DataTable();
            string sql = @"SELECT CODE_NAME, CODE_ID
                           FROM dbo.BOOK_CODE 
                           WHERE CODE_TYPE = 'BOOK_STATUS';";
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);

                sqlAdapter.Fill(dt);
                conn.Close();
            }
            foreach (DataRow row in dt.Rows)
            {
                result.Add(new SelectListItem()
                {
                    Text = row["CODE_NAME"].ToString(),
                    Value = row["CODE_ID"].ToString()
                });
            }
            return result;
        }

        public List<SelectListItem> GetBookClassName()
        {
            List<SelectListItem> result = new List<SelectListItem>();
            DataTable dt = new DataTable();
            string sql = @"Select BOOK_CLASS_NAME, BOOK_CLASS_ID
                           FROM dbo.BOOK_CLASS";
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);

                sqlAdapter.Fill(dt);
                conn.Close();
            }
            foreach (DataRow row in dt.Rows)
            {
                result.Add(new SelectListItem()
                {
                    Text = row["BOOK_CLASS_NAME"].ToString(),
                    Value = row["BOOK_CLASS_ID"].ToString()
                });
            }
            return result;
        }
    }
}
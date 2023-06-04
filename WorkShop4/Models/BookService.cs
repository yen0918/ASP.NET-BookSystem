using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace WorkShop4.Models
{
    //Transaction要有連貫性才用!!!(ex:更新書籍資料後新增一筆借閱紀錄 更新~新增)
    public class BookService
    {
        //取得連線字串
        private string GetDBConnectionString()
        {
            return
                System.Configuration.ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString.ToString();
        }

        /// <summary>
        /// 取得書籍資料
        /// </summary>
        /// <param name="booklendrecord"></param>
        /// <returns></returns>
        public List<Models.BookLendRecord> GetBookLendRecord(Models.BookLendRecord booklendrecord)
        {
            //宣告datatable物件
            DataTable dt = new DataTable();
            //Sql語法
            string sql = @"SELECT BOOK_CLASS.BOOK_CLASS_NAME, b.BOOK_NAME,CONVERT(VARCHAR, b.BOOK_BOUGHT_DATE, 111) AS BOOK_BOUGHT_DATE, BOOK_CODE.CODE_NAME, MEMBER_M.USER_ENAME, BOOK_ID
                           FROM　dbo.BOOK_DATA  AS b
                           LEFT JOIN dbo.BOOK_CLASS ON dbo.BOOK_CLASS.BOOK_CLASS_ID = b.BOOK_CLASS_ID
                           LEFT JOIN dbo.BOOK_CODE ON dbo.BOOK_CODE.CODE_ID = b.BOOK_STATUS
                           LEFT JOIN dbo.MEMBER_M ON dbo.MEMBER_M.USER_ID = b.BOOK_KEEPER
                           WHERE (dbo.BOOK_CODE.CODE_TYPE = 'BOOK_STATUS') AND
                                 (UPPER(b.BOOK_NAME)LIKE('%' + @BOOK_NAME + '%') OR @BOOK_NAME = '') AND
                                 (dbo.BOOK_CLASS.BOOK_CLASS_ID = @BOOK_CLASS_ID OR @BOOK_CLASS_ID = '') AND
                                 (dbo.MEMBER_M.USER_ID = @USER_ID OR @USER_ID = '') AND 
                                 (dbo.BOOK_CODE.CODE_ID = @CODE_ID OR @CODE_ID = '')
                           ORDER BY BOOK_BOUGHT_DATE DESC;";
            //Sql連線
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                //開啟連線
                conn.Open();
                //宣告SqlCommand物件
                SqlCommand cmd = new SqlCommand(sql, conn);
                //指定參數(某些欄位非必填若null則為空值)
                cmd.Parameters.Add(new SqlParameter("@BOOK_NAME", booklendrecord.BOOK_NAME == null ? string.Empty : booklendrecord.BOOK_NAME));
                cmd.Parameters.Add(new SqlParameter("@BOOK_CLASS_ID", booklendrecord.BOOK_CLASS_ID == null ? string.Empty : booklendrecord.BOOK_CLASS_ID));
                cmd.Parameters.Add(new SqlParameter("@USER_ID", booklendrecord.USER_ID == null ? string.Empty : booklendrecord.USER_ID));
                cmd.Parameters.Add(new SqlParameter("@CODE_ID", booklendrecord.CODE_ID == null ? string.Empty : booklendrecord.CODE_ID));
                cmd.Parameters.Add(new SqlParameter("@BOOK_BOUGHT_DATE", booklendrecord.BOOK_BOUGHT_DATE == null ? string.Empty : booklendrecord.BOOK_BOUGHT_DATE));
                cmd.Parameters.Add(new SqlParameter("@BOOK_ID", booklendrecord.BOOK_ID == null ? string.Empty : booklendrecord.BOOK_ID));
                //宣告一個SqlDataAdapter並傳入SqlCommand物件
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                //填入資料
                sqlAdapter.Fill(dt);
                //關閉連線
                conn.Close();
            }
            //將資料塞進List回傳
            return this.MapBookLendRecordToList(dt);
        }

        /// <summary>
        /// 接書籍資料的List
        /// </summary>
        /// <param name="booklendData"></param>
        /// <returns></returns>
        private List<Models.BookLendRecord> MapBookLendRecordToList(DataTable booklendData)
        {
            List<Models.BookLendRecord> result = new List<BookLendRecord>();
            foreach (DataRow row in booklendData.Rows)
            {
                result.Add(new BookLendRecord()
                {
                    BOOK_ID = row["BOOK_ID"].ToString(),
                    BOOK_NAME = row["BOOK_NAME"].ToString(),
                    BOOK_CLASS_NAME = row["BOOK_CLASS_NAME"].ToString(),
                    USER_ENAME = row["USER_ENAME"].ToString(),
                    CODE_NAME = row["CODE_NAME"].ToString(),
                    BOOK_BOUGHT_DATE = row["BOOK_BOUGHT_DATE"].ToString()
                });
            }
            return result;
        }

        /// <summary>
        /// 新增書籍資料
        /// </summary>
        /// <param name="insertbookdata"></param>
        /// <returns></returns>
        public int InsertBook(Models.InsertBookData insertbookdata)
        {
            //SQL語法，若資料表有必填欄位一定要給值
            string sql = @"INSERT INTO BOOK_DATA
                           (
                            BOOK_NAME, BOOK_AUTHOR, BOOK_BOUGHT_DATE, BOOK_PUBLISHER, BOOK_NOTE, BOOK_STATUS, BOOK_CLASS_ID, CREATE_DATE
                           )
                           VALUES
                           (
                            @BOOK_NAME, @BOOK_AUTHOR, @BOOK_BOUGHT_DATE, @BOOK_PUBLISHER, @BOOK_NOTE, 'A', @BOOK_CLASS_ID, GETDATE()
                           )
                            SELECT SCOPE_IDENTITY()";
            //BOOK_ID欄位為PK，要給流水號---SCOPE_IDENTITY傳回插入相同範圍之識別欄位中的最後一個識別值
            int BOOK_ID;
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                //從conn物件啟用Transaction
                SqlTransaction tran = conn.BeginTransaction();
                //把Transaction指給Command物件
                cmd.Transaction = tran;
                try
                {
                    cmd.Parameters.Add(new SqlParameter("@BOOK_NAME", insertbookdata.BOOK_NAME));
                    cmd.Parameters.Add(new SqlParameter("@BOOK_AUTHOR", insertbookdata.BOOK_AUTHOR));
                    cmd.Parameters.Add(new SqlParameter("@BOOK_BOUGHT_DATE", insertbookdata.BOOK_BOUGHT_DATE));
                    cmd.Parameters.Add(new SqlParameter("@BOOK_PUBLISHER", insertbookdata.BOOK_PUBLISHER));
                    cmd.Parameters.Add(new SqlParameter("@BOOK_NOTE", insertbookdata.BOOK_NOTE));
                    cmd.Parameters.Add(new SqlParameter("@BOOK_CLASS_ID", insertbookdata.BOOK_CLASS_ID));
                    //執行查詢，並傳回查詢所傳回之結果集中第一個資料列的第一個資料行(給值)
                    BOOK_ID = Convert.ToInt32(cmd.ExecuteScalar());
                    //成功Commit
                    tran.Commit();
                }
                catch (Exception)
                {
                    //失敗則恢復
                    tran.Rollback();
                    throw;
                }
                finally
                {
                    conn.Close();
                }
            }
            //回傳BOOK_ID
            return BOOK_ID;
        }

        /// <summary>
        /// 刪除書籍資料
        /// </summary>
        /// <param name="bookid"></param>
        /// <returns></returns>
        public string DeleteBookById(string bookid)
        {
            string sql = @"DELETE FROM BOOK_DATA WHERE BOOK_ID = @BOOK_ID";
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlTransaction tran = conn.BeginTransaction();
                cmd.Transaction = tran;
                try
                {
                    cmd.Parameters.Add(new SqlParameter("@BOOK_ID", bookid));
                    //執行使資料變動時傳回受影響的資料列數目
                    cmd.ExecuteNonQuery();
                    tran.Commit();
                    return "刪除成功！";
                }
                catch (Exception)
                {
                    return "刪除無效！";
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        /// <summary>
        /// 取得單個選取的資料填入頁面欄位
        /// </summary>
        /// <param name="bookId"></param>
        /// <returns></returns>
        public List<Models.UpdateBookData> GetBookUpadateData(string bookId)
        {
            List<Models.UpdateBookData> result = new List<Models.UpdateBookData>();
            DataTable dt = new DataTable();
            string sql = @"SELECT BOOK_NAME, BOOK_AUTHOR, BOOK_PUBLISHER, BOOK_NOTE, BOOK_BOUGHT_DATE, 
                                  BOOK_CLASS_ID, BOOK_CODE.CODE_ID, MEMBER_M.USER_ID, BOOK_ID
                           FROM BOOK_DATA AS b
                           LEFT JOIN dbo.BOOK_CODE ON dbo.BOOK_CODE.CODE_ID = b.BOOK_STATUS
                           FULL OUTER JOIN dbo.MEMBER_M ON dbo.MEMBER_M.USER_ID = b.BOOK_KEEPER
                           WHERE dbo.BOOK_CODE.CODE_TYPE = 'BOOK_STATUS' AND BOOK_ID = @BOOK_ID";
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@BOOK_ID", bookId));
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dt);
                conn.Close();
            }
            foreach (DataRow row in dt.Rows)
            {
                //加入List
                result.Add(new Models.UpdateBookData()
                {
                    BOOK_NAME = row["BOOK_NAME"].ToString(),
                    BOOK_AUTHOR = row["BOOK_AUTHOR"].ToString(),
                    BOOK_PUBLISHER = row["BOOK_PUBLISHER"].ToString(),
                    BOOK_NOTE = row["BOOK_NOTE"].ToString(),
                    BOOK_BOUGHT_DATE = (DateTime)row["BOOK_BOUGHT_DATE"],
                    BOOK_CLASS_ID = row["BOOK_CLASS_ID"].ToString(),
                    CODE_ID = row["CODE_ID"].ToString(),
                    USER_ID = row["USER_ID"].ToString(),
                    BOOK_ID = row["BOOK_ID"].ToString()
                });
            }
                //回傳結果
                return result;
        }

        /// <summary>
        /// 更新書籍資料
        /// </summary>
        /// <param name="updatebookdata"></param>
        public void UpdateBook(Models.UpdateBookData updatebookdata)
        {
            string sql = @"UPDATE BOOK_DATA SET BOOK_NAME = @BOOK_NAME, BOOK_AUTHOR = @BOOK_AUTHOR,
                                  BOOK_PUBLISHER = @BOOK_PUBLISHER, BOOK_NOTE = @BOOK_NOTE,
                                  BOOK_BOUGHT_DATE = @BOOK_BOUGHT_DATE, BOOK_CLASS_ID = @BOOK_CLASS_ID,
                                  BOOK_STATUS = @BOOK_STATUS, BOOK_KEEPER = @BOOK_KEEPER
                                  WHERE BOOK_ID = @BOOK_ID";
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlTransaction tran = conn.BeginTransaction();
                cmd.Transaction = tran;
                try
                {
                    cmd.Parameters.Add(new SqlParameter("@BOOK_NAME", updatebookdata.BOOK_NAME));
                    cmd.Parameters.Add(new SqlParameter("@BOOK_AUTHOR", updatebookdata.BOOK_AUTHOR));
                    cmd.Parameters.Add(new SqlParameter("@BOOK_PUBLISHER", updatebookdata.BOOK_PUBLISHER));
                    cmd.Parameters.Add(new SqlParameter("@BOOK_NOTE", updatebookdata.BOOK_NOTE));
                    cmd.Parameters.Add(new SqlParameter("@BOOK_BOUGHT_DATE", updatebookdata.BOOK_BOUGHT_DATE));
                    cmd.Parameters.Add(new SqlParameter("@BOOK_CLASS_ID", updatebookdata.BOOK_CLASS_ID));
                    cmd.Parameters.Add(new SqlParameter("@BOOK_STATUS", updatebookdata.CODE_ID));
                    cmd.Parameters.Add(new SqlParameter("@BOOK_KEEPER", updatebookdata.USER_ID == null ? string.Empty : updatebookdata.USER_ID));
                    cmd.Parameters.Add(new SqlParameter("@BOOK_ID", updatebookdata.BOOK_ID));
                    cmd.ExecuteNonQuery();
                    tran.Commit();
                }
                catch (Exception)
                {
                    tran.Rollback();
                    throw;
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        /// <summary>
        /// 新增借閱資料
        /// </summary>
        /// <param name="lendrecord"></param>
        /// <param name="userId"></param>
        /// <param name="bookId"></param>
        /// <returns></returns>
        public int UpdateBookAndInsertRecord(Models.LendRecord lendrecord, string userId, string bookId)
        {
            string sql = @"INSERT INTO BOOK_LEND_RECORD
                           (
                            LEND_DATE, KEEPER_ID, BOOK_ID
                           )
                           VALUES
                           (
                            GETDATE(), @KEEPER_ID, @BOOK_ID
                           )
                           SELECT SCOPE_IDENTITY()";
            //流水號
            int IDENTITY_FILED;
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlTransaction tran = conn.BeginTransaction();
                cmd.Transaction = tran;
                try
                {
                    cmd.Parameters.Add(new SqlParameter("@KEEPER_ID", userId));
                    cmd.Parameters.Add(new SqlParameter("@BOOK_ID", bookId));
                    IDENTITY_FILED = Convert.ToInt32(cmd.ExecuteScalar());
                    tran.Commit();
                }
                catch (Exception)
                {
                    tran.Rollback();
                    throw;
                }
                finally
                {
                    conn.Close();
                }
                return IDENTITY_FILED;
            }
        }

        /// <summary>
        /// 取得書本借閱紀錄資料
        /// </summary>
        /// <param name="bookId"></param>
        /// <returns></returns>
        public List<Models.LendRecord> LendRecord(string bookId)
        {
            List<Models.LendRecord> result = new List<Models.LendRecord>();
            DataTable dt = new DataTable();
            string sql = @"SELECT CONVERT(VARCHAR, LEND_DATE, 111) AS LEND_DATE, KEEPER_ID, MEMBER_M.USER_ENAME, MEMBER_M.USER_CNAME
                           FROM BOOK_LEND_RECORD
                           INNER JOIN MEMBER_M ON MEMBER_M.USER_ID = BOOK_LEND_RECORD.KEEPER_ID
                           WHERE BOOK_ID = @BOOK_ID";
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.Add(new SqlParameter("@BOOK_ID", bookId));
                    SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                    sqlAdapter.Fill(dt);
                    conn.Close();
            }
            foreach (DataRow row in dt.Rows)
            {
                result.Add(new Models.LendRecord()
                {
                    LEND_DATE = row["LEND_DATE"].ToString(),
                    KEEPER_ID = row["KEEPER_ID"].ToString(),
                    USER_ENAME = row["USER_ENAME"].ToString(),
                    USER_CNAME = row["USER_CNAME"].ToString()
                });
            }
            return result;
        }

    }
}
using System;
using System.Web.Mvc;
using WorkShop4.Model;
using WorkShop4.Service;
using WorkShop4.Common;



namespace WorkShop4.Controllers
{

    public class BookController : Controller
    {
        public IBookService bookService { get; set; }
        public IDropDownListData dropDownListData { get; set; }

        /// <summary>
        /// 3個下拉選單
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        public JsonResult GetBookClassName()
        {
            var result = dropDownListData.GetBookClassName();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet()]
        public JsonResult GetCodeName()
        {
            var result = this.dropDownListData.GetCodeName();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet()]
        public JsonResult GetUserName()
        {
            var result = this.dropDownListData.GetUserEname();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 首頁
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 查詢後之首頁
        /// </summary>
        /// <param name="booklendrecord"></param>
        /// <returns></returns>
        [HttpPost()]
        public JsonResult Index(BookLendRecord booklendrecord)
        {

            var result = bookService.GetBookLendRecord(booklendrecord);
            //將獲取資料透過json回傳
            return this.Json(result);
        }

        /// <summary>
        /// 新增頁面
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        public ActionResult InsertBook()
        {
            return View();
        }

        /// <summary>
        /// 進行新增
        /// </summary>
        /// <param name="insertbookdata"></param>
        /// <returns></returns>
        [HttpPost()]
        public JsonResult InsertBook(InsertBookData insertbookdata)
        {
            bookService.InsertBook(insertbookdata);
            return this.Json(insertbookdata);
        }

        /// <summary>
        /// 進行刪除
        /// </summary>
        /// <param name="bookId"></param>
        /// <returns></returns>
        [HttpPost()]
        public JsonResult DeleteBook(string bookId)
        {
            //透過bookid取得要刪除的那筆資料
            var data = bookService.GetBookUpadateData(bookId)[0];
            //進行借閱狀態判斷(已借出的書不能刪)
            if (data.CODE_ID != "B" && data.CODE_ID != "C")
            {
                var result = bookService.DeleteBookById(bookId);
                //回傳結果
                return this.Json(result);
            }
            else
            {
                return this.Json("書已借出，無法刪除！");
            }
        }

        /// <summary>
        /// 取得單一書本資料
        /// </summary>
        /// <param name="bookId"></param>
        /// <returns></returns>
        [HttpPost()]
        public JsonResult GetABook(string bookId)
        {
            try
            {
                //取得該書原始資料
                var result = bookService.GetBookUpadateData(bookId)[0];
                return this.Json(result, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                Logger.Write(Logger.LogCategoryEnum.Error, ex.ToString());
                return this.Json(ex);//匯出是
            }
        }

        /// <summary>
        /// 修改頁面，若bookId為不存在的值則跳到錯誤頁面
        /// </summary>
        /// <param name="bookId"></param>
        /// <returns></returns>
        [HttpGet()]
        public ActionResult UpdateBook(string bookId)
        {
                return View();
        }

            /// <summary>
            /// 進行編輯(考慮到已借出需新增資料至借閱紀錄)
            /// </summary>
            /// <param name="bookId"></param>
            /// <param name="updatebookdata"></param>
            /// <returns></returns>
            [HttpPost()]
        public JsonResult UpdateBook(string bookId, UpdateBookData updatebookdata)
        {
            //取得bookid
            updatebookdata.BOOK_ID = bookId;
            //取得進入編輯畫面各欄位初始值(用於判斷借閱狀態與借閱人有無發生改變)
            var result = bookService.GetBookUpadateData(bookId)[0];
            //必填驗證
            if (ModelState.IsValid)
            {
                //修改資料
                bookService.UpdateBook(updatebookdata);

                //取得修改完成後各欄位資料(與初始值進行比較)
                var updateResult = bookService.GetBookUpadateData(bookId)[0];

                //若一開始進入頁面的狀態為可以借出(A)或不可借出(U)
                //且更改後狀態為已借出或已借出(未領)，就新增借閱紀錄資料
                if (result.CODE_ID == "A" || result.CODE_ID == "U")
                {
                    if (updateResult.CODE_ID == "B" || updateResult.CODE_ID == "C")
                    {
                        LendRecord lendRecord = new LendRecord();
                        bookService.UpdateBookAndInsertRecord(lendRecord, updatebookdata.USER_ID, updatebookdata.BOOK_ID);
                    }
                }
                //若一開始進入頁面的狀態為已借出或已借出(未領)，
                //且更新後的狀態不為可借出與不可借出，若借閱人不同則新增借閱紀錄
                else
                {
                    if (updateResult.CODE_ID != "A" && updateResult.CODE_ID != "U" && updateResult.USER_ID != result.USER_ID)
                    {
                        LendRecord lendRecord = new LendRecord();
                        bookService.UpdateBookAndInsertRecord(lendRecord, updatebookdata.USER_ID, updatebookdata.BOOK_ID);
                    }
                }
            }
            return this.Json(updatebookdata);
        }

        /// <summary>
        /// 明細頁面
        /// </summary>
        /// <param name="bookId"></param>
        /// <returns></returns>
        [HttpGet()]
        public ActionResult Detail(string bookId)
        {
            //若查無資料將接到錯誤頁面
            if (bookService.GetBookUpadateData(bookId).Count != 1)
            {
                return View("Error");
            }
            else
            {
                //取得該書原始資料
                var result = bookService.GetBookUpadateData(bookId)[0];
                return View("UpdateBook", result);
            }
        }

        /// <summary>
        /// 借閱紀錄頁面
        /// </summary>
        /// <param name="bookId"></param>
        /// <returns></returns>

        [HttpGet()]
        public ActionResult LendRecord()
        {
            return View();
        }

        /// <summary>
        /// 取得借閱紀錄
        /// </summary>
        /// <param name="bookId"></param>
        /// <returns></returns>
        [HttpPost()]
        public JsonResult GetLendRecord(string bookId)
        {
            var result = bookService.LendRecord(bookId);
            return this.Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
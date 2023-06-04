using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkShop4.Model;
using WorkShop4.Dao;
using System.Data;

namespace WorkShop4.Service
{
    public class BookService : IBookService
    {
        private Dao.IBookServiceDao bookServiceDao { get; set; }

        public List<BookLendRecord> GetBookLendRecord(BookLendRecord booklendrecord)
        {
            return bookServiceDao.GetBookLendRecord(booklendrecord);
        }

        public List<BookLendRecord> MapBookLendRecordToList(DataTable booklendData)
        {
            return bookServiceDao.MapBookLendRecordToList(booklendData);
        }

        public int InsertBook(InsertBookData insertbookdata)
        {
            return bookServiceDao.InsertBook(insertbookdata);
        }

        public string DeleteBookById(string bookId)
        {
            return bookServiceDao.DeleteBookById(bookId);
        }

        public List<UpdateBookData> GetBookUpadateData(string bookId)
        {
            return bookServiceDao.GetBookUpadateData(bookId);
        }

        public void UpdateBook(UpdateBookData updatebookdata)
        {
            bookServiceDao.UpdateBook(updatebookdata);
        }

        public int UpdateBookAndInsertRecord(LendRecord lendrecord, string userId, string bookId)
        {
            return bookServiceDao.UpdateBookAndInsertRecord(lendrecord, userId, bookId);
        }

        public List<LendRecord> LendRecord(string bookId)
        {
            return bookServiceDao.LendRecord(bookId);
        }
    }
}

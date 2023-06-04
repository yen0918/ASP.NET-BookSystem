using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkShop4.Model;

namespace WorkShop4.Dao
{
    public class BookServiceTestDao: IBookServiceDao
    {
        public List<BookLendRecord> GetBookLendRecord(BookLendRecord booklendrecord)
        {

            var result = new List< BookLendRecord> ();
            result.Add(new BookLendRecord
            {
                BOOK_ID = "111",
                BOOK_NAME = "DDD",
                BOOK_CLASS_NAME = "LLL",
                USER_ENAME = "FFF",
                CODE_NAME = "ZZZ",
                BOOK_BOUGHT_DATE = "KKK"
            });

            result.Add(new BookLendRecord
            {
                BOOK_ID = "111",
                BOOK_NAME = "DDD",
                BOOK_CLASS_NAME = "LLL",
                USER_ENAME = "FFF",
                CODE_NAME = "ZZZ",
                BOOK_BOUGHT_DATE = "KKK"
            });

            return result;
        }

        public List<BookLendRecord> MapBookLendRecordToList(DataTable booklendData)
        {
            throw new NotImplementedException();
        }

        public int InsertBook(InsertBookData insertbookdata)
        {
            throw new NotImplementedException();
        }

        public string DeleteBookById(string bookId)
        {
            throw new NotImplementedException();
        }

        public List<UpdateBookData> GetBookUpadateData(string bookId)
        {
            throw new NotImplementedException();
        }

        public void UpdateBook(UpdateBookData updatebookdata)
        {
            throw new NotImplementedException();
        }

        public int UpdateBookAndInsertRecord(LendRecord lendrecord, string userId, string bookId)
        {
            throw new NotImplementedException();
        }

        public List<LendRecord> LendRecord(string bookId)
        {
            throw new NotImplementedException();
        }
    }
}

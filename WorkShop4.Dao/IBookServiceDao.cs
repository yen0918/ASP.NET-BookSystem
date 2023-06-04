using System.Collections.Generic;
using System.Data;
using WorkShop4.Model;

namespace WorkShop4.Dao
{
    public interface IBookServiceDao
    {
        string DeleteBookById(string bookid);
        List<BookLendRecord> GetBookLendRecord(BookLendRecord booklendrecord);
        List<UpdateBookData> GetBookUpadateData(string bookId);
        int InsertBook(InsertBookData insertbookdata);
        List<LendRecord> LendRecord(string bookId);
        List<BookLendRecord> MapBookLendRecordToList(DataTable booklendData);
        void UpdateBook(UpdateBookData updatebookdata);
        int UpdateBookAndInsertRecord(LendRecord lendrecord, string userId, string bookId);
    }
}
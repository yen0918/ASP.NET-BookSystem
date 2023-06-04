using System.Collections.Generic;
using System.Data;
using WorkShop4.Model;

namespace WorkShop4.Service
{
    public interface IBookService
    {
        string DeleteBookById(string bookId);
        List<BookLendRecord> GetBookLendRecord(BookLendRecord booklendrecord);
        List<UpdateBookData> GetBookUpadateData(string bookId);
        int InsertBook(InsertBookData insertbookdata);
        List<LendRecord> LendRecord(string bookId);
        List<BookLendRecord> MapBookLendRecordToList(DataTable booklendData);
        void UpdateBook(UpdateBookData updatebookdata);
        int UpdateBookAndInsertRecord(LendRecord lendrecord, string userId, string bookId);
    }
}
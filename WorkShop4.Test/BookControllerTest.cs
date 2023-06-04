using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WorkShop4.Service;
using WorkShop4.Model;
using WorkShop4.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkShop4.Test
{
    [TestClass]
    public class BookControllerTest
    {
        [TestMethod]
        public void DeleteBookTestStatusA()
        {
            //Arrange

            //bookId設為1
            string bookId = "1";

            //New一個IBookService介面
            Mock<IBookService> mockBookService = new Mock<IBookService>();
            //取得資料並給參數
            mockBookService.Setup(m => m.GetBookUpadateData(It.Is<string>(BookId => BookId == bookId)))
                .Returns(new List<UpdateBookData>
                {
                    new UpdateBookData
                    {
                        //回傳設定的結果(狀態為A時)
                        BOOK_ID = bookId,
                        CODE_ID = "A"
                    }
                });
            //狀態為A時應該跑進Controller的if判斷內並實作刪除且回傳刪除成功
            mockBookService.Setup(m => m.DeleteBookById(It.Is<string>(BookId => BookId == bookId)))
                .Returns("刪除成功！");
            //new一個Controller
            BookController bookController = new BookController();
            //將bookService導到Mock的mockBookService
            bookController.bookService = mockBookService.Object;

            //Act
            //實做回傳結果(Json要加Data)
            var result = bookController.DeleteBook(bookId).Data;

            //Assert
            //驗證回傳結果
            Assert.AreEqual("刪除成功！", result);
        }

        [TestMethod]
        public void DeleteBookTestStatusB()
        {
            //Arrange
            string bookId = "1";
            Mock<IBookService> mockBookService = new Mock<IBookService>();
            mockBookService.Setup(m => m.GetBookUpadateData(It.Is<string>(BookId => BookId == bookId)))
                .Returns(new List<UpdateBookData>
                {
                    new UpdateBookData
                    {
                        //回傳設定的結果(狀態為B時)
                        BOOK_ID = bookId,
                        CODE_ID = "B"
                    }
                });

            //因狀態為B不會進入到Controller之if判斷，因此不執行刪除

            BookController bookController = new BookController
            {
                bookService = mockBookService.Object
            };

            //Act
            var result = bookController.DeleteBook(bookId).Data;

            //Assert
            //驗證回傳結果
            Assert.AreEqual("書已借出，無法刪除！", result);
        }

        [TestMethod]
        public void DeleteBookTestStatusC()
        {
            //Arrange
            string bookId = "1";
            Mock<IBookService> mockBookService = new Mock<IBookService>();
            mockBookService.Setup(m => m.GetBookUpadateData(It.Is<string>(BookId => BookId == bookId)))
                .Returns(new List<UpdateBookData>
                {
                    new UpdateBookData
                    {
                        //回傳設定的結果(狀態為C時)
                        BOOK_ID = bookId,
                        CODE_ID = "C"
                    }
                });
            
            BookController bookController = new BookController();
            bookController.bookService = mockBookService.Object;

            //Act
            var result = bookController.DeleteBook(bookId).Data;

            //Assert
            Assert.AreEqual("書已借出，無法刪除！", result);
        }

        [TestMethod]
        public void DeleteBookTestStatusU()
        {
            //Arrange
            string bookId = "1";
            Mock<IBookService> mockBookService = new Mock<IBookService>();
            mockBookService.Setup(m => m.GetBookUpadateData(It.Is<string>(BookId => BookId == bookId)))
                .Returns(new List<UpdateBookData>
                {
                    new UpdateBookData
                    {
                        //回傳設定的結果(狀態為U時)
                        BOOK_ID = bookId,
                        CODE_ID = "U"
                    }
                });
            mockBookService.Setup(m => m.DeleteBookById(It.Is<string>(BookId => BookId == bookId)))
                .Returns("刪除成功！");
            BookController bookController = new BookController();
            bookController.bookService = mockBookService.Object;

            //Act
            var result = bookController.DeleteBook(bookId).Data;

            //Assert
            Assert.AreEqual("刪除成功！", result);
        }
    }

}










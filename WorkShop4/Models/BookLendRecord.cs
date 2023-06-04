using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WorkShop4.Models
{
    public class BookLendRecord
    {
        [DisplayName("書名")]
        public string BOOK_NAME { get; set; }

        [DisplayName("購書日期")]
        public string BOOK_BOUGHT_DATE { get; set; }

        [DisplayName("圖書類別")]
        public string BOOK_CLASS_ID { get; set; }
        [DisplayName("借閱人")]
        public string USER_ID { get; set; }
        [DisplayName("借閱狀態")]
        public string CODE_ID { get; set; }
        public string BOOK_CLASS_NAME { get; set; }
        public string USER_ENAME { get; set; }
        public string CODE_NAME { get; set; }
        public string BOOK_ID { get; set; }
 
    }
}
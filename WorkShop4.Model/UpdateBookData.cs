using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WorkShop4.Model.InsertBookData;

namespace WorkShop4.Model
{
    public class UpdateBookData
    {
        [MaxLengthLimit(400, ErrorMessage = "輸入長度不可超過400個字")]
        [DisplayName("書名")]
        [Required(ErrorMessage = "此欄位必填")]
        public string BOOK_NAME { get; set; }

        [MaxLengthLimit(60, ErrorMessage = "輸入長度不可超過60個字")]
        [DisplayName("作者")]
        [Required(ErrorMessage = "此欄位必填")]
        public string BOOK_AUTHOR { get; set; }

        [MaxLengthLimit(40, ErrorMessage = "輸入長度不可超過40個字")]
        [DisplayName("出版商")]
        [Required(ErrorMessage = "此欄位必填")]
        public string BOOK_PUBLISHER { get; set; }

        [MaxLengthLimit(2400, ErrorMessage = "輸入長度不可超過2400個字")]
        [DisplayName("內容簡介")]
        [Required(ErrorMessage = "此欄位必填")]
        public string BOOK_NOTE { get; set; }

        [DisplayName("購書日期")]
        [Required(ErrorMessage = "此欄位必填")]
        public DateTime BOOK_BOUGHT_DATE { get; set; }

        [MaxLengthLimit(8, ErrorMessage = "輸入長度不可超過8個字")]
        [DisplayName("圖書類別")]
        [Required(ErrorMessage = "此欄位必填")]
        public string BOOK_CLASS_ID { get; set; }

        [MaxLengthLimit(200, ErrorMessage = "輸入長度不可超過200個字")]
        [DisplayName("借閱狀態")]
        [Required(ErrorMessage = "此欄位必填")]
        public string CODE_ID { get; set; }

        [MaxLengthLimit(24, ErrorMessage = "輸入長度不可超過24個字")]
        [DisplayName("借閱人")]
        public string USER_ID { get; set; }
        public string BOOK_ID { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkShop4.Model
{
    public class LendRecord
    {
        [DisplayName("借閱日期")]
        public string LEND_DATE { get; set; }

        [DisplayName("借閱人員編號")]
        public string KEEPER_ID { get; set; }

        [DisplayName("英文姓名")]
        public string USER_ENAME { get; set; }

        [DisplayName("中文姓名")]
        public string USER_CNAME { get; set; }
        public string BOOK_ID { get; set; }
    }
}

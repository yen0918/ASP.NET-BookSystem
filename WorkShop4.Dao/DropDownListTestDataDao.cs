using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace WorkShop4.Dao
{
    public class DropDownListTestDataDao : IDropDownListDataDao
    {
        private string rootCodeDataFilePath = @"D:\OneDrive\桌面\WorkShop\BackEnd\WorkShop4\MockCode\";

        public List<SelectListItem> GetBookClassName()
        {
            string bookClassFilePath = rootCodeDataFilePath + "BOOK_CLASS.txt";

            var lines = File.ReadAllLines(bookClassFilePath);
            List<SelectListItem> result = new List<SelectListItem>();
            string splitChar = "\t";

            foreach (var item in lines)
            {
                result.Add(new SelectListItem()
                {
                    Text = item.Split(splitChar.ToCharArray())[1],
                    Value = item.Split(splitChar.ToCharArray())[0]
                });
            }
            return result;
        }

        public List<SelectListItem> GetUserEname()
        {
            string memborFilePath = rootCodeDataFilePath + "MEMBER_M.txt";

            var lines = File.ReadAllLines(memborFilePath);
            List<SelectListItem> result = new List<SelectListItem>();
            string splitChar = "\t";

            foreach (var item in lines)
            {
                result.Add(new SelectListItem()
                {
                    Text = item.Split(splitChar.ToCharArray())[1],
                    Value = item.Split(splitChar.ToCharArray())[0]
                });
            }
            return result;
        }

        public List<SelectListItem> GetCodeName()
        {
            string bookCodeFilePath = rootCodeDataFilePath + "BOOK_CODE.txt";

            var lines = File.ReadAllLines(bookCodeFilePath);
            List<SelectListItem> result = new List<SelectListItem>();
            string splitChar = "\t";

            foreach (var item in lines)
            {
                
                    result.Add(new SelectListItem()
                    {
                        Text = item.Split(splitChar.ToCharArray())[3],
                        Value = item.Split(splitChar.ToCharArray())[1]
                    });
                

            }
            return result;
        }
    }
}

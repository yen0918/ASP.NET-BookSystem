using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using WorkShop4.Model;
using WorkShop4.Dao;

namespace WorkShop4.Service
{
    public class DropDownListData : IDropDownListData
    {
        private Dao.IDropDownListDataDao dropDownListDataDao { get; set; }

        public List<SelectListItem> GetUserEname()
        {
            return dropDownListDataDao.GetUserEname();
        }

        public List<SelectListItem> GetCodeName()
        {
            return dropDownListDataDao.GetCodeName();
        }

        public List<SelectListItem> GetBookClassName()
        {
            return dropDownListDataDao.GetBookClassName();
        }
    }
}

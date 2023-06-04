using System.Collections.Generic;
using System.Web.Mvc;

namespace WorkShop4.Dao
{
    public interface IDropDownListDataDao
    {
        List<SelectListItem> GetBookClassName();
        List<SelectListItem> GetCodeName();
        List<SelectListItem> GetUserEname();
    }
}
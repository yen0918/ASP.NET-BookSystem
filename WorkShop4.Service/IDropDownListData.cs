using System.Collections.Generic;
using System.Web.Mvc;

namespace WorkShop4.Service
{
    public interface IDropDownListData
    {
        List<SelectListItem> GetBookClassName();
        List<SelectListItem> GetCodeName();
        List<SelectListItem> GetUserEname();
    }
}
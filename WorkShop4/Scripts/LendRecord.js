
$(function () {

    var getUrlString = location.href;
    var url = new URL(getUrlString);

    $("#lend_grid").kendoGrid({
        //填入資料庫資料
        dataSource: {
            transport: {
                read: {
                    url: "/Book/GetLendRecord?bookId=" + url.searchParams.get('bookId') ,
                    dataType: "json",
                    type: "POST",
                    data: function () {
                       
                    }
                }
            },
            //一頁20筆
            pageSize: 20,
        },
        //搜尋欄
        height: 550,
        sortable: true,
        pageable: {
            input: true,
            numeric: false
        },
        //列表欄位
        columns: [
            { field: "BOOK_ID", hidden: true },
            { field: "LEND_DATE", title: "借閱日期", width: "15px" },
            { field: "KEEPER_ID", title: "借閱人員編號", width: "15px" },
            { field: "USER_ENAME", title: "英文姓名", width: "30px" },
            { field: "USER_CNAME", title: "中文姓名", width: "15px" }
        ]
    });




});


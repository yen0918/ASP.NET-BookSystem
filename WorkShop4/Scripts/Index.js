
$(function () {

    $("#bookCategory").width(255).kendoDropDownList({
        dataTextField: "Text",
        dataValueField: "Value",
        dataSource: {
            transport: {
                read: {
                    url: "/Book/GetBookClassName",
                    dataType: "json",
                    type: "Get"
                }
            }
        },
        optionLabel: "請選擇圖書類別"
    });

    $("#bookStatus").width(255).kendoDropDownList({
        dataTextField: "Text",
        dataValueField: "Value",
        dataSource: {
            transport: {
                read: {
                    url: "/Book/GetCodeName",
                    dataType: "json",
                    type: "Get"
                }
            }
        },
        optionLabel: "請選擇借閱狀態"
    });

    $("#lendUser").width(255).kendoDropDownList({
        dataTextField: "Text",
        dataValueField: "Value",
        dataSource: {
            transport: {
                read: {
                    url: "/Book/GetUserName",
                    dataType: "json",
                    type: "Get"
                }
            }
        },
        optionLabel: "請選擇借閱人"
    });

    //查詢鈕按下產生grid
    $("#getBook").click(function () {
        $("#book_grid").kendoGrid({
            //填入資料庫資料
            dataSource: {
                transport: {
                    read: {
                        url: "/Book/Index",
                        dataType: "json",
                        type: "Post",
                        data: function () {
                            return {
                                BOOK_NAME: $("#bookName").val(),
                                BOOK_CLASS_ID: $("#bookCategory").data("kendoDropDownList").value(),
                                CODE_ID: $("#bookStatus").data("kendoDropDownList").value(),
                                USER_ID: $("#lendUser").data("kendoDropDownList").value()
                            }
                        }
                    }
                },
                //一頁20筆
                pageSize: 20,
            },
            height: 550,
            sortable: true,
            pageable: {
                input: true,
                numeric: false
            },
            //列表欄位
            columns: [
                { field: "BOOK_ID", title: "ID", hidden: true },
                { field: "BOOK_CLASS_NAME", title: "圖書類別", width: "15px" },
                //將書名加入連結
                { template: "<a href=/Book/Detail?bookId=${BOOK_ID}>${BOOK_NAME}</a>", title: "書名", width: "30px" },
                { field: "BOOK_BOUGHT_DATE", title: "購書日期", width: "15px" },
                { field: "CODE_NAME", title: "借閱狀態", width: "15px" },
                { field: "USER_ENAME", title: "借閱人", width: "10px" },
                //按鈕並塞入隱藏欄位
                {
                    command: [{ name: "借閱紀錄", class: "btn btn-default", click: GoLendRecord }], width: "12px"
                },
                { field: "BOOK_ID", title: "ID", hidden: true },
                {
                    command: [{ name: "編輯", class: "btn btn-default", click: GoUpdate  } ], width: "10px"
                },
                { field: "BOOK_ID", title: "ID", hidden: true },
                {
                    command: [{ name: "刪除", class: "btn btn-default", click: DeleteBook }], width: "10px"

                },
                { field: "BOOK_ID", title: "ID", hidden: true }
            ]
        });
    });


    //透過監聽觸發事件，取得點擊的欄列取值
    function DeleteBook() {
        event.preventDefault();
        if (confirm("確定刪除此筆資料?") == true) {
            var tr = $(event.target.closest("tr"))
            $.ajax({
                type: "POST",
                url: "/Book/DeleteBook",
                data: "bookId=" + $(event.target.closest("td")).next().text(),
                dataType: "json",
                success: function (response) {
                    if (response == "刪除成功！") {
                        $(tr).remove();
                        alert(response);
                    } else {
                        alert(response);
                    }
                }, error: function (error) {
                    alert("系統發生錯誤");
                }
            });
        }
        else {
            alert("刪除取消！");
        }
    }

    //至編輯頁面function
    function GoUpdate()
    {
        window.location.replace("/Book/UpdateBook?bookId=" + $(event.target.closest("td")).next().text());
    }

    //至借閱紀錄頁面function
    function GoLendRecord() {
        window.location.replace("/Book/LendRecord?bookId=" + $(event.target.closest("td")).next().text());
    }


});
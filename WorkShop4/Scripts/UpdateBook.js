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

    $("#bookKeeper").width(255).kendoDropDownList({
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

    $("#bookBoughtDate").width(255).kendoDatePicker({
        format: "yyyy/MM/dd",
        //輸入框鎖定
        dateInput: true
    });




    //取得網址存入變數
    var getUrlString = location.href;
    //將網址從string轉為url
    var url = new URL(getUrlString);

    //驗證
    var validator = $("#updateForm").kendoValidator().data("kendoValidator");


    //取得初始藉狀態欄位項目控制借閱人選單開關
    function InitStatus() {
        switch ($("#bookStatus").data("kendoDropDownList").value()) {
            case "A": $("#bookKeeper").data("kendoDropDownList").enable(false);
                break;
            case "U": $("#bookKeeper").data("kendoDropDownList").enable(false);
                break;
        }
    }

    

    $(document).ready(function () {

        

        //取得該筆資料放入欄位
        $.ajax({
            type: "POST",
            url: "/Book/GetABook?bookId=" + url.searchParams.get('bookId'),
            dataType: "json",
            success: function (response) {
                //將獲取的資料放入各欄位
                $("#bookName").val(response.BOOK_NAME);
                $("#bookAuthor").val(response.BOOK_AUTHOR);
                $("#bookPublisher").val(response.BOOK_PUBLISHER);
                $("#bookNote").val(response.BOOK_NOTE);
                $("#bookBoughtDate").data("kendoDatePicker").value(response.BOOK_BOUGHT_DATE);
                $("#bookCategory").data("kendoDropDownList").value(response.BOOK_CLASS_ID);
                $("#bookStatus").data("kendoDropDownList").value(response.CODE_ID);
                $("#bookKeeper").data("kendoDropDownList").value(response.USER_ID);
                InitStatus()
            }, error: function (error) {
                alert("系統發生錯誤");
            }
        });

        
        
        //透過借閱狀態判斷借閱人選單開關
        $("#bookStatus").change(function () {
            if ($("#bookStatus").data("kendoDropDownList").value() == "A" || $("#bookStatus").data("kendoDropDownList").value() == "U") {//可以先存變數  ===可以判斷到型態
                $("#bookKeeper").data("kendoDropDownList").value("");
                $("#bookKeeper").data("kendoDropDownList").enable(false);
                $("#btnSave").attr("disabled", false);
                
            } else {
                $("#bookKeeper").data("kendoDropDownList").enable(true);
            }
        });


        //判斷url路徑是否為明細，是的話隱藏按鈕與Disable
        if (url.pathname == "/Book/Detail") {
            $("#pageTitle").text("書籍明細");
            $(":input").attr('disabled', true);
            $("#bookBoughtDate").data("kendoDatePicker").enable(false);
            $("#bookCategory").data("kendoDropDownList").enable(false);
            $("#bookStatus").data("kendoDropDownList").enable(false);
            $("#bookKeeper").data("kendoDropDownList").enable(false);
            $("#btnDelete").hide();
            $("#btnSave").hide();
            $("#btnBack").attr('disabled', false);
        } else {
            $("#pageTitle").text("編輯書籍");
        }

        


        $("#btnSave").click(function () {
            //將輸入的值存入
            var data = {
                BOOK_NAME: $("#bookName").val(),
                BOOK_AUTHOR: $("#bookAuthor").val(),
                BOOK_PUBLISHER: $("#bookPublisher").val(),
                BOOK_BOUGHT_DATE: kendo.toString($("#bookBoughtDate").data("kendoDatePicker").value(), "yyyy/MM/dd"),
                BOOK_NOTE: $("#bookNote").val(),
                BOOK_CLASS_ID: $("#bookCategory").data("kendoDropDownList").value(),
                CODE_ID: $("#bookStatus").data("kendoDropDownList").value(),
                USER_ID: $("#bookKeeper").data("kendoDropDownList").value()
            }
            //驗證通過即向controller發起修改
            if (validator.validate()) {
                $.ajax({
                    type: "POST",
                    url: "/Book/UpdateBook?bookId=" + url.searchParams.get('bookId'),
                    data: data,
                    dataType: "json",
                    success: function (response) {
                        alert("修改成功！");
                    },
                    error: function (error) {
                        alert("修改失敗！");
                    }
                });
            }
        });

        //透過searchParams取得結果的Key並刪除資料
        $("#btnDelete").click(function (e) {
            if (confirm("確定刪除此筆資料?") == true) {
                //阻止按鈕預設事件
                e.preventDefault();
                var tr = $(this).closest('tr')
                $.ajax({
                    type: "POST",
                    url: "/Book/DeleteBook",
                    data: "bookId=" + url.searchParams.get('bookId'),
                    dataType: "json",
                    success: function (response) {
                        if (response == "刪除成功！") {
                            $(tr).remove();
                            alert(response);
                            window.location.replace("/Book/Index");
                        } else {
                            alert(response);
                        }
                    }, error: function (error) {
                        alert("系統發生錯誤");
                    }
                });
            } else {
                e.preventDefault();
                alert("刪除取消！");
            }
            return false;

        });
    });
});


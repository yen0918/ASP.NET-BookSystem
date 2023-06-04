

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

    //將日期預設為今日
    $("#bookBoughtDate").width(255).kendoDatePicker({
        format: "yyyy/MM/dd",
        value: new Date(),
        //輸入框鎖定
        dateInput: true
    });

    //驗證
    var validator = $("#insertForm").kendoValidator().data("kendoValidator");

    $("#btnSave").click(function () {

        //將欄位資料存入Model Item
        var data = {
            BOOK_NAME: $("#bookName").val(),
            BOOK_AUTHOR: $("#bookAuthor").val(),
            BOOK_PUBLISHER: $("#bookPublisher").val(),
            BOOK_BOUGHT_DATE: $("#bookBoughtDate").val(),
            BOOK_NOTE: $("#bookNote").val(),
            BOOK_CLASS_ID: $("#bookCategory").val()
        }

        //將資料送去controller做處理，並將欄位值設為空
        if (validator.validate()) {
            $.ajax({
                type: "POST",
                url: "/Book/InsertBook",
                data: data,
                dataType: "json",
                success: function (response) {
                    alert("新增成功！");
                    $(":input").val("");
                    $("#bookCategory").data("kendoDropDownList").value("");
                    $("#btnSave").val("存檔");
                    $("#btnBack").val("回上一頁");
                },
                error: function (response) {
                    alert("新增失敗！");
                }
            });
        }
    });
});
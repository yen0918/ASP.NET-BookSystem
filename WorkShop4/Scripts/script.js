
var bookDataFromLocalStorage = [];


$(function () {
    //一開始執行時先將資料存到localstorage
    loadBookData();

    //顯示彈跳視窗鈕的事件，按下後將視窗彈出
    $("#show_window").click(function () {
        $("#window").data("kendoWindow").center().open();
        $("#show_window").hide();//隱藏按鈕
    });

    //彈跳視窗的內容及屬性
    $("#window").kendoWindow({
        width: "500px",
        title: "請輸入書籍資料",
        //預設視窗隱藏
        visible: false,
        //鎖住背景
        modal: true,
        //關閉window時顯示開啟彈跳視窗鈕
        close: showButton
    });

    //下拉選單資料
    var data = [
        { text: "資料庫", value: "database" },
        { text: "網際網路", value: "internet" },
        { text: "應用系統整合", value: "system" },
        { text: "家庭保健", value: "home" },
        { text: "語言", value: "language" }
    ]

    //將data的值塞入下拉選單，並且更換圖片value
    $("#book_category").kendoDropDownList({
        dataTextField: "text",
        dataValueField: "value",
        dataSource: data,
        index: 0,
        change: function () {
            var value = this.value();
            $("#change_image").attr("src", "image/" + value + ".jpg");
        }
    });

    //加上KendoUI的DatePicker，並修改日期格式與設定最大日期為今日
    $("#bought_datepicker").kendoDatePicker({
        format: "yyyy-MM-dd",
        value: new Date(),
        max: new Date(),
        //輸入框鎖定雌參數
        dateInput: true
    });

    //將datepicker之input設定為唯讀
    // $("#bought_datepicker").attr("readonly",true);

    //新增按鈕事件，把kendowindw內的書籍資料傳到kendowgrid列表內
    $("#add_book_data").click(function () {
        //必填驗證
        var validator = $("#list").kendoValidator().data("kendoValidator");
        //必填驗證判斷式
        if (validator.validate()) {
            //將書籍加入bookData之函數
            addBook();
            //初始欄位
            $("#book_name").val("");
            $("#book_author").val("");
            $("#change_image").attr("src", "image/database.jpg");
            $("#book_category").data("kendoDropDownList").text("資料庫");
            kendo.alert("新增成功！");
            //關閉視窗
            $("#window").data("kendoWindow").close();
        }
    });

    //書籍列表
    $("#book_grid").kendoGrid({
        //填入資料庫資料
        dataSource: {
            data: bookDataFromLocalStorage,
            schema: {
                model: {
                    fields: {
                        BookId: { type: "int" },
                        BookName: { type: "string" },
                        BookCategory: { type: "string" },
                        BookAuthor: { type: "string" },
                        BookBoughtDate: { type: "string" }
                    }
                }
            },
            //一頁20筆
            pageSize: 20,
        },
        //搜尋欄
        toolbar: kendo.template("<div class='book-grid-toolbar'><input class='book-grid-search' placeholder='我想要找......' type='text' id='get_book'></input></div>"),
        height: 550,
        sortable: true,
        pageable: {
            input: true,
            numeric: false
        },
        //列表欄位
        columns: [
            { field: "BookId", title: "書籍編號", width: "10%" },
            { field: "BookName", title: "書籍名稱", width: "50%" },
            { field: "BookCategory", title: "書籍種類", width: "10%" },
            { field: "BookAuthor", title: "作者", width: "15%" },
            { field: "BookBoughtDate", title: "購買日期", width: "15%" },
            { command: { text: "刪除", click: deleteBook }, title: " ", width: "120px" }
        ],
    });

    //input偵測輸入事件
    $("#get_book").on("input", findBook);
});

/*funcion-------------------------------------------------------------------------------------------------------*/

//按鈕顯示
function showButton() {
    $("#show_window").show();
};

//最開始執行時會先載入此fucn並將書籍資料載入LocalStorage
function loadBookData() {
    //取得LocalStorage值(反序列化)
    bookDataFromLocalStorage = JSON.parse(localStorage.getItem("bookData"));
    //如果值為空就再存一次
    if (bookDataFromLocalStorage == null) {
        //將書籍資料存入bookDataFromLocalStorage
        bookDataFromLocalStorage = bookData;
        //將書籍資料(bookData)轉成JSON(序列化)存入LocalStorage
        localStorage.setItem("bookData", JSON.stringify(bookDataFromLocalStorage));
    }
}

function findBook() {
    var grid = $('#book_grid').data('kendoGrid');
    //取得grid的欄
    var columns = grid.columns;
    //創建一個空的filters陣列
    var filter = { logic: 'or', filters: [] };

    //將搜尋的值推入
    columns.forEach(function (x) {
        filter.filters.push({
            field: x.field,
            operator: 'contains',
            value: $("#get_book").val()
        })
    });
    //grid顯示fliter
    grid.dataSource.filter(filter);
}

function deleteBook(e) {
    //停止事件默認動作
    e.preventDefault();
    //取得點擊的物件
    var row = $(e.currentTarget).closest("tr");
    var grid = $("#book_grid").data("kendoGrid");
    $("<div></div>").kendoConfirm({
        content: "確定要刪除這筆資料嗎?",
        actions: [{
            text: "確認",
            action: function (e) {
             //刪除選擇的資料列
             grid.removeRow(row);
             //datasource同步更新
             grid.dataSource.sync();
             //將資料存入localstorage
             bookDataFromLocalStorage = grid.dataSource.data();
             localStorage.setItem("bookData", JSON.stringify(bookDataFromLocalStorage));
             kendo.alert("刪除成功！");
             return true;
            },
            primary: true
        }, {
            text: "取消"
        }]
    }).data("kendoConfirm").open()
}

function addBook() {
    //取資料內BookId最大值
    var maxId = Math.max.apply(null, bookDataFromLocalStorage.map(x => x.BookId));
    //將新增的資料加到dataSource內給grid顯示
    var grid = $("#book_grid").data("kendoGrid");
    grid.dataSource.add({
        BookId: maxId + 1,
        BookName: $("#book_name").val(),
        //取得選擇器的text值
        BookCategory: $("#book_category").data("kendoDropDownList").text(),
        BookAuthor: $("#book_author").val(),
        BookBoughtDate: $("#bought_datepicker").val()
    });
    //將頁面資料存到localstorage
    bookDataFromLocalStorage = grid.dataSource.data();
    localStorage.setItem("bookData", JSON.stringify(bookDataFromLocalStorage));
}

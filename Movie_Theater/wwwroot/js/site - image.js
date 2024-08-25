$(document).ready(function () {
    $('#MovieImage').on("change", function () {
        // 等待0.1秒
        setTimeout(function () {
            console.log($('#MovieImage').val());
            // 判斷是否有圖片
            if ($('#MovieImage').val() == "") {
                // 更換成沒照片的圖
                $('#MovieImage').prev().attr("src", "/images/noimage.jpg");
            }
        }, 100);
        // 允許的副檔名
        var allowType = "image.*";
        // 上傳的第一個檔案
        var file = this.files[0];
        if (file != undefined) {
            // 抓取檔案的型態是否為allowType的格式
            if (file.type.match(allowType)) {
                // 讀取圖片
                var reader = new FileReader();
                // 檔案讀取完成的事件
                reader.onload = function (e) {
                    // 選到#MovieImage的上一個元素修改src屬性
                    $('#MovieImage').prev().attr("src", e.target.result);
                    // 選到#MovieImage的上一個元素修改title屬性
                    $('#MovieImage').prev().attr("title", file.name);
                    imageTemp = e.target.result;
                }
                // 用圖片的URL讀取圖片
                reader.readAsDataURL(file);
            } else {
                alert("不支援的檔案上傳類型");
                // 清空內容
                $('#MovieImage').val('');
            }
        }

    });
    $('#SnackImages').on("change", function () {
        // 等待0.1秒
        setTimeout(function () {
            console.log($('#SnackImages').val());
            // 判斷是否有圖片
            if ($('#SnackImages').val() == "") {
                // 更換成沒照片的圖
                $('#SnackImages').prev().attr("src", "/images/noimage.jpg");
            }
        }, 100);
        // 允許的副檔名
        var allowType = "image.*";
        // 上傳的第一個檔案
        var file = this.files[0];
        if (file != undefined) {
            // 抓取檔案的型態是否為allowType的格式
            if (file.type.match(allowType)) {
                // 讀取圖片
                var reader = new FileReader();
                // 檔案讀取完成的事件
                reader.onload = function (e) {
                    // 選到#MovieImage的上一個元素修改src屬性
                    $('#SnackImages').prev().attr("src", e.target.result);
                    // 選到#MovieImage的上一個元素修改title屬性
                    $('#SnackImages').prev().attr("title", file.name);
                    imageTemp = e.target.result;
                }
                // 用圖片的URL讀取圖片
                reader.readAsDataURL(file);
            } else {
                alert("不支援的檔案上傳類型");
                // 清空內容
                $('#SnackImages').val('');
            }
        }

    });
});

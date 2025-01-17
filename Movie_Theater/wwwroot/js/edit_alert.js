﻿function EditConfirm(element) {
    // 使用 SweetAlert2 來顯示確認對話框
    Swal.fire({
        title: "是否確認要修改?",
        icon: "warning",
        showDenyButton: true, // 顯示「否」按鈕
        confirmButtonText: "是", // 「是」按鈕的文字
        denyButtonText: "否", // 「否」按鈕的文字
        confirmButtonColor: '#3085d6', // 設置「是」按鈕為藍色
        denyButtonColor: '#6c757d', // 設置「否」按鈕為灰色
    }).then((result) => {
        // 判斷用戶選擇的按鈕
        if (result.isConfirmed) {
            // 用戶選擇了「是」，繼續刪除操作，導航到新的URL
            //window.location.href = element.href;
            $('form').submit();
        } else if (result.isDenied) {
            // 如果用戶選擇了「取消」，什麼都不做
        }
    });
    return false; // 防止 a 標籤的預設行為
}
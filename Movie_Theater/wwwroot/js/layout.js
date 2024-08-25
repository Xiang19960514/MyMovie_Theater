// 獲取側邊欄切換按鈕
        const hamBurger = document.querySelector("#sidebar-toggle");

        // 設置點擊事件，切換側邊欄展開/收起狀態
        hamBurger.addEventListener("click", function () {
            document.querySelector("#sidebar").classList.toggle("expand");
        });
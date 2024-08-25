function DataChangeResult(result) {
    switch (result) {
        case "C_1":
            Swal.fire({
                title: "新增成功",
                icon: "success",
                timer: 1500,
            });
            break;
        case "C_0":
            Swal.fire({
                title: "新增失敗",
                icon: "error",
                timer: 1500,
            });
            break;
        case "D_1":
            Swal.fire({
                title: "刪除成功",
                icon: "success",
                timer: 1500,
            });
            break;
        case "D_0":
            Swal.fire({
                title: "刪除失敗",
                icon: "error",
                timer: 1500,
            });
            break;
        case "E_1":
            Swal.fire({
                title: "修改成功",
                icon: "success",
                timer: 1500,
            });
            break;
        case "E_0":
            Swal.fire({
                title: "修改失敗",
                icon: "error",
                timer: 1500,
            });
            break;
    }
}

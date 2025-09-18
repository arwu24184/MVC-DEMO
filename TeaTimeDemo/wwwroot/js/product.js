var dataTable;
$(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            url: '/admin/product/getall'
        },
        "columns": [
            { data: 'name', "width": "25%" },
            { data: 'category.name', "width": "15%" },
            { data: 'size', "width": "10%" },
            { data: 'price', "width": "15%" },
            { data: 'description', "width": "10%" },
            {
                data: 'id',
                "render": function (data) {
                    return `<div class="w-100 btn-group" role="group" mx-auto>
                        <a href="/admin/product/upsert?id=${data}" class="btn btn-primary ">
                            <i class="bi bi-pencil-square"></i>編輯
                        </a>
                        <a onClick=Delete('/admin/product/delete/${data}') class="btn btn-danger ">
                            <i class="bi bi-trash-fill"></i>刪除
                        </a>
                    </div>`
                },
                "width": "15%"
            }
        ]
    });
}
function Delete(url) {
    Swal.fire({
        title: "你確定嗎?",
        text: "資料會被刪除",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "確定刪除",
        cancelButtonText: "取消"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    dataTable.ajax.reload();
                    toastr.success(data.message);
                }
            })
        }
    })
}
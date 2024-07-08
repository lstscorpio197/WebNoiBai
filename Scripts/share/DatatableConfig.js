$DataTable = function (element, options = {}) {
    /* options.sDom = "<'scroll scrollX't><'row pagination-content'<'col-sm-6 d-flex align-items-center'<'float-left mr5px'l><'float-left'i>><'col-sm-6'p>>";*/
    options.sDom = "<'scroll scrollX't><'row pagination-content'<'col-sm-6'<'float-left'i>><'col-sm-6 d-flex align-items-center justify-content-end 'p l>>";
    options.language = {
        emptyTable: "Không có dữ liệu",
        info: "Hiển thị kết quả từ _START_ - _END_ trên tổng số _TOTAL_",
        infoEmpty: "Hiển thị kết quả từ 0 - 0 trên tổng số 0",
        infoFiltered: "(filtered from _MAX_ total entries)",
        infoPostFix: "",
        thousands: ",",
        lengthMenu: "_MENU_",
        loadingRecords: "Đang tải...",
        processing: '<div class="spinner"></div>',
        search: "",
        zeroRecords: "",
        paginate: {
            "first": "<<",
            "last": ">>",
            "next": ">",
            "previous": "<"
        },

    };
    options.sort = false;
    options.lengthMenu = [[1, 10, 25, 50, 75, 100, 150, 200], ["1/trang", "10/trang", "25/trang", "50/trang", "75/trang", "100/trang", "150/trang", "200/trang"]];
    options.pageLength = 50;
    options.destroy = true;
    options.searching = true;
    options.autoWidth = true;
    options.processing = true;
    options.drawCallback = function () {
        ResizeTable();
    }
    var table = $(element).DataTable(options)
    return table;
}
const YES = 1;
const NO = 0;
const NUMBER_ROWS_TABLE = 100;

const TOKEN_STORAGE_NAME = "TOKEN_STORAGE_NAME";
const REFRESHTOKEN_STORAGE_NAME = "REFRESHTOKEN_STORAGE_NAME";

const DN_USER_REGIS_NGHIEP_VU = {
    Add_NguoiKhaiHaiQuan: "OE1001",
    Edit_NguoiKhaiHaiQuan: "OE2102",

    Edit_ThongTinChung: "OE2102",
    Edit_For_Add_UserCode: "OE2103",
    Edit_For_Add_UserID: "OE2104",

    Delete_UserCode: "OE3102",
    Delete_TerminalID: "OE3103",
    Delete_UserID: "OE3104",
}

const DN_USER_REGIS_TINH_TRANG = {
    GetDescription: function (userRegis) {
        let tinhTrang = userRegis.TINH_TRANG;

        let result = "";
        switch (tinhTrang) {
            case DN_USER_REGIS_TINH_TRANG.TAT_CA:
                result = "Tất cả"
                break;
            case DN_USER_REGIS_TINH_TRANG.DANG_KY_MOI:
                result = '<span class="badge bg-info pt-2 pb-2">Đăng ký mới</span>';
                break;
            case DN_USER_REGIS_TINH_TRANG.DANG_KY_SUA:
                result = '<span class="badge bg-primary pt-2 pb-2">Đăng ký sửa</span>';
                break;
            case DN_USER_REGIS_TINH_TRANG.DANG_KY_HUY:
                result = '<span class="badge bg-primary pt-2 pb-2">Đăng ký hủy</span>';
                break;
            case DN_USER_REGIS_TINH_TRANG.DA_GUI:
                result = '<span class="badge bg-primary pt-2 pb-2">Đã gửi bản khai</span>';
                break;
            case DN_USER_REGIS_TINH_TRANG.CHO_HQ_DUYET:
                result = '<span class="badge bg-primary pt-2 pb-2">Chờ Hải quan duyệt</span>';
                break;
            case DN_USER_REGIS_TINH_TRANG.KIEM_TRA_CKS_KHONG_THANH_CONG:
                result = '<span class="badge bg-warning pt-2 pb-2">Kiểm tra chữ ký số không thành công</span>';
                break;
            case DN_USER_REGIS_TINH_TRANG.HQ_TU_CHOI:
                result = '<span class="badge bg-danger pt-2 pb-2">Hải quan từ chối</span>';
                break;
            case DN_USER_REGIS_TINH_TRANG.HQ_CHAP_NHAN:
                result = '<span class="badge bg-success pt-2 pb-2">Hải quan chấp nhận [Số tiếp nhận: ' + userRegis.SO_TN + ' ]</span>'
                break;
            default:
                result = '<span class="badge bg-info pt-2 pb-2">Đăng ký mới</span>';
        }
        return result;
    },

    TAT_CA: -1,
    DANG_KY_MOI: 1,
    DANG_KY_SUA: 15,
    DANG_KY_HUY: 18,
    DA_GUI: 2,
    CHO_HQ_DUYET: 4,
    KIEM_TRA_CKS_KHONG_THANH_CONG: 20,
    HQ_TU_CHOI: 40,
    HQ_CHAP_NHAN: 41

};


// Cấu hình Ajax
AjaxConfigHelper.AjaxSetup();
AjaxConfigHelper.AjaxStart();
AjaxConfigHelper.AjaxStop();

$(function () {
    FormatInputHelper.Init();
    TableHelper.Init();

    ResizePage();
    CancelEnterEvent();

    $('.select2').each(function () {
        $(this).select2({ dropdownParent: $(this).parent() });
    })
    /* $('.select2').select2();*/
    $('.select2-multi').select2();

});


function ResizePage() {
    PageResizeHelper.LoadDefaultSize();
    setTimeout(function () {
        PageResizeHelper.LoadDefaultSize();
    }, 1000);
    $(window).resize(function (e) {
        PageResizeHelper.LoadDefaultSize();
    });
}
function CancelEnterEvent() {
    // Hủy sự kiện enter cho toàn control nhập liệu
    $("input,textare").on('keydown', function (e) {
        var code = e.keyCode || e.which;
        if (code == 13 && !$(this).is("textarea, :button, :submit")) {
            e.stopPropagation();
            e.preventDefault();
            $(this)
                .nextAll(":input:not(:disabled, [readonly='readonly'])")
                .first()
                .focus();
        }
    });
}
var keydownBefore = 0;
$.fn.ForceNumericOnly =
    function () {
        return this.each(function () {
            $(this).keydown(function (e) {
                var key = e.charCode || e.keyCode || 0;
                // ký tự c để ctr + A, ctr + C, ctr + V, ctr + X
                if (keydownBefore == 17 && (key == 65 || key == 67 || key == 86 || key == 88)) return true;
                // allow backspace, tab, delete, enter, arrows, numbers and keypad numbers ONLY
                // home, end, period, and numpad decimal
                keydownBefore = key;
                //console.log(key);
                return (
                    key == 8 ||
                    key == 9 ||
                    key == 13 ||
                    key == 46 ||
                    key == 110 ||
                    key == 190 ||
                    (key >= 35 && key <= 40) ||
                    (key >= 48 && key <= 57) ||
                    (key >= 96 && key <= 105));
            });
        });
    };

$(document).ready(function () {
    $('.modal#CHITIET, .modal#CHITIETTK, .modal#CHITIETNQ').on('show.bs.modal', function () {
        let title = $(this).find('.modal-title').text();
        let id = $(this).find('[name=Id]').val() || 0;
        if (id > 0) {
            title = title.replace(/Thêm mới/g, 'Cập nhật');
        }
        else {
            title = title.replace(/Cập nhật/g, 'Thêm mới');
        }
        $(this).find('.modal-title').text(title);
    })


    $userInfo = {
        init: function () {
            this.showModal();
            this.hideModal();
            this.save();
            this.validate();
        },
        self: $('#UserInfo'),
        validate: function () {
            $userInfo.self.find('form').validate({
                rules: {
                    UserName: {
                        required: true
                    },
                    FullName: {
                        required: true
                    },
                    Email: {
                        required: true,
                        email: true
                    },
                    Phone: {
                        phone: true
                    }
                },
                messages: {
                    UserName: {
                        required: "Tài khoản không được để trống"
                    },
                    FullName: {
                        required: "Tên đầy đủ không được để trống"
                    },
                    Email: {
                        required: "Email không được để trống"
                    },
                }
            })
        },
        showModal: function () {
            $userInfo.self.on('shown.bs.modal', function () {
                
            })
        },
        hideModal: function () {
            $userInfo.self.on('shown.bs.modal', function () {
                var getResponse = AjaxConfigHelper.SendRequestToServer(`/User/GetUserInfo`, "GET", null);
                getResponse.then((res) => {
                    if (res.IsOk) {
                        data = res.Body.Data || {};
                        for (let prop in data) {
                            if (prop.indexOf('Time') > -1 || prop.indexOf('Ngay') > -1 || prop.indexOf('day') > -1) {
                                data[prop] = formatDateFrom_StringServer(data[prop]);
                            }
                            $userInfo.self.find(`[name=${prop}]`).val(data[prop]);
                        }
                    }
                })
            })
        },
        getData: function () {
            if (!$userInfo.self.find('form').valid()) {
                return false;
            }
            let data = GetFormDataToObject($userInfo.self.find('form'));
            data.Birthday = formatDateFromClientToServerEN(data.Birthday);
            return data;
        },
        save: function () {
            $userInfo.self.find('#btn-save').off('click').on('click', function () {
                let data = $userInfo.getData();
                if (!data)
                    return false;
                var getResponse = AjaxConfigHelper.SendRequestToServer(`/User/UpdateUserInfo`, "POST", { 'us': data });
                getResponse.then((res) => {
                    if (res.IsOk) {
                        ToastSuccess("Cập nhật");

                        $('#UsFullName').text(data.FullName);
                        $('#UsFullName').closest('div').attr('title',data.FullName);

                        $userInfo.self.modal('hide');
                    }
                })
            })
        }
    }

    $changePass = {
        init: function () {
            this.showModal();
            this.hideModal();
            this.save();
            this.validate();
        },
        self: $('#ChangePassword'),
        validate: function () {
            $changePass.self.find('form').validate({
                rules: {
                    OldPassword: {
                        required: true,
                        noSpace: true
                    },
                    
                    NewPassword: {
                        required: true,
                        noSpace: true
                    },
                    ConfirmPassword: {
                        required: true,
                        noSpace: true,
                        equal: 'NewPassword'
                    },
                },
                messages: {
                    OldPassword: {
                        required: "Mật khẩu cũ không được để trống",
                        noSpace: "Mật khẩu không được chứa khoảng trắng"
                    },
                    NewPassword: {
                        required: "Mật khẩu mới không được để trống",
                        noSpace: "Mật khẩu không được chứa khoảng trắng"
                    },
                    ConfirmPassword: {
                        required: "Xác nhận mật khẩu không được để trống",
                        noSpace: "Mật khẩu không được chứa khoảng trắng",
                        equal: "Xác nhận mật khẩu không trùng khớp"
                    },
                }
            })
        },
        showModal: function () {
            $changePass.self.on('shown.bs.modal', function () {

            })
        },
        hideModal: function () {
            $changePass.self.on('shown.bs.modal', function () {
                $changePass.self.find('input[type=password]').val('');
            })
        },
        getData: function () {
            if (!$changePass.self.find('form').valid()) {
                return false;
            }
            let data = GetFormDataToObject($changePass.self.find('form'));
            return data;
        },
        save: function () {
            $changePass.self.find('#btn-save').off('click').on('click', function () {
                let data = $changePass.getData();
                if (!data)
                    return false;

                var getResponse = AjaxConfigHelper.SendRequestToServer(`/User/ChangePassword`, "POST", { 'us': data });
                getResponse.then((res) => {
                    if (res.IsOk) {
                        ToastSuccess("Cập nhật");
                        $changePass.self.modal('hide');
                    }
                })
            })
        }
    }

    $userInfo.init();
    $changePass.init();
})

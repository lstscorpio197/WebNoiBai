$.validator.addMethod("equalToIdZero", function (value, element, params) {
    var idValue = $(`[name=${params}]`).closest('form').find('[name=Id]').val() || 0;
    return idValue != 0 || value === $(`[name=${params}]`).val();
})
$.validator.addMethod("equal", function (value, element, params) {
    return value === $(`[name=${params}]`).val();
})
$.validator.addMethod("email", function (value, element) {
    var emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return emailRegex.test(value);
}, "Vui lòng nhập chính xác địa chỉ email");
$.validator.addMethod("phone", function (value, element) {
    if (value == '') {
        return true;
    }
    var phoneRegex = /^[0-9\-\+]{10,12}$/;
    return phoneRegex.test(value);
}, "Vui lòng nhập chính xác số điện thoại");
$.validator.addMethod("noSpace", function (value, element) {
    return value.indexOf(" ") < 0;
}, "Không được nhập ký tự khoảng trắng");
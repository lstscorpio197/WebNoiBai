
// CÁC HÀM DÙNG CHUNG
function ValidateEmail(email) {
    let re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(email);
}
function formatNumberDisplay(number = 0, soLuongThapPhan = 0) {
    number = (number || 0);
    if (number == 0) return '';
    let result = $.number(number, soLuongThapPhan, '.', ',').replace('.000', '').replace('.00', '');
    result = result.replace('.500', '.5');
    result = result.replace('.250', '.25');
    result = result.replace('.750', '.75');
    return result;
}
function formatTextDisplay(str) {
    str = (str || '');
    return str;
}

function formatDateFrom_StringServer(date) {
    date = date || '';
    if (date.length < 10) return '';
    if (date == '0001-01-01T00:00:00') return '';

    let day = date.substring(0, 10);
    let dayArray = day.replace(/-/g, '/').split('/');
    day = dayArray[2] + '/' + dayArray[1] + '/' + dayArray[0];
    return day;
}
function formatDateFrom_StringServerHms(date) {
    date = date || '';
    if (date.length < 10) return '';

    let day = date.substring(0, 10);
    let dayArray = day.replace(/-/g, '/').split('/');
    day = dayArray[2] + '/' + dayArray[1] + '/' + dayArray[0];

    let hours = '';
    if (date.length > 10) {
        hours = date.substring(11);
    }
    return day + ' ' + hours;
}
function formatDateFromServer(date) {
    date = date || null;
    if (date == null) return '';
    var Result = new Date(parseInt(date.substr(6)));
    Result = Result == '' ? '' : (Result.getDate().toString().padStart(2, '0') + '/' + (Result.getMonth() + 1).toString().padStart(2, '0') + '/' + Result.getFullYear());
    if (Result == "01/01/1") return '';
    return Result;
}
function formatDateFromServerHms(date) {
    date = date || null;
    if (date == null) return '';
    var Result = new Date(parseInt(date.substr(6)));
    Result = Result == '' ? '' : (Result.getDate().toString().padStart(2, '0') + '/' + (Result.getMonth() + 1).toString().padStart(2, '0') + '/' + Result.getFullYear() + " " + Result.getHours().toString().padStart(2, '0') + ":" + Result.getMinutes().toString().padStart(2, '0') + ":" + Result.getSeconds().toString().padStart(2, '0'));
    if (Result == "01/01/1") return '';
    return Result;
}
function formatDateFromClientToServer(TextDate) {
    if (IsServerDateVN == Yes) return TextDate;
    TextDate = (TextDate || '').trim();
    var arrDate = TextDate.split("/");
    if (arrDate.length > 2) { return arrDate[1] + "/" + arrDate[0] + "/" + arrDate[2]; }
    return null;
}
function formatDateFromClientToServerEN(TextDate) {
    TextDate = (TextDate || '').trim();
    var arrDate = TextDate.split("/");
    if (arrDate.length > 2) { return arrDate[1] + "/" + arrDate[0] + "/" + arrDate[2]; }
    return '';
}

function setDisplayTarget_newClick() {
    $('a[target^="_new"]').off('click').on('click', function () {
        if (windowCurrent != null)
            windowCurrent.close();
        var widthCheck = $(this).data('width');
        var width = window.innerWidth * 0.6;
        if (widthCheck == '100') { width = window.innerWidth }
        // define the height in
        var height = width * window.innerHeight / window.innerWidth + 300;
        if (widthCheck == '100') { height = window.innerHeight - 30; }
        // Ratio the hight to the width as the user screen ratio
        windowCurrent = window.open(this.href, 'newwindow', 'width=' + width + ', height=' + height + ', top=' + ((window.innerHeight - height) / 2) + ', left=' + ((window.innerWidth - width) / 2));
        windowCurrent.onload = function () {
            if ($('.not-popup-print').length == 0) return false;
            windowCurrent.print();
        }
        return false;
    });
};
function openHtmlToNewWindow(title, htmldata) {
    var widthCheck = $(this).data('width');
    var width = window.innerWidth * 0.6;
    if (widthCheck == '100') { width = window.innerWidth }
    // define the height in
    var height = width * window.innerHeight / window.innerWidth + 300;
    if (widthCheck == '100') { height = window.innerHeight - 30; }
    var printWindow = window.open('', 'My div', 'height=' + height + ',width=' + width);
    printWindow.document.write('<html><head><title>' + title + '</title>');
    printWindow.document.write('</head><body >');
    printWindow.document.write(htmldata);
    printWindow.document.write('</body></html>');
    printWindow.document.close();
    printWindow.print();
}
var $docSo = {
    arr_textnumber: ['không', 'một', 'hai', 'ba', 'bốn', 'năm', 'sáu', 'bảy', 'tám', 'chín'],
    f_init: function (so, donvi) {
        var strso = so + '';
        var arr_so = strso.split('.');
        if (arr_so.length == 2) {
            if (parseInt(arr_so[1].substring(0, 1)) >= 5)
                so = parseInt(so) + 1;
        }
        so = so + '';
        so = parseInt(so);
        if (so == 0) return $docSo.arr_textnumber[0];
        var chuoi = "", hauto = "";
        do {
            ty = so % 1000000000; so = Math.floor(so / 1000000000);
            if (so > 0) { chuoi = $docSo.f_dochangtrieu(ty, true) + hauto + chuoi; }
            else { chuoi = $docSo.f_dochangtrieu(ty, false) + hauto + chuoi; } hauto = " tỷ";
        } while (so > 0);
        return $docSo.f_TextUppercase(chuoi + (donvi || " đồng."));
    },
    f_dochangchuc: function (so, daydu) {
        var chuoi = "";
        chuc = Math.floor(so / 10);
        donvi = so % 10;
        if (chuc > 1) {
            chuoi = " " + $docSo.arr_textnumber[chuc] + " mươi";
            if (donvi == 1) { chuoi += " mốt"; }
        } else if (chuc == 1) {
            chuoi = " mười";
            if (donvi == 1) { chuoi += " một"; }
        } else if (daydu && donvi > 0) { chuoi = " lẻ"; }
        if (donvi == 5 && chuc > 1) { chuoi += " lăm"; }
        else if (donvi > 1 || (donvi == 1 && chuc == 0)) {
            //console.log(donvi); donvi = parseInt(donvi); console.log(donvi);            
            //var strdonvi = donvi + '';
            //var arr_donvi = strdonvi.split('.');
            //if (arr_donvi.length == 2) {
            //    if (parseInt(arr_donvi[1].substring(0, 1)) >= 5)
            //        donvi = donvi + 1;
            //}
            chuoi += " " + $docSo.arr_textnumber[donvi];

        } return chuoi;
    },
    f_docblock: function (so, daydu) {
        var chuoi = ""; tram = Math.floor(so / 100); so = so % 100;
        if (daydu || tram > 0) { chuoi = " " + $docSo.arr_textnumber[tram] + " trăm"; chuoi += $docSo.f_dochangchuc(so, true); }
        else { chuoi = $docSo.f_dochangchuc(so, false); } return chuoi;
    },
    f_dochangtrieu: function (so, daydu) {
        var chuoi = ""; trieu = Math.floor(so / 1000000); so = so % 1000000;
        if (trieu > 0) { chuoi = $docSo.f_docblock(trieu, daydu) + " triệu"; daydu = true; }
        nghin = Math.floor(so / 1000); so = so % 1000;
        if (nghin > 0) { chuoi += $docSo.f_docblock(nghin, daydu) + " nghìn"; daydu = true; }
        if (so > 0) { chuoi += $docSo.f_docblock(so, daydu); } return chuoi;
    },
    f_TextUppercase: function (strSo) {
        strSo = strSo == null ? "  " : strSo.trim();
        var charFirst = "<span class='text-uppercase'>" + strSo.substring(0, 1) + "</span>";
        strSo = charFirst + strSo.substring(1);
        return strSo;
    }
};

function GetFormDataToObject($form) {
    let formObject = {};
    let formArray = $form.serializeArray();
    formArray.forEach((input) => {
        if (input.value != '') {
            input.value = input.value.trim();
        }
        formObject[input.name] = input.value;
       
    })
    $form.find('input.input-date').each((i, e) => {
        formObject[e.name] = formatDateFromClientToServerEN(formObject[e.name]);
    })
    $form.find('input:checkbox').each((i, e) => {
        formObject[e.name] = e.checked ? 1 : 0;
    })
    $form.find('input:radio').each((i, e) => {
        let name = e.name;
        let value = $form.find(`input[name=${name}]:checked`).data('value');
        formObject[name] = value;
    })
    return formObject;
}

function ConfirmDeleteWithCondition(conditon, descriptionWhenTrue, callBack) {
    if (conditon == true) {
        ConfirmWithCallBack(function () {
            callBack()
        }, descriptionWhenTrue, null, null, "btn-danger");
    }
    else {
        callBack();
    }
}

function ConfirmDeleteWithOption(callBack1, text1 = null, callBack2, text2 = null, btnClass1 = null, btnClass2 = null) {
    text1 = text1 || '<i class="far fa-trash-alt"></i>&nbsp; Xóa';
    text2 = text2 || '<i class="far fa-trash-alt"></i>&nbsp; Xóa';

    btnClass1 = btnClass1 || 'btn-danger';
    btnClass2 = btnClass2 || 'btn-danger';

    console.log(12345);
    $.confirm({
        title: '<i class="fa fa-bell-o"></i> THÔNG BÁO',
        content: 'Chọn tùy chọn xóa?',
        type: 'red',
        typeAnimated: true,
        columnClass: 'col-lg-4 col-md-4 col-xs-12',
        container: $('.content-wrapper'),
        buttons: {
            option1: {
                text: text1,
                btnClass: btnClass1,
                action: function () {
                    callBack1();
                }

            },
            option2: {
                text: text2,
                btnClass: btnClass2,
                action: function () {
                    callBack2();
                }

            },
            close: {
                text: '<i class="fa fa-close"></i>&nbsp;Đóng',
                action: function () { }
            }
        }

    });
}

function ConfirmDelete(callBack) {
    $.confirm({
        title: '<i class="fa fa-bell-o"></i> THÔNG BÁO',
        content: 'Bạn có chắc chắn muốn xóa không?',
        type: 'red',
        typeAnimated: true,
        columnClass: 'col-lg-4 col-md-4 col-xs-12',
        container: $('.content-wrapper'),
        buttons: {
            tryAgain: {
                text: '<i class="far fa-trash-alt"></i>&nbsp; Xóa',
                btnClass: 'btn-danger',
                action: function () {
                    callBack();
                }

            },
            close: {
                text: '<i class="fa fa-close"></i>&nbsp;Đóng',
                action: function () { }
            }
        }

    });
}

function ConfirmWithCallBack(callBack, action = null, typeColor = null, container = null, btnClassCallBack = null) {
    action = action || "thực hiện thao tác";
    typeColor = typeColor || "orange";
    container = container || $('.content-wrapper');
    btnClassCallBack = btnClassCallBack || "btn-success";


    $.confirm({
        title: '<i class="fa fa-bell-o"></i> THÔNG BÁO',
        content: action,
        type: typeColor,
        typeAnimated: true,
        columnClass: 'col-lg-4 col-md-4 col-xs-12',
        container: container,
        buttons: {
            tryAgain: {
                text: 'Thực hiện',
                btnClass: btnClassCallBack,
                action: function () {
                    callBack();
                }

            },
            close: {
                text: '<i class="fa fa-close"></i>&nbsp;Đóng',
                action: function () { }
            }
        }

    });
}

function ToastSuccess(action = '', container = null) {
    let description = action != '' ? action + ' thành công' : 'Thành công';
    $.confirm({
        title: '<i class="fa fa-bell-o"></i> THÔNG BÁO',
        content: '<i class="fa fa-check" style="color:green"></i>&nbsp;' + description,
        type: 'green',
        //autoClose: (resultType === 'blue' || resultType == 'green') ? 'close|5000' : 'close|30000',
        typeAnimated: true,
        columnClass: 'col-lg-4 col-md-4 col-xs-12', //
        container: container || $('.content-wrapper'),
        buttons: {
            close: {
                text: '<i class="fa fa-close"></i>&nbsp;Đóng',
                action: function () {
                    
                }
            }
        },
        onOpen: function () {
        },
        onClose: function () {
        }
    });
}
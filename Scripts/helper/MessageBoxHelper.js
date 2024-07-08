const MessageBoxType = {
    Informational: 100,
    Info: 100,
    Success: 200,
    Redirection: 300,
    ClientError: 400,
    NotAcceptable: 406,
    Warning: 400,
    ServerError: 500,
    Danger: 500
};

var MessageBoxHelper = {
    Init: function () {
        this.Type = MessageBoxType.Informational;
        this.Title = '<i class="fa-light fa-bell"></i>&nbsp;THÔNG BÁO';
        this.Container = $('body');
        this.HrefRedirect = null;
        this.ObjForcus = null;
        this.Callback = null;
    },
    Type: MessageBoxType.Informational,
    Title: '<i class="fa-light fa-bell"></i>&nbsp;THÔNG BÁO',
    Container: $('body'),
    HrefRedirect: null,
    ObjForcus: null,
    Callback: null,
    Confirm: function (type, title, description, containerAround, href, objFocus, callback) {
        title = (title || '<i class="fa-light fa-bell"></i>&nbsp;THÔNG BÁO');
        containerAround = (containerAround || $('body'));
        href = (href || null);
        objFocus = (objFocus || null);
        let resultType = this.GetTypeConfirmBox(type);
        $.confirm({
            title: title,
            content: resultType == 'green' ? '<i class="fa fa-check" style="color:green"></i>&nbsp;' + description : description,
            type: resultType,
            //autoClose: (resultType === 'blue' || resultType == 'green') ? 'close|5000' : 'close|30000',
            typeAnimated: true,
            columnClass: 'col-lg-4 col-md-4 col-xs-12', //
            container: containerAround,
            buttons: {
                close: {
                    text: '<i class="fa fa-close"></i>&nbsp;Đóng',
                    action: function () {
                        if (href != null) { location.href = href; }
                        if (objFocus != null) {
                            setTimeout(function () { objFocus.focus(); }, 500);
                            setTimeout(function () {
                                let $modal = objFocus.parents('.modal');
                                if ($modal.length > 0) {
                                    objFocus.parents('.modal').scrollTop(0);
                                    objFocus.focus();
                                }
                            }, 1000);
                        }
                        if (typeof (callback) === 'function') {
                            callback();
                        }
                    }
                }
            },
            onOpen: function () {
            },
            onClose: function () {
            }
        });
        return false;
    },
    Notification: function (type, description, objFocus, href) {
        description = (description || 'Không có nội dung thông báo.');
        let containerAround = (MessageBoxHelper.Container || $('body'));
        href = (href || null);
        objFocus = (objFocus || null);
        let resultType = this.GetTypeConfirmBox(type);
        $.confirm({
            title: MessageBoxHelper.Title,
            content: description,
            type: resultType,
            typeAnimated: true,
            columnClass: 'col-lg-4 col-md-4 col-xs-12',
            container: containerAround,
            buttons: {
                close: {
                    text: '<i class="fa fa-close"></i>&nbsp;Đóng',
                    action: function () {
                        if (objFocus != null) {
                            setTimeout(function () { objFocus.focus(); }, 1000);
                            setTimeout(function () {
                                let $modal = objFocus.parents('.modal');
                                if ($modal.length > 0) {
                                    objFocus.parents('.modal').scrollTop(0);
                                    objFocus.focus();
                                }
                            }, 1000);
                        } else if (href != null) {
                            location.href = href;
                        }

                        if (typeof (MessageBoxHelper.Callback) == "function") {
                            MessageBoxHelper.Callback();
                        }
                    }
                }
            }
        });
        MessageBoxHelper.Container = null;
    },
    Redirection: function (description, href) {
        description = (description || 'Không có nội dung thông báo.');
        containerAround = (MessageBoxHelper.Container || $('body'));
        $.confirm({
            title: MessageBoxHelper.Title,
            content: description,
            type: 'blue',
            autoClose: 'close|5000',
            typeAnimated: true,
            columnClass: 'col-lg-4 col-md-4 col-xs-12',
            container: containerAround,
            buttons: {
                close: {
                    text: '<i class="fa fa-close"></i>&nbsp;Đang chuyển hướng...',
                    action: function () {
                        location.href = href;
                    }
                }
            }
        });
        MessageBoxHelper.Container = null;
    },
    GetTypeConfirmBox: function (type) {
        let resultType = type;
        if (type == MessageBoxType.Informational) { resultType = 'blue'; }
        if (type == MessageBoxType.Success) { resultType = 'green'; }
        if (type == MessageBoxType.Redirection) { resultType = 'blue'; }
        if (type == MessageBoxType.ClientError || type == MessageBoxType.NotAcceptable) { resultType = 'orange'; }
        if (type == MessageBoxType.ServerError) { resultType = 'red'; }
        return resultType || '';
    },
    ConfirmListError: function () {
        let Description = "<div style='text-align: left;'>Bạn gặp lỗi bởi vì một trong các nguyên nhân sau: <br/>";
        Description += "<p><b>- Hết phiên làm việc. </b><i>(Khắc phục: Nhấn Ctrl-F5 và thử lại.)</i></p>";
        Description += "<p><b>- Không có quyền truy cập chức năng này.</b> <i>(Khắc phục: Liên hệ với quản trị hệ thống.)</i></p>";
        Description += "<p><b>- Server xử lý bị lỗi!.</b></p></div>";
        this.confirm('red', null, Description, null, null, null)
        //this.confirmDialog(null, Description, "red", null, null, null);
    },
};
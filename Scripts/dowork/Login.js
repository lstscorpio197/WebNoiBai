var $LogIn = {
    Self: $('.modal#ModalAdminLoginPartial'),
    Init: function () {
        this.BtnRegisterClick();   
        this.EnterLogin();
        ModalHelper.Close($LogIn.Self);
        ModalHelper.Open($LogIn.Self);
    },
    RequestLogin: function () {
        let formData = HandleDataHelper.GetRequestData($LogIn.Self, 0);
        console.log('formData', formData);
        let formDataParse = {};
        formData.forEach((item) => {
            formDataParse[item.Name] = item.Value;
        });
        console.log('formDataParse', formDataParse);

        if (formDataParse.MA_DV == '') {
            MessageBoxHelper.Notification(MessageBoxType.Warning, 'Vui lòng nhập mã đơn vị.', $LogIn.Self.find('[name=MA_DV]'));
            return;
        }
        if (formDataParse.MA_DV.length < 10 || formDataParse.MA_DV.length > 13) {
            MessageBoxHelper.Notification(MessageBoxType.Warning, 'Vui lòng nhập mã đơn vị có 10 hoặc 13(mã chi nhánh) ký tự.', $LogIn.Self.find('[name=MA_DV]'));
            return;
        }
        if (formDataParse.PASS == '') {
            MessageBoxHelper.Notification(MessageBoxType.Warning, 'Vui lòng nhập mật khẩu.', $LogIn.Self.find('[name=PASS]'));
            return;
        }

        let dataFile = null;
        let dataRequest = new RawDataSignDto();
        dataRequest.Mst = formDataParse.MA_DV;
        dataRequest.JsonDataRequest = JSON.stringify(formDataParse);

        SignatureOnWebsiteHelper.Init('/Login/GetSignContentForLogin', '/Login/Index/', 'get', 'post', dataFile, dataFile);
        SignatureOnWebsiteHelper.Cert.SignData(dataRequest, () => {
            setTimeout(() => {
                location.href = '/Home/Index';
            }, 4000);
        });
    },
    BtnRegisterClick: function () {
        $LogIn.Self.find('.btn-admin-login').click(function () {
            $LogIn.RequestLogin();
        });
    },
    EnterLogin: function () {
        $LogIn.Self.find("input").on('keydown', function (e) {
            var code = e.keyCode || e.which;
            if (code == 13) {
                $LogIn.RequestLogin();
                e.stopPropagation();
                e.preventDefault();              
            }
        });
    }
}

$(function () {
    $LogIn.Init();
});
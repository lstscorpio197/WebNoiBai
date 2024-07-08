var $SignatureExample = {
    Self: $('.modal#ModalKySoGuiHaiQuanPartial'),
    Init: function () {   
        ModalHelper.Open($SignatureExample.Self);
        ModalHelper.Close($SignatureExample.Self);
        $SignatureExample.SignAndSendToHaiQuanClick();
    },
    SignAndSendToHaiQuanClick: function () {
        $SignatureExample.Self.find('.btn-sign-and-send-to-hq').click(function () {
            let btnSelf = $(this);
            let formData = HandleDataHelper.GetRequestData($SignatureExample.Self.find('.modal-body'), 0);
            //console.log('formData', formData);
            let msgValid = HandleDataHelper.ValidateDataBeforeSendRequest($SignatureExample.Self.find('.modal-body'), formData);
            if (!msgValid.IsOk) {
                MessageBoxHelper.Notification(
                    MessageBoxType.Warning,
                    msgValid.Description,
                    $SignatureExample.Self.find(`[name=${msgValid.Name}]`));
                return;
            }
            let dataSend = msgValid.Data;         

            let dataFile = null;
            AjaxConfigHelper.ContainerLoader = $SignatureExample.Self.find('.modal-body');
            SignatureOnWebsiteHelper.Init('/SignatureExample/GetXmlContent', '/SignatureExample/HandleSignedResult/', 'get', 'post', dataFile, dataFile);
            let dataRequest = new RawDataSignDto();
            dataRequest.Mst = dataSend['MA_DV'];
            dataRequest.JsonDataRequest = JSON.stringify(dataSend);

            SignatureOnWebsiteHelper.Cert.SignData(dataRequest, function (data) {              
                btnSelf.hide();
                $ModalTraCuuThongTinDangKyPartial.GetList($ModalTraCuuThongTinDangKyPartial.Self.find('[type=radio][name=TINH_TRANG]:checked').val() || DN_USER_REGIS_TINH_TRANG.TAT_CA);
            });
        });
    }
};

$(function () {
    $SignatureExample.Init();
});
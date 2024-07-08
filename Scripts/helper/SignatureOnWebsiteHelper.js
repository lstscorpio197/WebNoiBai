/* 
ĐÂY LÀ ĐỐI TƯỢNG XỬ LÝ CÁC THAO TÁC VỚI CHỮ KÝ SỐ
1. LẤY THÔNG TIN CHỮ KÝ SỐ
2. THỰC HIỆN KÝ SỐ CÁC THÔNG TIN YÊU CẦU
*/
var SignatureOnWebsiteHelper = {
    Init: function (url, url2, requestType, requestType2, file1, file2) {
        SignatureOnWebsiteHelper.Infomation.url = url;
        SignatureOnWebsiteHelper.Infomation.url2 = url2;
        SignatureOnWebsiteHelper.Infomation.requestType = (requestType || SignatureOnWebsiteHelper.Infomation.requestType);
        SignatureOnWebsiteHelper.Infomation.requestType2 = (requestType2 || SignatureOnWebsiteHelper.Infomation.requestType2);
        SignatureOnWebsiteHelper.Infomation.fileRequest = (file1 || null);
        SignatureOnWebsiteHelper.Infomation.fileRequest2 = (file2 || null);
        //listen msg from ext
        //window.addEventListener("message", event => {
        //    if (event.source !== window) return;
        //    if (event.data.src && event.data.src === "background.js") {
        //        console.log("Received message from content.js");
        //        console.log(event);
        //        UpdateSignResult({ data: event.data.data });
        //    }
        //});
    },
    Infomation: {
        url: "",// Url request để khởi tạo DATA trước gửi đi ký số
        requestType: "post",
        url2: "",// Url request để gửi kết quả DATA sau khi ký số        
        requestType2: "post",
        fileRequest: null,
        fileRequest2: null
    },
    Self: $('.vnaccs-site'),
    Cert: {
        GetInformation: function () {
            $.confirm({
                title: '<i class="fa fa-bell-o"></i>&nbsp;THÔNG BÁO',
                content: 'Bạn muốn đăng ký chữ ký số lên hệ thống?',
                type: 'blue',
                typeAnimated: true,
                columnClass: 'col-md-6 col-md-offset-0',
                container: SignatureOnWebsiteHelper.Self,
                buttons: {
                    OK: {
                        text: '<i class="fa fa-paper-plane-o"></i>&nbsp;Thực hiện đăng ký',
                        btnClass: "btn-blue",
                        action: function () { SignatureOnWebsiteHelper.Cert.SignData(); }
                    },
                    close: {
                        text: '<i class="fa fa-close"></i>&nbsp;Đóng',
                        action: function () { }
                    }
                }
            });
        },
        Sign: function (lstId, title) {
            lstId = (lstId || []);
            if (lstId.lenght == 0) {
                MessageBoxHelper.Confirm('orange', null, 'Thông tin ký số không hợp lệ', SignatureOnWebsiteHelper.Self, null, null);
                return false;
            }
            $.confirm({
                title: '<i class="fa fa-bell-o"></i>&nbsp;THÔNG BÁO',
                content: title || 'Bạn có chắc chắn muốn ký số điện tử không?',
                type: 'blue',
                typeAnimated: true,
                columnClass: 'col-md-6 col-md-offset-0',
                container: SignatureOnWebsiteHelper.Self,
                buttons: {
                    OK: {
                        text: '<i class="fa fa-paper-plane-o"></i>&nbsp;Thực hiện ký số',
                        btnClass: "btn-blue",
                        action: function () { SignatureOnWebsiteHelper.Cert.SignData(lstId); }
                    },
                    close: {
                        text: '<i class="fa fa-close"></i>&nbsp;Đóng',
                        action: function () { }
                    }
                }
            });
        },
        SignData: function (requestData, callBackFunc) {
            if (!SignatureOnWebsiteHelper.Helper.IsSetUpExtension()) {
                return false;
            }
            SignatureOnWebsiteHelper.Helper.ShowWaittingMessage();
            let response = null;
            AjaxConfigHelper.ContainerLoader = SignatureOnWebsiteHelper.Self;
            if (SignatureOnWebsiteHelper.Infomation.fileRequest == null) {
                response = AjaxConfigHelper.SendRequestToServer(SignatureOnWebsiteHelper.Infomation.url, SignatureOnWebsiteHelper.Infomation.requestType, requestData);

            } else {
                let formData = new FormData();
                formData.append("file", SignatureOnWebsiteHelper.Infomation.fileRequest);
                formData.append("JsonDataRequest", JSON.stringify(requestData));
                response = AjaxConfigHelper.SendRequestFileToServer(SignatureOnWebsiteHelper.Infomation.url, SignatureOnWebsiteHelper.Infomation.requestType, formData);

            }
            response.then(respData => {
                let isOk = (respData.isOk || respData.IsOk);
                if (isOk) {
                    let dataSendToAppClient = { code: 1, data: respData.Body.Data };
                    let browser = SignatureOnWebsiteHelper.Helper.GetCurrentBrowser();
                    if (browser === "chrome") {
                        SignatureOnWebsiteHelper.SendMessageToChromeExtension(dataSendToAppClient).then(result => {
                            SignatureOnWebsiteHelper.Cert.UpdateAfterSignData(requestData, result, callBackFunc);

                        }).catch(err => {
                            AjaxConfigHelper.CatchError(err);
                        });
                    }
                    else {
                        SignatureOnWebsiteHelper.SendMessageToFirefoxExtension(dataSendToAppClient);
                    }
                } else {
                    MessageBoxHelper.Confirm('orange', null, (respData.message || respData.Body.Description), SignatureOnWebsiteHelper.Self, null, null);
                }
            }).catch(err => {
                AjaxConfigHelper.CatchError(err);
            });
        },
        UpdateAfterSignData: function (requestData, responseData, callBackFunc) {
            callBackFunc = (callBackFunc || null);
            //SignatureOnWebsiteHelper.Helper.ShowWaittingMessage();
            if (!responseData) return;

            let response2 = null;
            AjaxConfigHelper.ContainerLoader = SignatureOnWebsiteHelper.Self;
            if (SignatureOnWebsiteHelper.Infomation.fileRequest2 == null) {
                let dataAfterSign = requestData;
                dataAfterSign.DataSigned = responseData.data.data;
                dataAfterSign.RedirectUrl = window.location.href;
                response2 = AjaxConfigHelper.SendRequestToServer(SignatureOnWebsiteHelper.Infomation.url2, SignatureOnWebsiteHelper.Infomation.requestType2, dataAfterSign);

            } else {
                let formData = new FormData();
                formData.append("file", SignatureOnWebsiteHelper.Infomation.fileRequest2);
                formData.append("DataSigned", responseData.data.data);
                formData.append("JsonDataRequest", JSON.stringify(requestData));
                formData.append("RedirectUrl", window.location.href);
                response2 = AjaxConfigHelper.SendRequestFileToServer(SignatureOnWebsiteHelper.Infomation.url2, SignatureOnWebsiteHelper.Infomation.requestType2, formData);
            }
            response2.then(resp2 => {
                let isOk = (resp2.isOk || resp2.IsOk);
                let getColor = (isOk ? 'green' : 'orange');
                let description = (isOk ? (resp2.Body.Description || "Thành công!") : resp2.Body.MsgNoti.Description);

                if (isOk) {
                    if (typeof (callBackFunc) == "function") {
                        callBackFunc(resp2.Body.Data);
                    }
                    if (SignatureOnWebsiteHelper.Infomation.url2 == '/Login/Index/') {
                        // Riêng cho trường hợp đăng nhập hệ thống thành công!
                        MessageBoxHelper.Redirection(description, '/Home/Index');
                    } else {
                        MessageBoxHelper.Confirm(getColor, null, description, SignatureOnWebsiteHelper.Self, null, null);
                    }
                }
            }).catch(err => {
                AjaxConfigHelper.CatchError(err);
            });
        }
    },
    Helper: {
        IsSetUpExtension: function () {
            // Kiểm tra xem đã cài trình ký số chưa?
            if (!sessionStorage.getItem("mxext-sign-tsd@thaison.vn")) {
                SignatureOnWebsiteHelper.Helper.ShowGuidMessage();
                return false;
            }
            return true;
        },
        ShowGuidMessage: function () {
            let urlAppSignClienteExe = '/com.ecustsdmx.signonwebsite-tphcm.exe';
            let urlGuide = '/huong-dan-doanh-nghiep-khai-bao-to-khai-nop-phi';
            let content =
                "<div class=\"text-left\" style=\"line-height: 23px; font-size:16px;\">" +
                "<p>Máy tính của bạn chưa cài đặt chương trình hỗ trợ ký số. Thực hiện theo các bước sau:</p>"
                + "<p>1. Tải file cài đặt <a href='" + urlAppSignClienteExe + "' target=\"_blank\"><b>Chương trình hỗ trợ ký số</b><a/></p>"
                + "<p>2. Xem hướng dẫn <a href='" + urlGuide + "' target=\"_blank\"><b>Hướng dẫn cài đặt và sử dụng</b><a/> chương trình ký số</p>"
                + "<p>3. Mọi thắc mắc xin vui lòng liên hệ qua số Tổng đài để được hỗ trợ <span style=\"color:#ff0000;font-weight:bold;\">1900 1286</span></p></div>"
            MessageBoxHelper.Confirm('orange', null, content, SignatureOnWebsiteHelper.Self, null, null);
        },
        ShowWaittingMessage: function () {
            $.confirm({
                title: '<i class="fa fa-bell-o"></i>&nbsp;THÔNG BÁO',
                content: 'Hệ thống đang thực hiện yêu cầu của bạn, vui lòng đợi trong giây lát...',
                autoClose: 'close|5000',
                type: 'blue',
                typeAnimated: true,
                columnClass: 'col-md-4',
                container: SignatureOnWebsiteHelper.Self,
                buttons: {
                    close: {
                        text: '<i class="fa fa-close"></i>&nbsp;Đóng',
                        action: function () { }
                    }
                }
            });
        },
        GetCurrentBrowser: function () {
            // Opera 8.0+
            const isOpera = (navigator.userAgent.indexOf("Opera") || navigator.userAgent.indexOf('OPR')) != -1;
            if (isOpera) return "opera";
            //firefox 1.0++
            const isFirefox = navigator.userAgent.indexOf("Firefox") != -1;
            if (isFirefox) return "firefox";
            //chrome 1++
            const isChrome = navigator.userAgent.indexOf("Chrome") != -1;
            if (isChrome) return "chrome";
            return "undifined";
        }
    },
    SendMessageToChromeExtension: function (dataSend) {
        return new Promise((resolve, reject) => {
            const extensionID = "ljelghodcnccegncomkbfpkacbkagecf"; // LIVE
            //const extensionID = "fniamhnojiohffoohakpdgblgaieeidk";// TEST
            chrome.runtime.sendMessage(extensionID, { data: dataSend },
                function (response) {
                    if (response) {
                        resData = response.data || {
                            "Result": "ERROR",
                            "Note": "NO_NATIVE"
                        };
                        if (resData.Result == "ERROR" && resData.Note == "NO_NATIVE") {
                            //MessageBoxHelper.Confirm('orange', null, 'Vui lòng kiểm tra NativeMessagingHosts trong Registry Editor hoặc gọi tổng đài hỗ trợ 1900 1286.<br/>Trân trọng cảm ơn!', SignatureOnWebsiteHelper.Self, null, null);
                            reject('Vui lòng kiểm tra NativeMessagingHosts trong Registry Editor hoặc gọi tổng đài hỗ trợ 1900 1286.<br/>Trân trọng cảm ơn!');
                        }
                        console.log("==============Webpage - Received============");
                        resolve(response);
                    } else {
                        reject("Đã có lỗi xảy ra!");
                    }
                });
        });
    },
    SendMessageToFirefoxExtension: function (dataSend) {
        window.postMessage({ src: "ecusweb.js", from: window.location.href, "data": dataSend }, "*");
    }
};
$(function () {
    //listen msg from ext
    window.addEventListener("message", event => {
        if (event.source !== window) return;
        if (event.data.src && event.data.src === "background.js") {
            console.log("Received message from content.js");
            console.log(event);
        }
    });
    SignatureOnWebsiteHelper.Init('/controller/functionGetDataSign', '/controller/functionUpdateDataAfterSign/', 'post', 'post');
    SignatureOnWebsiteHelper.Self = $('.vnaccs-site');
});

// Hướng dẫn thực hiện
//let dataSend = { 'DON_HANG_ID': donHangId, 'SO_DON_HANG': soDonHang };
//SignatureOnWebsiteHelper.Init('/ChuKySo/GetContent_XacNhanThanhToanDonHang', '/DDonHang/XacNhanThanhToanPhi/', 'get', 'post');
//SignatureOnWebsiteHelper.Cert.SignData(dataSend, function () {
//    Self.prop('disabled', true);
//    Self.html('<i class="fa fa-check"></i>&nbsp;Xác nhận thành công!');
//    $modalData.find('.btn-save').addClass('hidden');
//    setTimeout(function () { Self.addClass('hidden'); }, 3000);
//});
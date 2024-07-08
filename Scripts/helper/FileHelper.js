var FileHelper = {};


var $Base64File = {
    init: function () { },
    GetFileTypeWebService: function (FileName) {
        var fileType = "application/pdf";
        var arrFile = FileName.split('.');
        if (arrFile.length > 0) {
            var valueCheck = arrFile[arrFile.length - 1];
            valueCheck = valueCheck.trim().toUpperCase();
            switch (valueCheck) {
                case "PDF":
                    fileType = "application/pdf";
                    break;
                case "DOC":
                    fileType = "application/msword";
                    break;
                case "DOCX":
                    fileType = "application/msword";
                    break;
                case "XLS":
                    fileType = "application/vnd.ms-excel";
                    break;
                case "XLSX":
                    fileType = "application/vnd.ms-excel";
                    break;
                case "TXT":
                    fileType = "text/plain";
                    break;
                case "TIF":
                    fileType = "image/tiff";
                    break;
                case "GIF":
                    fileType = "image/gif";
                    break;
                case "PNG":
                    fileType = "image/png";
                    break;
                case "JPG":
                    fileType = "image/jpeg";
                    break;
                case "JPEG":
                    fileType = "image/jpeg";
                    break;
                default:
                    fileType = "image/svg+xml";// Các type ảnh khác
                    break;
            }
        }
        return fileType;
    },
    DownLoadFile: function (LOAI_DULIEU, base64string, fileType, tenFile) {
        if (LOAI_DULIEU == "WEBSERVICE") {
            // base64 string
            var base64str = base64string;
            // decode base64 string, remove space for IE compatibility
            var binary = atob(base64str.replace(/\s/g, ''));
            var len = binary.length;
            var buffer = new ArrayBuffer(len);
            var view = new Uint8Array(buffer);
            for (var i = 0; i < len; i++) {
                view[i] = binary.charCodeAt(i);
            }
            // create the blob object with content-type "application/pdf"               
            var blob = new Blob([view], { type: $Base64File.GetFileTypeWebService(tenFile) });
            var url = URL.createObjectURL(blob);
            var a = $("<a>").attr("href", url).attr("download", tenFile.split('.')[0]).appendTo("body");
            a[0].click();
            a.remove();
        } else {
            var a = $("<a>").attr("href", "data:" + fileType + ";base64," + base64string).attr("download", tenFile.split('.')[0]).appendTo("body");
            a[0].click();
            a.remove();
        }
        // Hoặc dùng cách dưới link này
        //https://stackoverflow.com/questions/16968945/convert-base64-png-data-to-javascript-file-objects
    } 
};
var $ImportFile = {
    LoadEvent: function ($btn, URLCheckRoleImport, $file) {
        this.AddEventOpen($btn, URLCheckRoleImport);
        this.AddEventFileChange($file);
    },
    AddEventOpen: function ($btn, URLCheckRoleImport) {
        $btn.off('click').on('click', function () {
            var Self = $(this);
            if ((URLCheckRoleImport || "") === "") {
                AjaxConfigHelper.ContainerLoader = Self.parents('.modal-body');
                var result = AjaxConfigHelper.SendRequestToServer(URLCheckRoleImport, "GET", { __RequestVerificationToken: $('input[name=__RequestVerificationToken]').val() });
                result.then(function (response) {
                    let isOk = (response.isOk || response.IsOk);
                    if (isOk) {
                        Self.parent().find('input[type=file]').click();
                    }
                }).catch(function (xhr) {
                    AjaxConfigHelper.CatchError(xhr);
                });
            } else {
                Self.parent().find('input[type=file]').click();
            }
            return false;
        });
    },
    AddEventFileChange: function ($file) {
        $file.off('change').bind('change', function () {
            var Self = $(this);
            var file;
            if (this.hasAttribute("multiple")) {
                file = this.files;
                let lstName = '';
                for (var i = 0; i < file.length; i++) {
                    let item = file[i];
                    lstName += item.name + ", ";
                }
                lstName = lstName.substring(0, lstName.length - 2);
                Self.parent().find('.file-name').val($ValidateXSS.sanitizeHtml(lstName));
                Self.parent().find('.file-name').prop('title', 'Danh sách file đính kèm: ' + $ValidateXSS.sanitizeHtml(lstName));
            } else {
                file = this.files[0];
                Self.parent().find('.file-name').val($ValidateXSS.sanitizeHtml(file.name));
                Self.parent().find('.file-name').prop('title', 'File đính kèm: ' + $ValidateXSS.sanitizeHtml(file.name));
            }
            let sizeFile = file.size / 1024 / 1024;
            if (sizeFile > maxLengthFileAccept) {
                Self.val(null); Self.val("");
                MessageBoxHelper.confirm2(null, 'Chỉ chấp nhận file có dung lượng nhỏ hơn hoặc bằng <b>' + maxLengthFileAccept + 'MB</b>. File bạn chọn có dung lượng: ' + sizeFile + "MB", 'orange', Self.parents('.modal-body'), null, Self);
                return false;
            }
        });
    },
    AddEventFileCsvChange: function ($file) {
        $file.off('change').bind('change', function () {
            let Self = $(this);
            let files = this.files;
            let lstFileName = '',
                maxSizeCSV = 100;//100MB
            for (let i = 0; i < files.length; i++) {
                let file = files[i];
                lstFileName += (lstFileName == '' ? file.name : ', ' + file.name);
                let sizeFile = file.size / 1024 / 1024;
                if (sizeFile > maxSizeCSV) {
                    Self.val(null); Self.val("");
                    MessageBoxHelper.confirm2(null, 'Chỉ chấp nhận file .CSV có dung lượng nhỏ hơn hoặc bằng <b>' + maxSizeCSV + 'MB</b>. File bạn chọn có dung lượng: ' + sizeFile + "MB", 'orange', Self.parents('.modal-body'), null, Self);
                    lstFileName = '';
                    break;
                }
            }
            if (lstFileName == '') { return false; }
            Self.parent().find('.file-name').val($ValidateXSS.sanitizeHtml(lstFileName));
        });
    }
};

$FileDownLoad = {
    GetExcelFileByte: function (FileContents, FileName, extFile) {
        var bytes = new Uint8Array(FileContents);
        var blob = new Blob([bytes], { type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" });
        var link = document.createElement('a');
        link.href = window.URL.createObjectURL(blob);
        link.download = FileName + (extFile || '.xlsx');
        link.click();
    },
    GetCSVFileByte: function (FileContents, FileName) {
        var bytes = new Uint8Array(FileContents);
        var blob = new Blob([bytes], { type: "text/csv" });
        var link = document.createElement('a');
        link.href = window.URL.createObjectURL(blob);
        link.download = FileName + '.csv';
        link.click();
    },
    GetExcelBase64File: function (b64Data, FileName, extFile) {
        var contentType = 'application/vnd.ms-excel';
        var blob = $FileDownLoad.ConvertBase64ToBytes(b64Data, contentType);
        var link = document.createElement('a');
        link.href = window.URL.createObjectURL(blob);
        link.download = FileName + (extFile || '.xlsx');
        link.click();
    },
    GetExcelBase64File_Cach2: function (b64Data) {
        var contentType = 'application/vnd.ms-excel';
        var blob1 = b64toBlob(b64Data, contentType);
        var blobUrl1 = URL.createObjectURL(blob1);
        window.open(blobUrl1);
    },
    ConvertBase64ToBytes: function (b64Data, contentType, sliceSize) {
        contentType = contentType || '';
        sliceSize = sliceSize || 512;
        var byteCharacters = atob(b64Data);
        var byteArrays = [];
        for (var offset = 0; offset < byteCharacters.length; offset += sliceSize) {
            var slice = byteCharacters.slice(offset, offset + sliceSize);
            var byteNumbers = new Array(slice.length);
            for (var i = 0; i < slice.length; i++) {
                byteNumbers[i] = slice.charCodeAt(i);
            }
            var byteArray = new Uint8Array(byteNumbers);
            byteArrays.push(byteArray);
        }
        var blob = new Blob(byteArrays, { type: contentType });
        return blob;
    },
    GetPdfFromByteArray: function (FileContents, FileName, extFile) {
        var bytes = new Uint8Array(FileContents);
        var blob = new Blob([bytes], { type: "application/pdf" });
        var link = document.createElement('a');
        link.href = window.URL.createObjectURL(blob);
        link.download = FileName + '.pdf';
        link.click();
    },
    GetPdfFromBase64: function (FileBase64, FileName) {
        var link = $("<a>")
            .attr("href", "data:application/pdf;base64," + FileBase64)
            .attr("download", FileName).appendTo("body");
        link[0].click();
        link.remove();
    }
}
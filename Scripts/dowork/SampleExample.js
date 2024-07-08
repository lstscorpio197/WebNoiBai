var $SampleExample = {
    Self: $('.modal#ModalDangKyDaiLyPartial'),
    Init: function () {
        ModalHelper.Open($SampleExample.Self, function () {
            $SampleExample.GetList();
        });
        ModalHelper.Close($SampleExample.Self);
        $SampleExample.AddNewClick();       
    },
    GetList: function () {
        AjaxConfigHelper.ContainerLoader = $SampleExample.Self.find('.modal-body');
        var response = AjaxConfigHelper.SendRequestToServer("/SampleExample/GetList", "get", null);
        response.then(res => {
            if (res.IsOk) {
                let htmlBody = '';
                let arrData = (res.Body.Data || []);
                for (let i = 0; i < arrData.length; i++) {
                    let itemAgent = arrData[i];
                    htmlBody += '<tr>' +
                        '<td class="text-center td-event">' +
                        '<span class="item-event btn-delete-agent text-danger" data-madn="' + itemAgent.MA_SO_THUE_DN + '" data-agentcode="' + itemAgent.MA_SO_THUE_DL+'">' +
                        '<i class="fa-regular fa-trash-can"></i>' +
                        '</span>' +
                        '</td>' +
                        '<td class="text-center">' + (i + 1) + '</td>' +
                        '<td class="text-center">' + itemAgent.MA_SO_THUE_DL + '</td>' +
                        '<td class="text-center">' + itemAgent.TEN_DL + '</td>' +
                        +'</tr>';
                }
                if (arrData.length == 0) {
                    htmlBody = '<tr><td colspan = "4" class="text-center text-primary"> Chưa có dữ liệu.</td></tr> ';
                }
                $SampleExample.Self.find('table tbody').html(htmlBody);
                $SampleExample.DeleteAgentClick();
            }

        }).catch(err => {
            AjaxConfigHelper.CatchError(err);
        });
    },
    DeleteAgentClick: function () {
        $SampleExample.Self.find('table tbody .btn-delete-agent').off('click').click(function () {
            let $trParent = $(this).parents('tr');
            let agentCode = $(this).data('agentcode');
            let maDN = $(this).data('madn');
            let dataSend = {
                MA_DV: maDN,
                MA_DV_DAILY: agentCode
            };
            $.confirm({
                title: '<i class="fa-light fa-bell"></i>&nbsp;THÔNG BÁO',
                content: 'Bạn có chắc chắn muốn xóa đại lý ' + agentCode +' đã chọn không?',
                type: 'red',
                typeAnimated: true,
                columnClass: 'col-lg-4 col-md-4 col-xs-12',
                container: $SampleExample.Self.find('.modal-body'),
                buttons: {
                    ok: {
                        text: '<i class="fa-regular fa-trash-can"></i>&nbsp;Thực hiện xóa',
                        btnClass: 'btn-red',
                        action: function () {
                            AjaxConfigHelper.ContainerLoader = $SampleExample.Self.find('.moda-body');
                            var response = AjaxConfigHelper.SendRequestToServer("/SampleExample/Delete", "post", dataSend);
                            response.then(res => {
                                if (res.IsOk) {
                                    //$SampleExample.GetList();
                                    $trParent.hide();
                                    MessageBoxHelper.Notification(MessageBoxType.Success, res.Body.Description);
                                }

                            }).catch(err => {
                                AjaxConfigHelper.CatchError(err);
                            });
                        }
                    },
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
           
        });
    },
    AddNewClick: function () {
        $SampleExample.Self.find('.btn-add-agent').click(function () {
            let formData = HandleDataHelper.GetRequestData($SampleExample.Self.find('.add-agent-block'), 0);
            console.log('formData', formData);
            let msgValid = HandleDataHelper.ValidateDataBeforeSendRequest($SampleExample.Self.find('.add-agent-block'), formData);
            if (!msgValid.IsOk) {
                MessageBoxHelper.Notification(MessageBoxType.Warning, msgValid.Description,
                    $SampleExample.Self.find(`[name=${msgValid.Name}]`));
                return;
            }

            let requestData = msgValid.Data;          
            if (requestData.MA_DV_DAILY == '') {
                MessageBoxHelper.Notification(MessageBoxType.Warning, 'Vui lòng nhập mã số thuế của đại lý.', $SampleExample.Self.find('[name=MA_DV_DAILY]'));
                return;
            }
            if (requestData.MA_DV_DAILY.length != 10 && requestData.MA_DV_DAILY.length != 13) {
                MessageBoxHelper.Notification(MessageBoxType.Warning, 'Vui lòng nhập mã số thuế của đại lý có 10 hoặc 13 ký tự số.', $SampleExample.Self.find('[name=MA_DV_DAILY]'));
                return;
            }

            AjaxConfigHelper.ContainerLoader = $SampleExample.Self.find('.moda-body');
            var response = AjaxConfigHelper.SendRequestToServer("/SampleExample/AddNew", "post", formDataParse);
            response.then(res => {
                if (res.IsOk) {
                    $SampleExample.GetList();    
                    $SampleExample.Self.find('[name=MA_DV_DAILY]').val('');
                    MessageBoxHelper.Notification(MessageBoxType.Success, res.Body.Description, $SampleExample.Self.find('[name=MA_DV_DAILY]'));
                }
            }).catch(err => {
                AjaxConfigHelper.CatchError(err);
            });
        });
    }
};

$(function () {
    $SampleExample.Init();
});
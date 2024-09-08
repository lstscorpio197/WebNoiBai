var $header = $('.frame-search');
const $footer = $('.frame-footer');
const $table = $('#TBLDANHSACH');
const $tableBody = $table.find('tbody');
const $modal = $('#CHITIET');
const $modalUser = $('#UserRole');
const $router = "SRole";
const $form = $('#ModalForm');


function DataSearch(pageNum) {
    this.Ma = $header.find('[name=Ma]').val().trim();
    this.Ten = $header.find('[name=Ten]').val().trim();
    this.PageNum = pageNum || $footer.find('[name=PageNumber]').val();;
    this.PageSize = $footer.find('[name=PageLength]').val();
}

$(function () {
    var $page = {
        init: function () {
            $page.initValidate();
            $page.BtnSearchClick();
            $page.GetList(1);
        },
        initValidate: function () {
            $form.validate({
                rules: {
                    Ma: {
                        required: true
                    },
                    Ten: {
                        required: true
                    }
                },
                messages: {
                    Ma: {
                        required: "Mã nhóm quyền không được để trống"
                    },
                    Ten: {
                        required: "Tên nhóm quyền không được để trống"
                    }
                }
            })
        },
        Self: $('.card'),
        BtnSearchClick: function () {
            $header.find('.btnSearch').on('click', function () { $page.GetList(1); });
        },
        GetList: function (pageNum = 1) {

            let html = '';
            let search = new DataSearch(pageNum);

            var getResponse = AjaxConfigHelper.SendRequestToServer(`/${$router}/GetTable`, "GET", search);
            getResponse.then((res) => {
                if (res.IsOk) {
                    let data = res.Body.Data || [];

                    if (data.length == 0) {
                        html = `<tr><td class="text-center" colspan="5"><span>Không có bản ghi</span></td></tr>`;
                        $tableBody.html(html);
                        return false;
                    }

                    let startIndex = res.Body.Pagination.StartIndex || 1;
                    for (let item of data) {
                        let htmlShow = item.Enable == 1 ? '<span class="text-center green"><i class="text-center green icon-status-ico-tsd"></i></span>' : '<span class="text-center red"><i class="text-center red icon-exit-ico-tsd"></i></span>';
                        html += `<tr class="TR_${item.Id}">` +
                            `<td class="text-center"><span>${startIndex}</span></td>` +
                            `<td class="text-center event-handle">` +
                            `<i class="icon-edit-ico-tsd btn-action btnEdit blue mr10" data-id="${item.Id}" data-ma="${item.Ma}" title="Sửa"></i>` +
                            `<i class="icon-setting-ico-tsd btn-action btnViewUser green mr10" data-id="${item.Id}" title="Danh sách tài khoản"></i>` +
                            `<i class="icon-delete-ico-tsd btn-action btnDelete red" data-id="${item.Id}" data-ma="${item.Ma}" title="Xóa"></i>` +
                            `</td>` +
                            `<td class=""><span>${item.Ma}</span></td>` +
                            `<td class=""><span>${item.Ten}</span></td>` +
                            `<td class="text-center">${htmlShow}</td>` +
                            `</tr>`;
                        startIndex++;
                    }
                    $tableBody.html(html);

                    $pagination.Set($footer, res.Body.Pagination, $page.GetList);

                    $page.ViewClick();
                    $page.DeleteClick();
                    $page.ViewListUser();
                }
                else {

                }
            })
        },
        ViewClick: () => {
            $page.Self.find('.btnEdit').off('click').on('click', function () {
                let id = $(this).data('id');

                var getResponse = AjaxConfigHelper.SendRequestToServer(`/${$router}/GetItem`, "GET", { 'id': id });
                getResponse.then((res) => {
                    if (res.IsOk) {
                        let data = res.Body.Data.Item || {};
                        for (let prop in data) {
                            if (prop == 'Enable') {
                                $modal.find(`[name=${prop}]`).prop('checked', data[prop] == 1);
                                continue;
                            }
                            if (prop.indexOf('Time') > -1 || prop.indexOf('Ngay') > -1) {
                                data[prop] = formatDateFrom_StringServer(data[prop]);
                            }
                            $modal.find(`[name=${prop}]`).val(data[prop]);
                        }

                        let lstPermision = res.Body.Data.LstPermissionId || [];
                        for (let item of lstPermision) {
                            $modal.find(`[name=permission-${item}]`).prop('checked', true);
                        }
                        $modal.modal('show');
                    }
                    else {

                    }
                })
            })
        },
        DeleteClick: () => {
            $page.Self.find('.btnDelete').off('click').on('click', function () {
                let id = $(this).data('id');

                var getResponse = AjaxConfigHelper.SendRequestToServer(`/${$router}/CheckExistUser`, "POST", { 'id': id });
                getResponse.then((res) => {
                    if (res.IsOk) {
                        ConfirmDeleteWithCondition(res.Body.Data, "Nhóm quyền đã có tài khoản. Bạn có muốn tiếp tục thực hiện không?", function () {
                            ConfirmDelete(function () {
                                var getResponse = AjaxConfigHelper.SendRequestToServer(`/${$router}/Delete`, "POST", { 'id': id });
                                getResponse.then((res) => {
                                    if (res.IsOk) {
                                        ToastSuccess("Xóa thành công");
                                        $page.GetList();
                                    }
                                    else {

                                    }
                                })
                            })
                        })
                    }
                })

            })
        },
        ViewListUser: () => {
            $page.Self.find('.btnViewUser').off('click').on('click', function () {
                let id = $(this).data('id');

                var getResponse = AjaxConfigHelper.SendRequestToServer(`/${$router}/GetListUser`, "GET", { 'id': id });
                getResponse.then((res) => {
                    if (res.IsOk) {
                        let data = res.Body.Data || [];
                        for (let id of data) {
                            $modalUser.find('#tblUser tbody').find(`#user-${id}`).prop('checked', true);
                        }
                        $modalUser.find('[name=Id]').val(id);
                        $modalUser.modal('show');
                    }
                })
            })
        }
    };

    var $ChiTiet = {
        init: function () {
            this.Save();
            this.ResetForm();
        },
        Self: $('#CHITIET'),
        ResetForm: () => {
            $modal.on('hidden.modal.bs', () => {
                $form.find('input').val('');
                $modal.find('.permission-frame input:checkbox').prop('checked', false);
                $form.find('input[name=Enable]').prop('checked', true);
                $form.find('select').find('option:first-child').prop('selected', true);
                $form.validate().resetForm();
                $form.find('.error').removeClass('error');
            })
        },
        GetDataInput: () => {
            if (!$form.valid()) {
                return false;
            }
            let data = GetFormDataToObject($form);

            let lstPermissionId = [];
            $modal.find('.permission-frame').find('input:checkbox').each(function (i, e) {
                if (e.checked) {
                    let permissionId = $(e).data('id');
                    if (permissionId > 0) {
                        lstPermissionId.push(permissionId);
                    }
                }
            })

            let dataSend = {
                item: data,
                strPermissionId: JSON.stringify(lstPermissionId)
            }
            return dataSend;
        },
        Save: () => {
            $modal.find('#btn-save').off('click').on('click', () => {
                let data = $ChiTiet.GetDataInput();
                if (!data)
                    return false;
                let action = data.item.Id > 0 ? 'Update' : 'Create';
                var getResponse = AjaxConfigHelper.SendRequestToServer(`/${$router}/${action}`, "POST", data);
                getResponse.then((res) => {
                    if (res.IsOk) {
                        let actionSub = data.item.Id > 0 ? 'Cập nhật' : 'Thêm mới';
                        ToastSuccess(actionSub + ' thành công');
                        $page.GetList(data.item.Id > 0 ? null : 1);
                        $modal.modal('hide');
                    }
                    else {

                    }
                })
            })
        }
    };

    var $UserRole = {
        init: function () {
            this.ResetForm();
            this.Save();
        },
        ResetForm: () => {
            $modalUser.on('hidden.modal.bs', () => {
                $modalUser.find('input:checkbox').prop('checked', false);
            })
        },
        Save: () => {
            $modalUser.find('#btn-save').off('click').on('click', () => {
                let id = $modalUser.find('[name=Id]').val();
                let lstUserId = [];
                $modalUser.find('#tblUser tbody').find('input:checkbox').each(function (i, e) {
                    if (e.checked) {
                        let userId = $(e).data('id');
                        if (userId > 0) {
                            lstUserId.push(userId);
                        }
                    }
                })

                let dataSend = {
                    strUserId: JSON.stringify(lstUserId),
                    id:id
                }
                
                var getResponse = AjaxConfigHelper.SendRequestToServer(`/${$router}/UpdateListUser`, "POST", dataSend);
                getResponse.then((res) => {
                    if (res.IsOk) {
                        ToastSuccess('Cập nhật thành công');
                        $modalUser.modal('hide');
                    }
                    else {

                    }
                })
            })
        }
    }

    $page.init();
    $ChiTiet.init();
    $UserRole.init();
});
var $header = $('.frame-search');
const $footer = $('.frame-footer');
const $table = $('#TBLDANHSACH');
const $tableBody = $table.find('tbody');
const $modal = $('#CHITIET');
const $router = "SUser";
const $form = $('#ModalForm');
const $roleModal = $('#NHOMQUYEN');


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
                    Username: {
                        required: true
                    },
                    HoTen: {
                        required: true
                    },
                    PhongBan: {
                        required: true
                    },
                    Email: {
                        required: true,
                        email: true
                    },
                    Password: {
                        required: () => {
                            return !($form.find('[name=Id]').val() > 0);
                        }
                    },
                    ConfirmPassword: {
                        required: () => {
                            return !($form.find('[name=Id]').val() > 0);
                        },
                        equalToIdZero: 'Password'
                    },
                },
                messages: {
                    Username: {
                        required: "Tài khoản không được để trống"
                    },
                    HoTen: {
                        required: "Tên đầy đủ không được để trống"
                    },
                    PhongBan: {
                        required: "Vui lòng chọn phòng ban"
                    },
                    Email: {
                        required: "Email không được để trống"
                    },
                    Password: {
                        required: "Mật khẩu không được để trống"
                    },
                    ConfirmPassword: {
                        required: "Xác nhận mật khẩu không được để trống",
                        equalToIdZero: "Xác nhận mật khẩu không trùng khớp"
                    },
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
                        let htmlShow = item.IsActived == 1 ? '<span class="text-center green"><i class="text-center green icon-status-ico-tsd"></i></span>' : '<span class="text-center red"><i class="text-center red icon-exit-ico-tsd"></i></span>';
                        html += `<tr class="TR_${item.Id}">` +
                            `<td class="text-center"><span>${startIndex}</span></td>` +
                            `<td class="text-center event-handle">` +
                            `<i class="icon-edit-ico-tsd btn-action btnEdit blue mr10" data-id="${item.Id}" title="Sửa"></i>` +
                            `<i class="icon-setting-ico-tsd btn-action btnViewRole green mr10" data-id="${item.Id}" title="Phân quyền"></i>` +
                            `<i class="icon-delete-ico-tsd btn-action btnDelete red" data-id="${item.Id}" title="Xóa"></i>` +
                            `</td>` +
                            `<td class=""><span>${item.Username}</span></td>` +
                            `<td class=""><span>${item.HoTen}</span></td>` +
                            `<td class=""><span>${item.Email}</span></td>` +
                            `<td class=""><span>${item.PhongBan}</span></td>` +
                            `<td class=""><span>${item.ChucVuTxt}</span></td>` +
                            `<td class="text-center">${htmlShow}</td>` +
                            `</tr>`;
                        startIndex++;
                    }
                    $tableBody.html(html);

                    $pagination.Set($footer, res.Body.Pagination, $page.GetList);

                    $page.ViewClick();
                    $page.DeleteClick();
                    $page.ViewRoleClick();
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
                        let data = res.Body.Data || {};
                        for (let prop in data) {
                            if (prop == 'NhanEmail' || prop == 'IsActived') {
                                $modal.find(`[name=${prop}]`).prop('checked', data[prop] == 1);
                                continue;
                            }
                            if (prop.indexOf('Time') > -1 || prop.indexOf('Ngay') > -1) {
                                data[prop] = formatDateFromServer(data[prop]);
                                $modal.find(`[name=${prop}]`).val(data[prop]).trigger('change');
                                continue;
                            }
                            if (prop == 'GioiTinh') {
                                $modal.find(`[type="radio"][name=${prop}][data-value=${data[prop]}]`).prop('checked', true);
                                continue;
                            }
                            $modal.find(`[name=${prop}]`).val(data[prop]);
                        }
                        $modal.find('.is-password-field').addClass('hidden');
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

                ConfirmDelete(function () {
                    var getResponse = AjaxConfigHelper.SendRequestToServer(`/${$router}/Delete`, "POST", { 'id': id });
                    getResponse.then((res) => {
                        if (res.IsOk) {
                            ToastSuccess("Xóa tài khoản thành công");
                            $page.GetList();
                        }
                        else {

                        }
                    })
                })

            })
        },
        ViewRoleClick: () => {
            $page.Self.find('.btnViewRole').off('click').on('click', function () {
                let id = $(this).data('id');

                var getResponse = AjaxConfigHelper.SendRequestToServer(`/${$router}/GetRole`, "GET", { 'id': id });
                getResponse.then((res) => {
                    if (res.IsOk) {
                        let data = res.Body.Data || {};
                        $roleModal.find(`[name=Id]`).val(data.Id);
                        let lstRoleId = data.LstRoleId;
                        $roleModal.find(`input:checkbox`).prop('checked', false);
                        for (let id of lstRoleId) {
                            $roleModal.find(`input:checkbox[data-id="${id}"]`).prop('checked', true);
                        }
                        $roleModal.modal('show');
                    }
                    else {

                    }
                })
            })
        }
    };

    var $ChiTiet = {
        init: function () {
            this.Save();
            this.ResetForm();
            this.ResetPass();
        },
        Self: $('#CHITIET'),
        ResetForm: () => {
            $modal.on('hidden.modal.bs', () => {
                $form.find('input').val('');
                $form.find('input:checkbox').prop('checked', true);
                $form.find('input[name=NhanEmail]').prop('checked', false);
                $form.find('select').find('option:first-child').prop('selected', true);
                $form.validate().resetForm();
                $form.find('.error').removeClass('error');
                $modal.find('.is-password-field').removeClass('hidden');
                $form.find('.radio-group').find('input:first').prop('checked', true);
            })
        },
        GetDataInput: () => {
            if (!$form.valid()) {
                return false;
            }
            let data = GetFormDataToObject($form);
            return data;
        },
        Save: () => {
            $modal.find('#btn-save').off('click').on('click', () => {
                let data = $ChiTiet.GetDataInput();
                if (!data)
                    return false;
                let action = data.Id > 0 ? 'Update' : 'Create';
                var getResponse = AjaxConfigHelper.SendRequestToServer(`/${$router}/${action}`, "POST", data);
                getResponse.then((res) => {
                    if (res.IsOk) {
                        let actionSub = data.Id > 0 ? 'Cập nhật' : 'Thêm mới';
                        ToastSuccess(actionSub + ' thành công');
                        $page.GetList(data.Id > 0 ? null : 1);
                        $modal.modal('hide');
                    }
                    else {

                    }
                })
            })
        },
        ResetPass: () => {
            $modal.find('#btn-resetpass').off('click').on('click', () => {
                let data = $ChiTiet.GetDataInput();
                if (!data)
                    return false;

                ConfirmWithCallBack(function () {
                    var getResponse = AjaxConfigHelper.SendRequestToServer(`/${$router}/ResetPassword`, "POST", { 'id': data.Id });
                    getResponse.then((res) => {
                        if (res.IsOk) {
                            let des = res.Body.Description;
                            ToastSuccess(des);
                            $page.GetList(data.Id > 0 ? null : 1);
                            $modal.modal('hide');
                        }
                        else {

                        }
                    })
                }, "Bạn có chắc chắn muốn reset mật khẩu cho tài khoản này không?");
                
            })
        }
    };

    var $UserRole = {
        init: function () {
            this.LoadPermission();
            this.Save();
            this.ResetForm();
        },
        ResetForm: () => {
            $roleModal.on('hidden.modal.bs', () => {
                $roleModal.find('input:checkbox').prop('checked', false);
            })
        },
        LoadPermission: () => {
            $roleModal.find('#btn-view-permission').off('click').on('click', function () {
                $roleModal.find(`.permission-item`).prop('disabled', true).prop('checked', false);

                let id = $roleModal.find('[name=Id]').val();
                let lstRoleId = [];
                $roleModal.find('.role-frame').find('input:checkbox').each(function (i, e) {
                    if (e.checked) {
                        let roleId = $(e).data('id');
                        if (roleId > 0) {
                            lstRoleId.push(roleId);
                        }
                    }
                })
                let dataSend = {
                    strRoleId: JSON.stringify(lstRoleId),
                    id: id
                };
                var getResponse = AjaxConfigHelper.SendRequestToServer(`/${$router}/GetPermission`, "GET", dataSend);
                getResponse.then((res) => {
                    if (res.IsOk) {
                        let lstPermissionAll = res.Body.Data.All || [];
                        let lstPermissionNotGranted = res.Body.Data.NotGrant || [];

                        for (let id of lstPermissionAll) {
                            $roleModal.find(`[name=permission-${id}]`).prop('disabled', false).prop('checked', true);
                        }
                        for (let id of lstPermissionNotGranted) {
                            $roleModal.find(`[name=permission-${id}]`).prop('checked', false);
                        }
                    }
                    else {

                    }
                })
            })
        },
        GetDataInput: () => {
            let lstRole = [];
            let lstPermissNotGranted = [];

            $roleModal.find('input.role-item:checked').each(function () {
                let id = $(this).data('id');
                lstRole.push(id);
            })
            $roleModal.find('input.permission-item').not(':checked,:disabled').each(function () {
                let id = $(this).data('id');
                lstPermissNotGranted.push(id);
            })
            return {
                strRoleId: JSON.stringify(lstRole),
                strPermissionId: JSON.stringify(lstPermissNotGranted),
                id: $roleModal.find('[name=Id]').val()
            }
        },
        Save: () => {
            $roleModal.find('#btn-save').off('click').on('click', () => {
                let data = $UserRole.GetDataInput();
                if (!data)
                    return false;
                let action = data.Id > 0 ? 'Update' : 'Create';
                var getResponse = AjaxConfigHelper.SendRequestToServer(`/${$router}/UpdatePermission`, "POST", data);
                getResponse.then((res) => {
                    if (res.IsOk) {
                        ToastSuccess("Phân quyền thành công");
                        $roleModal.modal('hide');
                    }
                })
            })
        }
    };

    $page.init();
    $ChiTiet.init();
    $UserRole.init();
});
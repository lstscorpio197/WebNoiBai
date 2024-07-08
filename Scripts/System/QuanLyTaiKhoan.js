const $headerUser = $('#DSTaiKhoanNguoiDung .frame-search');
const $footerUser = $('#DSTaiKhoanNguoiDung .frame-footer');
const $tableUser = $('#DSTaiKhoanNguoiDung #TBLUSER');
const $tableUserBody = $tableUser.find('tbody');
const $modalUser = $('#DSTaiKhoanNguoiDung #CHITIETTK');
const $formUser = $('#DSTaiKhoanNguoiDung #ModalForm');

const $headerRole = $('#NhomNguoiDungVaPhanQuyenSuDung .frame-search');
const $footerRole = $('#NhomNguoiDungVaPhanQuyenSuDung .frame-footer');
const $tableRole = $('#NhomNguoiDungVaPhanQuyenSuDung #TBLDANHSACH');
const $tableRoleBody = $tableRole.find('tbody');
const $modalRole = $('#NhomNguoiDungVaPhanQuyenSuDung #CHITIETNQ');
const $formRole = $('#NhomNguoiDungVaPhanQuyenSuDung #ModalFormRole');

const $modalSelectUser = $('#NhomNguoiDungVaPhanQuyenSuDung #CHITIETTV');
const $tableUserInDepartment = $('#NhomNguoiDungVaPhanQuyenSuDung #TBLUSER tbody');

const $modalPermission = $('#NhomNguoiDungVaPhanQuyenSuDung #CHITIETPQ');
const $tablePermission = $modalPermission.find('#TBLPERMISSION');

const $modalUserRole = $('#NhomNguoiDungVaPhanQuyenSuDung #CHITIETTVNHOM');
const $tableUserRole = $modalUserRole.find('#TBLUSERROLE');

const $modalUserPermission = $('#NhomNguoiDungVaPhanQuyenSuDung #USERPERMISSION');
const $tableUserPermission = $modalUserPermission.find('#TBLUSERPERMISSION');

const $router = "QuanLyTaiKhoan";


function DataSearchUser(pageNum) {
    this.Code = $headerUser.find('[name=Code]').val().trim();
    this.Name = $headerUser.find('[name=Name]').val().trim();
    this.DepartmentId = $headerUser.find('[name=DepartmentId]').val().trim();
    this.TeamId = $headerUser.find('[name=TeamId]').val().trim();
    this.PageNum = pageNum || $footerUser.find('[name=PageNumber]').val();;
    this.Length = $footerUser.find('[name=PageLength]').val();
}
function DataSearchRole(pageNum) {
    this.Code = $headerRole.find('[name=Code]').val().trim();
    this.Name = $headerRole.find('[name=Name]').val().trim();
    this.DepartmentId = $headerRole.find('[name=DepartmentId]').val().trim();
    this.PageNum = pageNum || $footerRole.find('[name=PageNumber]').val();;
    this.Length = $footerRole.find('[name=PageLength]').val();
}


$(function () {
    var $pageUser = {
        init: function () {
            this.changePhongBan();
            this.initValidate();
            $pageUser.BtnSearchClick();
            $pageUser.GetList(1);
        },
        initValidate: function () {
            $formUser.validate({
                rules: {
                    UserName: {
                        required: true
                    },
                    FullName: {
                        required: true
                    },
                    DepartmentId: {
                        required: true
                    },
                    Position: {
                        required: true
                    },
                    Email: {
                        required: true,
                        email: true
                    },
                    Phone: {
                        phone: true
                    },
                    Password: {
                        required: () => {
                            return !($formUser.find('[name=Id]').val() > 0);
                        }
                    },
                    ConfirmPassword: {
                        required: () => {
                            return !($formUser.find('[name=Id]').val() > 0);
                        },
                        equalToIdZero: 'Password'
                    },
                },
                messages: {
                    UserName: {
                        required: "Tài khoản không được để trống"
                    },
                    FullName: {
                        required: "Tên đầy đủ không được để trống"
                    },
                    DepartmentId: {
                        required: "Vui lòng chọn phòng ban"
                    },
                    Position: {
                        required: "Vui lòng chọn chức vụ"
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
        changePhongBan: function () {
            $headerUser.find('select[name=DepartmentId]').off('change').on('change', () => {
                let id = $headerUser.find('select[name=DepartmentId]').val() || 0;
                var getResponse = AjaxConfigHelper.SendRequestToServer(`/${$router}/GetListTeamForUserSelect`, "GET", { 'departmentId': id });
                getResponse.then((res) => {
                    if (res.IsOk) {
                        let data = res.Body.Data || {};
                        let lstOption = data.ListOption || [];
                        $headerUser.find(`select[name=${data.SelectName}]`).html('<option value="">Chọn đội</option>');
                        for (option of lstOption) {
                            $headerUser.find(`select[name=${data.SelectName}]`).append(`<option value="${option.Value}" class="${option.IsShow == 1 ? "" : "hidden"}">${option.Display}</option>`);
                        }
                    }
                    else {

                    }
                })
            });
            $formUser.find('select[name=DepartmentId]').off('change').on('change', () => {
                let id = $formUser.find('select[name=DepartmentId]').val() || 0;
                var getResponse = AjaxConfigHelper.SendRequestToServer(`/${$router}/GetListTeamForUserSelect`, "GET", { 'departmentId': id });
                getResponse.then((res) => {
                    if (res.IsOk) {
                        let data = res.Body.Data || {};
                        let lstOption = data.ListOption || [];
                        $formUser.find(`select[name=${data.SelectName}]`).html('<option value="">Chọn đội</option>');
                        for (option of lstOption) {
                            $formUser.find(`select[name=${data.SelectName}]`).append(`<option value="${option.Value}" class="${option.IsShow == 1 ? "" : "hidden"}">${option.Display}</option>`);
                        }
                        let teamIdDefault = $formUser.find(`select[name=${data.SelectName}]`).data('default') || '';
                        if ($formUser.find(`select[name=${data.SelectName}]`).find(`option[value = "${teamIdDefault}"]`).length > 0)
                            $formUser.find(`select[name=${data.SelectName}]`).val(teamIdDefault);
                        else {
                            $formUser.find(`select[name=${data.SelectName}]`).val('');
                        }
                    }
                    else {

                    }
                })
            })
        },
        BtnSearchClick: function () {
            $headerUser.find('.btnSearch').on('click', function () { $pageUser.GetList(1); });
        },
        GetList: function (pageNum = 1) {

            let html = '';
            let search = new DataSearchUser(pageNum);

            var getResponse = AjaxConfigHelper.SendRequestToServer(`/${$router}/GetTableUser`, "GET", search);
            getResponse.then((res) => {
                if (res.IsOk) {
                    let data = res.Body.Data.Items || [];

                    if (data.length == 0) {
                        html = `<tr><td class="text-center" colspan="7"><span>Không có bản ghi</span></td></tr>`;
                        $tableUserBody.html(html);
                        return false;
                    }

                    let startIndex = res.Body.Pagination.StartIndex || 1;
                    for (let item of data) {
                        let htmlShow = item.Status == 1 ? '<span class="text-center green"><i class="text-center green icon-status-ico-tsd"></i></span>' : '';
                        html += `<tr class="TR_${item.Id}">` +
                            `<td class="text-center"><span>${startIndex}</span></td>` +
                            `<td class="text-center event-handle">` +
                            `<i class="icon-edit-ico-tsd btn-action btnEdit blue mr10" data-id="${item.Id}" title="Sửa"></i>` +
                            `<i class="icon-delete-ico-tsd btn-action btnDelete red mr10" data-id="${item.Id}" title="Xóa"></i>` +
                            `<i class="icon-retweet-ico-tsd btn-action btnResetPass aqua" data-id="${item.Id}" title="Reset Password"></i>` +
                            `</td>` +
                            `<td class=""><span>${item.UserName}</span></td>` +
                            `<td class=""><span>${item.FullName}</span></td>` +
                            `<td class=""><span>${item.DepartmentName}</span></td>` +
                            `<td class="text-center"><span>${item.TeamName || ''}</span></td>` +
                            `<td class="text-center">${htmlShow}</td>` +
                            `</tr>`;
                        startIndex++;
                    }
                    $tableUserBody.html(html);

                    $pagination.Set($footerUser, res.Body.Pagination, $pageUser.GetList);

                    $pageUser.ViewClick();
                    $pageUser.DeleteClick();
                    $pageUser.ResetPass();

                }
                else {

                }
            })
        },
        ViewClick: () => {
            $tableUserBody.find('.btnEdit').off('click').on('click', function () {
                let id = $(this).data('id');

                var getResponse = AjaxConfigHelper.SendRequestToServer(`/${$router}/GetItemUser`, "GET", { 'id': id });
                getResponse.then((res) => {
                    if (res.IsOk) {
                        let data = res.Body.Data || {};
                        $modalUser.find(`[name=TeamId]`).data('default', data["TeamId"]);
                        for (let prop in data) {
                            if (prop == 'IsShow' || prop == 'Status') {
                                $modalUser.find(`[name=${prop}]`).prop('checked', data[prop] == 1);
                                continue;
                            }
                            if (prop.indexOf('Time') > -1 || prop.indexOf('Ngay') > -1 || prop.indexOf('day') > -1) {
                                data[prop] = formatDateFrom_StringServer(data[prop]);
                            }
                            $modalUser.find(`[name=${prop}]`).val(data[prop]).trigger('change');
                        }
                        $modalUser.find('[name=UserName]').prop('disabled', true);
                        $modalUser.find('.password-area').addClass('hidden');
                        $modalUser.modal('show');
                    }
                    else {

                    }
                })
            })
        },
        DeleteClick: () => {
            $tableUserBody.find('.btnDelete').off('click').on('click', function () {

                let id = $(this).data('id');
                ConfirmDelete(function () {

                    var getResponse = AjaxConfigHelper.SendRequestToServer(`/${$router}/DeleteUser`, "POST", { 'id': id });
                    getResponse.then((res) => {
                        if (res.IsOk) {
                            ToastSuccess("Xóa");
                            $pageUser.GetList();
                        }
                        else {

                        }
                    })
                })

            })
        },
        ResetPass: () => {
            $tableUserBody.find('.btnResetPass').off('click').on('click', function () {

                let id = $(this).data('id');
                ConfirmWithCallBack(function () {
                    var getResponse = AjaxConfigHelper.SendRequestToServer(`/${$router}/ResetPassword`, "POST", { 'id': id });
                    getResponse.then((res) => {
                        if (res.IsOk) {
                            ToastSuccess("Đặt lại mật khẩu");
                        }
                    })
                }, 'Đặt lại mật khẩu cho tài khoản này?');

            })
        }
    };

    var $ChiTietUser = {
        init: function () {
            this.Save();
            this.ResetForm();
        },
        ResetForm: () => {
            $modalUser.on('hidden.modal.bs', () => {
                $formUser.find('input').val('');
                $formUser.find('input:checkbox').prop('checked', true);
                $modalUser.find(`[name=TeamId]`).data('default', '');
                $formUser.find('select').find('option:first-child').prop('selected', true).trigger('change');

                $modalUser.find('[name=UserName]').prop('disabled', false);
                $modalUser.find('.password-area').removeClass('hidden');

                $formUser.validate().resetForm();
                $formUser.find('.error').removeClass('error');
            })
        },
        GetDataInput: () => {
            if (!$formUser.valid()) {
                return false;
            }
            let data = GetFormDataToObject($formUser);
            data.Birthday = formatDateFromClientToServerEN(data.Birthday);
            return data;
        },
        Save: () => {
            $modalUser.find('#btn-save').off('click').on('click', () => {
                let data = $ChiTietUser.GetDataInput();
                if (!data)
                    return false;
                let action = data.Id > 0 ? 'UpdateUser' : 'CreateUser';
                var getResponse = AjaxConfigHelper.SendRequestToServer(`/${$router}/${action}`, "POST", data);
                getResponse.then((res) => {
                    if (res.IsOk) {
                        let actionSub = data.Id > 0 ? 'Cập nhật' : 'Thêm mới';
                        ToastSuccess(actionSub);
                        $pageUser.GetList(data.Id > 0 ? null : 1);
                        $modalUser.modal('hide');
                    }
                    else {

                    }
                })
            })
        }
    };


    var $pageRole = {
        init: function () {
            this.initValidate();
            $pageRole.BtnSearchClick();
            $pageRole.GetList(1);
        },
        initValidate: function () {
            $formRole.validate({
                rules: {
                    Code: {
                        required: true
                    },
                    Name: {
                        required: true
                    }
                },
                messages: {
                    Code: {
                        required: "Mã nhóm không được để trống"
                    },
                    Name: {
                        required: "Tên nhóm đầy đủ không được để trống"
                    }
                }
            })
        },
        BtnSearchClick: function () {
            $headerRole.find('.btnSearch').on('click', function () { $pageRole.GetList(1); });
        },
        GetList: function (pageNum = 1) {

            let html = '';
            let search = new DataSearchRole(pageNum);

            var getResponse = AjaxConfigHelper.SendRequestToServer(`/${$router}/GetTableRole`, "GET", search);
            getResponse.then((res) => {
                if (res.IsOk) {
                    let data = res.Body.Data.Items || [];

                    if (data.length == 0) {
                        html = `<tr><td class="text-center" colspan="7"><span>Không có bản ghi</span></td></tr>`;
                        $tableRoleBody.html(html);
                        return false;
                    }

                    let startIndex = res.Body.Pagination.StartIndex || 1;
                    for (let item of data) {
                        let htmlShow = item.IsShow == 1 ? '<span class="text-center green"><i class="text-center green icon-status-ico-tsd"></i></span>' : '';
                        let htmlHasPermission = item.HasPermission ? '<span class="text-center green"><i class="text-center green icon-status-ico-tsd"></i></span>' : '';
                        html += `<tr class="TR_${item.Id}">` +
                            `<td class="text-center"><span>${startIndex}</span></td>` +
                            `<td class="text-center event-handle">` +
                            `<i class="icon-audit-ico-tsd btn-action btnViewUser blue mr10" data-id="${item.Id}" title="Xem"></i>` +
                            `<i class="icon-edit-ico-tsd btn-action btnEdit blue mr10" data-id="${item.Id}" title="Sửa"></i>` +
                            `<i class="icon-delete-ico-tsd btn-action btnDelete red" data-id="${item.Id}" title="Xóa"></i>` +
                            `</td>` +
                            `<td class=""><span>${item.Code}</span></td>` +
                            `<td class=""><span>${item.Name}</span></td>` +
                            `<td class=""><span>${item.DepartmentName || ""}</span></td>` +
                            `<td class="text-center"><span>${htmlHasPermission}</span></td>` +
                            `<td class="text-center">${htmlShow}</td>` +
                            `</tr>`;
                        startIndex++;
                    }
                    $tableRoleBody.html(html);

                    $pagination.Set($footerRole, res.Body.Pagination, $pageRole.GetList);

                    $pageRole.ViewUserClick();
                    $pageRole.EditClick();
                    $pageRole.DeleteClick();

                }
                else {

                }
            })
        },
        ViewUserClick: () => {
            $tableRoleBody.find('.btnViewUser').off('click').on('click', function () {
                let id = $(this).data('id');
                $UserRoleModal.GetList(id);
            })
        },
        EditClick: () => {
            $tableRoleBody.find('.btnEdit').off('click').on('click', function () {
                let id = $(this).data('id');

                var getResponse = AjaxConfigHelper.SendRequestToServer(`/${$router}/GetItemRole`, "GET", { 'id': id });
                getResponse.then((res) => {
                    if (res.IsOk) {
                        let data = res.Body.Data.Role || {};
                        for (let prop in data) {
                            if (prop == 'IsShow' || prop == 'Status') {
                                $modalRole.find(`[name=${prop}]`).prop('checked', data[prop] == 1);
                                continue;
                            }
                            if (prop.indexOf('Time') > -1 || prop.indexOf('Ngay') > -1 || prop.indexOf('day') > -1) {
                                data[prop] = formatDateFrom_StringServer(data[prop]);
                            }
                            $modalRole.find(`[name=${prop}]`).val(data[prop]);
                        }
                        $modalRole.modal('show');

                        $SelectUserModal.LstUserSelected = res.Body.Data.LstUserId || [];
                        $SelectUserModal.CountSelected();

                        $SelectPermissionModal.LstPermissionSelected = res.Body.Data.LstPermissionId || [];
                        if ($SelectPermissionModal.LstPermissionSelected.length > 0) {
                            $modalRole.find('[name=StatusPQ]').removeClass('gray').addClass('green');
                        }
                        else {
                            $modalRole.find('[name=StatusPQ]').removeClass('green').addClass('gray');
                        }
                    }
                })
            })
        },
        DeleteClick: () => {
            $tableRoleBody.find('.btnDelete').off('click').on('click', function () {
                let id = $(this).data('id');
                ConfirmDelete(function () {

                    var getResponse = AjaxConfigHelper.SendRequestToServer(`/${$router}/DeleteRole`, "POST", { 'id': id });
                    getResponse.then((res) => {
                        if (res.IsOk) {
                            ToastSuccess("Xóa");
                            $pageRole.GetList();
                        }
                        else {

                        }
                    })
                })

            })
        }
    };
    var $ChiTietRole = {
        init: function () {
            this.Save();
            this.ResetForm();
            this.ViewUserClick();
        },
        ResetForm: () => {
            $modalRole.on('hidden.modal.bs', () => {
                $formRole.find('input').val('');
                $formRole.find('input:checkbox').prop('checked', true);
                $formRole.find('select').find('option:first-child').prop('selected', true);
                $formRole.validate().resetForm();
                $formRole.find('.error').removeClass('error');

                $modalRole.find('[name=StatusPQ]').removeClass('green').addClass('gray');
                $modalRole.find('[name=StatusUS]').removeClass('green').addClass('gray');

                $SelectUserModal.LstUserSelected = [];
                $SelectPermissionModal.LstPermissionSelected = [];
            })
        },
        GetDataInput: () => {
            if (!$formRole.valid()) {
                return false;
            }
            let data = GetFormDataToObject($formRole);
            data.UserIds = $SelectUserModal.LstUserSelected;
            data.PermissionIds = $SelectPermissionModal.LstPermissionSelected;
            return data;
        },
        Save: () => {
            $modalRole.find('#btn-save').off('click').on('click', () => {
                let data = $ChiTietRole.GetDataInput();
                if (!data)
                    return false;
                let action = data.Id > 0 ? 'UpdateRole' : 'CreateRole';
                var getResponse = AjaxConfigHelper.SendRequestToServer(`/${$router}/${action}`, "POST", data);
                getResponse.then((res) => {
                    if (res.IsOk) {
                        let actionSub = data.Id > 0 ? 'Cập nhật' : 'Thêm mới';
                        ToastSuccess(actionSub);
                        $pageRole.GetList(data.Id > 0 ? null : 1);
                        $modalRole.modal('hide');

                    }
                    else {

                    }
                })
            })
        },
        ViewUserClick: () => {
            $modalRole.find('.btn-selectuser').off('click').on('click', function () {
                $modalSelectUser.modal('show');
                $modalSelectUser.find('.btn-getuser:first-child').trigger('click');
            })
        }
    }

    var $SelectUserModal = {
        init: function () {
            this.ChooseDepartment();
            this.ResetModal();
            this.Search();
        },
        ResetModal: function () {
            $modalSelectUser.on('hidden.modal.bs', function () {
                $modalSelectUser.find('.btn-getuser:first-child').trigger('click');
                $modalSelectUser.find('input').val('');
            })
        },
        LstUserSelected: [],
        Search: function () {
            $modalSelectUser.find('.btnSearch').off('click').on('click', function () {
                $SelectUserModal.ChooseDepartment();
                $SelectUserModal.GetUsers();
            })
        },
        ChooseDepartment: function () {
            $modalSelectUser.find('.btn-getuser').off('click').on('click', function () {
                $modalSelectUser.find('.btn-getuser.active').removeClass('active');
                $(this).addClass('active');
                $SelectUserModal.GetUsers();
            })
        },
        GetUsers: function () {
            let id = $modalSelectUser.find('.btn-getuser.active').data('id') || 0;
            let userSearch = $modalSelectUser.find('[name=SearchUser]').val().trim();
            var getResponse = AjaxConfigHelper.SendRequestToServer(`/${$router}/GetListUserInDepartment`, "GET", { 'departmentId': id, 'userSearch': userSearch });
            getResponse.then((res) => {
                if (res.IsOk) {

                    let data = res.Body.Data || [];
                    let html = '';
                    if (data.length == 0) {
                        html = `<tr><td class="text-center" colspan="5"><span>Không có bản ghi</span></td></tr>`;
                        $tableUserInDepartment.html(html);
                        return false;
                    }

                    let startIndex = 1;
                    for (let item of data) {
                        let htmlShow = item.Status == 1 ? '<span class="text-center green"><i class="text-center green icon-status-ico-tsd"></i></span>' : '';
                        let checked = $SelectUserModal.LstUserSelected.includes(item.Id) ? "checked" : "";
                        html += `<tr class="TR_${item.Id}">` +
                            `<td><span><input type="checkbox" name="CHECKBOXITEM" data-id="${item.Id}" ${checked}></span></td>` +
                            `<td class="text-center"><span>${startIndex}</span></td>` +
                            `<td class=""><span>${item.UserName}</span></td>` +
                            `<td class=""><span>${item.PositionName}</span></td>` +
                            `<td class="text-center">${htmlShow}</td>` +
                            `</tr>`;
                        startIndex++;
                    }
                    $tableUserInDepartment.html(html);
                    $SelectUserModal.SelectedEvent();

                }
            })
        },
        SelectedEvent: function () {
            $tableUserInDepartment.find('[name="CHECKBOXITEM"]').off('change').on('change', function () {
                let id = $(this).data('id');
                if ($(this).is(':checked')) {
                    if (!$SelectUserModal.LstUserSelected.includes(id)) {
                        $SelectUserModal.LstUserSelected.push(id)
                    }
                }
                else {
                    $SelectUserModal.LstUserSelected = $.grep($SelectUserModal.LstUserSelected, function (value) {
                        return value !== id;
                    })
                }
                $SelectUserModal.CountSelected();
            })
        },
        CountSelected: function () {
            let count = $SelectUserModal.LstUserSelected.length;
            $modalSelectUser.find('#CountSelected').text(count);

            if (count > 0) {
                $modalRole.find('[name=StatusUS]').addClass('green').removeClass('gray');
            }
            else {
                $modalRole.find('[name=StatusUS]').removeClass('green').addClass('gray');
            }
        }

    }

    var $SelectPermissionModal = {
        init: function () {
            this.Confirm();
            this.ResetModal();
            this.ShowModal();
            this.Checked();
        },
        LstPermissionSelected: [],
        ResetModal: function () {
            $modalPermission.on('hidden.modal.bs', function () {
                $modalPermission.find('input:checkbox').prop('checked', false);
            })
        },
        ShowModal: function () {
            $modalPermission.on('shown.modal.bs', function () {
                $tablePermission.find('input:checkbox').each(function (i, e) {
                    let id = $(this).data('id');
                    if ($SelectPermissionModal.LstPermissionSelected.includes(id)) {
                        $(this).prop('checked', true).trigger('change');;
                    }
                    else {
                        $(this).prop('checked', false).trigger('change');;
                    }
                });
            })
        },
        Checked: function () {
            $modalPermission.find('[name=CHECKBOX_ALL]').on('change', function () {
                let checked = $(this).is(':checked');
                $modalPermission.find('[name=CHECKBOXALL_ROW]').not('[disabled]').prop('checked', checked).trigger('change');
            })
            $modalPermission.find('[name=CHECKBOXALL_ROW]').on('change', function () {
                let $cb = $(this);
                let $row = $(this).closest('tr');
                let checked = $(this).is(':checked');
                $row.find('input.item-role:checkbox').not('[disabled]').not($cb).prop('checked', checked).trigger('change');
            })

            $modalPermission.find('input.item-role:checkbox').on('change', function () {
                let $cb = $(this);
                let $row = $(this).closest('tr');
                let $modal = $(this).closest('tbody');
                let uncheckedRow = $row.find('input.item-role:checkbox').not(':checked') ;
                let uncheckedModal = $modal.find('input.item-role:checkbox').not(':checked');
                $row.find('[name=CHECKBOXALL_ROW]').not('[disabled]').not($cb).prop('checked', uncheckedRow.length == 0);
                $modalPermission.find('[name=CHECKBOX_ALL]').not('[disabled]').not($cb).prop('checked', uncheckedModal.length == 0);
            })
        },
        Confirm: function () {
            $modalPermission.find('#btn-save').off('click').on('click', function () {
                let checked = [];
                $tablePermission.find('input[type=checkbox]:checked').each(function (i, e) {
                    let id = $(this).data('id');
                    checked.push(id);
                });
                $modalPermission.modal('hide');
                $SelectPermissionModal.LstPermissionSelected = checked;

                if (checked.length > 0) {
                    $modalRole.find('[name=StatusPQ]').addClass('green').removeClass('gray');
                }
                else {
                    $modalRole.find('[name=StatusPQ]').removeClass('green').addClass('gray');
                }
            })
        }
    }

    var $UserRoleModal = {
        init: function () {
            this.closeModal();
        },
        closeModal: function () {
            $modalUserRole.on('hidden.bs.modal', function () {
                $modalUserRole.find('[name=UserSearch]').val('');
                html = `<tr><td class="text-center" colspan="6"><span>Không có bản ghi</span></td></tr>`;
                $tableUserRole.find('tbody').html(html);
            })
        },
        GetList: function (roleId) {
            var getResponse = AjaxConfigHelper.SendRequestToServer(`/${$router}/GetUserOfRole`, "GET", { 'roleId': roleId });
            getResponse.then((res) => {
                if (res.IsOk) {
                    let data = res.Body.Data || [];
                    let html = '';

                    if (data.length == 0) {
                        html = `<tr><td class="text-center" colspan="6"><span>Không có bản ghi</span></td></tr>`;
                        $tableUserRole.find('tbody').html(html);
                        $modalUserRole.modal('show');
                        return false;
                    }

                    let startIndex = 1;
                    for (let item of data) {
                        let htmlShow = item.Status == 1 ? '<span class="text-center green"><i class="text-center green icon-status-ico-tsd"></i></span>' : '';
                        html += `<tr class="TR_${item.Id}">` +
                            `<td class="text-center"><span>${startIndex}</span></td>` +
                            `<td class="text-center event-handle">` +
                            `<i class="icon-declaration-ico-tsd btn-action btnEditUserRole blue mr10" data-id="${item.Id}" data-roleid="${roleId}" title="Xem"></i>` +
                            `<i class="icon-delete-ico-tsd btn-action btnDelete red" data-id="${item.Id}" data-roleid="${roleId}" title="Xóa"></i>` +
                            `</td>` +
                            `<td class=""><span>${item.UserName}</span></td>` +
                            `<td class=""><span>${item.FullName}</span></td>` +
                            `<td class=""><span class="line-clamp-1" title="${item.DepartmentName}">${item.DepartmentName}</span></td>` +
                            `<td class="text-center">${htmlShow}</td>` +
                            `</tr>`;
                        startIndex++;
                    }
                    $tableUserRole.find('tbody').html(html);

                    $modalUserRole.find('#count').text(data.length);
                    $modalUserRole.modal('show');

                    $UserRoleModal.Search();
                    $UserRoleModal.DeleteClick();
                    $UserRoleModal.EditUserRole();
                }
                else {

                }
            })
        },
        DeleteClick: function () {
            $tableUserRole.find('.btnDelete').off('click').on('click', function () {
                let id = $(this).data('id');
                let roleId = $(this).data('roleid');
                var getResponse = AjaxConfigHelper.SendRequestToServer(`/${$router}/DeleteUserRole`, "POST", { 'roleId': roleId, 'userId': id });
                getResponse.then((res) => {
                    if (res.IsOk) {
                        $UserRoleModal.GetList(roleId);

                    }
                    else {

                    }
                })
            })
        },
        EditUserRole: function () {
            $tableUserRole.find('.btnEditUserRole').off('click').on('click', function () {
                let userId = $(this).data('id');
                let roleId = $(this).data('roleid');
                var getResponse = AjaxConfigHelper.SendRequestToServer(`/${$router}/GetPermissionOfUser`, "GET", { 'roleId': roleId, 'userId': userId });
                getResponse.then((res) => {
                    if (res.IsOk) {
                        var lstPermissionOfRole = res.Body.Data.LstPermissionRole || [];
                        var lstPermissionNotGranted = res.Body.Data.LstPermissionNotGranted || [];
                        var lstPermissionOfUser = res.Body.Data.LstPermissionOfUser || [];

                        $SelectUserPermissionModal.LstPermissionSelected = lstPermissionOfUser;
                        $tableUserPermission.find('input:checkbox.item-role').each(function (i, e) {
                            let id = $(this).data('id');

                            if (!lstPermissionOfRole.includes(id)) {
                                $(this).prop('disabled', true);
                            }
                        });
                        $modalUserPermission.data('userid', userId);
                        $modalUserPermission.data('roleid', roleId);
                        $modalUserPermission.modal('show');
                    }
                    else {

                    }
                })
            })
        },
        Search: function () {
            $modalUserRole.find('.btnSearch').off('click').on('click', function () {
                let textSearch = $modalUserRole.find('[name=UserSearch]').val() || '';
                textSearch = textSearch.toLowerCase();
                $tableUserRole.find('tbody tr').filter(function () {
                    $(this).toggle($(this).text().toLowerCase().indexOf(textSearch) > -1)
                })

                $tableUserRole.find('tbody tr').not('[style="display: none;"]').each(function (i, e) {
                    $(e).find('td:eq(0) span').text(Number(i)+1);
                })
            })
        }
    }

    var $SelectUserPermissionModal = {
        init: function () {
            this.Confirm();
            this.ResetModal();
            this.ShowModal();
            this.Checked();
        },
        LstPermissionSelected: [],
        ResetModal: function () {
            $modalUserPermission.on('hidden.modal.bs', function () {
                $modalUserPermission.find('input:checkbox').prop('checked', false);
                $SelectUserPermissionModal.LstPermissionSelected = [];
                $modalUserPermission.data('roleid', '');
                $modalUserPermission.data('userid', '');
            })
        },
        ShowModal: function () {
            $modalUserPermission.on('shown.modal.bs', function () {
                $tableUserPermission.find('input:checkbox').each(function (i, e) {
                    let id = $(this).data('id');
                    if ($SelectUserPermissionModal.LstPermissionSelected.includes(id)) {
                        $(this).prop('checked', true).trigger('change');
                    }
                    else {
                        $(this).prop('checked', false).trigger('change');
                    }
                });
            })
        },
        Checked: function () {
            $modalUserPermission.find('[name=CHECKBOX_ALL]').on('change', function () {
                let checked = $(this).is(':checked');
                $modalUserPermission.find('[name=CHECKBOXALL_ROW]').not('[disabled]').prop('checked', checked).trigger('change');
            })
            $modalUserPermission.find('[name=CHECKBOXALL_ROW]').on('change', function () {
                let $cb = $(this);
                let $row = $(this).closest('tr');
                let checked = $(this).is(':checked');
                $row.find('input.item-role:checkbox').not('[disabled]').not($cb).prop('checked', checked).trigger('change');
            })

            $modalUserPermission.find('input.item-role:checkbox').on('change', function () {
                let $cb = $(this);
                let $row = $(this).closest('tr');
                let $modal = $(this).closest('tbody');
                let uncheckedRow = $row.find('input.item-role:checkbox').not('[disabled]').not(':checked');
                let uncheckedModal = $modal.find('input.item-role:checkbox').not('[disabled]').not(':checked');
                $row.find('[name=CHECKBOXALL_ROW]').not('[disabled]').not($cb).prop('checked', uncheckedRow.length == 0);
                $modalUserPermission.find('[name=CHECKBOX_ALL]').not('[disabled]').not($cb).prop('checked', uncheckedModal.length == 0);
            })
        },
        Confirm: function () {
            $modalUserPermission.find('#btn-save').off('click').on('click', function () {
                let checked = [];
                $tableUserPermission.find('input[type=checkbox]:checked').each(function (i, e) {
                    let id = $(this).data('id');
                    checked.push(id);
                });
                $SelectUserPermissionModal.LstPermissionSelected = checked;

                let dataSend = {
                    RoleId: $modalUserPermission.data('roleid'),
                    UserId: $modalUserPermission.data('userid'),
                    PermissionIds: $SelectUserPermissionModal.LstPermissionSelected
                }
                console.log(dataSend);
                var getResponse = AjaxConfigHelper.SendRequestToServer(`/${$router}/UpdateUserPermission`, "POST", dataSend);
                getResponse.then((res) => {
                    if (res.IsOk) {
                        $modalUserPermission.modal('hide');
                    }
                    else {

                    }
                })

            })
        }
    }


    $pageUser.init();
    $pageRole.init();
    $ChiTietUser.init();
    $ChiTietRole.init();

    $SelectUserModal.init();
    $SelectPermissionModal.init();
    $SelectUserPermissionModal.init();

    $UserRoleModal.init();

});
var $header = $('.frame-search');
const $footer = $('.frame-footer');
const $table = $('#TBLDANHSACH');
const $tableBody = $table.find('tbody');
const $modal = $('#CHITIET');
const $modalUser = $('#UserRole');
const $router = "DHanhKhach";
const $form = $('#ModalForm');


function DataSearch(pageNum) {
    this.StartDate = formatDateFromClientToServerEN($header.find('[name=StartDate]').val());
    this.EndDate = formatDateFromClientToServerEN($header.find('[name=EndDate]').val());
    this.HoTen = $header.find('[name=HoTen]').val();
    this.SoGiayTo = $header.find('[name=SoGiayTo]').val();
    this.SoHieu = $header.find('[name=SoHieu]').val();
    this.NoiDen = $header.find('[name=NoiDen]').val();
    this.NoiDi = $header.find('[name=NoiDi]').val();
    this.PageNum = pageNum || $footer.find('[name=PageNumber]').val();
    this.PageSize = $footer.find('[name=PageLength]').val();
}

$(function () {
    var $page = {
        init: function () {
            $page.BtnSearchClick();
            $page.BtnExportClick();
            $page.GetList(1);
        },
        Self: $('.card'),
        BtnSearchClick: function () {
            $header.find('.btnSearch').on('click', function () { $page.GetList(1); });
        },
        BtnExportClick: function () {
            $page.Self.find('#btn-export').on('click', function () {

                let search = new DataSearch(1);
                search = $searchModal.getDataSearch(search);
                
                var getResponse = AjaxConfigHelper.SendRequestToServer(`/${$router}/ExportExcel`, "GET", search);
                getResponse.then((res) => {
                    if (res.IsOk) {
                        let fileGuid = res.Body.Data.FileGuid;
                        let fileName = res.Body.Data.FileName;
                        window.open(`/${$router}/Download?fileGuid=${fileGuid}&fileName=${fileName}`);
                    }
                })
            });
        },
        GetList: function (pageNum = 1) {

            let html = '';
            let search = new DataSearch(pageNum);
            search = $searchModal.getDataSearch(search);

            var getResponse = AjaxConfigHelper.SendRequestToServer(`/${$router}/GetTable`, "GET", search);
            getResponse.then((res) => {
                if (res.IsOk) {
                    if (search.IsViewDiChung == false) {
                        $('.is-view-dichung').addClass('hidden');
                    }
                    else {
                        $('.is-view-dichung').removeClass('hidden');
                    }
                    if (search.IsViewNgayDiGanNhat == false) {
                        $('.is-view-ngaygannhat').addClass('hidden');
                    }
                    else {
                        $('.is-view-ngaygannhat').removeClass('hidden');
                    }
                    if (search.IsViewSoKien == false) {
                        $('.is-view-sokien').addClass('hidden');
                    }
                    else {
                        $('.is-view-sokien').removeClass('hidden');
                    }


                    let data = res.Body.Data || [];

                    if (data.length == 0) {
                        let colCount = 13;
                        if (search.IsViewDiChung) {
                            colCount++;
                        }
                        if (search.IsViewNgayDiGanNhat) {
                            colCount++;
                        }
                        html = `<tr><td class="text-center" colspan="13"><span>Không có bản ghi</span></td></tr>`;
                        $tableBody.html(html);
                        return false;
                    }

                    let startIndex = res.Body.Pagination.StartIndex || 1;
                    for (let item of data) {
                        html += `<tr class="TR_${item.SOGIAYTO}">` +
                            `<td class="text-center"><span>${startIndex}</span></td>` +
                            `<td class="text-center">${item.SOHIEU}</td>` +
                            `<td class="text-center">${formatDateFromServer(item.FLIGHTDATE)}</td>` +
                            `<td class="">${(item.HO + ' ' + item.TENDEM + ' ' + item.TEN)}</td>` +
                            `<td class="text-center">${item.QUOCTICH}</td>` +
                            `<td class="">${item.SOGIAYTO}</td>` +
                            `<td class="text-center">${formatDateFromServer(item.NGAYSINH)}</td>` +
                            `<td class="text-center">${item.NOIDI}</td>` +
                            `<td class="text-center">${item.MANOIDI}</td>` +
                            `<td class="text-center">${item.MANOIDEN}</td>` +
                            `<td class="text-center">${item.NOIDEN}</td>` +
                            (search.IsViewSoKien ? `<td class="text-center is-view-sokien">${item.SoKien}</td>` : '') +
                            `<td class="">${item.HANHLY}</td>` +
                            (search.IsViewDiChung ? `<td class="text-center is-view-dichung">${item.SoNguoiDiCung}</td>` : '') +
                            (search.IsViewNgayDiGanNhat ? `<td class="text-center is-view-ngaygannhat">${item.NgayDiGanNhat_TXT}</td>` : '') +
                            `<td class="text-center"><span data-id="${item.SOGIAYTO}" data-chuyenbay="${item.IDCHUYENBAY}" class="fa fa-exclamation-triangle add-warning cursor" style="color:#ffc107;"></span></td>` +
                            `</tr>`;
                        startIndex++;
                    }
                    
                    $tableBody.html(html);

                    $pagination.Set($footer, res.Body.Pagination, $page.GetList);

                    $page.AddWarning();
                }
                else {

                }
            })
        },
        AddWarning: function () {
            $page.Self.find('.add-warning').off('click').on('click', function () {
                let sgt = $(this).data('id');
                let cb = $(this).data('chuyenbay');
                let dataSend = {
                    sogiayto: sgt,
                    idchuyenbay: cb
                }
                var getResponse = AjaxConfigHelper.SendRequestToServer(`/DHanhKhach/GetItem`, "GET", dataSend);
                getResponse.then((res) => {
                    if (res.IsOk) {
                        let data = res.Body.Data || {};
                        for (let prop in data) {
                            if (prop == 'Enable') {
                                $('#ObjectWarning').find(`[name=${prop}]`).prop('checked', data[prop] == 1);
                                continue;
                            }
                            if (prop.indexOf('Time') > -1 || prop.indexOf('Ngay') > -1) {
                                data[prop] = formatDateFromServer(data[prop]);
                            }
                            if (prop == 'GioiTinh') {
                                $('#ObjectWarning').find(`[type="radio"][name=${prop}][data-value=${data[prop]}]`).prop('checked', true);
                                continue;
                            }
                            $('#ObjectWarning').find(`[name=${prop}]`).val(data[prop]);
                        }
                        $('#ObjectWarning').modal('show');
                    }
                })
            })
        }
    };

    var $import = {
        init: function () {
            $import.ChooseFile();
            $import.FileChanged();
            $import.Import();
        },
        self: $('#ImportExcel'),
        ChooseFile: function () {
            $import.self.find('.btnChonFile').off('click').on('click', function () {
                $import.self.find('input[type=file]').trigger('click');
            })
        },
        FileChanged: function () {
            $import.self.find('input[type=file]').on('change', function () {
                var files = $import.self.find('input[type=file]')[0].files;
                if (files.length) {
                    let file = files[0];
                    $import.self.find('input[name=FileName]').val(file.name);
                }
            })
        },
        Import: function () {
            $import.self.find('#btn-import').off('click').on('click', function () {
                var files = $import.self.find('input[type=file]')[0].files;
                if (!files.length) {
                    return false;
                }
                let file = files[0];

                var getResponse = AjaxConfigHelper.SendRequestFileToServer(`/${$router}/ImportExcel`, "POST", file);
                getResponse.then((res) => {
                    if (res.IsOk) {
                        $page.GetList(1);
                        ToastSuccess("Import thành công");
                        $import.self.modal('hide');
                    }
                })

            })
        }
    }

    var $searchModal = {
        init: function () {
            $searchModal.closeModal();
            $searchModal.btnSearchClick();
        },
        Self: $('#SearchModal'),
        closeModal: function () {
            $searchModal.Self.on('hidden.modal.bs', function () {
                let badge = 0;
                $searchModal.Self.find('input, select').each(function (i, e) {
                    let name = e.name;
                    let value = $(e).val() || '';
                    if (e.type == 'checkbox') {
                        value = $(e).is(':checked');
                    }
                    if (value != '' && value != false) {
                        badge = badge + 1;
                    }
                })
                $page.Self.find('.badge').text(badge);
            })
        },
        btnSearchClick: function () {
            $searchModal.Self.find('#btn-search').off('click').on('click', function () {
                $searchModal.Self.modal('hide');
                $page.GetList(1);
            })
        },
        getDataSearch: function (dataSearch) {
            $searchModal.Self.find('input, select').each(function (i, e) {
                let name = e.name;
                let value = $(e).val() || '';
                if (e.type == 'checkbox') {
                    value = $(e).is(':checked');
                }
                dataSearch[name] = value;
            })
            return dataSearch;
        }
    }

    $page.init();
    $import.init();
    $searchModal.init();
});
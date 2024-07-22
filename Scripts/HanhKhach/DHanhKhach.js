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
    this.SoGiayTo = $header.find('[name=SoGiayTo]').val();
    this.SoHieu = $header.find('[name=SoHieu]').val();
    this.PageNum = pageNum || $footer.find('[name=PageNumber]').val();
    this.PageSize = $footer.find('[name=PageLength]').val();
}

$(function () {
    var $page = {
        init: function () {
            $page.BtnSearchClick();
            $page.GetList(1);
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
                            `<td class="">${item.HANHLY}</td>` +
                            `<td class="text-center"><span data-id="${item.SOGIAYTO}" data-chuyenbay="${item.ID_CHUYENBAY}" class="fa fa-exclamation-triangle add-warning cursor" style="color:#ffc107;"></span></td>` +
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
                console.log(sgt+ '/' + cb);
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

    $page.init();
    $import.init();
});
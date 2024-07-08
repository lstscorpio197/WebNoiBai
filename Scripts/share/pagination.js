const pageLengths = [5,15, 50, 75, 100, 250, 200, 300, 500];

$pagination = {
    defaltLength : 50,
    Set: ($container, httpMessagePagination, callBack) => {
        for (let prop in httpMessagePagination) {
            $container.find(`span[name=${prop}]`).text(httpMessagePagination[prop]);
        }
        $container.find(`select[name=PageLength]`).find(`option[value="${httpMessagePagination.NumberRowsOnPage}"`).prop('selected', true);
        $pagination.DrawSelectPageNumber($container, httpMessagePagination.TotalPage, httpMessagePagination.PageNumber);

        if (httpMessagePagination.PageNumber == 1) {
            $container.find('li.prev-page').addClass('disabled');
            $container.find('li.prev-page i').addClass('disabled');
        }
        else {
            $container.find('li.prev-page').removeClass('disabled');
            $container.find('li.prev-page i').removeClass('disabled');
        }

        if (httpMessagePagination.PageNumber == httpMessagePagination.TotalPage) {
            $container.find('li.next-page').addClass('disabled');
            $container.find('li.next-page i').addClass('disabled');
        }
        else {
            $container.find('li.next-page').removeClass('disabled');
            $container.find('li.next-page i').removeClass('disabled');
        }

        $pagination.ChangePageLength($container, callBack);
        $pagination.ChangePageNum($container, callBack);
        $pagination.NextPage($container, callBack);
        $pagination.PreviousPage($container, callBack);
    },
    SetDefalt: () => {
        $selectLength = $('.frame-footer .page-lengthOfPage [name=PageLength]');
        let options = '';
        for (let length of pageLengths) {
            options += `<option value="${length}">${length}</option>`;
        }
        $selectLength.html(options);
        $selectLength.find(`option[value=${$pagination.defaltLength}]`).prop('selected', true);
    },
    DrawSelectPageNumber: ($container, totalPage, pageNum) => {
        let options = '';
        for (let i = 0; i < totalPage; i++) {
            let selected = i + 1 == pageNum ? 'selected' : '';
            options += `<option value="${i + 1}" ${selected}>${i + 1}</option>`;
        }
        $container.find('[name=PageNumber]').html(options);
    },
    ChangePageLength: ($container,callBack) => {
        $container.find(`select[name=PageLength]`).off('change').on('change', () => {
            callBack();
        })
    },
    NextPage: ($container, callBack) => {
        $container.find('li.next-page').not('.disabled').off('click').on('click', () => {
            let pageNum = Number($container.find(`select[name=PageNumber]`).val());
            callBack(pageNum+1);
        })
    },
    PreviousPage: ($container, callBack) => {
        $container.find('li.prev-page').not('.disabled').off('click').on('click', () => {
            let pageNum = Number($container.find(`select[name=PageNumber]`).val());
            callBack(pageNum - 1);
        })
    },
    ChangePageNum: ($container, callBack) => {
        $container.find(`select[name=PageNumber]`).off('change').on('change', () => {
            let pageNum = $container.find(`select[name=PageNumber]`).val();
            callBack(pageNum);
        })
    }
}
$pagination.SetDefalt();
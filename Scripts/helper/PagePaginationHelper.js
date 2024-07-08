var PagePaginationHelper = {
    LoadPaginationPage: function ($page, _callBackLoad, lengthTotal, lengthCurrent, pageCurrent, lengthOfPage) {
        pageCurrent = parseInt(pageCurrent);
        var pageExtral = lengthTotal % lengthOfPage;
        var TotalPages = Math.floor(lengthTotal / lengthOfPage);
        if (pageExtral > 0) { TotalPages += 1; }
        $page.find('.pagination-des').html('- Trang <span class="color-blue">' + pageCurrent + '/ ' + TotalPages + '</span>');
        //Nếu hơn 10 trang thì chỉ lấy đến trang thứ 10, sau đấy mỗi sự kiện click thì tính tiếp
        $page.find('input[name=TOTALPAGES]').val(TotalPages);
        if (TotalPages > 10) {
            $page.find('input[name=TOTALPAGES]').val(TotalPages);
            TotalPages = 10;
        }
        $page.find('.countItem').text(lengthCurrent + '/' + lengthTotal);
        var maxPage = parseInt($page.find('input[name=TOTALPAGES]').val());
        if (maxPage > 10 && pageCurrent > 5) {
            var htmlPage = '<ul class="pagination">';
            htmlPage += '<li class="cursor"><span class="first-page">Trang đầu</span></li><li class="cursor"><span><i class="fa fa-angle-double-left"></i></span></li>';
            for (let i = pageCurrent - 5; i < pageCurrent + 5; i++) {
                if (i <= maxPage) {
                    if (pageCurrent == i) { htmlPage += '<li class="active"><span>' + i + '</span></li>'; }
                    else { htmlPage += '<li class="cursor"><span>' + i + '</span></li>'; }
                }
            }
            if (pageCurrent < maxPage) {
                htmlPage += '<li class="cursor" title="Tổng có ' + maxPage + ' trang"><span class="more">...</span></li>';
                htmlPage += '<li class="cursor"><span><i class="fa fa-angle-double-right"></i></span></li>';
                htmlPage += '<li class="cursor"><span class="last-page" id="id_' + maxPage + '">Trang cuối</span></li>';
            }
            htmlPage += '</ul>';
            $page.find('div.pagination-page').html(htmlPage);
        }
        else if (lengthTotal > lengthCurrent) {
            if (TotalPages > 1) {
                var htmlPage = ' <ul class="pagination">';
                if (pageCurrent > 1)
                    htmlPage += '<li class="cursor"><span class="first-page">Trang đầu</span></li><li class="cursor"><span><i class="fa fa-angle-double-left"></i></span></li>';
                for (let i = 1; i <= TotalPages; i++) {
                    if (pageCurrent == i) { htmlPage += '<li class="active"><span>' + i + '</span></li>'; }
                    else { htmlPage += '<li class="cursor"><span>' + i + '</span></li>'; }
                }
                if (pageCurrent < TotalPages) {
                    htmlPage += '<li class="cursor"><span><i class="fa fa-angle-double-right"></i></span></li><li class="cursor"><span class="last-page" id="id_' + maxPage + '">Trang cuối</span></li>';
                }
                htmlPage += '</ul>';
            }
            $page.find('div.pagination-page').html(htmlPage);
            //OBJ_DM_DV.ListenEvent();
            //$('html,body').scrollTop(0);
        }
        PagePaginationHelper.SetPaginationClick($page, _callBackLoad);
        //let $ItemFrame =  $('.vnaccs-site .frame-body') || $('.vnaccs-site .frame-data') ;
        let $ItemFrame = $page.find('table').parent();
        if ($ItemFrame.length > 0) {
            $ItemFrame.scrollTop(0);
        }
        else {
            $('html,body').scrollTop(0);
        }
    },
    SetPaginationClick: function ($page, _callBackLoad) {
        $page.find('div.pagination-page ul li span').off('click').on('click', function () {
            if (!$(this).hasClass('more')) {
                $('html,body').scrollTop(0);
                if (!$(this).parent().hasClass('active')) {
                    if ($(this).find('.fa-angle-double-right').length > 0) {
                        _callBackLoad(parseInt($page.find('div.pagination-page ul li.active span').text()) + 1);
                    } else if ($(this).find('.fa-angle-double-left').length > 0) {
                        _callBackLoad(parseInt($page.find('div.pagination-page ul li.active span').text()) - 1);
                    } else if ($(this).hasClass('first-page')) {
                        _callBackLoad(1);
                    } else if ($(this).hasClass('last-page')) {
                        _callBackLoad(parseInt($(this).attr('id').replace('id_', '')));
                    } else {
                        var _getPage = $(this).text();
                        _callBackLoad(parseInt(_getPage));
                    }
                }
            }
        });
    }
};

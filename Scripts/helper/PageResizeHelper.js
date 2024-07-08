var PageResizeHelper = {
    outerHeight: function () { return window.outerHeight; },
    innerHeight: function () { return window.innerHeight; },
    Selector: {
        'cardbody': $('.vnaccs-site .card-body'),
        'tabs': $('.vnaccs-site .card-body .nav.nav-tabs'),
        'cardbodyFrameHeader': $('.vnaccs-site .card-body .frame-header'),
        'cardbodyFrameBody': $('.vnaccs-site .card-body .frame-body'),
        'modalbody': $('.modal .modal-body'),
        'modalbodyFull': $('.modal.full .modal-body'),
        'modalbody_body': $('.modal .modal-body .modal-body-body'),
        'menuHorizontal': $('.navbar.top-navbar'),
        'menuVertical': $('body.mx-vertical')
    },
    LoadDefaultSize: function () {
        let heightHorizontal = 0;
        if (this.Selector.menuHorizontal.length > 0) {
            heightHorizontal = this.Selector.menuHorizontal.find('.navbar-collapse').height();
            this.Selector.menuHorizontal.find('.vnaccs-site').css("margin-top", heightHorizontal + 8);
        }
        this.Selector.cardbody.css("max-height", PageResizeHelper.innerHeight() - (this.Selector.menuHorizontal.length > 0 ? 50 : 0) + 35 - heightHorizontal);
        this.Selector.modalbody.css("max-height", PageResizeHelper.innerHeight() - 150 - heightHorizontal);
        this.Selector.modalbodyFull.css("max-height", PageResizeHelper.innerHeight() - 100 - heightHorizontal);

        this.LoadHeightFrameBody();
        this.LoadHeightFrameBody_IntoTabs();
        this.LoadWidthFastSearch();
        this.LoadpaddingTopModal();
        this.LoadHeightModalBody_Body();
        this.ScrollTable();
    },
    LoadHeightFrameBody: function () {
        let navbarHeaderHeight = $('.navbar-collapse').height();
        let cardHeaderHeight = (this.Selector.menuVertical.length > 0 ? 15 : $('.card-header').height());
        let frameHeaderHeight = $('.card-body .frame-header').height();
        frameHeaderHeight = (frameHeaderHeight < 30 ? 30 : frameHeaderHeight)
        let mainFooterHeight = ($('.main-footer').height() || 0) + $('.card-body .frame-footer').height();
        if (mainFooterHeight == 0) { mainFooterHeight = 10; }
        let heightHorizontal = (this.Selector.menuHorizontal.length > 0 ? this.Selector.menuHorizontal.find('.navbar-collapse').height() + 20 : 0);
        let height_nav_tabs = 0;
        if (this.Selector.cardbodyFrameBody.parents('.tab-pane').length >= 1) {
            height_nav_tabs = this.Selector.cardbody.find('.nav.nav-tabs').height() + 20;
            if (heightHorizontal > 0) { heightHorizontal += 35; }
        }
        if (this.Selector.cardbodyFrameBody.length == 1) {
            this.Selector.cardbodyFrameBody.css("max-height", PageResizeHelper.innerHeight() - navbarHeaderHeight - cardHeaderHeight - frameHeaderHeight - mainFooterHeight - heightHorizontal - height_nav_tabs + 80);
        } else if (this.Selector.cardbodyFrameBody.length > 1) {
            this.Selector.cardbodyFrameBody.each((index, itemBody) => {
                frameHeaderHeight = $(itemBody).parent().find('.frame-header').height();
                mainFooterHeight = $(itemBody).parent().find('.frame-footer').height();
                $(itemBody).css("max-height", PageResizeHelper.innerHeight() - navbarHeaderHeight - cardHeaderHeight - frameHeaderHeight - mainFooterHeight - heightHorizontal - height_nav_tabs + 50);
            });
        }
    },
    LoadHeightFrameBody_IntoTabs: function () {
        PageResizeHelper.Selector.tabs.find('li').click(function () {
            setTimeout(function () { PageResizeHelper.LoadHeightFrameBody(); PageResizeHelper.LoadWidthFastSearch(); }, 500);
        });
    },
    LoadpaddingTopModal: function () {
        let heightHorizontal = (PageResizeHelper.Selector.menuHorizontal.find('.navbar-collapse').height() || 40) + 10;
        $('.modal').css('padding-top', heightHorizontal - 5);
    },
    LoadHeightModalBody_Body: function () {
        this.Selector.modalbody_body.each(function (index, item_modaBody) {           
            let $item_modaBody = $(item_modaBody);
            let $parentModal = $item_modaBody.parents('.modal');
            let heightModalHeader = $parentModal.find('.modal-header').height();

            let modalBodyHeader = $item_modaBody.parent().find('.modal-body-header'), heightModalBodyHeader = 0;
            if (modalBodyHeader.length > 0) {
                heightModalBodyHeader = modalBodyHeader.height() || 70;
            }
            if ($item_modaBody.parents('td').length > 0 || $item_modaBody.parents('.tab-content').length > 0) {
                heightModalBodyHeader = heightModalBodyHeader + 80;
            }
            if ($item_modaBody.parents('.modal.full').length > 0) {
                heightModalBodyHeader = modalBodyHeader.height() || ($item_modaBody.parent().find('.modal-body-footer ul.pagination').length > 0 ? 45 : 0);
            }
            if ($item_modaBody.parents('.modal.left').length > 0) {
                heightModalBodyHeader = 30;
            }
            let modalBodyFotter = $item_modaBody.parent().find('.modal-body-fotter');
            let heightmodalBodyFotter = 0;
            if (modalBodyFotter.length > 0) { heightmodalBodyFotter = modalBodyFotter.height(); }

            let heightExtension = ($item_modaBody.parents('.modal.full').length > 0 ? -40 : 0);
            heightExtension = ($item_modaBody.parents('.modal.slide').length > 0 ? 100 : heightExtension);
            $item_modaBody.css("max-height", PageResizeHelper.innerHeight() - 250 - heightModalHeader - heightModalBodyHeader - heightmodalBodyFotter  + heightExtension);
        });
        //let modalBodyHeader = this.Selector.modalbody_body.parent().find('.modal-body-header'), heightModalBodyHeader = 0;
        //if (modalBodyHeader.length > 0) {
        //    heightModalBodyHeader = modalBodyHeader.height() || 70;
        //}
        //if (this.Selector.modalbody_body.parents('td').length > 0 || this.Selector.modalbody_body.parents('.tab-content').length > 0) {
        //    heightModalBodyHeader = heightModalBodyHeader + 80;
        //}
        //let modalBodyFotter = this.Selector.modalbody_body.parent().find('.modal-body-fotter'), heightmodalBodyFotter = 0;
        //if (modalBodyFotter.length > 0) { heightmodalBodyFotter = modalBodyFotter.height(); }
        //this.Selector.modalbody_body.css("max-height", PageResizeHelper.innerHeight() - 200 - heightModalBodyHeader - heightmodalBodyFotter + (this.Selector.modalbody_body.parents('.modal.slide').length > 0 ? 100 : 0));
        this.Selector.modalbody_body.css("overflow-y", "auto");
    },
    LoadWidthFastSearch: function () {
        let $searchFast = $('.card-body .frame-header .search-fast');
        if ($searchFast.length <= 0) return;
        $searchFast.each(function (index, item) {
            let $frameHeader = $(item).parents('.frame-header');
            let frameHeaderLeftWidth = $frameHeader.find('.pull-left').width();
            let minLeftWidth = 200;
            $(item).width(frameHeaderLeftWidth < minLeftWidth ? minLeftWidth : frameHeaderLeftWidth);
        });
        //Minhmx Update 230720
        //$searchFast.find("input[name=HighLightText]").bind('keyup', function (e) {
        //    var text = $.trim($(this).val().trim());
        //    if (text !== '' && text !== ' ') { var pattern = new RegExp(text, "gi"); } 
        //    $('table tbody tr td:not(.event-handle)').each(function (i) {
        //        var $td = $(this);
        //        var orgText = $td.text();
        //        orgText = orgText.replace(pattern, function ($Content) {
        //            return "<span class='highlight'>" + $Content + "</span>";
        //        });                  
        //        $td.html(orgText);
        //    });
        //});
        $('.card-body .frame-header').find("input[name=HighLightText]").bind('keyup', function (e) {
            var text = $.trim($(this).val().trim());
            if (text == '' || text.replace(/ /g, '') == '') {
                $('table tbody tr').removeClass('hidden');
            }
            else {
                var pattern = new RegExp(text, "gi");
            }
            $('table tbody tr td:not(.event-handle)').each(function (i) {
                var $td = $(this);
                var orgText = $td.text();
                orgText = orgText.replace(pattern, function ($Content) {
                    return "<span class='highlight'>" + $Content + "</span>";
                });
                $td.html(orgText);
            });
            if (text != '') { $('table tbody tr').addClass('hidden'); $('span.highlight').parents('tr').removeClass('hidden'); }
        });
        //Minhmx Update 230720 End
    },
    ScrollTable: function () {
        $('table').parent().on('scroll', function () {
            let Self = $(this);
            let vertical = Self.scrollTop();
            let horizontal = Self.scrollLeft();
            Self.find('table .sticky-col').removeClass('active');
            if (horizontal > 0) {
                Self.find('table .sticky-col').addClass('active');
            }
            //console.log('horizontal:' + horizontal + ', vertical:' + vertical);
        });
    }
};

var TableHelper = {
    Self: null,
    CheckBoxAll: null,
    CheckBoxItem: null,
    Init: function ($CheckBoxAll, $CheckBoxItem) {
        this.CheckBoxAll = $CheckBoxAll || $('table input[type=checkbox][name=CHECKBOX_ALL]');
        ///this.CheckBoxItem = $CheckBoxItem || $('table input[type=checkbox][name=CHECKBOX_ITEM]');
        this.TableEvent();
    },
    TableEvent: function () {
        this.CheckBoxChangeDefault();
        this.TrEventClick(true);
    },
    CheckBoxChangeDefault: function () {
        $(TableHelper.CheckBoxAll).off('change').on("change", function () {
            var $tblParent = $(this).parents('table');
            var IsChecked = $(this).is(':checked');
            if (IsChecked) {
                $.each($tblParent.find('input[name=CHECKBOXITEM]'), function (index, item) { $(item).prop("checked", true).trigger('change'); });
            } else {
                $.each($tblParent.find('input[name=CHECKBOXITEM]'), function (index, item) { $(item).prop("checked", false); }).trigger('change');
            }
        });
    },
    CheckBoxTableOnChange: function ($CheckBoxAll, $CheckBoxItem) {
        $CheckBoxAll.off('change').on("change", function () {
            let $tblParent = $(this).parents('table');
            let IsChecked = $(this).is(':checked');
            if (IsChecked) {
                $.each($tblParent.find('input[name=' + $CheckBoxItem.attr('name') + ']'), function (index, item) { $(item).prop("checked", true).trigger('change'); });
            } else {
                $.each($tblParent.find('input[name=' + $CheckBoxItem.attr('name') + ']'), function (index, item) { $(item).prop("checked", false).trigger('change'); });
            }
        });
        $CheckBoxItem.off('change').on("change", function () {
            let $tblParent = $(this).parents('table');
            let IsChecked = $(this).is(':checked');
            let lenChecked = $tblParent.find('input[name=' + $CheckBoxItem.attr('name') + ']:checked').length,
                lenCheckBox = $tblParent.find('input[name=' + $CheckBoxItem.attr('name') + ']').length;
            if (!IsChecked && lenChecked < lenCheckBox) {
                $CheckBoxAll.prop('checked', false);
            }
            if (IsChecked && lenChecked == lenCheckBox) {
                $CheckBoxAll.prop('checked', true);
            }
        });
    },
    EventCheckBoxTableRow: function ($CheckBoxRow) {
        $CheckBoxRow.off('change').on("change", function () {
            let $trParent = $(this).parents('tr');
            let IsChecked = $(this).is(':checked');
            if (IsChecked) {
                $.each($trParent.find('input[type=checkbox]'), function (index, item) { $(item).prop("checked", true).trigger('change'); });
            } else {
                $.each($trParent.find('input[type=checkbox]'), function (index, item) { $(item).prop("checked", false).trigger('change'); });
            }
        });
    },
    EventCheckBox_EnableRowItem: function ($CheckBoxRow) {
        $CheckBoxRow.off('change').on("change", function () {
            let $trParent = $(this).parents('tr'), checboxName = $(this).attr('name');
            let IsChecked = $(this).is(':checked');
            if (IsChecked) {
                $trParent.find('input:not([name=' + checboxName + '])').prop("disabled", false);
            } else {
                $trParent.find('input:not([name=' + checboxName + '])').prop("disabled", true);
                $trParent.find('input:not([name=' + checboxName + '])').prop("readonly", false);
                $trParent.find('input[type=checkbox]:not([name=' + checboxName + '])').prop("checked", false).trigger('change');
                $trParent.find('input[type=radio]:not([name=' + checboxName + '])').prop("checked", false).trigger('change');
                $trParent.find('input[type=text]').val('');
                $trParent.find('textarea').val('');
                $trParent.find('select option:first-child').prop('selected', true);
            }
        });
    },
    TrEventClick: function (IsEnableClick, $parentTbl) {
        IsEnableClick = (typeof (IsEnableClick) == "boolean" ? IsEnableClick : true);
        if (!IsEnableClick) {
            return false;
        }
        $parentTbl = ($parentTbl || $('table'));
        $parentTbl.find('tbody tr:not(.not-bkg-yellow)').on('click', function () {
            if ($(this).parents('.theodoi-tokhai').length > 0) return;
            let $tbl = $(this).parents('table');
            let $tr = $tbl.find('tbody tr');
            $tr.removeClass('bkg-yellow');
            $(this).addClass('bkg-yellow');
        });
        $parentTbl.find('tbody tr').off('dblclick').on('dblclick', function () {
            if ($(this).parents('.theodoi-tokhai').length > 0) return;
            let $tbl = $(this).parents('table');
            let $tr = $tbl.find('tbody tr');
            $tr.removeClass('bkg-yellow');
        });
    },
    HasScroll: function ($FrameAroundTable, TableHelper, IsEnableClick) {
        $FrameAroundTable.removeClass('has-scroll');
        let heightFrame = $FrameAroundTable.height();
        let heightTable = TableHelper.height();
        if (heightTable >= heightFrame) {
            $FrameAroundTable.addClass('has-scroll');
        }
        this.TrEventClick(IsEnableClick, $FrameAroundTable);
        this.LoadScrollStickyColumn(TableHelper);
    },
    LoadScrollStickyColumn: function (TableHelper) {
        let $trItem = TableHelper.find('tr:first-child');
        // Cách tính class sticky-col CŨ
        if ($trItem.find('.sticky-col.third-col').length > 0) {
            let widthFirstColumn = $trItem.find('.sticky-col.first-col').width();
            let widthSecondColumn = $trItem.find('.sticky-col.second-col').width();
            TableHelper.find('.sticky-col.third-col').css('left', widthFirstColumn + widthSecondColumn + 11);
        }
        if ($trItem.find('.sticky-col.fourth-col').length > 0) {
            let widthFirstColumn = $trItem.find('.sticky-col.first-col').width();
            let widthSecondColumn = $trItem.find('.sticky-col.second-col').width();
            let widththirdColumn = $trItem.find('.sticky-col.third-col').width();
            TableHelper.find('.sticky-col.fourth-col').css('left', widthFirstColumn + widthSecondColumn + widththirdColumn + 17);
        }
        // KẾT THÚC CŨ
        //Cách tính class sticky-col MỚI
        let arrWidthCol = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0];
        let arrLen = arrWidthCol.length, widthOther = 0;
        for (let i = 1; i <= arrLen; i++) {
            let ItemCol = TableHelper.find('.sticky-col.sticky-col-' + i);
            if (ItemCol.length == 0) continue;
            arrWidthCol[i] = ItemCol.width();
            if (i == 1) {
                ItemCol.css('left', -1);
            }
            else if (i == 2) {
                ItemCol.css('left', 33);
            }
            else {
                let sub_arrWidthCol = arrWidthCol.slice(1, i);
                let sum_sub_array = sub_arrWidthCol.reduce(function (a, b) { return a + b; });
                widthOther = (i == 3 ? 11 : widthOther + 6);// Left cộng thêm 6px cho mỗi col, bắt đầu từ sticky-col-3
                sum_sub_array = sum_sub_array + widthOther;
                ItemCol.css('left', sum_sub_array);
            }
            //if (i == 3) { ItemCol.css('left', arrWidthCol[i - 1] + arrWidthCol[i - 2] + 11); console.log("i=3: " + (arrWidthCol[i - 1] + arrWidthCol[i - 2] + 11)); }
            //if (i == 4) { ItemCol.css('left', arrWidthCol[i - 1] + arrWidthCol[i - 2] + arrWidthCol[i - 3] + 17); console.log("i=4: " + (arrWidthCol[i - 1] + arrWidthCol[i - 2] + arrWidthCol[i - 3] + 17)); }
            //if (i == 5) { ItemCol.css('left', arrWidthCol[i - 1] + arrWidthCol[i - 2] + arrWidthCol[i - 3] + arrWidthCol[i - 4] + 23); console.log("i=5: " + (arrWidthCol[i - 1] + arrWidthCol[i - 2] + arrWidthCol[i - 3] + arrWidthCol[i - 4] + 23)); }
        }
        // KẾT THÚC MỚI
    },
    ShowHighlightText: function (text, TableHelper) {
        if (text == '' || text.replace(/ /g, '') == '') { return; }
        var pattern = new RegExp(text, "gi");
        TableHelper.find('tbody tr td:not(.event-handle)').each(function (i) {
            var $td = $(this);
            var orgText = $td.text();
            orgText = orgText.replace(pattern, function ($Content) {
                return "<span class='highlight'>" + $Content + "</span>";
            });
            $td.html(orgText);
        });
    },
    GetPageNumActive: function ($parentTag) {
        return $parentTag.find('.pagination li.active span').text() || 1;
    },
    SearchFast: function (TableHelper) {
        let $parentTable = TableHelper.parent();
        $parentTable.find("input[name=ContentSearchFast]").bind('keyup', function (e) {
            let $tblSearch = $(this).parent().find('table');
            var text = $.trim($(this).val().trim());
            if (text == '' || text.replace(/ /g, '') == '') {
                $tblSearch.find('tbody tr').removeClass('hidden');
            }
            else {
                var pattern = new RegExp(text, "gi");
            }
            $tblSearch.find('tbody tr td:not(.event-handle)').each(function (i) {
                var $td = $(this);
                var orgText = $td.text();
                orgText = orgText.replace(pattern, function ($Content) {
                    return "<span class='highlight'>" + $Content + "</span>";
                });
                $td.html(orgText);
            });
            if (text != '') {
                $tblSearch.find('tbody tr').addClass('hidden'); $tblSearch.find('span.highlight').parents('tr').removeClass('hidden');
            }
        });
    },
    SearchFast2: function (TableHelperSearch, $InputSearch) {
        $InputSearch.bind('keyup', function (e) {
            var text = $.trim($(this).val().trim());
            if (text == '' || text.replace(/ /g, '') == '') {
                TableHelperSearch.find('tbody tr').removeClass('hidden');
            }
            else {
                var pattern = new RegExp(text, "gi");
            }
            TableHelperSearch.find('tbody tr td:not(.event-handle)').each(function (i) {
                var $td = $(this);
                var orgText = $td.text();
                orgText = orgText.replace(pattern, function ($Content) {
                    return "<span class='highlight'>" + $Content + "</span>";
                });
                $td.html(orgText);
            });
            if (text != '') {
                TableHelperSearch.find('tbody tr').addClass('hidden'); TableHelperSearch.find('span.highlight').parents('tr').removeClass('hidden');
            }
        });
    },
};

var FormatInputHelper = {
    Init: function () {
        FormatInputHelper.FormatInputDate();
        FormatInputHelper.FormatInputNumber();
        FormatInputHelper.FormatDateTime();
        FormatInputHelper.FormatEmail();
        FormatInputHelper.FormatTextName();
        FormatInputHelper.FormatInputRequired();
    },
    FormatInputNumber: function () {
        //Reference: https://nosir.github.io/cleave.js/
        $('.input-number').each(function () {
            var cleave = new Cleave($(this), {
                numeral: true,
                numeralThousandsGroupStyle: 'thousand',
                numeralDecimalScale: 4
            });
        });
        $('.input-number-int').each(function () {
            var cleave = new Cleave($(this), {
                numeral: true,
                numeralThousandsGroupStyle: 'thousand',
                numeralDecimalScale: 0
            });
        });
        $('input.only-number').each(function () {
            $(this).ForceNumericOnly();
        });
    },
    FormatInputDate: function () {
        //$('.input-date-month').each(function () {
        //    var cleave = new Cleave($(this), {
        //        date: true,
        //        datePattern: ['m', 'Y']
        //    });
        //});
        //$('.input-date').each(function () {
        //    var cleave = new Cleave($(this), {
        //        date: true,
        //        datePattern: ['d', 'm', 'Y']
        //    });
        //});
        //$('.input-date-hour').each(function () {
        //    new Cleave($(this), {
        //        blocks: [2, 2, 4, 2, 2],
        //        delimiters: ['/', '/', ' ', ':']
        //    });
        //});
        //$('.input-date-hms').each(function () {
        //    new Cleave($(this), {
        //        blocks: [2, 2, 4, 2, 2, 2],
        //        delimiters: ['/', '/', ' ', ':', ':']
        //    });
        //});
        //$('.input-hour').each(function () {
        //    new Cleave($(this), {
        //        blocks: [2, 2],
        //        delimiters: [':']
        //    });
        //});
        $.fn.datetimepicker.dates['vi'] = {
            days: ["CN", "Thứ 2", "Thứ 3", "Thứ 4", "Thứ 5", "Thứ 6", "Thứ 7", "CN"],
            daysShort: ["CN", "Hai", "Ba", "Tư", "Năm", "Sáu", "Bảy", "CN"],
            daysMin: ["CN", "Hai", "Ba", "Tư", "Năm", "Sáu", "Bảy", "CN"],
            months: ["Tháng 1", "Tháng 2", "Tháng 3", "Tháng 4", "Tháng 5", "Tháng 6", "Tháng 7", "Tháng 8", "Tháng 9", "Tháng 10", "Tháng 11", "Tháng 12"],
            monthsShort: ["T1", "T2", "T3", "T4", "T5", "T6", "T7", "T8", "T9", "T10", "T11", "T12"],
            today: "Hôm nay"
        };
        $('.is_Datetimepicker').each(function () {
            $($(this)).datetimepicker({
                format: "dd/MM/yyyy",
                showToday: true,
                language: 'vi'
            }).on('changeDate', function (ev) {
                $(this).datetimepicker('hide');
            });
        });
        $('.is_Datetimepicker_hours ').each(function () {
            $($(this)).datetimepicker({
                format: "dd/MM/yyyy hh:mm",
                showToday: true,
                language: 'vi'
            }).on('changeDate', function (ev) { $(this).datetimepicker('hide'); });
        });
        //$('form input.input-date').focusout(function () {
        //    $('form').bootstrapValidator('revalidateField', $(this).attr('name'));
        //});
        //$('form input.input-date-hour').focusout(function () {
        //    $('form').bootstrapValidator('revalidateField', $(this).attr('name'));
        //});

        //$.datetimepicker.setLocale('vi');
        //$('.is_Datetimepicker').each(function () {
        //    $(this).find('input').attr('autocomplete', 'off');
        //    $($(this)).find('input').datetimepicker({
        //        format: "d/m/Y",
        //        timepicker: false,
        //        scrollInput: false,
        //    }).on('changeDate', function (ev) { $(this).datetimepicker('hide'); });
        //    $(this).find('span').on('click', function () {
        //        $(this).closest('.is_Datetimepicker').find('input').datetimepicker('show');
        //    })
        //});
        //$('.is_Datetimepicker_hours').each(function () {
        //    $($(this)).find('input').datetimepicker({
        //        format: "H:i",
        //        datepicker: false
        //    }).on('changeDate', function (ev) { $(this).datetimepicker('hide'); });
        //    $(this).find('span').on('click', function () {
        //        $(this).closest('.is_Datetimepicker_hours').find('input').datetimepicker('show');
        //    })
        //});
        //$('.is_Datetimepicker_years').each(function () {
        //    $(this).closest('.is_Datetimepicker_years').find('input').attr('autocomplete', 'off');
        //    $($(this)).find('input').datetimepicker({
        //        format: "Y",
        //        timepicker: false
        //    }).on('changeDate', function (ev) { $(this).datetimepicker('hide'); });
        //    $(this).find('span').on('click', function () {
        //        $(this).closest('.is_Datetimepicker_years').find('input').datetimepicker('show');
        //    })
        //});
        //$('.is_Datetimepicker_hours').each(function () {
        //    $(this).find('input').attr('autocomplete', 'off');
        //    $($(this)).find('input').datetimepicker({
        //        format: "d/m/Y H:i:s",
        //        timepicker: true,
        //        datepicker: true
        //    }).on('changeDate', function (ev) { $(this).datetimepicker('hide'); });
        //    $(this).find('span').on('click', function () {
        //        $(this).closest('.is_Datetimepicker_hours').find('input').datetimepicker('show');
        //    })
        //});

        //$('form input.input-date').focusout(function () {
        //    $('form').bootstrapValidator('revalidateField', $(this).attr('name'));
        //});
        //$('form input.input-date-hour').focusout(function () {
        //    $('form').bootstrapValidator('revalidateField', $(this).attr('name'));
        //});
    },
    FormatDateTime: function () {
        $.fn.datetimepicker.dates['vi'] = {
            days: ["CN", "Thứ 2", "Thứ 3", "Thứ 4", "Thứ 5", "Thứ 6", "Thứ 7", "CN"],
            daysShort: ["CN", "Hai", "Ba", "Tư", "Năm", "Sáu", "Bảy", "CN"],
            daysMin: ["CN", "Hai", "Ba", "Tư", "Năm", "Sáu", "Bảy", "CN"],
            months: ["Tháng 1", "Tháng 2", "Tháng 3", "Tháng 4", "Tháng 5", "Tháng 6", "Tháng 7", "Tháng 8", "Tháng 9", "Tháng 10", "Tháng 11", "Tháng 12"],
            monthsShort: ["T1", "T2", "T3", "T4", "T5", "T6", "T7", "T8", "T9", "T10", "T11", "T12"],
            today: "Hôm nay"
        };
        $('.bootstrap-datetimepicker-month').each(function () {
            $($(this)).datetimepicker({
                format: 'MM/yyyy',
                viewMode: 'months',
                minViewMode: "months",
                language: 'vi',
                endDate: "0m",
                maxDate: new Date("2013-07-01")
            }).on('changeDate', function (ev) {
                $(this).datetimepicker('hide');
            });
        });
        $('.bootstrap-datetimepicker-hours-only').each(function () {
            $($(this)).datetimepicker({
                dateFormat: '',
                datepicker: false,
                pickDate: false,
                //format: "HH:mm",
                timeOnly: true,
                showSecond: false,
                format: 'H:i:s',
                inline: true,
                sideBySide: true
            }).on('changeDate', function (ev) {
                //$(this).datetimepicker('hide');
            });
        });
        $('.bootstrap-datetimepicker').each(function () {
            $($(this)).datetimepicker({
                format: "dd/MM/yyyy",
                showToday: true,
                language: 'vi'
            }).on('changeDate', function (ev) {
                $(this).datetimepicker('hide');
            });
        });
        $('.bootstrap-datetimepicker-hours').each(function () {
            $($(this)).datetimepicker({
                format: "dd/MM/yyyy hh:mm",
                showToday: true,
                language: 'vi'
            }).on('changeDate', function (ev) {
                $(this).datetimepicker('hide');

            });
        });
        //$('form input.input-date').focusout(function () {
        //    $('form').bootstrapValidator('revalidateField', $(this).attr('name'));
        //});
        //$('form input.input-date-hour').focusout(function () {
        //    $('form').bootstrapValidator('revalidateField', $(this).attr('name'));
        //});
    },
    GET_DATE_FORMAT: function (valDate) {
        var res = "";
        var arrNgayPC = valDate.split("/");
        if (arrNgayPC.length > 2)
            res = arrNgayPC[1] + "/" + arrNgayPC[0] + "/" + arrNgayPC[2];
        return res;
    },
    FormatEmail: function () {
        $('.input-email').focusout(function () {
            let $parent = $(this).parent();
            if ($parent.find('span.alert-email').length > 0) {
                $parent.find('span.alert-email').remove();
            }
            let email = $(this).val();
            if (email == '') return false;
            if (!ValidateEmail(email)) {
                $parent.append('<span class="alert-email red italic" style="font-size: 13px;">Địa chỉ email không hợp lệ</span>');
            }
        });
    },
    FormatTextName: function () {
        $('.input-text-name').focusout(function () {
            let $parent = $(this).parent();
            if ($parent.find('span.alert-text-name').length > 0) {
                $parent.find('span.alert-text-name').remove();
            }
            let name = $(this).val();
            if (name == '') return false;
            if (+name >= 0) {
                $parent.append('<span class="alert-text-name red italic" style="font-size: 13px;">Tên nhập vào không hợp lệ</span>');
            }
        });
    },
    FormatInputRequired: function () {
        $('input:required, textarea:required, select:required').off('keyup').on('keyup', function () {
            let $this = $(this);
            let value = $this.val();
            let $messageTag = $this.parent().find('.message-validate');
            if (value !== '') {
                $this.addClass('border-success').removeClass('border-danger');
                if ($messageTag.length > 0) {
                    $messageTag.removeClass('text-danger').html('');
                }                           
            } else {
                $this.removeClass('border-success').addClass('border-danger');
                if ($messageTag.length > 0) {
                    $messageTag.addClass('text-danger').html('Vui lòng nhập giá trị.');
                } else {
                    $this.parent().append('<div class="message-validate text-danger">Vui lòng nhập giá trị.</div>');
                }                
            }
        });
        $('input:required, textarea:required, select:required').off('change').on('change', function () {
            let $this = $(this);
            let value = $this.val();
            let $messageTag = $this.parent().find('.message-validate');
            if (value !== '') {
                $this.addClass('border-success').removeClass('border-danger');
                if ($messageTag.length > 0) {
                    $messageTag.removeClass('text-danger').html('');
                }
            } else {
                $this.removeClass('border-success').addClass('border-danger');
                if ($messageTag.length > 0) {
                    $messageTag.addClass('text-danger').html('Vui lòng nhập giá trị.');
                } else {
                    $this.parent().append('<div class="message-validate text-danger">Vui lòng nhập giá trị.</div>');
                }
            }
        });
        $('input:required, textarea:required, select:required').off('focusout').on('focusout', function () {
            let $this = $(this);
            let value = $this.val();
            let $messageTag = $this.parent().find('.message-validate');
            if (value !== '') {
                $this.addClass('border-success').removeClass('border-danger');
                if ($messageTag.length > 0) {
                    $messageTag.removeClass('text-danger').html('');
                }
            } else {
                $this.removeClass('border-success').addClass('border-danger');
                if ($messageTag.length > 0) {
                    $messageTag.addClass('text-danger').html('Vui lòng nhập giá trị.');
                } else {
                    $this.parent().append('<div class="message-validate text-danger">Vui lòng nhập giá trị.</div>');
                }
            }
        });
    }
    
};
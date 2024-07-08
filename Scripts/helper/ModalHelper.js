var ModalHelper = {
    Self: $('.modal'),
    Init: function ($modalTarget) {
        $modalTarget = ($modalTarget || ModalHelper.Self);
        ModalHelper.Open($modalTarget);
        ModalHelper.Close($modalTarget);
    },
    Open: function ($modalTarget, callbackFunc) {
        /* ModalHelper.Self.on('shown.bs.modal', () => {*/
        $modalTarget.on('shown.bs.modal', () => {
            $modalTarget.find('.modal-body input[type=text]:nth(0)').focus();
            setTimeout(() => {
                let inputTarget = $modalTarget.find('.modal-body input[type=text]:nth(0)');
                if (inputTarget.prop('readonly') || inputTarget.prop('disabled')) {
                    inputTarget = $modalTarget.find('.modal-body input:not([type=radio],[type=checkbox],[readonly],[disabled]):nth(0)');
                    inputTarget.focus();
                    console.log('inputNameFocus 1', inputTarget.attr('name'));
                } else {
                    $modalTarget.find('.modal-body input:nth(0)').focus();
                }
                console.log('inputNameFocus', inputTarget.attr('name'));
            }, 1000);

            if (typeof (callbackFunc) == "function") {
                callbackFunc();
            }
        });
    },
    Close: function ($modalTarget, callbackFunc) {
        /*ModalHelper.Self.on('hidden.bs.modal', () => {*/
        $modalTarget.on('hidden.bs.modal', () => {
            $modalTarget.find('input[type="text"]').val('');
            $modalTarget.find('input[type="hidden"]').val('');
            $modalTarget.find('input[type="radio"]:first-child').prop('checked', true);
            $modalTarget.find('input[type="checkbox"]').prop('checked', false);
            $modalTarget.find('select option:first-child').prop('selected', true);
            $modalTarget.find('textarea').val('');
            $modalTarget.find('table:not(.tbl-fixed,.table-input) tbody').html('<tr><td colspan="100" class="text-center text-info">No data.</td></tr>');
            $modalTarget.find('div.image-preview').html('');
            $modalTarget.find(':input').removeClass('border-success').removeClass('border-danger');
            $modalTarget.find('div.message-validate').html('');
            if (typeof (callbackFunc) == "function") {
                callbackFunc();
            }
        });
    },
    InputRequiedOnChange: function ($frame) {
        ModalHelper.Self.find('input.border-danger, textarea.border-danger, select.border-danger').off('keydown, change').on('keydown, change', function () {
            let $this = $(this);
            let value = $this.val();
            if (value != '') {
                $this.addClass('border-success');
                $this.parent().find('.message-validate').removeClass('text-danger').html('');
            } else {
                $this.removeClass('border-success');
                $this.parent().find('.message-validate').addClass('text-danger').html('Vui lòng nhập giá trị.');
            }
        });
    },
    ViewItem: function (data, $modalTarget) {
        if (data == null) {
            return;
        }
        $modalTarget = ($modalTarget || ModalHelper.Self);
        for (var name in data) {
            let firstChar = name.substring(0, 1).toUpperCase();
            let InputName = firstChar + name.substring(1);

            let $input = $modalTarget.find('[name=' + InputName + ']');
            let $inputText = $modalTarget.find('[name=' + InputName + 'Text]');
            if ($input.length == 0 && $inputText.length == 0) {
                continue;
            }
            if (InputName == 'Image') {
                const myFile = new File(['Hello World!'], data[name], {
                    type: 'text/plain',
                    lastModified: new Date(),
                });
                // Now let's create a DataTransfer to get a FileList
                const dataTransfer = new DataTransfer();
                dataTransfer.items.add(myFile);
                const fileInput = $input;
                //fileInput.files = dataTransfer.files;
                fileInput[0].files = dataTransfer.files;
                $modalTarget.find('div.image-preview').html('<div class="text-center mb-2 mt-3"><img style="max-width: 200px;" src="' + data[name] + '"/></div>');

            } else if (InputName.indexOf('Img') >= 0) {
                $modalTarget.find('div.image-preview').html('<div class="text-center mb-2 mt-3"><img style="max-width: 200px;" src="' + data[name] + '"/></div>');
            }
            else if ($input.attr('type') == 'checkbox') {
                if ($input.val() !== '') {
                    $input.prop('checked', data[name] == $input.val());
                } else {
                    $input.prop('checked', data[name] == YES);
                }
            }
            else if ($input.attr('type') == 'radio') {
                $modalTarget.find('[name=' + InputName + '][value="' + data[name] + '"]').prop('checked', true);
            }
            else if ((name.toLowerCase().indexOf('ngay') >= 0 || name.toLowerCase().indexOf('date') >= 0 || name.toLowerCase().indexOf('day') >= 0)
                && (data[name] || '').length > 10) {
                if ($inputText.length == 0) {
                    $input.val(formatDateFrom_StringServer(data[name]));
                } else {
                    $inputText.val(formatDateFrom_StringServer(data[name]));
                }
            }
            else {
                $input.val(data[name]);
            }
        }
        //$modalTarget.modal('show');
        setTimeout(() => { $modalTarget.find('input:first-child').focus(); }, 2000);
    }
};
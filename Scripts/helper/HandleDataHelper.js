var HandleDataHelper = {
    FillDataAnnotation: function (arrDataAnnotation) {
        // Lấy các Annotations attribute để gán vào các thẻ html nhập liệu tương ứng
        arrDataAnnotation.forEach((item) => {
            let $input = $('[name=' + item.Name + ']');
            if ($input.length == 0) return false;

            $input.prop('required', item.Required);
            if ($input.parent().find('.input-display-name').html() == '') {
                $input.parent().find('.input-display-name').html((item.DisplayName || item.Name).replace(/_/g, ' '));
            }

            if (item.Maxlength > 0) {
                $input.attr('maxlength', item.Maxlength);
            }
            if (item.Minlength > 0) {
                $input.attr('minlength', item.Minlength);
            }
            if (!item.AllowEdit) {
                $input.prop('readonly', true).prop('disabled', true);
            }
        });
        HandleDataHelper.OnChangeInputRequired($('body'));
    },
    GetRequestData: function ($frameData, pageNumber = 0) {
        let lstRawData = [];
        $frameData.find(':input:not(button, [type="button"])').each(function (index) {
            let input = $(this);
            let type = input.attr('type') || 'text';
            let name = input.attr('name');
            let value = (input.val() || '');
            let required = input.is(':required');
            let maxLength = (+input.attr('maxlength') || 0);

            if (type == 'checkbox') {
                let checkVal = $frameData.find(`[name=${name}]`).val() || '';
                if (checkVal == '') {
                    value = $frameData.find(`[name=${name}]`).is(':checked') ? 1 : 0;
                } else {
                    value = $frameData.find(`[name=${name}]`).is(':checked') ? checkVal : 0;
                }
            } else if (type == 'radio') {
                value = $frameData.find(`[name=${name}]:checked`).val() || '';
            } else if (input.hasClass('editor')) {
                value = CKEDITOR.instances.editor.getData();
            }
            let itemChild = {
                DOM: input,
                Required: required,
                Name: name,
                Type: type,
                Value: (Array.isArray(value) ? value.toString() : value),
                Maxlength: maxLength
            };
            lstRawData.push(itemChild);
        });
        if (pageNumber > 0) {
            let itemChild = { Required: false, Name: 'PageNumber', Type: '', Value: pageNumber, Maxlength: 0 };
            lstRawData.push(itemChild);
        }
        return lstRawData;
    },
    ValidateDataBeforeSendRequest: function ($frame, lstRawData) {
        let msg = new MessageDto(true);
        msg.Data = {};
        if (lstRawData == null) {
            return msg;
        }
        let lstItemWarning = [];
        let length = lstRawData.length;
        for (var i = 0; i < length; i++) {
            let item = lstRawData[i];
            if (item.Required && (item.Value || '') == '') {
                msg.IsOk = false;
                lstItemWarning.push(item);
            } else if (item.Maxlength > 0 && (item.Value || '').length > item.Maxlength) {
                msg.IsOk = false;
                lstItemWarning.push(item);
            }
            msg.Data[item.Name] = item.Value;
        }

        length = lstItemWarning.length;
        for (var i = 0; i < length; i++) {
            let itemWarning = lstItemWarning[i];
            $frame.find(`[name=${itemWarning.Name}]`).addClass('border-danger');

            let msgWarning = '';
            if (itemWarning.Required && (itemWarning.Value || '') == '') {
                msgWarning = 'Vui lòng nhập giá trị';
            } else if (itemWarning.Maxlength > 0 && (itemWarning.Value || '').length > itemWarning.Maxlength) {
                msgWarning = 'Độ dài tối đa là ' + itemWarning.Maxlength;
            }

            let $domWarning = (itemWarning.DOM || $frame.find(`[name=${itemWarning.Name}]`).parent());
            let $messageValid = $domWarning.find('.message-validate');
            if ($messageValid.length > 0) {
                $messageValid.addClass('text-danger').html(msgWarning);
            } else {
                $domWarning.append('<div class="message-validate text-danger">' + msgWarning + '</div>');
            }
        }

        if (length > 0) {
            msg.Name = lstItemWarning[0].Name;
            msg.Description = 'Vui lòng nhập giá trị cho các thông tin đánh dấu <span class="text-danger">*</span> bắt buộc.';
            //HandleDataHelper.OnChangeInputRequired($frame);
            // Đã dùng hàm formatInputRequired trong file FormatInputHelper.js
        }

        // Nếu có nhập dữ liệu danh sách trong table
        if ($frame.find('table.table-input').length == 0) {
            return msg;
        }
        $frame.find('table.table-input tbody').each(function (tblIndex, tblInput) {
            let lstChild = [];
            $(tblInput).find('tr').each(function (trIndex, trInput) {
                let itemChild = {};
                $(trInput).find(':input:not(button, [type="button"])').each(function () {
                    let $tr = $(this).parents('tr');
                    let input = $(this);
                    let type = input.attr('type') || 'text';
                    let name = input.attr('name');
                    let value = (input.val() || '');

                    if (type == 'checkbox') {
                        let checkVal = $tr.find(`[name=${name}]`).val() || '';
                        if (checkVal == '' || checkVal=='on') {
                            value = $tr.find(`[name=${name}]`).is(':checked') ? 1 : 0;
                        } else {
                            value = $tr.find(`[name=${name}]`).is(':checked') ? checkVal : 0;
                        }
                    } else if (type == 'radio') {
                        value = $tr.find(`[name=${name}]:checked`).val() || '';
                    } else if (input.hasClass('editor')) {
                        value = CKEDITOR.instances.editor.getData();
                    }
                    itemChild[name] = (Array.isArray(value) ? value.toString() : value);
                });
                lstChild.push(itemChild);
            });
            msg.Data['tableData' + tblIndex] = lstChild;
        });
        return msg;
    },
    OnChangeInputRequired: function ($frame) {
        $frame.find('input.border-danger, textarea.border-danger, select.border-danger').off('keyup, change').on('keyup, change', function () {
            let $this = $(this);
            let value = $this.val();
            if (value !== '') {
                $this.addClass('border-success').removeClass('border-danger');
                $this.parent().find('.message-validate').removeClass('text-danger').html('');
            } else {
                $this.removeClass('border-success').addClass('border-danger');
                $this.parent().find('.message-validate').addClass('text-danger').html('Vui lòng nhập giá trị.');
            }
        });
        $frame.find('input:required, textarea:required, select:required').off('keyup, change, focusout').on('keyup, change', function () {
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

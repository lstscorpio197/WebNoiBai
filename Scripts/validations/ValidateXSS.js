// Validate XSS output
var ValidateXSS = {
    Encode: function (content) {
        return $('<div/>').text(content).text();
    },
    Decode: function (content) {
        //Example:
        //var text = '&lt;p&gt;name&lt;/p&gt;&lt;p&gt;&lt;span style="font-size:xx-small;"&gt;ajde&lt;/span&gt;&lt;/p&gt;&lt;p&gt;&lt;em&gt;da&lt;/em&gt;&lt;/p&gt;';
        //var decoded = $('<div/>').html(text).text();
        //console.log(decoded);
        return $('<div/>').html(content).text();
    },
    sanitizeAttributes: function (node) {
        $.each(node.attributes, function () {
            var attrName = this.name;
            var attrValue = this.value;
            // remove attribute name start with "on", possible unsafe,
            // for example: onload, onerror...
            //
            // remvoe attribute value start with "javascript:" pseudo protocol, possible unsafe,
            // for example href="javascript:alert(1)"
            if (attrName.indexOf('on') == 0 || attrValue.indexOf('javascript:') == 0) {
                $(node).removeAttr(attrName);
            }
        });
    },
    sanitizeHtml: function (htmlContent) {
        try {
            var output = $($.parseHTML('<div>' + htmlContent + '</div>', null, false));
            output.find('*').each(function () {
                $ValidateXSS.sanitizeAttributes(this);
            });
            return output.html();
        } catch (ex) {
            return $String.Empty();
        }
    },
    sanitizeHtmlTable: function ([htmlContent = '', tag = 'tr']) {
        try {
            tag = tag.toLowerCase();
            var options = {
                tbody: tag == 'tbody' ? '<table>' + htmlContent + '</table>' : '',
                tr: tag == 'tr' ? '<table><tbody>' + htmlContent + '</tbody></table>' : '',
                td: tag == 'td' ? '<table><tbody><tr>' + htmlContent + '<tr></tbody></table>' : ''
            };
            var output = $($.parseHTML('<div>' + options[tag] + '</div>', null, false));
            output.find('*').each(function () {
                $ValidateXSS.sanitizeAttributes(this);
            });
            var result = {
                tbody: tag == 'tbody' ? output.find('table').html() : '',
                tr: tag == 'tr' ? output.find('table tbody').html() : '',
                td: tag == 'td' ? output.find('table tbody tr').html() : ''
            }
            return result[tag];
        }
        catch (ex) {
            return $String.Empty();
        }
    }
}

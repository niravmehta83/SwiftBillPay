(function () {
    window.setTimeout(function () { initializeCheckBoxes(); }, 1000);
    function initializeCheckBoxes() {
        $(function () {
            $('#allCheckBox').change(function () {
                var isChecked = $(this).is(':checked') ? true : false;
                if (isChecked) {
                    $('.singleCheckBox').attr('checked', isChecked);
                    window.alert(isChecked);
                }
                if (isChecked ? true : false) {
                    $('.singleCheckBox').closest('tr').addClass('tr-selected');
                } else {
                    $('.singleCheckBox').closest('tr').removeClass('tr-selected');
                }
            });
            $('.singleCheckBox').click(function () {
                var isChecked = $(this).is(':checked');
                if (isChecked ? true : false) {
                    $(this).closest('tr').addClass('tr-selected');
                } else {
                    $(this).closest('tr').removeClass('tr-selected');
                }
                if (isChecked && $('.singleCheckBox').length == $('.selected-row').length)
                    $('#allCheckBox').attr('checked', true);
                else
                    $('#allCheckBox').attr('checked', false);

            });
        });
    }
})();
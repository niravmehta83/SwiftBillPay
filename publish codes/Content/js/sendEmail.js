$(document).ready(function () {
    showSelectedCustmerNames();
    $(".allCheckBox").click(function () {
      
        var isChecked = this.checked;
        $('.allCheckBox').each(function () { this.checked = (isChecked ? true : false) });
        $('.singleCheckBox').each(function () { this.checked = (isChecked ? true : false) });
        $('.singleCheckBox').closest('tr').addClass(isChecked ? 'selected-row' : 'not-selected-row');
        $('.singleCheckBox').closest('tr').removeClass(isChecked ? 'not-selected-row' : 'selected-row');
        showSelectedCustmerNames();
    });

    $('.singleCheckBox').click(function () {
        var isChecked = this.checked;//this.checked;
        $(this).closest('tr').addClass(isChecked ? 'selected-row' : 'not-selected-row');
        $(this).closest('tr').removeClass(isChecked ? 'not-selected-row' : 'selected-row');
        if (isChecked && $('.singleCheckBox').length == $('.selected-row').length)
            $('.allCheckBox').each(function () { this.checked = true });
        else
            $('.allCheckBox').each(function () { this.checked = false });
        showSelectedCustmerNames();

    });
});
function showSelectedCustmerNames() {
    var customerselected = $.map($(".singleCheckBox:checkbox:not(:checked)"),
   function (e) {
       return $(e).val();
   });
    if (customerselected.length == 0) {
        customerselected = "Please un select any One customer to send an email.";
    }
    else if (customerselected.length <= 6) {
        customerselected = $.map($(".singleCheckBox:checkbox:not(:checked)"),
            function (e, k) {
                var value = "<span>" + parseInt(k + 1) + ". " + $("#divfn_" + $(e).val()).text() + "</span><br/>";
                return value;
            });
    }
    else {
        customerselected = $.map($(".singleCheckBox:checkbox:not(:checked)"),
            function (e, k) {
                var value = "<span>" + parseInt(k + 1) + ". " + $("#divfn_" + $(e).val()).text() + "</span>";
                return value;
            });
    }
    $("#divSendMail").html(customerselected);
};

$(document).ready(function () {
    var opts = {
        lines: 13, // The number of lines to draw
        length: 5, // The length of each line
        width: 3, // The line thickness
        radius: 40, // The radius of the inner circle
        corners: 1, // Corner roundness (0..1)
        rotate: 0, // The rotation offset
        direction: 1, // 1: clockwise, -1: counterclockwise
        color: '#fff', // #rgb or #rrggbb or array of colors
        speed: 1, // Rounds per second
        trail: 60, // Afterglow percentage
        shadow: true, // Whether to render a shadow
        hwaccel: false, // Whether to use hardware acceleration
        className: 'spinner', // The CSS class to assign to the spinner
        zIndex: 2e9, // The z-index (defaults to 2000000000)
        top: '400px', // Top position relative to parent in px
        left: '650px' // Left position relative to parent in px
    };
    $('#btnnotebox').click(function (event) {
        $('#myModal').modal('toggle');
        if ($(".singleCheckBox:checkbox:not(:checked)").length > 0) {

            if (confirm('Are you sure ?')) {
                $('body').append('<div id="foon" style="background-color: rgba(0, 0, 0, 0.6); width:100%; height:100%; position:fixed; top:0px; left:0px; z-index:' + (opts.zIndex - 1) + '"/>');

                var target = document.getElementById('wrapper');
                var spinner = new Spinner(opts).spin(target);
                var customerselected = $.map($(".singleCheckBox:checkbox:not(:checked)"),
              function (e) { return $(e).val(); });
                $('#CustomertSelected').val(customerselected);
                //alert($('#CustomertSelected').val());
                $.ajax({
                    type: 'Post',
                    url: '/User/EMailCustomer',
                    data: { CustomerSelected: $('#CustomertSelected').val(), EmailNote: $('#EmailNote').val() },
                    datatype: 'Json',
                    success: function (data) {
                        spinner.stop();
                        $('#foon').remove();
                        $('.singleCheckBox').attr('checked', false);
                        var isChecked = $('.singleCheckBox').attr('checked');
                        $('.singleCheckBox').closest('tr').removeClass(isChecked ? 'not-selected-row' : 'selected-row');
                        $('#EmailNote').val("");
                        $.notify({ message: 'Mail Sent Successfully' }, { type: 'success' });
                        location.reload();
                    },
                    Error: function () { alert('Some Error Occured Please Try again later'); }

                });
            }
            return false;
        }
        else {
            $.notify({ message: 'Please un select any One customer to send an email..' }, { type: 'info' });
            return false;
        }


    });

    $('#btnRemnotebox').click(function (event) {
        $('#myModal').modal('toggle');
        if ($(".singleCheckBox:checkbox:not(:checked)").length > 0) {

            if (confirm('Are you sure ?')) {
                $('body').append('<div id="foon" style="background-color: rgba(0, 0, 0, 0.6); width:100%; height:100%; position:fixed; top:0px; left:0px; z-index:' + (opts.zIndex - 1) + '"/>');

                var target = document.getElementById('wrapper');
                var spinner = new Spinner(opts).spin(target);
                var customerselected = $.map($(".singleCheckBox:checkbox:not(:checked)"),
              function (e) { return $(e).val(); });
                $('#InvoiceSelected').val(customerselected);
                //alert($('#CustomertSelected').val());
                $.ajax({
                    type: 'Post',
                    url: '/User/RemainderEmail',
                    data: { InvoiceSelected: $('#InvoiceSelected').val(), EmailNote: $('#EmailNote').val() },
                    datatype: 'Json',
                    success: function (data) {
                        spinner.stop();
                        $('#foon').remove();
                        $('.singleCheckBox').attr('checked', false);
                        var isChecked = $('.singleCheckBox').attr('checked');
                        $('.singleCheckBox').closest('tr').removeClass(isChecked ? 'not-selected-row' : 'tr-selected');
                        $('#EmailNote').val("");
                        $.notify({ message: 'Mail Sent Successfully' },{  type: 'success' });
                        location.reload();
                    },
                    Error: function () { alert('Some Error Occured Please Try again later'); }

                });
            }
            return false;
        }
        else {
            $.notify({ message: 'Please un select any One customer to send an email..' },{  type: 'info' });
            return false;
        }


    });



});




/// <summary>
/// Description: these are the scripts along with the accountbox html helper, to enter only numbers and exact digits like account numbers
/// created by :Raja
/// On :16-06-2014
/// </summary>


$(document).ready(function () {

    //called when key is pressed in textbox
    $(".acc").keypress(function (e) {
        var id = $(this).attr('id');
        var a = id.split('-');
        var rawid = a[0].replace('acc', '');
        //if the letter is not digit then display error and don't type anything
        if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
            //display error message
            $('#' + rawid + 'errmsg').html("Enter Only Numbers").show().fadeOut("slow");
            return false;
        }

        if ($(this).val().length > 0) {
            return false;
        }

    });
    $(".acc").keyup(function (e) {
        var id = $(this).attr('id');
        var a = id.split('-');
        var newid = parseInt(a[1]) + 1;
        var rawid = a[0].replace('acc', '');
        var c = String.fromCharCode(e.which);
        var tbval = FindTbvalue();

        $('#' + rawid + 'tbacc').val(tbval);
        $('#' + a[0] + '-' + newid).focus();

    });


    function FindTbvalue() {
        var TbValue = '';
        $('.acc').each(function () {
            TbValue += $(this).val();

        });
        return TbValue;
    }

});

function TbValidate(e) {
    var id = $(e).attr('id');
    var a = id.split('-');
    var rawid = a[0].replace('acc', '');
    var min = $('#' + rawid + 'min').val();
    var max = $('#' + rawid + 'max').val();
    if (min == 0) {
        var Value = $(e).val();
        if (Value == '') {

            $('#' + rawid + 'errmsg').html("Should be " + max + " Digits").show().fadeOut("slow");
            $(e).focus();
        }

    }
    else {

        if ($('#' + rawid + 'tbacc').val().length < parseInt(min)) {
            var Value = $(e).val();
            if (Value == '') {

                $('#' + rawid + 'errmsg').html("Minimum length must be " + min + " max length must be " + max + " Digits").show().fadeOut("slow");
                $(e).focus();
            }
        }
    }

}
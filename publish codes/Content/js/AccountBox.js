/// <summary>
/// Description: these are the scripts along with the accountbox html helper, to enter only numbers and exact digits like account numbers
/// created by :Raja
/// On :16-06-2014
/// </summary>


$(document).ready(function () {

    $('.acc').click(function () { selectAllText($(this)) });
    function selectAllText(textbox) {
        textbox.focus();
        textbox.select();
    }
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
        var max = $('#' + rawid + 'max').val();

        //if the letter is not digit then display error and don't type anything
        if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
            //display error message
            $('#' + rawid + 'errmsg').html("Enter Only Numbers").show().fadeOut("slow");
            return false;
        }
        else {

            var c = String.fromCharCode(e.which);
            $(this).val(c);
            console.log(c + 'pressed');
            var tbval = FindTbvalue(rawid);
            console.log('value of' + rawid + 'tbacc= ' + tbval);
            $('#' + rawid + 'tbacc').val(tbval);
            if (e.keyCode != 8) {
                if (tbval != "") {
                    $('#' + a[0] + '-' + newid).focus();
                }
            }

            if (tbval.length == parseInt(max)) {

                $('#' + rawid + 'errmsg').hide();

            }

        }

    });

    function FindTbvalue(rawid) {
        var TbValue = '';
        $('.acc').each(function () {
            var id = $(this).attr('id');
            var a = id.split('-');
            var rid = a[0].replace('acc', '');
            if (rawid == rid) {
                TbValue += $(this).val();
            }
            // console.log(TbValue);
        });
        return TbValue;
    }


});


function TbValidate(e) {

    //console.log('onblur');
    var id = $(e).attr('id');
    var a = id.split('-');
    var rawid = a[0].replace('acc', '');
    var min = $('#' + rawid + 'min').val();
    var max = $('#' + rawid + 'max').val();
    var aval = $('#' + a[0] + '-' + "1").val();
    if (min == 0) {

        if ($('#' + rawid + 'tbacc').val().length < parseInt(max)) {

            $('#' + rawid + 'errmsg').html("Should be " + max + " Digits").show();

        }
        else {
            $('#' + rawid + 'errmsg').hide();
        }

    }
    else {

        if ($('#' + rawid + 'tbacc').val().length < parseInt(min)) {

            $('#' + rawid + 'errmsg').html("Minimum length must be " + min + " Digits").show();

        }
        else {
            $('#' + rawid + 'errmsg').hide();
        }

    }

}


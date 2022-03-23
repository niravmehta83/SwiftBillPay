
//for dynamic view
function Show(id, e) {

    if (!$(id).is(':visible')) {
        $('.questbody').hide(2000);
        $('.simage').show();
        $('.himage').hide();
        
    }
    $(id).toggle(2000);
    $(id + 'show').toggle();
    $(id + 'hide').toggle();
    $("#preview").remove();
}


//for preview
this.questPreview = function () {
    /*CONFIG*/
    xOffset = 40;
    yOffset = 0;
    //thease 2 variables determine the popup'from cursor
    /*END CONFIG */
    $("a.button").hover(function (e) {
        if (!$(this.name).is(':visible')) {

            var divContents = $(this.name).html();
            $("body").append("<div id='preview' class='callout right'>" + divContents + "</div>");
            $('#preview').css("top", (e.pageY - xOffset) + "px").css("left", (e.pageX + yOffset) + "px").fadeIn("fast");
        }
    },
function () { $("#preview").remove(); });
    $("a.button").mousemove(function (e) {
        if (!$(this.name).is(':visible')) {
            $('#preview').css("top", (e.pageY - xOffset) + "px").css("left", (e.pageX + yOffset) + "px");
        }
    });
};
//starting the script on page load
$(document).ready(function () {
    questPreview();
});

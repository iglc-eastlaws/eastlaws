function printdiv(printpage, Type,printID) {
    if ($("#print_frame").length == 0) {
        $("body").append("<iframe id='print_frame' name='print_frame' width='0' height='0' frameborder='0' src='about:blank' ></iframe>");
    }
    var headstr = "<html><head><title></title>" + document.getElementsByTagName('head')[0].innerHTML + "<link rel='stylesheet' type='text/css' href='/content/css/print.css'></head><body>";
    var footstr = "</body></html>" 
    var newstr = "";
    //if (Type == 0) {
    newstr = $('#' + printID).html();
   // }
   // else {

    //    newstr = "hiiiiiiiiii"
    //
   // }
    window.frames["print_frame"].document.write(headstr + newstr + footstr);
    setTimeout(function () {
        window.frames["print_frame"].window.focus()
        window.frames["print_frame"].window.print()
    }, 1000);
    window.frames["print_frame"].document.close();
    return false;
}

$(document).on('click', '.printing', function () {
    printdiv($(this).attr('PrintID'), 1, $(this).attr('PrintID'))
})
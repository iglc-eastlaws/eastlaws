// JavaScript Document
(function ($) {
    // Tooltip
    $('[data-toggle="tooltip"]').tooltip();

    $(function () {

        var selectFormGroup = function (event) {
            event.preventDefault();

            var $selectGroup = $(this).closest('.input-group-select');
            var param = $(this).attr("href").replace("#", "");
            var concept = $(this).text();

            $selectGroup.find('.concept').text(concept);
            $selectGroup.find('.input-group-select-val').val(param);

        }

        $(document).on('click', '.dropdown-menu a', selectFormGroup);

    });
})(jQuery);
$('#myCarousel').carousel({
    interval: 10000
})
$(document).ready(function () {
    $('.carousel .item').each(function () {
        var next = $(this).next();
        if (!next.length) {
            next = $(this).siblings(':first');
        }
        next.children(':first-child').clone().appendTo($(this));

        if (next.next().length > 0) {
            next.next().children(':first-child').clone().appendTo($(this));
        }
        else {
            $(this).siblings(':first').children(':first-child').clone().appendTo($(this));
        }
    });

    $("#slide-slides").cslide();
    $("#cslide-slides").cslide();
    // Scroll 
    $(".filter-search .well").mCustomScrollbar({
        setHeight: "200px",
        theme: "3d",
        scrollButtons: "1"

    });
    // muilt check

    // Hide Element Filter
    $(".filter-element ul li a i").click(function () {
        $(this).parent().parent().fadeOut("slow", function () {
            $(this).remove();
        });
    });

    //Grid View
    $(".layout-view li a").click(function () {
        $(".layout-view li a").removeClass("active");
        $(this).addClass("active");
        if ($(this).parent().hasClass("list")) {
            $(".list-content .result-block").removeClass("col-md-4");
        }else{
         $(".list-content .result-block").addClass("col-md-4");
        }

    });

    // toolbar
 
    $(".format-result li a.sort i").click(function () {
       
            if ($(this).hasClass("fa-sort-numeric-asc active")) {
                $(this).prev().addClass("active");
                $(this).removeClass("active");
              
            }else{
                $(this).next().addClass("active");
                $(this).removeClass("active");
              
            }

        });





});


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
    function toggleChevron(e) {
        $(e.target)
            .prev('.panel-heading')
            .find("i.indicator")
            .toggleClass('glyphicon-chevron-down glyphicon-chevron-up');
    }
    $('#accordion').on('hidden.bs.collapse', toggleChevron);
    $('#accordion').on('shown.bs.collapse', toggleChevron)

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


    // menu hide
    $(".menu-side").click(function () {
        $(".container-fluid").toggleClass("hide-sidebar", 1000, "easeOutSine");

    });
    $(window).on('resize', function () {
        var win = $(this);
        if (win.width() < 1200) {
            $('.container-fluid').removeClass('hide-sidebar');
          //  $('.menu-side').click(false);
        }
      });

    // Scroll 
    //$(".filter-search .well .panel-body").mCustomScrollbar({
    //    setHeight: "200px",
    //    theme: "3d",
    //    scrollButtons: "1"

    //});

    // muilt select
    $('#example-getting-started').multiselect();

    //$('#example-enableCollapsibleOptGroups-enableClickableOptGroups').multiselect({
    //    enableClickableOptGroups: true,
    //    enableCollapsibleOptGroups: true
    //});
    // muilt check

    // Hide Element Filter
    //$(".filter-element ul li a i").click(function () {
    //    $(this).parent().parent().fadeOut("slow", function () {
    //        $(this).remove();
    //    });
    //});

    //Grid View
    $(".layout-view li a").click(function () {
        $(".layout-view li a").removeClass("active");
        $(this).addClass("active");
        if ($(this).parent().hasClass("view-list")) {
            $(".list-content .result-block").removeClass("col-md-4 height-scroll");
        } else if ($(this).parent().hasClass("view-grid"))
        {
            $(".list-content .result-block").addClass("col-md-4 height-scroll");
        }


    });
    // Row Tabel View


    //$(function () {
    // var newheight = $(window).innerHeight();
    // var newHeight = $(".list-table table").height();
    //alert(newHeight);

    //  $("#hokmview").css({ 'height': ($(window).height()) + 'px' });


    //});

    // toolbar
    $(".format-result li a.sort i").click(function () {

        if ($(this).hasClass("fa-sort-numeric-asc active")) {
            $(this).prev().addClass("active");
            $(this).removeClass("active");

        } else {
            $(this).next().addClass("active");
            $(this).removeClass("active");

        }

    });

    // Tree Create
    $.fn.extend({
        treed: function (o) {

            var openedClass = 'glyphicon-minus-sign';
            var closedClass = 'glyphicon-plus-sign';

            if (typeof o != 'undefined') {
                if (typeof o.openedClass != 'undefined') {
                    openedClass = o.openedClass;
                }
                if (typeof o.closedClass != 'undefined') {
                    closedClass = o.closedClass;
                }
            };

            //initialize each of the top levels
            var tree = $(this);
            tree.addClass("tree");
            tree.find('li').has("ul").each(function () {
                var branch = $(this); //li with children ul
                branch.prepend("<i class='indicator glyphicon " + closedClass + "'></i>");
                branch.addClass('branch');
                branch.on('click', function (e) {
                    if (this == e.target) {
                        var icon = $(this).children('i:first');
                        icon.toggleClass(openedClass + " " + closedClass);
                        $(this).children().children().toggle();
                    }
                })
                branch.children().children().toggle();
            });
            //fire event from the dynamically added icon
            tree.find('.branch .indicator').each(function () {
                $(this).on('click', function () {
                    $(this).closest('li').click();
                });
            });
            //fire event to open branch if the li contains an anchor instead of text
            tree.find('.branch>a').each(function () {
                $(this).on('click', function (e) {
                    $(this).closest('li').click();
                    e.preventDefault();
                });
            });
            //fire event to open branch if the li contains a button instead of text
            tree.find('.branch>button').each(function () {
                $(this).on('click', function (e) {
                    $(this).closest('li').click();
                    e.preventDefault();
                });
            });
        }
    });

    //treeviews
    $('.tree-fahrs').treed({ openedClass: 'glyphicon-folder-open', closedClass: 'glyphicon-folder-close' });





});


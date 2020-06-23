


let docScroll = null;
let tableData = null;
let a = 0;
let b = 0;
window.onscroll = function () {
    tableData = document.getElementById("headTable");
    docScroll = document.documentElement.scrollTop;

    if (docScroll > 0 && docScroll < 200) {
        scrollFunction();
        if (tableData != this.undefined) {
            tableHeaderFixed(false);
        }
    }

    if (docScroll > 200 && docScroll < 300) {
        if (tableData != this.undefined) {
            tableHeaderFixed(true);
        }
    }
};

    let currentPaddingNav = "30px";
    let clicked = false;

    $(document).ready(function () {
        $(".navdown").hover(
            function () {
                $("#navshrink").css("padding-bottom", "170px");
            },
            function () {
                $("#navshrink").css("padding-bottom", currentPaddingNav);
                $(".dropdown-menu").css("display", "none");
            }
        );
    });

    function navClicked() {
        if (!clicked) {
            $("#navshrink").css("padding-bottom", "170px");
            clicked = true;
        } else {
            $("#navshrink").css("padding-bottom", currentPaddingNav);
            clicked = false;
        }
    }

    function scrollFunction() {

        if (document.body.scrollTop > 80 || document.documentElement.scrollTop > 80) {
            document.getElementById("navshrink").style.padding = "11px 10px";
            document.getElementById("Logo").style.fontSize = "25px";
            currentPaddingNav = "11px";
        } else {
            document.getElementById("navshrink").style.padding = "30px 10px";
            document.getElementById("Logo").style.fontSize = "29px";
            currentPaddingNav = "30px";
        }

    }

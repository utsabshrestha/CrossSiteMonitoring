$(document).ready(function () {
    $('#example').dataTable({
        "iDisplayLength": 10,
        "aLengthMenu": [[5, 10, 25, 50, 100, -1], [5, 10, 25, 50, 100, "All"]]
    });
    $('#example5').dataTable({
        "iDisplayLength": 5,
        "aLengthMenu": [[5, 10, 25, 50, 100, -1], [5, 10, 25, 50, 100, "All"]]
    });
    scrollFunction2();
});

//window.onscroll = function () { scrollFunction2() };
let tableRoot = null;
let head = null;

function scrollFunction2() {
    if (document.body.scrollTop > 200 || document.documentElement.scrollTop > 200) {
        tableHeaderFixed(true);
    } else {
        tableHeaderFixed(false);
    }
}

function tableHeaderFixed(bool) {
    tableRoot = document.getElementById("example_wrapper");
    head = document.getElementById("headTable");
    if (bool && head != undefined && tableRoot != undefined)
    {
        tableRoot.firstChild.classList.add("tableHeaderFixed-bar");
        head.classList.add("tableHeaderFixed-head");
    }
    else if (!bool && head != undefined && tableRoot != undefined)
    {
        tableRoot.firstChild.classList.remove("tableHeaderFixed-bar");
        head.classList.remove("tableHeaderFixed-head");
    }
}

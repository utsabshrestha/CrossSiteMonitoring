


$(document).ready(function () {
    document.getElementById("navshrink").style.width = `${screen.width - 17}px`;
});



let isFixed = false;
function makeNavBarFixed() {
    if (!isFixed) {
        document.getElementById("navshrink").style.width = `${screen.width - 17}px`;
        isFixed = true;
    }
    else {
        document.getElementById("navshrink").style.width = `100%`;
        isFixed = false;
    }
}
//modal-backdrop

$('#exampleModal2').click(function () {
    console.log("clicked");
    if (isFixed)
        makeNavBarFixed()
});

$('#exampleModal').click(function () {
    console.log("clicked");
    if (isFixed)
        makeNavBarFixed()
});

let myStatus;
// Example POST method implementation:
async function postData(url = '', data = {}, RqVeTkn) {
    console.log(data);

    // Default options are marked with *
    const response = await fetch(url, {
        method: 'POST', // *GET, POST, PUT, DELETE, etc.
        mode: 'cors', // no-cors, *cors, same-origin
        cache: 'no-cache', // *default, no-cache, reload, force-cache, only-if-cached
        credentials: 'same-origin', // include, *same-origin, omit
        headers: {
            'Content-Type': 'application/json',
            'RequestVerificationToken': RqVeTkn
        },
        redirect: 'follow', // manual, *follow, error
        referrerPolicy: 'no-referrer', // no-referrer, *no-referrer-when-downgrade, origin, origin-when-cross-origin, same-origin, strict-origin, strict-origin-when-cross-origin, unsafe-url
        body: JSON.stringify(data) // body data type must match "Content-Type" header
    });
    myStatus = null;
    myStatus = response.status;
    return response.json(); // parses JSON response into native JavaScript objects
}

let RoadInfo = {
    "form_id": null,
    "road_name": null,
    "road_code": null,
    "date": null,
    "observer_email": null,
    "id": null
}


function Finalize(form_id, road_code, date, observer_email, road_name, id) {
    clearRoadInfo();
    makeNavBarFixed();
    $('#exampleModal').modal('show');
    RoadInfo.form_id = form_id;
    RoadInfo.road_code = road_code;
    RoadInfo.date = date;
    RoadInfo.observer_email = observer_email;
    RoadInfo.road_name = road_name;
    RoadInfo.id = id;
}

let msg = document.getElementById("msg");

function clearRoadInfo() {
    Object.keys(RoadInfo).forEach(function (key) {
        RoadInfo[key] = null;
    });
}

function ReportFinalize() {

    makeNavBarFixed();
    $('#exampleModal').modal('hide');
    if (RoadInfo.road_code != null) {
        let url = getUrl();
        let status = document.getElementById(RoadInfo.id + "+stat");
        let drpdwn = document.getElementById(RoadInfo.id);

        let formData = new FormData(document.getElementById(RoadInfo.form_id));
        let __RequestVerificationToken = formData.get("__RequestVerificationToken");

        postData(url,
            {
                "form_id": RoadInfo.form_id,
                "road_name": RoadInfo.road_name,
                "road_code": RoadInfo.road_code,
                "date": RoadInfo.date,
                "observer_email": RoadInfo.observer_email
            }, __RequestVerificationToken
        ).then(data => {
            console.log(data);
            msg.innerHTML = data.message;
            if (myStatus == 200) {
                status.innerHTML = "Final Report";
                status.style.color = "#0366d6";
                drpdwn.style.display = "none";
                $('.toast-header').css("background-color", "white");
            } else {
                $('.toast-header').css("background-color", "red");
            }
            $('.toast').toast('show');
        });

        clearRoadInfo();
    }
}


    let deldataId = {
        id: null
    }
    function deleteReport(id)
    {
        makeNavBarFixed();
        $('#exampleModal2').modal('show');
        deldataId.id = id;
    }

    function deleteReportCnf()
    {
        makeNavBarFixed();
        $('#exampleModal2').modal('hide');
        isFixed = false;
        let id = 'del_' + deldataId.id;
        let rowId = 'rowId_' + deldataId.id;
        deldataId.id = null;
        let formData = new FormData(document.getElementById(id));
        let row = document.getElementById(rowId);
        let __RequestVerificationToken = formData.get("__RequestVerificationToken");

        const dataJson = {};
        for (var pair of formData.entries()) {
            if (pair[0] == "__RequestVerificationToken")
                continue;
            dataJson[pair[0]] = pair[1];
        }

        console.log(dataJson);
        postData("https://localhost:5001/Dashboard/DeleteReport", dataJson, __RequestVerificationToken)
            .then(data => {
                if (myStatus == 200) {
                    msg.innerHTML = data.message;
                    row.style.display = "none";
                    $('.toast-header').css("background-color", "white");
                } else {
                    $('.toast-header').css("background-color", "red");
                    msg.innerHTML = data.message;
                }
                $('.toast').toast('show');
            });
    }



$(document).ready(function () {
    document.getElementById("navshrink").style.width = `${screen.width - 17}px`;
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
    $('#exampleModal').modal('show');
    RoadInfo.form_id = form_id;
    RoadInfo.road_code = road_code;
    RoadInfo.date = date;
    RoadInfo.observer_email = observer_email;
    RoadInfo.road_name = road_name;
    RoadInfo.id = id;
}

function ReportFinalize() {
    $('#exampleModal').modal('hide');

    if (RoadInfo.road_code != null) {
        let url = getUrl();
        let msg = document.getElementById("msg");
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
            } else if (myStatus == 400) {

            } else if (myStatus == 401) {

            } else if (myStatus == 404) {

            }
            $('.toast').toast('show');
        });

        Object.keys(RoadInfo).forEach(function (key) {
            RoadInfo[key] = null;
        });
    }
}
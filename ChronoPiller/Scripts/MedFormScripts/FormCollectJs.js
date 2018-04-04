var currentPrescription = {};
var medicines = [];

function appendTitle(selector, prescriptionName) {
    $(selector).append(
        `<h3 name="${prescriptionName}> ${prescriptionName} </h3>`);
}

function appendInputWithLabel(name, hourId) {
    let result =
        `<div class="inputLabel"><label for="${name}"> ${name} </label>
   <input name="${name}" type="number" data-input="${hourId}">
   </input></div>`;
    return result;
};

function appendMultipleInputs(listOfInputs, id) {
    let result = "";
    $.each(listOfInputs, function (index, value) {
        let inputLabel = appendInputWithLabel(value, id);
        result = result.concat(inputLabel);
    });

    return result;
}
function populateList(list, inputs) {

    if ($(list + ' li').length < 1) {

        let i;

        for (i = 0; i < 24; i++) {
            $(list).append(
       $(`<li class="hour" data-picked="no">
       <span class="pill">
       <span class="hours">${i}</span>
       <span class="minutes">00</span>
       </span>
       <div class="collapsible" id=${i}>
       ${appendMultipleInputs(inputs, i)}
       </div></li>`)
            );
        };
    }

};

function changeDataPicked(selector) {

    if ($(selector).attr('data-picked') === 'no') {
        $(selector).attr('data-picked', 'yes');

    } else {
        $(selector).attr('data-picked', 'no');
    };
    $(selector).find('.collapsible').slideToggle('slow');
}

function submitHours(selector) {
    $(selector).click(function () {

        var pairs = {};

        $("[data-picked='yes']").each(function(e) {

            var id = $(this).find('.collapsible').attr('id');
            var hour = `${id}:00`;
            pairs[hour] = {};

            $(`[data-input="${id}"]`).each(function() {
                let name = $(this).attr('name');
                let val = $(this).val();
                pairs[hour][name] = val;
            });
        });
        console.log(pairs);
        console.log(JSON.stringify(pairs));
    });
};

function enableCloseWithEscape() {
    window.onkeydown = function (event) {
        if (event.keyCode == 27) {
            $('.popup').fadeOut(300);
        };
    };

}
$(function () {


    enableCloseWithEscape();

    $("#startUsageDate").val($("#dateOfIssue").val());

    $("#addMed").click(function() {
        var currentMed = {};
        $(".newMed").each(
            function() {
                let key = $(this).attr("name");
                let value = $(this).val();
                currentMed[key] = value;
            });
        medicines.push(currentMed);

        var jsonMed = JSON.stringify(currentMed);
        console.log(jsonMed);
        if (!$("#listTable td:contains(" +
                $("[data-medicine='name']").val() +
                ")").length >
            0) {
            $("#listTable").find('tbody').append(
                "<tr><td>" +
                $("[data-medicine='name']").val() +
                "</td><td>" +
                $("#prescriptedBoxCount").val() +
                "</td>");
            $("[data-medicine]").val('');
            $("#startUsageDate").val($("#dateOfIssue").val());
            $('[data-popup="popup-1"]').fadeOut(350);
            $("tr:last").slideDown();
        } else {
            $("#errorMessage").text("You already have this med!");
            console.log('you already have it');
        }
    });

    $('#addPrescription').click(function() {
        var obj = {};
        currentPrescription['name'] = $("[data-prescription='name']").val();
        currentPrescription['dateOfIssue'] = $("#dateOfIssue").val();
        obj['prescription'] = currentPrescription;
        obj['medicines'] = medicines;

        var jsonAble = JSON.stringify(obj);

        $.ajax({
            type: "POST",
            url: "/Prescription/AddPrescriptionAsync/",
            contentType: "application/json;charset=utf-8",
            data: JSON.stringify({ json: jsonAble }),
            dataType: "json",
            error: function(err) {
                $("#errorMessage").text(err.message);

            },
            success: function(data) {

                if (data["message"] === "success") {
//                    sessionStorage.setItem("popUp",
//                        `The prescription ${currentPrescription['name']}has been created!`);

                    $('#times').fadeIn(300);
                    populateList('#hours', Object.keys(data['meds']));
                    $(".hour").click(function (e) {


                        var sender = e.target;

                        if ($(sender).is('li')) {
                            changeDataPicked(this);

                        }
                    });
                    submitHours('#submit');
                } else {
                    $("#errorMessage").text(data["message"]);

                }


            }

        });
    });
});

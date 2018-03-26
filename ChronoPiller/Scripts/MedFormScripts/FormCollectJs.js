var currentPrescription = {};
var medicines = [];

$(function() {

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
            $("[data-medicine='name']").val() + ")").length > 0) {

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
        };

    });

    $('#addPrescription').click(function() {
        var obj = {};
        currentPrescription['name'] = $("[data-prescription='name']").val();
        currentPrescription['dateOfIssue'] = $("#dateOfIssue").val();
        obj['prescription'] = currentPrescription;
        obj['medicines'] = medicines;

        var jsonAble = JSON.stringify(obj);

        console.log(jsonAble);

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
                console.log(typeof data);
                console.log(data);
                console.log(window.location.href);

                if (data["message"] === "success") {
                    window.location.href = "/";
                    sessionStorage.setItem("popUp",
                        `The prescription${currentPrescription['name']}has been created!`);
                } else {
                    $("#errorMessage").text(data["message"]);
                    
                }


            }

        });
    });
});
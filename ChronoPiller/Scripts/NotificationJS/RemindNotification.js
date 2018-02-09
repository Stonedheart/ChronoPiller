var remindOptions = {
    body: "Go on!\n",
    icon: '../Content/Images/yoda.jpg',
    requireInteraction: true,
    sticky: true
};


function remind() {
    var date = new Date();
    var dateString = date.getDate() + '.' + (date.getMonth() + 1) + '.' + date.getFullYear();
    $(
        $.ajax({
            url: '/Notification/Check',
            type: 'GET',
            data: { 'clientDate': dateString },
            contentType: 'application/json',
            fail: function(data) {
                console.log("Couldn't send " + data);
            },
            success: function(result) {

                if (result === true) {
                    new Notification("TAKE YOU PILL", remindOptions);
                    $.ajax({
                        url: "/Notification/SendMail",
                        type: "GET",
                        contentType: 'application/json',
                        success: function() {
                            console.log("MAIL SENT");

                        },
                        fail: function() {
                            console.log("Mail not sent.");
                        }
                    });
                }

            }
        }).done(function() {
            console.log("Done!");
        }));


}
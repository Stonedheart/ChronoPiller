var remindOptions = {
    body: "Go on!\n",
    icon: '../Content/Images/yoda.jpg',
    requireInteraction: true,
    sticky: true
};




function remind() {
    var date = new Date();
    var dateString = (date.getDate()) + '.' + (date.getMonth() + 1) + '.' + date.getFullYear();
    $(
        $.ajax({
            url: '/Home/Remind/',
            type: 'GET',
            data: {'clientDate': dateString},
            contentType: 'application/json',
            fail: function () {
                console.log("FAILED")
            },
            success: function (result) {

                console.log(result);
                new Notification("Accepted JSON!", remindOptions)
            }
        }).done(function () {
            console.log("Done!")
        }));


}

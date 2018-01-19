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

                if (result === true) {
                    new Notification("TAKE YOU PILL", remindOptions);
                    // $.ajax({
                    //     url: "/Home/SendMail",
                    //     type: "GET",
                    //     success: function () {
                    //         console.log("MAIL SENT")
                    //        
                    //     }
                    // })
                }
                
            }
        }).done(function () {
            console.log("Done!")
        }));


}

var items = [];

function remind() {
    var date = new Date();
    var dayMonth = date.getDay() + '.' + date.getMonth();
    console.log('dupa');
    $(
        $.ajax({
            url: '/Home/Remind/',
            type: 'GET',
            contentType: 'application/json',
            fail: function () {
                console.log("FAILED")
            },
            success: function (data) {

                console.log(data);
            }
        }).done(function () {
            console.log("Done!")
        }));


}

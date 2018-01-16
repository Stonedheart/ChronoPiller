var options = {
    body: "Surf on, nigga!\n",
    icon: 'http://i0.kym-cdn.com/photos/images/newsfeed/001/104/774/107.jpg'
};


function checkForPermission() {
    $(function () {
        console.log(Notification.permission);
        if (Notification.permission === 'granted') {
            var helloNote = new Notification('Already granted!');

        } else {

            Notification.requestPermission().then(function (result) {
                if (result === 'granted') {
                    var grantedNote = new Notification("Granted Now", options);
                }
                else {

                    console.log("Failed")
                }
            })
        }
    })

}

function remind() {
    console.log("I see function!");
    $(function () {
        $("#remind").click(function () {
            console.log("I see button!");
            var reminderNote = new Notification("Take your pill!", options);

        })


    })

}

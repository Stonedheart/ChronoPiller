var options = {
    body: "Surf on, nigga!\n",
    icon: '../Content/Images/yoda.jpg'
};

var remindOptions = {
    body: "Go on!\n",
    icon: '../Content/Images/yoda.jpg',
    requireInteraction: true,
    sticky: true
};


function checkForPermission() {
    $(function () {
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
    $(function () {
        $("#remind").click(function () {
            console.log("I see button!");
            var reminderNote = new Notification("Take your pill!", remindOptions);
            reminderNote.onclick = function (ev) {
                ev.preventDefault();
                new Notification("Das gut, \nkeep da good work, my man!", options);
            }

        })


    })

}

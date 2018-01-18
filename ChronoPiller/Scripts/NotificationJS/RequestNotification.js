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
            var helloNote = new Notification('Already granted!', options);

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


var options = {
    body: "Surf on, nigga!\n",
    icon: '../Content/Images/yoda.jpg'
};

function checkForPermission() {
    $(function() {
        if (Notification.permission === 'granted') {
            console.log('granted!');

        } else {

            Notification.requestPermission().then(function(result) {
                if (result === 'granted') {
                    var grantedNote = new Notification("Granted Now", options);
                } else {

                    console.log("Failed");
                }
            });
        }
    });
}


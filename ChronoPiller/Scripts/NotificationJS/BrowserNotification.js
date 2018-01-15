var d = new Date();

var options = {
    body: "Surf on, nigga! " + d.toDateString(),
    icon: 'http://i0.kym-cdn.com/photos/images/newsfeed/001/104/774/107.jpg'
};


function checkForPermission() {
    $(function () {
        var helloNote = new Notification("Already granted!");
        if (Notification.permission === "granted") {
            // If it's okay let's create a notification
            helloNote.show();
        }

        Notification.requestPermission().then(function (result) {
            if (result === 'granted') {
                var grantedNote = new Notification("Granted Now", options);
            }
            else {

                console.log("Failed")
            }
        })
    })

}

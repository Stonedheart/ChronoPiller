var options = {
    body: "Surf on, nigga!",
    icon: 'http://i0.kym-cdn.com/photos/images/newsfeed/001/104/774/107.jpg'
};

function ready() {
    $(function () {
        if (Notification.permission == 'granted') {

            var hello = new Notification('Hello!', options);
        }
        else {
            Notification.requestPermission().then(function (result) {
                if (result == 'granted') {
                    var grantedNote = new Notification("Permission granted!", options);
                }

            })
        }

    })

}

var options = {
    body: "Surf on, nigga!\n",
    icon: '../Content/Images/yoda.jpg'
};

function checkForPermission() {
    $(function() {
        if (Notification.permission === 'granted') {
            console.log('granted!');
            if (sessionStorage.getItem("popUp") !== null) {
                options.body = "That is great!";
                let note = new Notification(sessionStorage.getItem("popUp"), options);
                sessionStorage.clear();
            }

        } else {

            Notification.requestPermission().then(function(result) {
                if (result === 'granted') {
                    var grantedNote = new Notification("Granted Now", options);
                }
            });
        }
    });
}
            
       
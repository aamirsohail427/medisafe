var loginId = $('#userid').val();
var apiKey = '46267432';
var session;
var sessionId = '1_MX40NjI2NzQzMn5-MTU0OTk2Nzc1NzcxOX5id0tXM0RhZWdKMldiQzQ4LzRpMEFzeTR-fg';
var token = $('#token').val();

if (sessionId !== '' && token !== '') {

    connect(sessionId, token);


}
function sendNotification(userId, message, type, username) {

    var notification = {
        NotificationTo: userId,
        NotificationFrom: loginId,
        NotificationMessage: message,
        NotificationType: type,
        SenderName: username
    };

    setTimeout(function () {

        var customJson = JSON.stringify(notification);
        sendLiveNotification(customJson);
    }, 5000);
}
function sendMessage(userId, message, type, username, Messagefrom,Chatid, UserPhoto, msgid) {
    debugger
    var notification = {
        NotificationTo: userId,
        NotificationFrom: Messagefrom,
        NotificationMessage: message,
        NotificationType: type,
        SenderName: username,
        Photo: UserPhoto,
        msgidd: msgid,
        Chatid: Chatid
    };

    setTimeout(function () {

        var customJson = JSON.stringify(notification);
        sendLiveMessage(customJson);
    }, 5000);
}
function sendLiveNotification(customJson) {
    session.signal({
        type: "notification",
        data: customJson
    },
        function (error) {
            if (error) { }
            console.log(error);
        }
    );
}
function sendLiveMessage(customJson) {
    debugger
    session.signal({
        type: "msg",
        data: customJson
    },
        function (error) {
            if (error) { }
            console.log(error);
        }
    );
}
//notification method end here


function connect(sessionId, token) {
    OT.on("exception", exceptionHandler);

    // Un-comment the following to set automatic logging:
    OT.setLogLevel(OT.DEBUG);

    if (!(OT.checkSystemRequirements())) {
        alert("You don't have the minimum requirements to run this application.");
    } else {

        session = OT.initSession(sessionId);	// Initialize session
        session.connect(apiKey, token);
        // Add event listeners to the session
        session.on('sessionConnected', sessionConnectedHandler);
        session.on('sessionDisconnected', sessionDisconnectedHandler);
        session.on('connectionCreated', connectionCreatedHandler);
        session.on('connectionDestroyed', connectionDestroyedHandler);

        session.on("signal", signalEventHandler);
    }
}

function disconnect() {

    session.disconnect();

}


function sessionConnectedHandler(event) {
    console.log(event)

}



//******************** End Call Logs *******************************************
function signalEventHandler(event) {
   
    debugger
    var ChatID = $("#chatid").val();
    if (event.type == 'signal:notification') {

        var msgObj = JSON.parse(event.data);
        if (msgObj.NotificationTo == loginId) {
            setTimeout(function () {



                if ('Notification' in window) {
                    let ask = Notification.requestPermission();
                    ask.then(permission => {
                        if (permission === "granted") {
                            let msg = new Notification("Incoming notification!", {
                                body: msgObj.NotificationMessage,
                                icon: 'https://www.google.com/url?sa=i&source=images&cd=&cad=rja&uact=8&ved=2ahUKEwj0-sv5qrbgAhWR-KQKHdWyCJsQjRx6BAgBEAU&url=https%3A%2F%2Fwww.thehandbook.com%2Fcelebrity%2Fkatie-pix%2F&psig=AOvVaw2gqvfbjB955E7LJnijj9wx&ust=1550065715523572'
                            });
                            msg.addEventListener("click", event => {
                                alert("Click received");
                            })
                        }
                    });
                }

                //ShowMessage(msgObj.message, "Notification!", "success", true);

            }, 2000);

            GetAllNotifications();
        }
    } else {
        var d = new Date($.now());
        var n = d.getTimezoneOffset() / 60;
        if (n < 0) {

            n = -(n);
        }
        var currdatetime = (d.getHours() - n) + ":" + d.getMinutes();
        if (event.type == 'signal:msg') {
        //    debugger;
            var msgObj = JSON.parse(event.data);
            var msgidarray = msgObj.NotificationTo;                        var checkUser = $.grep(msgidarray, function (n, i) {                return (n == loginId);            });
            
           

            if (checkUser.length >0 ) {
                if (typeof ChatID !== "undefined") {
                    if (ChatID == msgObj.Chatid) {
                        var appendmsgs = "";
                        var likeimage = "/Assets/Smiles/like.png";

                        appendmsgs += "<div><div class='message-container' onmouseover='ShowReact(" + msgObj.msgidd + ") ' onmouseout='HideReact(" + msgObj.msgidd + ")'><div name ='message_4016952' class='message' > <div class='message-gutter'><div class='avatar'><img alt='asimmb' class='img-circle bordered-grey' src='" + msgObj.Photo + "' ></div> </div><div class='message-content'><div class='message-header'><div class='user-profile-menu dropdown btn-group'> <a id='user-profile-menu' role='button' class='name dropdown-toggle' aria-haspopup='true' aria-expanded='false' href='#'>" + msgObj.SenderName + "</a><div class='portal-dropdown-wrapper'></div> </div><span class='time'>" + currdatetime + "</span></div><div class='message-content-wrapper'><div properties='[object Object]' class='message-body-wrapper'><div class='message-body' >" +  "<div id='message-assets-" + msgObj.msgidd + "' class='message-items'>" +
                        "<button class='title-dropdown-toggle dropdown-toggle btn btn-default pull-right cstm-btn' type='button' id='dropdownbtn' data-toggle='dropdown' aria-haspopup='true' aria-expanded='false'>" +
                            "<i class='fa fa-ellipsis-h'></i>" +
                            " </button>" +
                            "<div class='dropdown-menu dropdown-menu-right' aria-labelledby='dropdownbtn'>" +
                            "<a class='dropdown-item' onclick='SetMessageReaction("+ msgObj.msgidd +",5)'>" +
                            " <img src='/Assets/Smiles/like.png' width='25px' height='25px' />" +
                            "Like" +
                            " </a>" +
                            "<a class='dropdown-item'  onclick='SetMessageReaction("+ msgObj.msgidd +",4)'>" +
                            "<img src='/Assets/Smiles/emoticons.png' width='25px' height='25px' />" +
                            "Happy" +
                            "</a>" +
                            "<a class='dropdown-item'  onclick='SetMessageReaction("+ msgObj.msgidd +",1)'>" +
                            "<img src='/Assets/Smiles/haha.png' width='25px' height='25px' />" +
                            "Hahaha" +
                            "</a>" +
                            "<a class='dropdown-item'  onclick='SetMessageReaction("+ msgObj.msgidd +",2)'>" +
                            "<img src='/Assets/Smiles/angry.png' width='22px' height='22px' />" +
                            "Angry" +
                            "</a>" +
                            "</div>"+
                            "<div class='pull-right' onclick ='EditMessage(" + msgObj.msgidd + ")'>"+

                                "<i class='fa fa-pencil-square-o' aria-hidden='true' style='margin-top:4px; margin-left:10px;'></i>"+
                                                                            "</div>"+
                            "<div class='pull-right' onclick='SetMessageReaction("+ msgObj.msgidd+",3)'>"+
                            "<img src='/Assets/Smiles/star.png' width='23' height='20' style='margin-top: -6px;' />"+
                            "</div>" +
                            " </div>"
                            + "<span msgsender='" + msgObj.NotificationFrom + "' id='chatmsgtext-" + msgObj.msgidd + "'>" + msgObj.NotificationMessage + "</span > <div class='message-reactions' id='reactdisplay-" + msgObj.msgidd + "'> </div> </div >  </div > </div ></div > </div ></div ></div > "
                        $("#newmsget").append(appendmsgs);
                    }
                    else {
                        //var count = $("#chatcount-" + msgObj.Chatid + "").text();
                        //if (count != "" && count != null) {

                        //    count = parseInt(count) + 1;
                        //    $("#chatcount-" + msgObj.Chatid + "").text(count);
                        //}
                        //else {
                        //    $("#chatcount-" + msgObj.Chatid + "").attr("class", "label label-rouded label-menu")
                        //    $("#chatcount-" + msgObj.Chatid + "").text("1");
                        //}

                    }   
                }
                else {


                    //var count = $("#chatcount-" + msgObj.Chatid + "").text();
                    //if (count != "" && count != null) {

                    //    count = parseInt(count) + 1;
                    //    $("#chatcount-" + msgObj.Chatid + "").text(count);
                    //}
                    //else {
                    //    $("#chatcount-" + msgObj.Chatid + "").attr("class","label label-rouded label-menu")
                    //    $("#chatcount-" + msgObj.Chatid + "").text("1");
                    //}

                }
            }
        }


    }
}


    function sessionDisconnectedHandler(event) {
        // This signals that the user was disconnected from the Session. Any subscribers and publishers
        // will automatically be removed. This default behaviour can be prevented using event.preventDefault()
        //// 

        session.off('sessionConnected', sessionConnectedHandler);
        session.off('streamCreated', streamCreatedHandler);
        session.off('streamDestroyed', streamDestroyedHandler);
        session.off('connectionCreated', connectionCreatedHandler);
        session.off("signal", signalEventHandler);
        OT.off("exception", exceptionHandler);
        session.off('sessionDisconnected', sessionDisconnectedHandler);



    }




    function connectionDestroyedHandler(event) {


    }


    function connectionCreatedHandler(event) {
        console.log(event)

    }

    /*
    If you un-comment the call to OT.setLogLevel(), above, OpenTok automatically displays exception event messages.
    */
    function exceptionHandler(event) {
        console.log(event)

    }



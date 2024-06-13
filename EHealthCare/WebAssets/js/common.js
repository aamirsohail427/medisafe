function alertG(msg) {
    new PNotify({
        title: 'Congratulations!',
        text: msg,
        delay: 2000,
        type: 'success',
        addclass: "stack-bottomright",
        backgroundColor: "#D94747",
        borderColor: "#D94747"
    });
}
function alertR(msg) {
    new PNotify({
        title: 'Error!',
        text: msg,
        delay: 2000,
        type: 'error',
        addclass: "stack-bottomright"
    });
}
function alertE(msg) {
    new PNotify({
        title: 'Warning',
        text: msg,
        delay: 2000,
        type: 'info',
        addclass: "stack-bottomright"
    });
}

function alertW(msg) {
    new PNotify({
        title: 'Username Taken!',
        text: msg,
        delay: 2000,
        type: 'warning',
        addclass: "stack-bottomright",
        backgroundColor: "#fff3cd",
        borderColor: "#fff3cd"
    });
}

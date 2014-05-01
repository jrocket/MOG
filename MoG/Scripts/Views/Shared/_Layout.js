
$(function () {
    var $btnNotification = $('#BtnNotifications');
    // Reference the auto-generated proxy for the hub.
    var chat = $.connection.notificationHub;
    // Create a function that the hub can call back to display messages.
    chat.client.addNotification = function (message) {
        $('#Notifications').prepend('<li>'+message+'</li>')
        $btnNotification.css('color', 'red');
    };
    // Start the connection.
    $.connection.hub.start();
});


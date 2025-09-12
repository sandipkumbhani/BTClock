(function () {
    var messageDiv = document.getElementById('temporaryMessage');
    if (messageDiv) {

        setTimeout(function () {
            messageDiv.style.display = 'none';
        }, 5000);

    }
    var deptmessageDiv = document.getElementById('deptTemporaryMessage');
    if (deptmessageDiv) {

        setTimeout(function () {
            deptmessageDiv.style.display = 'none';
        }, 5000);

        var desigmessageDiv = document.getElementById('desigtemporaryMessage');
        if (desigmessageDiv) {
            setTimeout(function () {
                desigmessageDiv.style.display = 'none';
            }, 5000);

            var leavemessageDiv = document.getElementById('leavetemporaryMessage');
            if (leavemessageDiv) {
                setTimeout(function () {
                    leavemessageDiv.style.display = 'none';
                }, 5000);
            }
        }
    }
})();
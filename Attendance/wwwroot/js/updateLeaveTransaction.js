function updateTotalDays(start, end) {
    let days = end.diff(start, 'days') + 1;
    if (days < 1) days = 1; // prevent zero or negative days

    const isHalfDay = document.getElementById("Ishalfday").checked;
    const totalDays = isHalfDay ? 0.5 : days;

    document.getElementById("TotalDays").value = totalDays;

    // Optional: if you display total days somewhere in the view, update it here
    const totalDaysDisplay = document.getElementById("TotalDaysDisplay");
    if (totalDaysDisplay) {
        totalDaysDisplay.textContent = totalDays;
    }
}

$(function () {
    var start = $('#StartDate').val() ? moment($('#StartDate').val()) : moment();
    var end = $('#EndDate').val() ? moment($('#EndDate').val()) : moment();

    function updateDisplay(start, end) {
        $('#reportrange span').text(start.format('MM/DD/YYYY') + ' - ' + end.format('MM/DD/YYYY'));
        $('#StartDate').val(start.format('YYYY-MM-DD'));
        $('#EndDate').val(end.format('YYYY-MM-DD'));
        updateTotalDays(start, end);
    }

    $('#reportrange').daterangepicker({
        startDate: start,
        endDate: end,
        opens: 'right',
        locale: {
            format: 'MM/DD/YYYY'
        }
    }, updateDisplay);

    updateDisplay(start, end);

    $('#Ishalfday').change(function () {
        const startDate = moment($('#StartDate').val());
        const endDate = moment($('#EndDate').val());
        updateTotalDays(startDate, endDate);
    });

    document.getElementById('AddFile').addEventListener('change', function () {
        var fileName = this.files.length > 0 ? this.files[0].name : "No file chosen";
        document.getElementById('pdfFileName').textContent = fileName;
    });
});

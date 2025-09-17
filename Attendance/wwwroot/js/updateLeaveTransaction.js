$(function () {
    var start = $('#StartDate').val() ? moment($('#StartDate').val()) : moment();
    var end = $('#EndDate').val() ? moment($('#EndDate').val()) : moment();

    function updateDisplay(start, end) {
        $('#reportrange span').text(start.format('MM/DD/YYYY') + ' - ' + end.format('MM/DD/YYYY'));
        $('#StartDate').val(start.format('YYYY-MM-DD'));
        $('#EndDate').val(end.format('YYYY-MM-DD'));

        updateTotalDays(start, end);
    }

    //function updateTotalDays(start, end) {
    //    var diff = end.diff(start, 'days') + 1;

    //    if ($('#Ishalfday').is(':checked')) {
    //        diff -= 0.5;
    //    }

    //    if (diff < 0) diff = 0;

    //    $('#TotalDays').val(diff);
    //}

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
        var startDate = moment($('#StartDate').val());
        var endDate = moment($('#EndDate').val());
        updateTotalDays(startDate, endDate);
    });
});
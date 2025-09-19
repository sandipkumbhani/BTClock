function updateTotalDays(start, end) {
    var days = end.diff(start, 'days') + 1;
    $('#TotalDays').val(days);
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
        autoUpdateInput: false,
        locale: {
            format: 'MM/DD/YYYY',
            cancelLabel: 'Clear'
        }
    }, function (start, end) {
        updateDisplay(start, end);
    });

    updateDisplay(start, end);

    $('#Ishalfday').change(function () {
        var startDate = moment($('#StartDate').val());
        var endDate = moment($('#EndDate').val());
        updateTotalDays(startDate, endDate);
    });
});

$(function () {
    const defaultStart = moment().add(1, 'days');
    const defaultEnd = moment().add(1, 'days');

    function cb(start, end) {
        $('#reportrange span').html(start.format('MMMM D, YYYY') + ' - ' + end.format('MMMM D, YYYY'));
        $('#TotalDays').val(end.diff(start, 'days') + 1);
        $('#StartDate').val(start.format('YYYY-MM-DD'));
        $('#EndDate').val(end.format('YYYY-MM-DD'));
    }

    $('#reportrange').daterangepicker({
        startDate: defaultStart,
        endDate: defaultEnd,
        locale: { format: 'MMMM D, YYYY' }
    }, cb);

    cb(defaultStart, defaultEnd);
});
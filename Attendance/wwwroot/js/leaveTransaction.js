$(function () {
    const defaultStart = moment();
    const defaultEnd = moment();

    function cb(start, end) {
        const totalDays = end.diff(start, 'days') + 1;
        $('#reportrange span').html(start.format('MMMM D, YYYY') + ' - ' + end.format('MMMM D, YYYY'));
        $('#TotalDays').val(totalDays);
        $('#StartDate').val(start.format('YYYY-MM-DD'));
        $('#EndDate').val(end.format('YYYY-MM-DD'));

        if (totalDays > 1) {
            $('#Ishalfday').prop('checked', false).prop('disabled', true);
        } else {
            $('#Ishalfday').prop('disabled', false);
        }
    }

    $('#reportrange').daterangepicker({
        startDate: defaultStart,
        endDate: defaultEnd,
        locale: { format: 'MMMM D, YYYY' }
    }, cb);

    cb(defaultStart, defaultEnd);
});

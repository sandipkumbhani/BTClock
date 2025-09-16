$('.date').datepicker({
	format: 'yyyy-mm-dd',
	autoclose: true,
	beforeShowDay: function (date) {
	}
}).on('change', function () {
	$('.datepicker').hide();
});
$('.yearpicker').datepicker({
	format: 'yyyy',         // Show only year
	autoclose: true,
	minViewMode: 'years',   // Restrict to year view
	beforeShowDay: function (date) {
		return true; // Keep if you want to disable specific years later
	}
}).on('change', function () {
	$('.datepicker').hide();
});
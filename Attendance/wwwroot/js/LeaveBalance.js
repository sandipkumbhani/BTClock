$(document).ready(function () {
    $("#dataLoader").show()
    $("#corporateTable tbody").empty();

    $.ajax({
        url: '/LeaveBalance/GetallLeavebalance',
        datatype: 'json',
        method: 'GET',
        success: function (result) {
            console.log(result);

            var table = $('#corporateTable').DataTable();
            table.clear();
            table.on('draw', function () {
                var visibleRows = table.rows({ filter: 'applied' }).count();
                $('#totalRemindersCorporate').text(`Total List: ${visibleRows}`);
            });

            if (result && result.length > 0) {
                $('#leavelist').show();
                const newRows = [];
                $.each(result, function (index, detail) {
                    newRows.push([
                        detail.user.name,
                        detail.leaveMaster.name,
                        detail.assignedLeaves,
                        detail.usedLeaves,
                        detail.remainingLeaves,
                        detail.extraLeaves
                    ]);

                });
                table.rows.add(newRows).draw();
                $('#totalRemindersCorporate').text(`Total List: ${newRows.length}`);
            }
            else {
                table.row.add([
                    `<span class="text-center" style="display:block;" colspan="9">No Data Found</span>`,
                    '', '', '', '', '', '', '', ''
                ]).draw();
                $('#totalRemindersCorporate').text(`Total List: 0`);
            }
            $("#dataLoader").hide();
        },
        error: function (err) {
            console.log(err);
        }
    });
});


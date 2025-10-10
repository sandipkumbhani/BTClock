$(document).ready(function () {
    $("#dataLoader").show();
    $("#corporateTable tbody").empty();

    let table = $('#corporateTable').DataTable();
    table.clear().draw();

    $.ajax({
        url: '/LeaveAssignment/LeaveAssignmentViewDetails',
        method: 'GET',
        dataType: 'json',
        success: function (result) {
            console.log(result);

            var table = $('#corporateTable').DataTable();
            table.clear();


            $('#corporateTable').DataTable();

            if (result.data && result.data.length > 0) {
                $('#userList').show();
                const newRows = [];

                $.each(result.data, function (index, detail) {
                    console.log(detail);
                    var leaveMaster = result.masters.find(m => m.id === detail.leavemasterId);

                    newRows.push([
                        leaveMaster ? leaveMaster.name : '',
                        detail.totalAllocatedLeaves,
                        detail.paidAllocatedLeaves,
                        `<a href="${url + detail.leaveAssignmentId}" style="margin-right: 10px;"><i class="ri-edit-2-line" style="font-size:x-large;"></i></a>`
                    ]);
                });

                table.rows.add(newRows).draw();

                $('#totalRemindersCorporate').text(`Total List: ${newRows.length}`);
            } else {
                var noDataRow = `<tr><td colspan="4" class="text-center">No Data Found</td></tr>`;
                $('#corporateTable tbody').html(noDataRow);

                $('#totalRemindersCorporate').text(`Total List: 0`);
            }

            $("#dataLoader").hide();
        },
        error: function (err) {
            console.log(err);
            $("#dataLoader").hide();
        }

    });


    $('#corporateTable').on('draw.dt', function () {
        const visibleRows = table.rows({ filter: 'applied' }).count();
        $('#totalRemindersCorporate').text(`Total List: ${visibleRows}`);
    });
});

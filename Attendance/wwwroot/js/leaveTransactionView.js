$(document).ready(function () {
    $("#dataLoader").show();
    $("#corporateTable tbody").empty();

    $.ajax({
        url: '/LeaveTransaction/LeaveTransactionViewDetails',
        method: 'GET',
        dataType: 'json',
        success: function (response) {
            console.log(response);

            var table = $('#corporateTable').DataTable();

            table.clear();

            table.on('draw', function () {
                var visibleRows = table.rows({ filter: 'applied' }).count();
                $('#totalRemindersCorporate').text(`Total List: ${visibleRows}`);
            });

            if (response.data && response.data.length > 0) {
                $('#userList').show();
                const newRows = [];

                $.each(response.data, function (index, detail) {
                    var employee = response.users.find(user => user.id === detail.employeeId);
                    var approvedBy = response.users.find(user => user.id === detail.approvedBy);
                    var leaveMaster = response.masters.find(m => m.id === detail.leaveMasterId);

                    var statusMap = {
                        0: "Pending",
                        1: "Approved",
                        2: "Rejected"
                    };

                    var statusText = statusMap[detail.leaveStatus];

                    let actionButtons = '';

                    if (detail.leaveStatus === 0) {
                        actionButtons += `<a href="${url + detail.leaveTransactionId}" style="margin-right: 10px;">
                            <i class="ri-edit-2-line" style="font-size:x-large;"></i>
                        </a>`;
                    }

                    actionButtons += `<a href="${deleteurl + detail.leaveTransactionId}" onclick="return confirm('Are you sure you want to delete this record?');">
                        <i class="ri-delete-bin-3-line" style="font-size:x-large;color:red;"></i>
                    </a>`;

                    const row = [
                        employee?.name,
                        leaveMaster ? leaveMaster.name : '',
                        detail.isPaid ? 'Yes' : 'No',
                        detail.ishalfday ? 'Yes' : 'No',
                        detail.startDate ? new Date(detail.startDate).toLocaleDateString() : '',
                        detail.endDate ? new Date(detail.endDate).toLocaleDateString() : '',
                        detail.totalDays,
                        detail.reason,
                        statusText,
                        actionButtons,
                    ];

                    newRows.push(row);
                });

                table.rows.add(newRows).draw();

                table.rows().every(function () {
                    var rowData = this.data();
                    var status = rowData[8];

                    if (status === 'Approved' || status === 'Rejected') {
                        $(this.node()).addClass('disabled-row');
                        $(this.node()).find('a, input, button, select, textarea')
                            .addClass('disabled-link')
                            .attr('tabindex', -1);
                    }
                });

                $('#totalRemindersCorporate').text(`Total List: ${newRows.length}`);
            } else {
                table.row.add([
                    `<span class="text-center" style="display:block;" colspan="12">No Data Found</span>`,
                    '', '', '', '', '', '', '', '', ''
                ]).draw();

                $('#totalRemindersCorporate').text(`Total List: 0`);
            }

            $("#dataLoader").hide();
        },
        error: function (err) {
            console.error(err);
            $("#dataLoader").hide();
        }
    });
});


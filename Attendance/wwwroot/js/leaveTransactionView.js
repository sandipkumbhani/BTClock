$(document).ready(function () {
    $("#dataLoader").show();
    $("#corporateTable tbody").empty();

    let table = $('#corporateTable').DataTable();
    table.clear().draw();

    $.ajax({
        url: '/LeaveTransaction/LeaveTransactionViewDetails',
        method: 'GET',
        dataType: 'json',
        success: function (response) {
            console.log(response);

            const newRows = [];

            if (response.data && response.data.length > 0) {
                $('#userList').show();

                $.each(response.data, function (index, detail) {
                    const leaveMaster = response.masters.find(m => m.id === detail.leaveMasterId);
                    const updatedByUser = response.users.find(x => x.id === detail.updatedby)?.name || '';
                    const approvedByUser = response.users.find(x => x.id === detail.approvedBy)?.name || '';

                    const appliedOn = detail.appliedOn || "";
                    const startDate = detail.startDate ? new Date(detail.startDate).toLocaleDateString() : '';
                    const endDate = detail.endDate ? new Date(detail.endDate).toLocaleDateString() : '';
                    const updatedAt = detail.updatedat ? detail.updatedat.split('T')[0] : '';
                    const approvedAt = detail.approvedAt ? detail.approvedAt.split('T')[0] : '';
                    const splitAppliedOn = appliedOn.split('T')[0];

                    const statusMap = {
                        0: "Pending",
                        1: "Approved",
                        2: "Rejected"
                    };

                    const statusText = statusMap[detail.leaveStatus];

                    const fileCell = detail.addFile
                        ? `<a href="/leave_pdf/${detail.addFile}" target="_blank" title="View PDF">
        <i class="ri-file-pdf-2-line" style="font-size: 24px; color: red;"></i></a>`
                        : '';

                    let actionButtons = '';
                    if (detail.leaveStatus === 0 || detail.leaveStatus === 2) {
                        actionButtons += `<a href="${url + detail.leaveTransactionId}" style="margin-right: 10px;">
                            <i class="ri-edit-2-line" style="font-size:x-large;"></i>
                        </a>`;

                        actionButtons += `<a onclick="deleteLeaveTransaction(${detail.leaveTransactionId})" style="color:red;">
                            <i class="ri-delete-bin-3-line" style="font-size:x-large;"></i>
                        </a>`;
                    }

                    newRows.push([
                        leaveMaster ? leaveMaster.name : '',
                        detail.isPaid ? 'Yes' : 'No',
                        detail.ishalfday ? 'Yes' : 'No',
                        splitAppliedOn,
                        startDate,
                        endDate,
                        detail.ishalfday ? 0.5 : detail.totalDays,
                        detail.reason,
                        updatedAt,
                        updatedByUser,
                        approvedAt,
                        approvedByUser,
                        statusText,
                        fileCell,
                        actionButtons
                    ]);
                });

                newRows.sort(function (a, b) {
                    const statusA = a[12];
                    const statusB = b[12];

                    if (statusA === "Pending" && statusB !== "Pending") return -1;
                    if (statusA === "Approved" && statusB === "Rejected") return -1;
                    if (statusA === "Rejected" && statusB !== "Rejected") return 1;
                    if (statusB === "Pending" && statusA !== "Pending") return 1;

                    return 0;
                });
                table.rows.add(newRows).draw();

                table.rows().every(function () {
                    const rowData = this.data();
                    const status = rowData[12];

                    if (status === 'Approved') {
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
                    '', '', '', '', '', '', '', '', '', '', '', '', '', ''
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

    $('#corporateTable').on('draw.dt', function () {
        const visibleRows = table.rows({ filter: 'applied' }).count();
        $('#totalRemindersCorporate').text(`Total List: ${visibleRows}`);
    });
});

function deleteLeaveTransaction(id) {
    if (!confirm('Are you sure you want to delete this record?')) return;

    $.ajax({
        url: '/LeaveTransaction/DeleteLeaveTransaction',
        type: 'POST',
        data: { id: id },
        success: function () {
            location.reload();
        },
        error: function () {
            alert('An error occurred while deleting the leave transaction.');
        }
    });
}

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
                    var username = response.users.find(x => x.id === detail.updatedby);
                    var Updatebyuser = username?.name || '';
                    var appliedon = detail.appliedOn || "";
                    var splitApproveon = appliedon.split('T')[0]
                    //const employee = response.users.find(user => user.id === detail.employeeId);
                    const leaveMaster = response.masters.find(m => m.id === detail.leaveMasterId);
                    var Updatedate = detail.updatedat || "";
                    var splitdate = Updatedate.split('T')[0];

                    var username = response.users.find(x => x.id === detail.approvedBy);
                    var approvebyuser = username?.name || '';
                    var approveddate = detail.approvedAt || "";
                    var splitApprovedate = approveddate.split('T')[0]
                    const statusMap = {
                        0: "Pending",
                        1: "Approved",
                        2: "Rejected"
                    };

                    const statusText = statusMap[detail.leaveStatus];

                    let actionButtons = '';

                    if (detail.leaveStatus === 0 || detail.leaveStatus === 2) {
                        actionButtons += `<a href="${url + detail.leaveTransactionId}" style="margin-right: 10px;">
                            <i class="ri-edit-2-line" style="font-size:x-large;"></i>
                        </a>`;

                        actionButtons += `<a onclick="deleteLeaveTransaction(${detail.leaveTransactionId})" style="color:red;">
                            <i class="ri-delete-bin-3-line" style="font-size:x-large;"></i>
                        </a>`;
                    } 

                    const row = [
                        leaveMaster ? leaveMaster.name : '',
                        detail.isPaid ? 'Yes' : 'No',
                        detail.ishalfday ? 'Yes' : 'No',
                        splitApproveon,
                        detail.startDate ? new Date(detail.startDate).toLocaleDateString() : '',
                        detail.endDate ? new Date(detail.endDate).toLocaleDateString() : '',
                        detail.totalDays,
                        detail.reason,
                        splitdate,
                        Updatebyuser,
                        splitApprovedate,
                        approvebyuser,
                        statusText,
                        actionButtons,
                    ];

                    newRows.push(row);
                });

                table.rows.add(newRows).draw();

                table.rows().every(function () {
                    const rowData = this.data();
                    const status = rowData[8];

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

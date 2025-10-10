var selectedLeaveIds = [];

$(document).ready(function () {
    $("#dataLoader").show();
    $("#corporateTable tbody").empty();

    $.ajax({
        url: '/LeaveApproval/LeaveApprovalDetails',
        method: 'GET',
        dataType: 'json',
        success: function (response) {
            console.log(response);
            var table = $('#corporateTable').DataTable();

            table.clear();

            table.on('draw', function () {
                $('#corporateTable tbody input.row-checkbox').each(function () {
                    const leaveId = $(this).data('leave-transaction-id');
                    $(this).prop('checked', selectedLeaveIds.includes(leaveId));
                });

                updateSelectAllCheckboxState();

                const visibleRows = table.rows({ filter: 'applied' }).count();
                $('#totalRemindersCorporate').text(`Total List: ${visibleRows}`);
            });

            if (response.data && response.data.length > 0) {
                $('#userList').show();
                const newRows = [];

                $.each(response.data, function (index, detail) {
                    const employee = response.users.find(user => user.id === detail.employeeId);
                    const leaveMaster = response.masters.find(m => m.id === detail.leaveMasterId);
                    var appliedon = detail.appliedOn || "";
                    var splitAppliedon = appliedon.split('T')[0];
                    const statusMap = {
                        0: "Pending",
                        1: "Approved",
                        2: "Rejected"
                    };

                    const statusText = statusMap[detail.leaveStatus];
                    const fileColumn = detail.addFile && detail.addFile !== ''
                        ? `<a href="/leave_pdf/${detail.addFile}" target="_blank" title="View PDF">
                        <i class="ri-file-pdf-line" style="font-size: 20px; color: red;"></i></a>`
                        : '';
                    let checkbox = "";

                    if (statusText === "Pending") {
                        checkbox = `<div class='form-check' style='margin-left:10px;'>
                            <input class='form-check-input row-checkbox' type='checkbox' data-leave-transaction-id='${detail.leaveTransactionId}' style='background-size: 100% 100%;'>
                        </div>`;
                    } else {
                        checkbox = `<div class='form-check' style='margin-left:10px;'>
                            <input class='form-check-input row-checkbox' type='checkbox' data-leave-transaction-id='${detail.leaveTransactionId}' style='background-size: 100% 100%;' disabled checked>
                        </div>`;
                    }

                    const row = [
                        checkbox,
                        statusText,
                        employee?.name || '',
                        leaveMaster?.name || '',
                        detail.isPaid ? 'Yes' : 'No',
                        detail.ishalfday ? 'Yes' : 'No',
                        splitAppliedon,
                        detail.startDate ? new Date(detail.startDate).toLocaleDateString() : '',
                        detail.endDate ? new Date(detail.endDate).toLocaleDateString() : '',
                        detail.totalDays,
                        detail.reason,
                        fileColumn
                    ];

                    newRows.push(row);
                });
                newRows.sort(function (a, b) {
                    const statusA = a[1];
                    const statusB = b[1];

                    if (statusA === "Pending" && statusB !== "Pending") return -1;
                    if (statusA === "Approved" && statusB === "Rejected") return -1;
                    if (statusA === "Rejected" && statusB !== "Rejected") return 1;
                    if (statusB === "Pending" && statusA !== "Pending") return 1;

                    return 0;
                });

                table.rows.add(newRows).draw();

                table.rows().every(function () {
                    const rowData = this.data();
                    const status = rowData[1];

                    if (status === 'Approved' || status === 'Rejected') {
                        $(this.node()).addClass('disabled-row');
                        $(this.node()).find('a, input, button, select, textarea')
                            .addClass('disabled-link')
                            .attr('tabindex', -1);
                    }
                });

                $('#totalRemindersCorporate').text(`Total List: ${newRows.length}`);
            } else {
                var noDataRow = `<tr><td colspan="12" class="text-center">No Data Found</td></tr>`;
                $('#corporateTable tbody').html(noDataRow);

                $('#totalRemindersCorporate').text(`Total List: 0`);
            }

            $("#dataLoader").hide();
        },
        error: function (err) {
            console.error(err);
            $("#dataLoader").hide();
        }
    });

    // Bulk Approve
    $('#approveSelectedBtn').on('click', function () {
        if (selectedLeaveIds.length > 0) {
            updateLeaveStatus(selectedLeaveIds, 'Approved');
        } else {
            alert('Please select at least one leave transaction to approve.');
        }
    });

    // Bulk Reject
    $('#rejectSelectedBtn').on('click', function () {
        if (selectedLeaveIds.length > 0) {
            updateLeaveStatus(selectedLeaveIds, 'Rejected');
        } else {
            alert('Please select at least one leave transaction to reject.');
        }
    });

    // Individual checkbox change
    $('#corporateTable tbody').on('change', '.row-checkbox', function () {
        const leaveId = $(this).data('leave-transaction-id');
        const isChecked = $(this).is(':checked');

        if (isChecked) {
            if (!selectedLeaveIds.includes(leaveId)) {
                selectedLeaveIds.push(leaveId);
            }
        } else {
            selectedLeaveIds = selectedLeaveIds.filter(id => id !== leaveId);
        }

        updateSelectAllCheckboxState();
    });

    $('#checkAll').on('click', function () {
        selectcheckAllboxs(this);
    });
});


function selectcheckAllboxs(checkbox) {
    const isChecked = checkbox.checked;
    const table = $('#corporateTable').DataTable();
    selectedLeaveIds = [];

    table.rows().every(function () {
        const rowNode = $(this.node());
        const rowCheckbox = rowNode.find("input.row-checkbox:not(:disabled)");
        const leaveId = rowCheckbox.data('leave-transaction-id');
        const status = this.data()[1];

        if (leaveId !== undefined && status === "Pending") {
            rowCheckbox.prop('checked', isChecked);
            if (isChecked) {
                selectedLeaveIds.push(leaveId);
            }
        } else {
            rowCheckbox.prop('checked', false);
        }
    });

    $('#checkAll').prop('checked', isChecked);
}

function updateSelectAllCheckboxState() {
    const all = $('#corporateTable tbody input.row-checkbox:not(:disabled)');
    const checked = all.filter(':checked');
    $('#checkAll').prop('checked', all.length > 0 && checked.length === all.length);
}

function updateLeaveStatus(leaveIds, status) {
    $.ajax({
        url: '/LeaveTransaction/UpdateLeaveTransactionStatus',
        method: 'POST',
        data: {
            leaveTransactionIds: leaveIds,
            status: status
        },
        success: function (response) {
            if (response.success) {
                alert(response.message);
                selectedLeaveIds = [];
                location.reload();
            } else {
                alert(response.message);
            }
        },
        error: function (err) {
            console.error(err);
            alert('An error occurred while updating the leave status.');
        }
    });
}

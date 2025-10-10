$(document).ready(function () {
    var table = $('#corporateTable').DataTable();

    $('#customcorporateSearch').on('keyup', function () {
        table.search(this.value).draw();
    });

    $('#pageLength').on('change', function () {
        var length = parseInt($(this).val());
        table.page.len(length).draw();
    });

    function loadLeaveTransactions() {
        $("#dataLoader").show();

        $.ajax({
            url: '/LeaveTransaction/LeaveTransactionViewDetails',
            method: 'GET',
            dataType: 'json',
            success: function (response) {
                console.log(response)
                $("#dataLoader").hide();

                if (response.data && response.data.length > 0) {
                    $('#userList').show();

                    const statusMap = {
                        0: "Pending",
                        1: "Approved",
                        2: "Rejected"
                    };

                    //const badgeMap = {
                    //    "Pending": '<span class="badge bg-warning text-dark">Pending</span>',
                    //    "Approved": '<span class="badge bg-success">Approved</span>',
                    //    "Rejected": '<span class="badge bg-danger">Rejected</span>'
                    //};
                    const badgeMap = {
                        "Pending": 'Pending',
                        "Approved": 'Approved',
                        "Rejected": 'Rejected'
                    };

                    const rows = response.data.map(function (detail) {
                        const leaveMaster = response.masters.find(m => m.id === detail.leaveMasterId);
                        const updatedByUser = (response.users.find(x => x.id === detail.updatedby) || {}).name || '';
                        const approvedByUser = (response.users.find(x => x.id === detail.approvedBy) || {}).name || '';

                        const appliedOn = detail.appliedOn ? detail.appliedOn.split('T')[0] : '';
                        const startDate = detail.startDate ? new Date(detail.startDate).toLocaleDateString() : '';
                        const endDate = detail.endDate ? new Date(detail.endDate).toLocaleDateString() : '';
                        const updatedAt = detail.updatedat ? detail.updatedat.split('T')[0] : '';
                        const approvedAt = detail.approvedAt ? detail.approvedAt.split('T')[0] : '';

                        const statusText = statusMap[detail.leaveStatus];
                        const statusBadge = badgeMap[statusText];

                        const fileCell = detail.addFile
                            ? `<a href="/leave_pdf/${detail.addFile}" target="_blank" title="View PDF">
                                <i class="ri-file-pdf-2-line" style="font-size: 24px; color: red;"></i>
                           </a>`
                            : '';

                        let actionButtons = '';
                        if (detail.leaveStatus === 0 || detail.leaveStatus === 2) {
                            actionButtons += `<a href="${urlUpdate + detail.leaveTransactionId}" style="margin-right: 10px;" title="Edit">
                                            <i class="ri-edit-2-line" style="font-size:x-large;"></i>
                                          </a>`;
                            actionButtons += `<a href="#" onclick="deleteLeaveTransaction(${detail.leaveTransactionId}); return false;" style="color:red;" title="Delete">
                                            <i class="ri-delete-bin-3-line" style="font-size:x-large;"></i>
                                          </a>`;
                        }

                        return {
                            sortStatus: statusText,
                            row: [
                                leaveMaster ? leaveMaster.name : '',
                                detail.isPaid ? 'Yes' : 'No',
                                detail.ishalfday ? 'Yes' : 'No',
                                appliedOn,
                                startDate,
                                endDate,
                                detail.ishalfday ? 0.5 : detail.totalDays,
                                detail.reason,
                                updatedAt,
                                updatedByUser,
                                approvedAt,
                                approvedByUser,
                                statusBadge, // HTML shown in table
                                fileCell,
                                actionButtons
                            ]
                        };
                    });

                    // Sort using raw status text (not the HTML badge)
                    rows.sort(function (a, b) {
                        const statusA = a.sortStatus;
                        const statusB = b.sortStatus;

                        if (statusA === "Pending" && statusB !== "Pending") return -1;
                        if (statusA === "Approved" && statusB === "Rejected") return -1;
                        if (statusA === "Rejected" && statusB !== "Rejected") return 1;
                        if (statusB === "Pending" && statusA !== "Pending") return 1;

                        return 0;
                    });

                    const sortedRowData = rows.map(item => item.row);

                    table.clear();
                    table.rows.add(sortedRowData);
                    table.draw();

                    // Disable Approved rows
                    table.rows().every(function () {
                        const rowData = this.data();
                        const statusText = $('<div>').html(rowData[12]).text(); // extract text from badge
                        if (statusText === 'Approved') {
                            $(this.node()).addClass('disabled-row');
                            $(this.node()).find('a, input, button, select, textarea')
                                .addClass('disabled-link')
                                .attr('tabindex', -1);
                        }
                    });

                    $('#totalRemindersCorporate').text(`Total List: ${rows.length}`);
                } else {
                    table.clear().draw();
                    $('#totalRemindersCorporate').text(`Total List: 0`);
                }
            },
            error: function (err) {
                $("#dataLoader").hide();
                console.error(err);
                alert('Error loading leave transactions. Please try again later.');
            }
        });
    }


    loadLeaveTransactions();

    $('#corporateTable').on('draw.dt', function () {
        var visibleRows = table.rows({ filter: 'applied' }).count();
        $('#totalRemindersCorporate').text(`Total List: ${visibleRows}`);
    });
});

function deleteLeaveTransaction(id) {
    if (!confirm('Are you sure you want to delete this record?')) return;

    $.ajax({
        url: urlDelete + id,
        type: 'POST',
        success: function () {
            alert('Record deleted successfully.');
            location.reload();
        },
        error: function () {
            alert('An error occurred while deleting the leave transaction.');
        }
    });
}
$(document).ready(function () {
    $("#dataLoader").show()
    $("#corporateTable tbody").empty();

    $.ajax({
        url: '/Permission/GetAllPermissions',
        datatype: 'html',
        method: 'GET',
        success: function (result) {
            console.log(result);

            var table = $('#corporateTable').DataTable();
            table.clear();

            if (result.data && result.data.length > 0) {
                $('#employeelist').show();
                const newRows = [];
                $.each(result.data, function (index, detail) {
                    var designationname = detail.designation ? detail.designation.designationName : '';
                    var departmentname = detail.department ? detail.department.name : '';
                    var managerlist = ((result.data).find(x => x.employeeId == detail.managerId));
                    var managername = managerlist?.name || '';

                    newRows.push([

                        detail.designation.designationName,
                        detail.moduleMaster.moduleName,
                        detail.canAccess,
                            `<a href="${url + detail.permissionId}" style="margin-right: 10px;"><i class="ri-edit-2-line" style="font-size:x-large;"></i></a><a href="${deleteurl + detail.permissionId}" onclick = "return confirm('Are you sure you want to delete this record?');" })><i class="ri-delete-bin-3-line" style="font-size:x-large;color:red;"></i></a>`,

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
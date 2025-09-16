$(document).ready(function () {
    $("#dataLoader").show()
    $("#DepartmentTable tbody").empty();

    $.ajax({
        url: '/Master/GetAllDepartment',
        datatype: 'json',
        method: 'GET',
        success: function (result) {

            console.log(result);

            var table = $('#DepartmentTable').DataTable();
            table.clear();

            if (result && result.length > 0) {
                $('#departmentlist').show();
                const newRows = [];
                $.each(result, function (index, detail) {
                    newRows.push([
                        detail.name,
                        `<a href="javascript:void(0)" onclick="editDepartment(${detail.id})" style="margin-right: 10px;">
        <i class="ri-edit-2-line" style="font-size:x-large;"></i>
     </a>
     <a href="${deleteDeptUrl + detail.id}" onclick="return confirm('Are you sure you want to delete this record?');">
        <i class="ri-delete-bin-3-line" style="font-size:x-large;color:red;"></i>
     </a>`
                    ]);

                });
                table.rows.add(newRows).draw();
                $('#totalRemindersDepartment').text(`Total List: ${newRows.length}`);
            }
            else {
                table.row.add([
                    `<span class="text-center" style="display:block;" colspan="9">No Data Found</span>`,
                    '', '', '', '', '', '', '', ''
                ]).draw();
                $('#totalRemindersDepartment').text(`Total List: 0`);
            }
            $("#dataLoader").hide();
        },
        error: function (err) {
            console.log(err);
        }
    });
});


function editDepartment(id) {
    $.ajax({
        url: '/Master/EditDepartment',
        type: 'GET',
        data: { id: id },
        success: function (dept) {
            console.log("Dept JSON:", dept);

            if (dept) {
                // Ensure hidden field is filled
                $("#DepartmentId").val(dept.id || dept.Id);
                $("#Name").val(dept.name || dept.Name);

                // Toggle buttons
                $("#btnAdd-dept").hide();
                $("#btnUpdate-dept").show();
            }
        },
        error: function (xhr) {
            console.log("Error loading department:", xhr);
        }
    });
}


function resetDepartmentForm() {
    $("#DepartmentId").val("");
    $("#Name").val("");

    $("#btnAdd-dept").show();
    $("#btnUpdate-dept").hide();
}


$(document).ready(function () {
    loadDesignations();
});

function loadDesignations() {
    $("#dataLoader").show();
    $("#DesignationTable tbody").empty();

    $.ajax({
        url: '/Master/GetAllDesignation',
        type: 'GET',
        dataType: 'json',
        success: function (result) {
            console.log(result);

            var table = $('#DesignationTable').DataTable();
            table.clear();

            if (result && result.length > 0) {
                $('#designationlist').show();
                const newRows = [];

                $.each(result, function (index, detail) {
                    newRows.push([
                        detail.designationName,
                        `<a href="javascript:void(0)" onclick="editDesignation(${detail.designationId})" style="margin-right: 10px;">
                            <i class="ri-edit-2-line" style="font-size:x-large;"></i>
                         </a>
                         <a href="/Master/DeleteDesig/${detail.designationId}" onclick="return confirm('Are you sure you want to delete this record?');">
                            <i class="ri-delete-bin-3-line" style="font-size:x-large;color:red;"></i>
                         </a>`
                    ]);
                });

                table.rows.add(newRows).draw();
                $('#totalRemindersDesignation').text(`Total List: ${newRows.length}`);
            } else {
                table.row.add([
                    `<span class="text-center" style="display:block;" colspan="2">No Data Found</span>`,
                    ''
                ]).draw();
                $('#totalRemindersDesignation').text(`Total List: 0`);
            }

            $("#dataLoader").hide();
        },
        error: function (err) {
            console.error("Error loading designations:", err);
            $("#dataLoader").hide();
        }
    });
}

// ----------------- EDIT -----------------
function editDesignation(id) {
    $.ajax({
        url: '/Master/EditDesignation',
        type: 'GET',
        data: { id: id },
        success: function (designation) {
            if (designation) {
                $("#DesignationId").val(designation.designationId || designation.Id);
                $("#DesignationName").val(designation.designationName || designation.Name);

                // Toggle buttons
                $("#btnAdd-designation").hide();
                $("#btnUpdate-designation").show();
            }
        },
        error: function (xhr) {
            console.error("Error fetching designation:", xhr);
        }
    });
}

// ----------------- RESET -----------------
function resetDesignationForm() {
    $("#DesignationId").val("");
    $("#DesignationName").val("");

    $("#btnAdd-designation").show();
    $("#btnUpdate-designation").hide();
}

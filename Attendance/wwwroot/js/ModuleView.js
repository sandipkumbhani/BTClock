$(document).ready(function () {
    $("#dataLoader").show()
    $("#corporateTable tbody").empty();

    $.ajax({
        url: '/Master/GetAllModule',
        datatype: 'json',
        method: 'GET',
        success: function (result) {
            var table = $('#corporateTable').DataTable();
            table.clear();
            table.on('draw', function () {
                var visibleRows = table.rows({ filter: 'applied' }).count();
                $('#totalRemindersCorporate').text(`Total List: ${visibleRows}`);
            });

            if (result && result.length > 0) {
                $('#modulelist').show();
                const newRows = [];
                $.each(result, function (index, detail) {

                    newRows.push([

                        detail.moduleName,
                        //`<a href="${editUrl + detail.id}" style="margin-right: 10px;"><i class="ri-edit-2-line" style="font-size:x-large;"></i></a><a href="${deleteurl + detail.id}" onclick = "return confirm('Are you sure you want to delete this record?');" })><i class="ri-delete-bin-3-line" style="font-size:x-large;color:red;"></i></a>`,
                        `<a href="javascript:void(0);" 
    class="edit-module" 
    data-id="${detail.id}" 
    style="margin-right: 10px;">
    <i class="ri-edit-2-line" style="font-size:x-large;"></i>
</a><a href="${deleteurl + detail.id}" onclick = "return confirm('Are you sure you want to delete this record?');"><i class="ri-delete-bin-3-line" style="font-size:x-large;color:red;"></i></a>`,
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
function editModule(id) {
    $.ajax({
        url: '/Master/EditModule',
        type: 'GET',
        data: { id: id },
        success: function (module) {
            if (module) {
                $("#ModuleId").val(module.id || module.Id);
                $("#ModuleName").val(module.moduleName || module.Name);

                $("#btnAdd-module").hide();
                $("#btnUpdate-module").show();
            }
        },
        error: function (xhr) {
            console.error("Error fetching module:", xhr);
        }
    });
}

function resetModuleForm() {
    $("#ModuleId").val("");
    $("#ModuleName").val("");

    $("#btnAdd-module").show();
    $("#btnUpdate-module").hide();
}
// Attach event to edit buttons
$('#corporateTable').on('click', '.edit-module', function () {
    const id = $(this).data('id');
    editModule(id);
});



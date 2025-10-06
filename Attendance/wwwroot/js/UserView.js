$(document).ready(function () {
    $("#dataLoader").show()
    $("#corporateTable tbody").empty();

    $.ajax({
        url: '/User/UserViewDetail',
        datatype: 'json',
        method: 'GET',
        success: function (result) {

            var table = $('#corporateTable').DataTable();
            table.clear();
            table.on('draw', function () {
                var visibleRows = table.rows({ filter: 'applied' }).count();
                $('#totalRemindersCorporate').text(`Total List: ${visibleRows}`);
            });

            if (result.data && result.data.length > 0) {
                $('#employeelist').show();
                const newRows = [];

                $.each(result.data, function (index, detail) {
                    console.log(detail);

                    var roleName = result.roles?.find(r => r.id === detail.roleId)?.name || "";
                    var parentName = detail.parentName || "";

                    newRows.push([
                        detail.name,
                        detail.email,
                        roleName,   
                        parentName,
                        //updatedBy,
                        //updatedAt
                        `
                        
                        <a href="${url + detail.userId}" onclick="return confirm('Are you sure you want to delete this record?');">
                            <i class="ri-delete-bin-3-line" style="font-size:x-large;color:red;"></i>
                        </a>`
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

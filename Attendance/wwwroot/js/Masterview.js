//$(document).ready(function () {
//    $("#dataLoader").show()
//    $("#corporateTable tbody").empty();

//    $.ajax({
//        url: '/MenuMaster/MasterViewDetails',
//        method: 'GET',
//        dataType: 'json',
//        success: function (result) {

//            console.log(result);

//            var table = $('#corporateTable').DataTable();
//            table.clear();

//            if (result.data && result.data.length > 0) {
//                $('#userList').show();
//                const newRows = [];
//                $.each(result.data, function (index, detail) {
//                    var menulist = ((result.data).find(x => x.menuId === detail.parentId));
//                    var ParentName = menulist?.menuName || '';
//                    console.log(ParentName);
//                    //var username = ((result.users).find(x => x.id === detail.updateBy));
//                    //var user = username?.name || '';
//                    var IsActive = detail.isActive ? "Active" : "Deactive";

//                    newRows.push([
//                        detail.menuName,
//                        detail.icon,
//                        detail.moduleMaster.moduleName,
//                        ParentName,
//                        detail.isDefault,
//                        IsActive,
//                        //user,
//                        detail.updateDate,
//                        `<a href="${url + detail.menuId}" style="margin-right: 10px;"><i class="ri-edit-2-line" style="font-size:x-large;"></i></a><a href="${deleteurl}" onclick = "return confirm('Are you sure you want to delete this record?');" })><i class="ri-delete-bin-3-line" style="font-size:x-large;color:red;"></i></a>`,
//                    ]);
//                });
//                table.rows.add(newRows).draw();
//                $('#totalRemindersCorporate').text(`Total List: ${newRows.length}`);
//            }
//            else {
//                table.row.add([
//                    `<span class="text-center" style="display:block;" colspan="9">No Data Found</span>`,
//                    '', '', '', '', '', '', '', ''
//                ]).draw();
//                $('#totalRemindersCorporate').text(`Total List: 0`);
//            }
//            $("#dataLoader").hide();
//        },
//        error: function (err) {
//            console.log(err);
//        }
//    });
//});
$(document).ready(function () {
    $("#dataLoader").show();
    $("#corporateTable tbody").empty();

    $.ajax({
        url: '/MenuMaster/MasterViewDetails',
        method: 'GET',
        dataType: 'json',
        success: function (result) {
            console.log(result);

            var table = $('#corporateTable').DataTable();
            table.clear();
            table.on('draw', function () {
                var visibleRows = table.rows({ filter: 'applied' }).count();
                $('#totalRemindersCorporate').text(`Total List: ${visibleRows}`);
            });

            if (result.data && result.data.length > 0) {
                $('#userList').show();
                const newRows = [];

                $.each(result.data, function (index, detail) {
                    var parent = result.data.find(x => x.menuid === detail.parentId);
                    var parentName = parent ? parent.menuname : '';

                    var username = result.users.find(x => x.id === detail.updatedBy);
                    var user = username?.name || '';
                    var updatedAt = detail.updatedAt
                        ? new Date(detail.updatedAt).toLocaleString()
                        : '—';

                    newRows.push([
                        detail.menuname || '',
                        detail.icon || '',
                        detail.moduleMaster?.moduleName || '',
                        parentName || '',
                        detail.isDefault ? 'Yes' : 'No',
                        detail.isActive ? 'Active' : 'Deactive',
                        user,
                        updatedAt,
                        `<a href="${url + detail.menuid}" style="margin-right: 10px;"><i class="ri-edit-2-line" style="font-size:x-large;"></i></a>
     <a href="${deleteurl + detail.menuid}" onclick="return confirm('Are you sure you want to delete this record?');">
     <i class="ri-delete-bin-3-line" style="font-size:x-large;color:red;"></i></a>`
                    ]);

                });


                table.rows.add(newRows).draw();
                $('#totalRemindersCorporate').text(`Total List: ${newRows.length}`);
            } else {
                table.row.add([
                    `<span class="text-center" style="display:block;" colspan="9">No Data Found</span>`,
                    'No Data Found', '', '', '', '', '', '', ''
                ]).draw();
                $('#totalRemindersCorporate').text(`Total List: 0`);
            }

            $("#dataLoader").hide();
        },
        error: function (err) {
            console.log(err);
            $("#dataLoader").hide();
        }
    });
});


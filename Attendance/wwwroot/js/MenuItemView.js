$(document).ready(function () {
    $("#dataLoader").show();
    $("#corporateTable tbody").empty();

    var table = $('#corporateTable').DataTable();

    $.ajax({
        url: '/MenuItem/MenuItemViewDetails',
        method: 'GET',
        dataType: 'json',
        success: function (result) {
            console.log(result);
            table.clear();

            const newRows = [];

            if (result.data && result.data.length > 0) {
                $('#userList').show();

                $.each(result.data, function (index, item) {
                    var parent = result.parents.find(x => x.menuid === item.parentId);
                    var parentName = parent ? parent.menuname : '-';

                    var user = result.users.find(x => x.id === item.updatedBy);
                    var updatedBy = user ? user.name : '';

                    var updatedAt = item.updatedAt
                        ? new Date(item.updatedAt).toLocaleString()
                        : '';

                    newRows.push([
                        item.menuName || '',
                        parentName || '',
                        item.sortingOrder || '',
                        item.isActive ? 'Active' : 'Deactive',
                        updatedBy,
                        updatedAt,
                        `<a href="${url + item.menuItemId}" style="margin-right: 10px;">
                            <i class="ri-edit-2-line" style="font-size:x-large;"></i></a>
                         <a href="${deleteurl + item.menuItemId}" onclick="return confirm('Are you sure you want to delete this record?');">
                            <i class="ri-delete-bin-3-line" style="font-size:x-large;color:red;"></i></a>`
                    ]);
                });

                table.rows.add(newRows).draw();
                $('#totalRemindersCorporate').text(`Total List: ${newRows.length}`);
            } else {
                table.row.add([
                    `<span class="text-center" style="display:block;" colspan="7">No Data Found</span>`,
                    '', '', '', '', '', ''
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

    $('#customcorporateSearch').on('keyup', function () {
        table.search(this.value).draw();
    });

    $('#pageLength').on('change', function () {
        table.page.len(this.value).draw();
    });
});

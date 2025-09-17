$(document).ready(function () {
    $("#dataLoader").show();
    $("#LeavemasterTable tbody").empty();

    $.ajax({
        url: '/Master/GetAllLeave',
        datatype: 'json',
        method: 'GET',
        success: function (result) {
            console.log(result);

            var table = $('#LeavemasterTable').DataTable();
            table.clear();
            table.on('draw', function () {
                var visibleRows = table.rows({ filter: 'applied' }).count();
                $('#totalRemindersleavemaster').text(`Total List: ${visibleRows}`);
            });
            if (result && result.length > 0) {
                $('#leavemasterlist').show();
                const newRows = [];
                $.each(result, function (index, detail) {
                    newRows.push([
                        detail.leaveType,
                        `<a href="javascript:void(0);" 
                             class="edit-leave" 
                             data-id="${detail.leaveMasterId}" 
                             data-name="${detail.leaveType}" 
                             style="margin-right: 10px;">
                             <i class="ri-edit-2-line" style="font-size:x-large;"></i>
                         </a>
                         <a href="${deleteleavemasterUrl + detail.leaveMasterId}" 
                             onclick="return confirm('Are you sure you want to delete this record?');">
                             <i class="ri-delete-bin-3-line" style="font-size:x-large;color:red;"></i>
                         </a>`
                    ]);
                });
                table.rows.add(newRows).draw();
                $('#totalRemindersleavemaster').text(`Total List: ${newRows.length}`);
            } else {
                table.row.add([
                    `<span class="text-center" style="display:block;" colspan="9">No Data Found</span>`,
                    ''
                ]).draw();
                $('#totalRemindersleavemaster').text(`Total List: 0`);
            }
            $("#dataLoader").hide();
        },
        error: function (err) {
            console.log(err);
        }
    });
});


// Handle Edit Leave button click
$(document).on("click", ".edit-leave", function () {
    var id = $(this).data("id");
    var name = $(this).data("name");

    // Fill input field
    $("#LeaveType").val(name);

    // Hide Add, Show Update
    $("#btnAdd-leave").hide();
    $("#btnUpdate-leave").show();

    // Store the id in hidden field
    if ($("#LeaveMasterId").length === 0) {
        $("<input>").attr({
            type: "hidden",
            id: "LeaveMasterId",
            name: "Id",
            value: id
        }).appendTo("form[action*='Leave']");
    } else {
        $("#LeaveMasterId").val(id);
    }
});


// Cancel button function
function resetLeaveForm() {
    $("#LeaveMasterId").val("");
    $("#LeaveType").val("");

    $("#btnAdd-leave").show();
    $("#btnUpdate-leave").hide();
}

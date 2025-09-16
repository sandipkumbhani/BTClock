$(document).ready(function () {
    $("#dataLoader").show();
    $("#holidaymasterTable tbody").empty();

    $.ajax({
        url: '/Master/GetAllHoliday',
        datatype: 'json',
        method: 'GET',
        success: function (result) {
            console.log(result);

            var table = $('#holidaymasterTable').DataTable();
            table.clear();

            if (result && result.length > 0) {
                $('#holidaymasterlist').show();
                const newRows = [];
                $.each(result, function (index, detail) {
                    newRows.push([
                        detail.holidayDescription,                              // Description
                        detail.holidayDate,                                    // Date
                        detail.year,                                           // Year
                        detail.saveWeekend ? "Yes" : "No",                     // Weekend flag
                        `<a href="javascript:void(0);" 
                             class="edit-holiday" 
                             data-id="${detail.holidayMasterId}" 
                             data-date="${detail.holidayDate}" 
                             data-desc="${detail.holidayDescription}" 
                             data-year="${detail.year}" 
                             data-weekend="${detail.saveWeekend}">
                                <i class="ri-edit-2-line" style="font-size:x-large;"></i>
                         </a>
                         <a href="${deleteholidaymasterUrl + detail.holidayMasterId}" 
                            onclick="return confirm('Are you sure you want to delete this record?');">
                                <i class="ri-delete-bin-3-line" style="font-size:x-large;color:red;"></i>
                         </a>`
                    ]);
                });
                table.rows.add(newRows).draw();
                $('#totalRemindersholidaymaster').text(`Total List: ${newRows.length}`);
            }
            else {
                table.row.add([
                    `<span class="text-center" style="display:block;" colspan="5">No Data Found</span>`,
                    '', '', '', ''
                ]).draw();
                $('#totalRemindersholidaymaster').text(`Total List: 0`);
            }
            $("#dataLoader").hide();
        },
        error: function (err) {
            console.log(err);
        }
    });
});


// Handle Edit button click
$(document).on("click", ".edit-holiday", function () {
    var id = $(this).data("id");
    var date = $(this).data("date");
    var desc = $(this).data("desc");
    var year = $(this).data("year");
    var weekend = $(this).data("weekend");

    // Fill form fields
    $("#HolidayMasterId").val(id);
    $("#datepicker").val(date);
    $("#HolidayDescription").val(desc);
    $("#visitYear").val(year);
    $("#SaveWeekend").prop("checked", weekend);

    // Toggle buttons
    $("#btnAdd-holiday").hide();
    $("#btnUpdate-holiday").show();
});

// Reset / Cancel Form
function resetholidayForm() {
    $("#HolidayMasterId").val("");
    $("#datepicker").val("");
    $("#HolidayDescription").val("");
    $("#visitYear").val("");
    $("#SaveWeekend").prop("checked", false);

    // Reset buttons
    $("#btnAdd-holiday").show();
    $("#btnUpdate-holiday").hide();
}

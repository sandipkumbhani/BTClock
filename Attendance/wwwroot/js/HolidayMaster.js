$(document).ready(function () {
    let allHolidayData = [];
    let table = null;

    function extractYears(data) {
        const years = new Set();
        data.forEach(holiday => {
            const date = new Date(holiday.holidayDate);
            if (!isNaN(date)) {
                years.add(date.getFullYear());
            }
        });
        return Array.from(years).sort((a, b) => b - a);
    }
    function populateYearPickerFromData(data) {
        const years = new Set();
        data.forEach(holiday => {
            const date = new Date(holiday.holidayDate);
            if (!isNaN(date)) {
                years.add(date.getFullYear());
            }
        });

        const yearArray = Array.from(years).sort((a, b) => b - a);
        const yearPicker = $("#yearPicker");
        yearPicker.empty();
        yearPicker.append(`<option value="" disabled selected>--Select Year--</option>`);
        yearArray.forEach(year => {
            yearPicker.append(`<option value="${year}">${year}</option>`);
        });
    }

    function initializeDataTable() {
        table = $('#holidaymasterTable').DataTable();

        $('#holidaymasterSearch').on('keyup', function () {
            table.search(this.value).draw();
            updateTotalCount();
        });

        table.on('draw', function () {
            updateTotalCount();
        });
    }

    function updateTotalCount() {
        if (!table) return;
        const visibleRowsCount = table.rows({ filter: 'applied' }).count();
        $('#totalRemindersholidaymaster').text(`Total List: ${visibleRowsCount}`);
    }

    function fetchAndRenderHolidayData(year, month) {
        if (!table) return;

        table.clear();

        let filtered = allHolidayData;

        if (year) {
            filtered = filtered.filter(item => {
                const d = new Date(item.holidayDate);
                return d.getFullYear() === parseInt(year);
            });
        }

        if (month) {
            filtered = filtered.filter(item => {
                const d = new Date(item.holidayDate);
                return d.getMonth() + 1 === parseInt(month);
            });
        }

        if (filtered.length > 0) {
            const rows = filtered.map(detail => ([
                detail.holidayDescription,
                detail.holidayDate,
                detail.saveWeekend ? "Yes" : "No",
                `<a href="javascript:void(0);"
                 class="edit-holiday"
                 data-id="${detail.holidayMasterId}"
                 data-date="${detail.holidayDate}"
                 data-desc="${detail.holidayDescription}"
                 data-weekend="${detail.saveWeekend}">
                 <i class="ri-edit-2-line" style="font-size:x-large;"></i>
             </a>
             <a href="${deleteholidaymasterUrl + detail.holidayMasterId}"
                onclick="return confirm('Are you sure you want to delete this record?');">
                 <i class="ri-delete-bin-3-line" style="font-size:x-large;color:red;"></i>
             </a>`
            ]));

            table.rows.add(rows).draw();
        } else {
            table.row.add(["No data found", "", "", ""]).draw();
        }

        updateTotalCount();
        $("#dataLoader").hide();
    }

    function loadInitialHolidayData() {
        $("#dataLoader").show();
        $.ajax({
            url: '/Master/GetAllHoliday',
            datatype: 'json',
            method: 'GET',
            success: function (result) {
                allHolidayData = result || [];
                populateYearPickerFromData(allHolidayData);
                fetchAndRenderHolidayData();
            },
            error: function (err) {
                console.error(err);
                $("#dataLoader").hide();
            }
        });
    }

    $("#yearPicker, #monthPicker").change(function () {
        const selectedYear = $("#yearPicker").val();
        const selectedMonth = $("#monthPicker").val();

        $('#holidaymasterSearch').val('');
        if (table) {
            table.search('').draw();
        }

        fetchAndRenderHolidayData(selectedYear, selectedMonth);
    });

    $(document).on("click", "#exportExcel", function () {
        $("#dataLoader").show();

        const selectedYear = $("#yearPicker").val();
        const selectedMonth = $("#monthPicker").val();

        let exportData = allHolidayData;

        if (selectedYear) {
            exportData = exportData.filter(item => {
                const d = new Date(item.holidayDate);
                return d.getFullYear() === parseInt(selectedYear);
            });
        }

        if (selectedMonth) {
            exportData = exportData.filter(item => {
                const d = new Date(item.holidayDate);
                return d.getMonth() + 1 === parseInt(selectedMonth);
            });
        }

        if (exportData.length === 0) {
            alert('No data available for export with the selected filters.');
            $("#dataLoader").hide();
            return;
        }

        let csvData = "Description,Date,Is Weekend\n";
        exportData.forEach(detail => {
            csvData += [
                detail.holidayDescription,
                detail.holidayDate,
                detail.saveWeekend ? "Yes" : "No"
            ].join(",") + "\n";
        });

        const filename = selectedYear ? `${selectedYear}_HolidayList.csv` : 'HolidayList.csv';
        const blob = new Blob([csvData], { type: 'text/csv' });
        const link = document.createElement('a');
        link.href = URL.createObjectURL(blob);
        link.download = filename;
        link.click();

        $("#dataLoader").hide();
    });

    $(document).on("click", ".edit-holiday", function () {
        var id = $(this).data("id");
        var date = $(this).data("date");
        var desc = $(this).data("desc");
        var weekend = $(this).data("weekend");

        $("#HolidayMasterId").val(id);
        $("#datepicker").val(date);
        $("#HolidayDescription").val(desc);
        $("#SaveWeekend").prop("checked", weekend);

        $("#btnAdd-holiday").hide();
        $("#btnUpdate-holiday").show();
    });

    function resetholidayForm() {
        $("#HolidayMasterId").val("");
        $("#datepicker").val("");
        $("#HolidayDescription").val("");
        $("#SaveWeekend").prop("checked", false);

        $("#btnAdd-holiday").show();
        $("#btnUpdate-holiday").hide();
    }

    $(document).on("click", ".btn-primary", function () {
        if ($(this).text() === "Cancel") {
            resetholidayForm();
        }
    });

    initializeDataTable();
    loadInitialHolidayData();
});

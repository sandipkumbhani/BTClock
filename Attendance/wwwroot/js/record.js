let allRecords = [];
let currentPage = 1;
let recordsPerPage = 20;
let filteredRecords = [];
let monthFilteredRecords = [];
let data = [];
let currentSort = {
    column: null,
    dir: 'asc'
};
$(document).ready(async function () {
    await fetchAttendanceData();
    populatePageSizeOptions();
    setupSearch();
    attachHeaderSortHandlers();
    addExportButton();
    setupMonthPicker();

    const now = new Date();
    const year = now.getFullYear();
    const month = String(now.getMonth() + 1).padStart(2, '0');
    const currentMonth = `${year}-${month}`;
    await fetchAndApplyMonth(currentMonth);
    $("#dataLoader").hide();

});


$('#customFranchiseSearch').on('keyup', function () {
    const term = this.value.trim().toLowerCase();

    filteredRecords = term === ""
        ? [...monthFilteredRecords]
        : monthFilteredRecords.filter(r =>
            (r.date ?? "").toLowerCase().includes(term) ||
            (r.clockIn ?? "").toLowerCase().includes(term) ||
            (r.clockOut ?? "").toLowerCase().includes(term) ||
            (r.totalTime ?? "").toLowerCase().includes(term) ||
            (r.overtimeHours ?? "").toLowerCase().includes(term)
        );

    currentPage = 1;
    renderTable();
    renderPagination();
});

async function fetchAttendanceData() {
    try {
        const response = await fetch('/Record/GetAttendanceTableData');
        $("#dataLoader").show()

        if (!response.ok) throw new Error(`HTTP error! status: ${response.status}`);
        data = await response.json();
        allRecords = (data || []).filter(r => r.clockOut !== null && r.clockOut !== "-");
        monthFilteredRecords = [...allRecords];
        filteredRecords = [...allRecords];


    } catch (err) {
        console.error("Error fetching attendance data:", err);
        const tbody = document.querySelector("#franchiseTable tbody");
        if (tbody) {
            tbody.innerHTML = `<tr><td colspan="4" style="text-align:center;">Error loading data</td></tr>`;
        }
    }
}

function timeToSeconds(t) {
    if (!t) return 0;
    const [h = 0, m = 0, s = 0] = t.split(':').map(Number);
    return (h * 3600) + (m * 60) + s;
}

function sortFilteredRecords() {
    if (currentSort.column === null) return;  /* no sort requested */

    const col = currentSort.column;
    const dir = currentSort.dir === 'asc' ? 1 : -1;

    filteredRecords.sort((a, b) => {
        let aVal, bVal;

        switch (col) {
            case 0:
                aVal = new Date(a.date);
                bVal = new Date(b.date);
                break;
            case 1:
                aVal = timeToSeconds(a.clockIn);
                bVal = timeToSeconds(b.clockIn);
                break;
            case 2:
                aVal = timeToSeconds(a.clockOut);
                bVal = timeToSeconds(b.clockOut);
                break;
            case 3:
                aVal = timeToSeconds(a.totalTime);
                bVal = timeToSeconds(b.totalTime);
                break;
            case 4:
                aVal = timeToSeconds(a.overtimeHours);
                bVal = timeToSeconds(b.overtimeHours);
                break;
            default:
                aVal = bVal = 0;
        }
        if (aVal < bVal) return -1 * dir;
        if (aVal > bVal) return 1 * dir;
        return 0;
    });
}

function renderTable() {
    $("#dataLoader").show()
    sortFilteredRecords();
    const tbody = document.querySelector("#franchiseTable tbody");
    if (!tbody) return;
    tbody.innerHTML = "";
    const start = (currentPage - 1) * recordsPerPage;
    const paginatedData = filteredRecords.slice(start, start + recordsPerPage);

    if (paginatedData.length === 0 && data.length > 0) {
        const row = document.createElement("tr");
        row.innerHTML = `<td colspan="5" style="text-align:center;">No records found.</td>`;
        tbody.appendChild(row);
        $("#dataLoader").hide();

    } else {
        paginatedData.forEach(rec => {
            const row = document.createElement("tr");
            row.innerHTML = `
            <td>${rec.date}</td>
            <td>${rec.clockIn}</td>
            <td>${rec.clockOut}</td>
            <td>${rec.totalTime}</td>
            <td>${rec.overtimeHours ?? '-'}</td>
        `;
            tbody.appendChild(row);
        });

        $("#dataLoader").hide();
    }

    const totalListElement = document.getElementById("totalList");
    if (totalListElement) {
        totalListElement.innerHTML = `Total List: <span id="recordCount">${filteredRecords.length}</span>`;
    }
    const headers = document.querySelectorAll('#franchiseTable thead th');
    updateSortIcons(headers);
}

function updateSortIcons(headers) {
    headers.forEach((th, i) => {
        th.innerHTML = th.textContent.trim();
    });
}

function renderPagination() {
    const wrapper = document.getElementById("customFranchisePagination");
    if (!wrapper) return;
    if (recordsPerPage === filteredRecords.length) {
        wrapper.innerHTML = "";
        return;
    }
    wrapper.innerHTML = "";

    const totalPages = Math.ceil(filteredRecords.length / recordsPerPage);
    if (totalPages <= 1) return;

    const ul = document.createElement("ul");
    ul.className = "pagination";

    // Prev button
    ul.appendChild(createPageItem("prev", currentPage - 1, currentPage === 1, true));

    const addPage = (page) => {
        ul.appendChild(createPageItem(page, page));
    };

    if (totalPages <= 7) {
        for (let i = 1; i <= totalPages; i++) {
            addPage(i);
        }
    } else {
        if (currentPage <= 3) {
            // Show first 5 + ... + last
            for (let i = 1; i <= 3; i++) addPage(i);
            ul.appendChild(createEllipsis());
            addPage(totalPages);
        } else if (currentPage >= totalPages - 2) {
            // Show 1 + ... + last 5
            addPage(1);
            ul.appendChild(createEllipsis());
            for (let i = totalPages - 2; i <= totalPages; i++) addPage(i);
        } else {
            // Show 1 + ... + current-1, current, current+1 + ... + last
            addPage(1);
            ul.appendChild(createEllipsis());
            //for (let i = currentPage - 1; i <= currentPage + 1; i++) addPage(i);
            addPage(currentPage);
            ul.appendChild(createEllipsis());
            addPage(totalPages);
        }
    }

    ul.appendChild(createPageItem("next", currentPage + 1, currentPage === totalPages, true));

    wrapper.appendChild(ul);
}

function createPageItem(label, page, disabled = false, isArrow = false) {
    const li = document.createElement("li");
    const active = currentPage === page && !isArrow;
    li.className = `paginate_button page-item ${disabled ? "disabled" : ""} ${active ? "active" : ""}`;
    if (isArrow) li.classList.add("arrow");

    const a = document.createElement("a");
    a.href = "#";
    a.className = isArrow ? "" : "page-link";
    a.innerHTML = isArrow
        ? (label === "prev" ? '<i class="ri-arrow-left-s-line"></i>' : '<i class="ri-arrow-right-s-line"></i>')
        : label;

    a.onclick = (e) => {
        e.preventDefault();
        if (!disabled) {
            currentPage = page;
            renderTable();
            renderPagination();
        }
    };

    li.appendChild(a);
    return li;
}
function createEllipsis() {
    const li = document.createElement("li");
    li.className = "paginate_button page-item disabled";
    li.innerHTML = `<a class="page-link">...</a>`;
    return li;
}

function populatePageSizeOptions() {
    const select = document.getElementById("pageLength");
    if (!select) return;

    const options = [5, 10, 20, 25, 'All'];
    select.innerHTML = "";

    options.forEach(v => {
        const o = document.createElement("option");
        o.value = v;
        o.textContent = v === 'All' ? 'All' : v;
        if (v === recordsPerPage) o.selected = true;
        select.appendChild(o);
    });

    select.addEventListener("change", () => {
        if (select.value === 'All') {
            recordsPerPage = filteredRecords.length;
        } else {
            recordsPerPage = parseInt(select.value, 10);
        }
        currentPage = 1;
        renderTable();
        renderPagination();
    });

}

function addExportButton() {
    const btn = document.getElementById("exportExcel");
    if (!btn) return;

    btn.addEventListener("click", () => {
        const exportData = [...monthFilteredRecords];
        if (!exportData || exportData.length === 0) {
            alert("No records found for the selected month or range.");
            return;
        }

        const header = ["Date", "Clock In", "Clock Out", "Total Time", "Overtime Hours"];
        const rows = exportData.map(r => [
            r.date, r.clockIn, r.clockOut, r.totalTime, r.overtimeHours ?? '-'
        ]);

        const ws = XLSX.utils.aoa_to_sheet([header, ...rows]);
        const wb = XLSX.utils.book_new();
        XLSX.utils.book_append_sheet(wb, ws, "Attendance");

        let fileLabel = "All_Months";

        const rangeSpan = document.querySelector("#reportrange span");
        const monthLabel = document.getElementById("selectedMonthLabel");

        if (rangeSpan && rangeSpan.textContent.trim()) {
            fileLabel = rangeSpan.textContent.trim();
        } else if (monthLabel && monthLabel.textContent.trim()) {
            fileLabel = monthLabel.textContent.trim();
        }


        fileLabel = fileLabel
            .replace(/[,]/g, '')
            .replace(/\s*-\s*/g, '_to_')
            .replace(/\s+/g, '_')
            .replace(/[^a-zA-Z0-9_-]/g, '');

        const fileName = `Attendance_${fileLabel}.xlsx`;

        const binary = XLSX.write(wb, { bookType: "xlsx", type: "array" });
        const blob = new Blob([binary], {
            type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        });

        const link = document.createElement("a");
        link.href = URL.createObjectURL(blob);
        link.download = fileName;
        document.body.appendChild(link);
        link.click();
        URL.revokeObjectURL(link.href);
        document.body.removeChild(link);
    });
}

function setupSearch() {
    const searchInput = document.getElementById("customFranchiseSearch");
    if (!searchInput) return;

    searchInput.addEventListener("keyup", function () {
        const term = this.value.trim().toLowerCase();

        filteredRecords = term === ""
            ? [...monthFilteredRecords]
            : monthFilteredRecords.filter(r =>
                (r.date ?? "").toLowerCase().includes(term) ||
                (r.clockIn ?? "").toLowerCase().includes(term) ||
                (r.clockOut ?? "").toLowerCase().includes(term) ||
                (r.totalTime ?? "").toLowerCase().includes(term) ||
                (r.overtimeHours ?? "").toLowerCase().includes(term)
            );

        currentPage = 1;
        renderTable();
        renderPagination();
    });
}
function attachHeaderSortHandlers() {
    const headers = document.querySelectorAll('#franchiseTable thead th');
    headers.forEach((th, idx) => {
        if (idx > 4) return;
        th.style.cursor = 'pointer';
        th.addEventListener('click', () => {
            if (currentSort.column !== idx) {

                currentSort.column = idx;
                currentSort.dir = 'asc';
            } else {
                currentSort.dir = currentSort.dir === 'asc' ? 'desc' : 'asc';
            }
            currentPage = 1;
            renderTable();
            renderPagination();
            updateSortIcons(headers);
        });
    });
}
function setupMonthPicker() {
    const monthInput = document.getElementById("filterDatePicker");
    if (!monthInput) return;

    const now = new Date();
    const year = now.getFullYear();
    const month = String(now.getMonth() + 1).padStart(2, '0');
    const currentMonth = `${year}-${month}`;

    monthInput.value = currentMonth;
    monthInput.addEventListener("change", function () {
        const selectedMonth = this.value;
        if (selectedMonth) {
            fetchAndApplyMonth(selectedMonth);
        }
    });
}


async function fetchAndApplyMonth(monthStr) {
    const userId = UserID;
    try {
        const response = await fetch(`/Record/GetAttendanceByMonth?userId=${userId}&month=${monthStr}`);
        if (!response.ok) throw new Error(`Fetch failed: ${response.statusText}`);
        const data = await response.json();
        monthFilteredRecords = [...data];
        filteredRecords = [...data];
        currentPage = 1;
        renderTable();
        renderPagination();

        updateMonthLabel(monthStr);
    } catch (err) {
        console.error("Error fetching month data:", err);
        const tbody = document.querySelector("#franchiseTable tbody");
        if (tbody) {
            tbody.innerHTML = `<tr><td colspan="4" style="text-align:center;">Failed to load data.</td></tr>`;
        }
    }
}
function updateMonthLabel(monthStr) {
    const labelElement = document.getElementById("selectedMonthLabel");
    if (!labelElement) return;
    const [year, month] = monthStr.split("-");
    const date = new Date(`${year}-${month}-01`);
    const monthName = date.toLocaleString('default', { month: 'long' });

    labelElement.textContent = `${monthName} ${year}`;
}

function filterByDateRange(start, end) {
    const startDate = start.startOf('day');
    const endDate = end.endOf('day');

    monthFilteredRecords = allRecords.filter(rec => {
        const recDate = moment(rec.date, 'DD-MMM-YYYY');
        return recDate.isBetween(startDate, endDate, null, '[]');
    });

    filteredRecords = [...monthFilteredRecords];
    currentPage = 1;
    renderTable();
    renderPagination();

    const labelElement = document.getElementById("selectedMonthLabel");
    if (labelElement) {
        labelElement.textContent = `${start.format('MMMM D, YYYY')} - ${end.format('MMMM D, YYYY')}`;
    }
}

$(function () {
    const defaultStart = moment().startOf('month');
    const defaultEnd = moment().endOf('month');

    function cb(start, end) {
        $('#reportrange span').html(start.format('MMMM D, YYYY') + ' - ' + end.format('MMMM D, YYYY'));
        filterByDateRange(start, end);
    }

    $('#reportrange').daterangepicker({
        startDate: defaultStart,
        endDate: defaultEnd,
        ranges: {
            'This Month': [moment().startOf('month'), moment().endOf('month')],
            'Last Month': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')],
            //'Custom Range': [moment().subtract(7, 'days'), moment()]
        },
        locale: {
            format: 'MMMM D, YYYY'
        }
    }, cb);
    cb(defaultStart, defaultEnd);
});
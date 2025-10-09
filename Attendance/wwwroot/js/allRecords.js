let allRecords = [];
let currentPage = 1;
let recordsPerPage = 20;
let filteredRecords = [];
let monthFilteredRecords = [];
let data = [];
let currentSort = { column: 1, dir: 'desc' };

$(document).ready(async function () {
    $("#dataLoader").show();

    await fetchAttendanceData();

    populatePageSizeOptions();
    setupSearch();
    attachHeaderSortHandlers();
    addExportButton();
    setupMonthPicker();
    syncUserDropdownWidth();

    const datepickerElement = document.getElementById('reportrange');
    if (datepickerElement) {
        const resizeObserver = new ResizeObserver(() => {
            syncUserDropdownWidth();
        });

        resizeObserver.observe(datepickerElement);
    };

    $("#UserId").on("change", function () {
        applyUserAndDateFilter();
    });
    // Set default to current month
    const now = new Date();
    const year = now.getFullYear();
    const month = String(now.getMonth() + 1).padStart(2, '0');
    const currentMonth = `${year}-${month}`;
    await fetchAndApplyMonth(currentMonth);
    $("#dataLoader").hide();

    // Initialize daterangepicker with current month
    initializeDateRangePicker();
});

async function fetchAttendanceData() {
    try {
        const response = await fetch('/AllRecords/GetAllAttendanceTableData');
        if (!response.ok) throw new Error(`HTTP error! status: ${response.status}`);
        data = await response.json();
        allRecords = (data || []).filter(r => r.clockOut !== null && r.clockOut !== "-").map(r => ({
            ...r,
            overtimeHours: r.overtimeHours || "00:00:00"
        }));
        console.log(allRecords)
        monthFilteredRecords = [...allRecords];
        filteredRecords = [...allRecords];
    } catch (err) {
        console.error("Error fetching attendance data:", err);
        const tbody = document.querySelector("#franchiseTable tbody");
        if (tbody) tbody.innerHTML = `<tr><td colspan="5" style="text-align:center;">Error loading data</td></tr>`;
    }
}

function timeToSeconds(t) {
    if (!t || t === '-' || typeof t !== 'string') return 0;
    const parts = t.split(':').map(Number);
    if (parts.length !== 3 || parts.some(isNaN)) return 0;
    const [h, m, s] = parts;
    return (h * 3600) + (m * 60) + s;
}


function sortFilteredRecords() {
    if (currentSort.column === null) return;

    const col = currentSort.column;
    const dir = currentSort.dir === 'asc' ? 1 : -1;

    filteredRecords.sort((a, b) => {
        let aVal, bVal;
        switch (col) {
            case 0: aVal = a.userName?.toLowerCase(); bVal = b.userName?.toLowerCase(); break;
            case 1: aVal = new Date(a.date); bVal = new Date(b.date); break;
            case 2: aVal = timeToSeconds(a.clockIn); bVal = timeToSeconds(b.clockIn); break;
            case 3: aVal = timeToSeconds(a.clockOut); bVal = timeToSeconds(b.clockOut); break;
            case 4: aVal = timeToSeconds(a.totalTime); bVal = timeToSeconds(b.totalTime); break;
            case 5: aVal = timeToSeconds(a.overtimeHours); bVal = timeToSeconds(b.overtimeHours); break;
            default: aVal = bVal = 0;
        }
        return (aVal < bVal ? -1 : aVal > bVal ? 1 : 0) * dir;
    });
}


function renderTable() {
    $("#dataLoader").show();
    sortFilteredRecords();
    const tbody = document.querySelector("#franchiseTable tbody");
    if (!tbody) return;
    tbody.innerHTML = "";

    const start = (currentPage - 1) * recordsPerPage;
    const paginatedData = filteredRecords.slice(start, start + recordsPerPage);

    if (paginatedData.length === 0) {
        tbody.innerHTML = `<tr><td colspan="7" style="text-align:center;">No records found.</td></tr>`;
    } else {

        paginatedData.forEach(rec => {
            const row = document.createElement("tr");
            row.innerHTML = `
                <td>${rec.userName}</td>
                <td>${rec.date}</td>
                <td>${rec.clockIn}</td>
                <td>${rec.clockOut}</td>
                <td>${rec.totalTime}</td>
                <td>${rec.overtimeHours ?? '-'}</td>
                <td>
        <a href="/AllRecords/UpdateAttendanceRecord/${rec.id}" style="margin-right: 10px;">
            <i class="ri-edit-2-line" style="font-size:x-large; cursor:pointer;"></i>
        </a>
    </td>
                `;
            tbody.appendChild(row);
        });
    }

    const totalListElement = document.getElementById("totalList");
    if (totalListElement) totalListElement.innerHTML = `Total List: <span id="recordCount">${filteredRecords.length}</span>`;

    const headers = document.querySelectorAll('#franchiseTable thead th');
    updateSortIcons(headers);

    $("#dataLoader").hide();
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

    const totalPages = Math.ceil(filteredRecords.length / recordsPerPage);
    if (totalPages <= 1) {
        wrapper.innerHTML = "";
        return;
    }

    const ul = document.createElement("ul");
    ul.className = "pagination";

    ul.appendChild(createPageItem("prev", currentPage - 1, currentPage === 1, true));

    const addPage = (page) => ul.appendChild(createPageItem(page, page));

    if (totalPages <= 7) {
        for (let i = 1; i <= totalPages; i++) addPage(i);
    } else {
        if (currentPage <= 3) {
            for (let i = 1; i <= 3; i++) addPage(i);
            ul.appendChild(createEllipsis());
            addPage(totalPages);
        } else if (currentPage >= totalPages - 2) {
            addPage(1);
            ul.appendChild(createEllipsis());
            for (let i = totalPages - 2; i <= totalPages; i++) addPage(i);
        } else {
            addPage(1);
            ul.appendChild(createEllipsis());
            //for (let i = currentPage - 1; i <= currentPage + 1; i++) addPage(i);
            addPage(currentPage);
            ul.appendChild(createEllipsis());
            addPage(totalPages);
        }
    }

    ul.appendChild(createPageItem("next", currentPage + 1, currentPage === totalPages, true));

    wrapper.innerHTML = "";
    wrapper.appendChild(ul);
}

function createPageItem(label, page, disabled = false, isArrow = false) {
    const li = document.createElement("li");
    li.className = `paginate_button page-item ${disabled ? "disabled" : ""} ${currentPage === page && !isArrow ? "active" : ""}`;
    if (isArrow) li.classList.add("arrow");

    const a = document.createElement("a");
    a.href = "#";
    a.className = isArrow ? "" : "page-link";
    a.innerHTML = isArrow ? (label === "prev" ? '<i class="ri-arrow-left-s-line"></i>' : '<i class="ri-arrow-right-s-line"></i>') : label;

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
        recordsPerPage = select.value === 'All' ? filteredRecords.length : parseInt(select.value, 10);
        currentPage = 1;
        renderTable();
        renderPagination();
    });
}

function addExportButton() {
    const btn = document.getElementById("exportExcel");
    if (!btn) return;

    btn.addEventListener("click", () => {
        const exportData = [...filteredRecords];
        if (!exportData || exportData.length === 0) {
            alert("No records to export.");
            return;
        }

        // Generate CSV content
        const header = ["User Name", "Date", "Clock In", "Clock Out", "Total Time", "Overtime Hours"];
        const rows = exportData.map(r => [
            r.userName,
            r.date,
            r.clockIn,
            r.clockOut,
            r.totalTime,
            r.overtimeHours ?? "-"
        ]);

        let csvContent = header.join(",") + "\n";
        csvContent += rows.map(r => r.join(",")).join("\n");

        const userSelect = document.getElementById("UserId");
        const userText = userSelect?.options[userSelect.selectedIndex]?.text || "All_Users";

        // Get date range
        const rangeSpan = document.querySelector("#reportrange span");
        let dateRangeText = rangeSpan?.textContent.trim() || "All_Dates";

        // Clean filename parts
        const cleanText = (text) => text.replace(/[,]/g, '').replace(/\s*-\s*/g, '_to_').replace(/\s+/g, '_').replace(/[^a-zA-Z0-9_-]/g, '');
        const filename = `Attendance_${cleanText(userText)}_${cleanText(dateRangeText)}.csv`;

        // Create and download CSV file
        const blob = new Blob([csvContent], { type: "text/csv;charset=utf-8;" });
        const link = document.createElement("a");
        const url = URL.createObjectURL(blob);
        link.href = url;
        link.setAttribute("download", filename);
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
        URL.revokeObjectURL(url);
    });
}


function setupSearch() {
    const searchInput = document.getElementById("customFranchiseSearch");
    if (!searchInput) return;

    searchInput.addEventListener("keyup", function () {
        const term = this.value.trim().toLowerCase();
        filteredRecords = term === "" ? [...monthFilteredRecords] : monthFilteredRecords.filter(r =>
            (r.userName ?? "").toLowerCase().includes(term) ||
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
        if (idx > 6) return; // Skip non-sortable columns
        th.style.cursor = 'pointer';
        th.addEventListener('click', () => {
            if (currentSort.column === idx) {
                // Toggle direction
                currentSort.dir = currentSort.dir === 'asc' ? 'desc' : 'asc';
            } else {
                // New column, start with ASC
                currentSort.column = idx;
                currentSort.dir = 'asc';
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
    monthInput.value = `${year}-${month}`;

    monthInput.addEventListener("change", function () {
        const selectedMonth = this.value;
        if (selectedMonth) fetchAndApplyMonth(selectedMonth);
    });
}

async function fetchAndApplyMonth(monthStr) {
    try {
        const response = await fetch(`/AllRecords/GetAttendanceByMonth=${monthStr}`);
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
        if (tbody) tbody.innerHTML = `<tr><td colspan="5" style="text-align:center;">Failed to load data.</td></tr>`;
    }
}

function updateMonthLabel(monthStr) {
    const labelElement = document.getElementById("selectedMonthLabel");
    if (!labelElement) return;
    const [year, month] = monthStr.split("-");
    const date = new Date(`${year}-${month}-01`);
    labelElement.textContent = `${date.toLocaleString('default', { month: 'long' })} ${year}`;
}

function filterByDateRange(start, end) {
    const startDate = start.startOf('day');
    const endDate = end.endOf('day');

    monthFilteredRecords = allRecords.filter(rec => {
        const recDate = moment(rec.date, 'DD-MMM-YYYY');
        return recDate.isBetween(startDate, endDate, null, '[]');
    });

    applyUserAndDateFilter(); // ✅ Call combined filter after date change
}
function applyUserAndDateFilter() {
    const selectedUserId = $("#UserId").val();

    filteredRecords = selectedUserId
        ? monthFilteredRecords.filter(r => r.userId == selectedUserId)
        : [...monthFilteredRecords];

    currentPage = 1;
    renderTable();
    renderPagination();
}
function syncUserDropdownWidth() {
    const datepicker = document.getElementById('reportrange');
    const userDropdown = document.getElementById('UserId');

    if (!datepicker || !userDropdown) return;

    const width = datepicker.offsetWidth;
    userDropdown.style.width = width + 'px';
}


function initializeDateRangePicker() {
    const defaultStart = moment().startOf('month');
    const defaultEnd = moment().endOf('month');

    function cb(start, end, picker) {
        $('#reportrange span').html(start.format('MMMM D, YYYY') + ' - ' + end.format('MMMM D, YYYY'));
        filterByDateRange(start, end);
    }

    $('#reportrange').daterangepicker({
        startDate: defaultStart,
        endDate: defaultEnd,
        ranges: {
            'This Month': [moment().startOf('month'), moment().endOf('month')],
            'Last Month': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')],
        },
        locale: { format: 'MMMM D, YYYY' },
    }, cb);

    $('#reportrange').on('apply.daterangepicker', function (ev, picker) {
        picker.hide();
    });

    $('#reportrange').data('daterangepicker').callback = cb;
    cb(defaultStart, defaultEnd, $('#reportrange').data('daterangepicker'));
}

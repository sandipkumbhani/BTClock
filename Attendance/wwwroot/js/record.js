let allRecords = [];
let currentPage = 1;
let recordsPerPage = 5;
let filteredRecords = [];
let monthFilteredRecords = [];
let selectedMonthNum = null;  // 1‑12, null = all
let selectedMonthName = null;  // "January" … "December", null = all

let currentSort = {
    column: null,
    dir: 'asc'
};

$('#customFranchiseSearch').on('keyup', function () {
    const term = this.value.trim().toLowerCase();

    filteredRecords = term === ""
        ? [...monthFilteredRecords]
        : monthFilteredRecords.filter(r =>
            (r.date ?? "").toLowerCase().includes(term) ||
            (r.clockIn ?? "").toLowerCase().includes(term) ||
            (r.clockOut ?? "").toLowerCase().includes(term) ||
            (r.totalTime ?? "").toLowerCase().includes(term)
        );

    currentPage = 1;
    renderTable();
    renderPagination();
});

async function fetchAttendanceData() {
    try {
        const response = await fetch('/Attendance/GetAttendanceTableData');
        if (!response.ok) throw new Error(`HTTP error! status: ${response.status}`);

        const data = await response.json();
        allRecords = data || [];
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
            case 0: /* Date */
                aVal = new Date(a.date);
                bVal = new Date(b.date);
                break;
            case 1: /* Clock In */
                aVal = timeToSeconds(a.clockIn);
                bVal = timeToSeconds(b.clockIn);
                break;
            case 2: /* Clock Out */
                aVal = timeToSeconds(a.clockOut);
                bVal = timeToSeconds(b.clockOut);
                break;
            case 3: /* Total Time */
                aVal = timeToSeconds(a.totalTime);
                bVal = timeToSeconds(b.totalTime);
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
    sortFilteredRecords();

    const tbody = document.querySelector("#franchiseTable tbody");
    if (!tbody) return;

    tbody.innerHTML = "";

    const start = (currentPage - 1) * recordsPerPage;
    const paginatedData = filteredRecords.slice(start, start + recordsPerPage);

    if (paginatedData.length === 0) {
        const row = document.createElement("tr");
        row.innerHTML = `<td colspan="4" style="text-align:center;">No records found.</td>`;
        tbody.appendChild(row);
    } else {
        paginatedData.forEach(rec => {
            const row = document.createElement("tr");
            row.innerHTML = `
                <td>${rec.date}</td>
                <td>${rec.clockIn}</td>
                <td>${rec.clockOut}</td>
                <td>${rec.totalTime}</td>
            `;
            tbody.appendChild(row);
        });
    }


    const totalListElement = document.getElementById("totalList");
    if (totalListElement) {
        totalListElement.innerHTML = `Total List: <span id="recordCount">${filteredRecords.length}</span>`;
    }
}

function renderPagination() {
    const wrapper = document.getElementById("customFranchisePagination");
    if (!wrapper) return;

    wrapper.innerHTML = "";
    const totalPages = Math.ceil(filteredRecords.length / recordsPerPage);
    if (totalPages <= 1) return;

    const ul = document.createElement("ul");
    ul.className = "pagination";

    ul.appendChild(createPageItem("prev", currentPage - 1, currentPage === 1, true));

    const windowSize = recordsPerPage; /* pages to show before ellipsis */

    if (totalPages <= windowSize + 1) {
        for (let i = 1; i <= totalPages; i++) ul.appendChild(createPageItem(i, i));
    } else {
        if (currentPage <= windowSize) {
            for (let i = 1; i <= windowSize; i++) ul.appendChild(createPageItem(i, i));
            ul.appendChild(createEllipsis());
            ul.appendChild(createPageItem(totalPages, totalPages));
        } else if (currentPage >= totalPages - 4) {
            ul.appendChild(createPageItem(1, 1));
            ul.appendChild(createEllipsis());
            for (let i = totalPages - 4; i <= totalPages; i++) ul.appendChild(createPageItem(i, i));
        } else {
            ul.appendChild(createPageItem(1, 1));
            ul.appendChild(createEllipsis());
            for (let i = currentPage - 1; i <= currentPage + 1; i++) ul.appendChild(createPageItem(i, i));
            ul.appendChild(createEllipsis());
            ul.appendChild(createPageItem(totalPages, totalPages));
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

    const options = [5, 10, 20, 25];
    select.innerHTML = "";

    options.forEach(v => {
        const o = document.createElement("option");
        o.value = v;
        o.textContent = v;
        if (v === recordsPerPage) o.selected = true;
        select.appendChild(o);
    });

    select.addEventListener("change", () => {
        recordsPerPage = parseInt(select.value, 10);
        currentPage = 1;
        renderTable();
        renderPagination();
    });
}
async function loadMonthFilterOptions() {
    const dropdownMenu = document.getElementById("monthDropdownMenu");
    const dropdownBtn = document.getElementById("filterDropdown");

    if (!dropdownMenu || !dropdownBtn) return;

    dropdownMenu.innerHTML = "";

    const resp = await fetch('/Attendance/GetAvailableMonths');
    if (!resp.ok) {
        console.error("Failed to load available months");
        return;
    }

    const availableMonths = await resp.json(); // ["January 2025", "March 2025", ...]

    const now = new Date();
    const currentMonth = now.getMonth();  // 0-based
    const currentYear = now.getFullYear();

    const monthNames = [
        "January", "February", "March", "April", "May", "June",
        "July", "August", "September", "October", "November", "December"
    ];

    // Set initial label
    dropdownBtn.innerHTML = `${monthNames[currentMonth]} ${currentYear} <i class="ri-arrow-down-s-fill" style="font-size: smaller;"></i>`;

    // -- All Months --
    const allItem = document.createElement("li");
    allItem.innerHTML = `<a class="dropdown-item" href="#">-- All Months --</a>`;
    allItem.onclick = () => {
        dropdownBtn.innerHTML = `-- All Months -- <i class="ri-arrow-down-s-fill" style="font-size: smaller;"></i>`;
        applyMonthFilter(null);
    };
    dropdownMenu.appendChild(allItem);

    for (let i = 0; i < 12; i++) {
        const monthName = monthNames[i];
        const fullLabel = `${monthName} ${currentYear}`;
        const isAvailable = availableMonths.includes(fullLabel);

        const li = document.createElement("li");
        li.innerHTML = `<a class="dropdown-item${!isAvailable ? ' text-muted' : ''}" href="#">${monthName}</a>`;

        li.onclick = () => {
           
            dropdownBtn.innerHTML = `${monthName} ${currentYear} <i class="ri-arrow-down-s-fill" style="font-size: smaller;"></i>`;
            applyMonthFilter(i + 1);
        };

        dropdownMenu.appendChild(li);
    }
    applyMonthFilter(currentMonth + 1);
}

function applyMonthFilter(monthNum) {
    monthFilteredRecords = (!monthNum)
        ? [...allRecords]
        : allRecords.filter(r => r.date?.slice(5, 7) === String(monthNum).padStart(2, '0'));

    /* Re‑apply search term after month filter */
    const term = $('#customFranchiseSearch').val().trim().toLowerCase();
    filteredRecords = term === ""
        ? [...monthFilteredRecords]
        : monthFilteredRecords.filter(r =>
            (r.date ?? "").toLowerCase().includes(term) ||
            (r.clockIn ?? "").toLowerCase().includes(term) ||
            (r.clockOut ?? "").toLowerCase().includes(term) ||
            (r.totalTime ?? "").toLowerCase().includes(term)
        );

    currentPage = 1;
    renderTable();
    renderPagination();
}


function addExportButton() {
    const btn = document.getElementById("exportExcel");
    if (!btn) return;

    btn.addEventListener("click", () => {
        const exportData = [...monthFilteredRecords];

        if (!exportData || exportData.length === 0) {
            alert("No records found for the selected month.");
            return;
        }

        const header = ["Date", "Clock In", "Clock Out", "Total Time"];
        const rows = exportData.map(r => [
            r.date, r.clockIn, r.clockOut, r.totalTime
        ]);

        const ws = XLSX.utils.aoa_to_sheet([header, ...rows]);
        const wb = XLSX.utils.book_new();
        XLSX.utils.book_append_sheet(wb, ws, "Attendance");

        const dropdownBtn = document.getElementById("filterDropdown");
        let fileLabel = dropdownBtn?.textContent
            .trim()
            .replace(/^--\s*|\s*--$/g, '') 
            .replace(/\s+/g, '_') || 'All_Months';

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
                (r.totalTime ?? "").toLowerCase().includes(term)
            );

        currentPage = 1;
        renderTable();
        renderPagination();
    });
}

//function attachHeaderSortHandlers() {
//    const headers = document.querySelectorAll('#franchiseTable thead th');
//    if (headers.length === 0) return;

//    headers.forEach((th, idx) => {
//        if (idx > 3) return; // Limit sorting to first 4 columns
//        th.style.cursor = 'pointer';

//        th.addEventListener('click', () => {
//            // Cycle: unsorted → asc → desc → unsorted
//            if (currentSort.column !== idx) {
//                currentSort.column = idx;
//                currentSort.dir = 'asc';
//            } else if (currentSort.dir === 'asc') {
//                currentSort.dir = 'desc';
//            } else if (currentSort.dir === 'desc') {
//                currentSort.column = null;
//                currentSort.dir = 'asc';
//            }

//            currentPage = 1;
//            renderTable();
//            renderPagination();
//        });
//    });
//}
function attachHeaderSortHandlers() {
    const headers = document.querySelectorAll('#franchiseTable thead th');
    if (headers.length === 0) return;

    headers.forEach((th, idx) => {
        if (idx > 3) return; // Only allow sorting on the first 4 columns
        th.style.cursor = 'pointer';

        th.addEventListener('click', () => {
            if (currentSort.column !== idx) {
                currentSort.column = idx;
                currentSort.dir = 'desc'; // 👈 first click is descending
            } else {
                currentSort.dir = currentSort.dir === 'desc' ? 'asc' : 'desc';
            }

            currentPage = 1;
            renderTable();
            renderPagination();
        });
    });
}

function updateSortIcons(headers) {
    headers.forEach((th, i) => {
        /* strip previous arrows */
        th.innerHTML = th.textContent.replace(/[\u25B2\u25BC]\s*$/, '').trim();

        if (currentSort.column === i) {
            th.innerHTML += currentSort.dir === 'asc' ? ' ▲' : ' ▼';
        }
    });
}

$(document).ready(async function () {
    await fetchAttendanceData();
    populatePageSizeOptions();
    await loadMonthFilterOptions();
    setupSearch();
    renderTable();
    renderPagination();
    attachHeaderSortHandlers();
    addExportButton();
});


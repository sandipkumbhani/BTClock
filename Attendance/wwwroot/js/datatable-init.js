 $(document).ready(function () {
    const table = $('#remindersTable').DataTable({
        responsive: true,
        dom: 'Bfrtip',
        buttons: [
            {
                text: '<i class="ri-file-excel-line"></i> Export All',
            },
        ],
        paging: true,
        info: true,
        lengthChange: false,
        pageLength: 10,
        columnDefs: [
            // { orderable: false, targets: [] } // all sortable
            { orderable: false, targets: 'no-sort' }
        ],
        language: {
            paginate: {
            previous: '<i class="ri-arrow-left-s-line"></i>',
            next: '<i class="ri-arrow-right-s-line"></i>'
            }
        }
    });

    // Move export buttons
    table.buttons().container().appendTo('#exportButtons');

    // Search
    $('#customSearch').on('keyup', function () {
        table.search(this.value).draw();
    });

    // Move pagination to custom div
    $('#remindersTable_paginate').appendTo('#customPagination');

    // Filter dropdown logic
    $('.filter-option').on('click', function () {
        const value = $(this).data('value');
        const label = $(this).text();

        // Update filter label after selection
        $('#filterDropdown').text(label === 'All Status' ? 'Filter' : `${label}`);

        // Apply DataTables column filter (status is column 2)
        table.column(2).search(value).draw();
    });

    // Page length
    $('#pageLength').on('change', function () {
        table.page.len(this.value).draw();
    });

    // Update total reminders
    $('#totalReminders').text(`Total Reminders: ${table.rows().count()}`);
});


$(document).ready(function () {
    const table = $('#franchiseTable').DataTable({
        responsive: true,
        dom: 'Bfrtip',
        buttons: [
            {
                text: '<i class="ri-file-excel-line"></i> Export All',
            },
        ],
        paging: true,
        info: true,
        lengthChange: false,
        pageLength: 10,
        columnDefs: [
            // { orderable: false, targets: [] } // all sortable
            { orderable: false, targets: 'no-sort' }
        ],
        language: {
            paginate: {
            previous: '<i class="ri-arrow-left-s-line"></i>',
            next: '<i class="ri-arrow-right-s-line"></i>'
            }
        }
    });

    // Move export buttons
    table.buttons().container().appendTo('#exportFranchiseButtons');

    // Search
    $('#customFranchiseSearch').on('keyup', function () {
        table.search(this.value).draw();
    });

    // Move pagination to custom div
    $('#franchiseTable_paginate').appendTo('#customFranchisePagination');

    // Filter dropdown logic
    $('.filter-option').on('click', function () {
        const value = $(this).data('value');
        const label = $(this).text();

        // Update filter label after selection
        $('#filterDropdown').text(label === 'All Status' ? 'Filter' : `${label}`);

        // Apply DataTables column filter (status is column 2)
        table.column(2).search(value).draw();
    });

    // Page length
    $('#pageLength').on('change', function () {
        table.page.len(this.value).draw();
    });

    // Update total reminders
    $('#totalList').text(`Total List: ${table.rows().count()}`);
});


$(document).ready(function () {
    $.fn.DataTable.ext.pager.numbers_length = 3;
    
    const table = $('#vehicleTypesTable').DataTable({
        responsive: true,
        dom: 'Bfrtip',
        buttons: [
            {
                text: '<i class="ri-file-excel-line"></i> Export All',
            },
        ],
        paging: true,
        info: true,
        lengthChange: false,
        pageLength: 3,
        columnDefs: [
            // { orderable: false, targets: [] } // all sortable
            { orderable: false, targets: 'no-sort' }
        ],
        language: {
            paginate: {
            previous: '<i class="ri-arrow-left-s-line"></i>',
            next: '<i class="ri-arrow-right-s-line"></i>'
            }
        }
    });

    // Move export buttons
    table.buttons().container().appendTo('#exportvehicleTypesButtons');

    // Search
    $('#customvehicleTypesSearch').on('keyup', function () {
        table.search(this.value).draw();
    });

    // Move pagination to custom div
    $('#vehicleTypesTable_paginate').appendTo('#customvehicleTypesPagination');

    // Filter dropdown logic
    $('.filter-option-vehicletypes').on('click', function () {
        const value = $(this).data('value');
        const label = $(this).text();

        // Update filter label after selection
        $('#filterVehicleTypesDropdown').text(label === 'All Status' ? 'Filter' : `${label}`);

        // Apply DataTables column filter (status is column 2)
        table.column(2).search(value).draw();
    });

    // Page length
    $('#pageLength').on('change', function () {
        table.page.len(this.value).draw();
    });

    // Update total reminders
    $('#totalVehicleTypesReminders').text(`Total List: ${table.rows().count()}`);
});


$(document).ready(function () {

    $.fn.DataTable.ext.pager.numbers_length = 3;

    const table = $('#applicableRouteTable').DataTable({
        responsive: true,
        dom: 'Bfrtip',
        buttons: [
            {
                text: '<i class="ri-file-excel-line"></i> Export All',
            },
        ],
        paging: true,
        info: true,
        lengthChange: false,
        pageLength: 3,
        columnDefs: [
            // { orderable: false, targets: [] } // all sortable
            { orderable: false, targets: 'no-sort' }
        ],
        language: {
            paginate: {
            previous: '<i class="ri-arrow-left-s-line"></i>',
            next: '<i class="ri-arrow-right-s-line"></i>'
            }
        }
    });

    // Move export buttons
    table.buttons().container().appendTo('#exportapplicableRouteButtons');

    // Search
    $('#customapplicableRouteSearch').on('keyup', function () {
        table.search(this.value).draw();
    });

    // Move pagination to custom div
    $('#applicableRouteTable_paginate').appendTo('#customapplicableRoutePagination');

    // Filter dropdown logic
    $('.filter-option-applicableroute').on('click', function () {
        const value = $(this).data('value');
        const label = $(this).text();

        // Update filter label after selection
        $('#filterApplicableRouteDropdown').text(label === 'All Status' ? 'Filter' : `${label}`);

        // Apply DataTables column filter (status is column 2)
        table.column(2).search(value).draw();
    });

    // Page length
    $('#pageLength').on('change', function () {
        table.page.len(this.value).draw();
    });

    // Update total reminders
    $('#totalapplicableRouteReminders').text(`Total List: ${table.rows().count()}`);
});


// $(document).ready(function () {
//     const table = $('#corporateTable').DataTable({
//         responsive: true,
//         dom: 'Bfrtip',
//         buttons: [
//             {
//                 text: '<i class="ri-file-excel-line"></i> Export All',
//             },
//         ],
//         paging: true,
//         info: true,
//         lengthChange: false,
//         pageLength: 10,
//         columnDefs: [
//             // { orderable: false, targets: [] } // all sortable
//             { orderable: false, targets: 'no-sort' }
//         ],
//         language: {
//             paginate: {
//             previous: '<i class="ri-arrow-left-s-line"></i>',
//             next: '<i class="ri-arrow-right-s-line"></i>'
//             }
//         }
//     });

//     // Move export buttons
//     table.buttons().container().appendTo('#exportcorporateButtons');

//     // Search
//     $('#customcorporateSearch').on('keyup', function () {
//         table.search(this.value).draw();
//     });

//     // Move pagination to custom div
//     $('#corporateTable_paginate').appendTo('#customcorporatePagination');

//     // Filter dropdown logic
//     $('.filter-option-corporate').on('click', function () {
//         const value = $(this).data('value');
//         const label = $(this).text();

//         // Update filter label after selection
//         $('#filterDropdownCorporate').text(label === 'All Status' ? 'Filter' : `${label}`);

//         // Apply DataTables column filter (status is column 2)
//         table.column(2).search(value).draw();
//     });

//     // Page length
//     $('#pageLength').on('change', function () {
//         table.page.len(this.value).draw();
//     });

//     // Update total reminders
//     $('#totalRemindersCorporate').text(`Total List: ${table.rows().count()}`);
// });








$(document).ready(function () {
    const table = $('#corporateTable').DataTable({
        responsive: false,
        dom: 'Bfrtip',
        buttons: [
            {
                text: '<i class="ri-file-excel-line"></i> Export All',
            },
        ],
        paging: true,
        info: true,
        lengthChange: false,
        pageLength: 10,
        columnDefs: [
            { orderable: false, className: 'details-control', targets: 0 }, // Expand control
            { orderable: false, targets: 'no-sort' }
        ],
        language: {
            paginate: {
                previous: '<i class="ri-arrow-left-s-line"></i>',
                next: '<i class="ri-arrow-right-s-line"></i>'
            }
        }
    });
    // Add child row toggle
    $('#corporateTable tbody').on('click', 'td.details-control', function () {
        const tr = $(this).closest('tr');
        const row = table.row(tr);
        if (row.child.isShown()) {
            row.child.hide();
            tr.removeClass('shown fulltable');
        } else {
            row.child(format(row.data())).show();
            tr.addClass('shown fulltable'); // Add your custom class here
        }
    });
    // Function to format child row content ${rowData[1]}
    // Move export buttons
    table.buttons().container().appendTo('#exportcorporateButtons');
    // Search
    $('#customcorporateSearch').on('keyup', function () {
        table.search(this.value).draw();
    });
    // Move pagination
    $('#corporateTable_paginate').appendTo('#customcorporatePagination');
    // Filter dropdown
    $('.filter-option-corporate').on('click', function () {
        const value = $(this).data('value');
        const label = $(this).text();
        $('#filterDropdownCorporate').text(label === 'All Status' ? 'Filter' : `${label}`);
        table.column(2).search(value).draw();
    });
    // Page length
    $('#pageLength').on('change', function () {
        table.page.len(this.value).draw();
    });
    // Update total reminders
    $('#totalRemindersCorporate').text(`Total List: ${table.rows().count()}`);
});
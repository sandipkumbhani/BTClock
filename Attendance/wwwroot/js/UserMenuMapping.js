document.addEventListener("DOMContentLoaded", function () {
    const clearButton = document.querySelector(".outline-bar");
    const $userSelect = $("#UserId"); // Ensure you're targeting the correct select element

    // Initialize Select2
    if ($userSelect.length) {
        $userSelect.select2();
    }

    const checkboxes = document.querySelectorAll(".menu-checkbox");

    // Clear checkboxes on page load
    checkboxes.forEach(checkbox => {
        checkbox.checked = false;
    });

    // Event listener for the "Clear All" button
    if (clearButton) {
        clearButton.addEventListener("click", function (e) {
            e.preventDefault();
            const form = document.getElementById("userForm");

            // Reset the form
            if (form) {
                form.reset();
            }

            // Reset checkboxes
            checkboxes.forEach(cb => cb.checked = false);

            // Reset the Select2 dropdown to its default state
            $userSelect.val("").trigger("change");
        });
    }

    // Event listener for the user selection change
    if ($userSelect.length) {
        $userSelect.on("change", function () {
            const selectedUser = $(this).val();

            // Reset checkboxes each time a user is selected
            checkboxes.forEach(cb => cb.checked = false);

            if (selectedUser) {
                // Fetch menus for the selected user
                fetch(`/UserMenuMapping/GetUserMenus?userId=${selectedUser}`)
                    .then(response => {
                        if (!response.ok) throw new Error("Menu fetch failed");
                        return response.json();
                    })
                    .then(data => {
                        // Check if the response contains valid menuIds
                        if (data && Array.isArray(data.menuIds)) {
                            // Update checkboxes based on the fetched menuIds
                            checkboxes.forEach(cb => {
                                if (data.menuIds.includes(parseInt(cb.value))) {
                                    cb.checked = true;
                                }
                            });
                        }
                    })
                    .catch(error => {
                        console.error("Error fetching user menus:", error);
                        alert("Unable to load user menus.");
                    });
            }
        });
    }
});

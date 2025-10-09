document.addEventListener("DOMContentLoaded", function () {
    const clearButton = document.querySelector(".outline-bar");
    const $userSelect = $("#UserId"); // FIXED: You were using an undefined variable before
    $userSelect.select2();

    const checkboxes = document.querySelectorAll(".menu-checkbox");

    checkboxes.forEach(checkbox => {
        checkbox.checked = false;
    });

    if (clearButton) {
        clearButton.addEventListener("click", function (e) {
            e.preventDefault();
            const form = document.getElementById("userForm");
            if (form) {
                form.reset(); 
            }

            checkboxes.forEach(cb => cb.checked = false);

            $userSelect.val("").trigger("change");
        });
    }

    if ($userSelect.length) {
        $userSelect.on("change", function () {
            const selectedUser = $(this).val();

            checkboxes.forEach(cb => cb.checked = false);

            if (selectedUser) {
                fetch(`/UserMenuMapping/GetUserMenus?userId=${selectedUser}`)
                    .then(response => {
                        if (!response.ok) throw new Error("Menu fetch failed");
                        return response.json();
                    })
                    .then(data => {
                        if (data && Array.isArray(data.menuIds)) {
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




document.addEventListener("DOMContentLoaded", function () {
    const clearButton = document.querySelector(".outline-bar");
    const saveButton = document.getElementById("saveButton");
    const $userSelect = $("#UserId");
    $userSelect.select2();

    const checkboxes = document.querySelectorAll(".menu-checkbox");
    checkboxes.forEach(cb => cb.checked = false);

    if (clearButton) {
        clearButton.addEventListener("click", function (e) {
            e.preventDefault();
            const form = document.getElementById("userForm");
            if (form) form.reset();
            checkboxes.forEach(cb => cb.checked = false);
            $userSelect.val("").trigger("change");
            document.getElementById("temporaryMessage").innerHTML = "";
        });
    }

    if ($userSelect.length) {
        $userSelect.on("change", function () {
            const selectedUser = $(this).val();
            checkboxes.forEach(cb => cb.checked = false);

            if (selectedUser) {
                fetch(`/api/UserMenuMapping/GetAllUserMenuMappings/${selectedUser}`)
                    .then(res => res.json())
                    .then(data => {
                        if (data && Array.isArray(data.menuItemIds)) {
                            checkboxes.forEach(cb => {
                                if (data.menuItemIds.includes(parseInt(cb.value))) {
                                    cb.checked = true;
                                }
                            });
                        }
                    })
                    .catch(err => {
                        console.error("Error fetching user menus:", err);
                        alert("Unable to load user menus.");
                    });
            }
        });
    }

    if (saveButton) {
        saveButton.addEventListener("click", function () {
            const selectedUser = $userSelect.val();
            if (!selectedUser) {
                alert("Please select a user.");
                return;
            }

            const selectedMenuIds = Array.from(checkboxes)
                .filter(cb => cb.checked)
                .map(cb => parseInt(cb.value));

            fetch(`/api/UserMenuMapping/UpdateUserMenuMappingsForUser/${selectedUser}`, {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({ MenuItemIds: selectedMenuIds })
            })
                .then(res => res.json())
                .then(data => {
                    const msgDiv = document.getElementById("temporaryMessage");
                    if (data && data.message) {
                        msgDiv.innerHTML = `<span style="color:#00af00;font-weight:600;">${data.message}</span>`;
                    } else {
                        msgDiv.innerHTML = `<span style="color:#d21313;font-weight:600;">Error updating menus.</span>`;
                    }
                })
                .catch(err => {
                    console.error("Error updating menus:", err);
                    alert("Unable to save user menus.");
                });
        });
    }
});
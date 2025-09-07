/* ============= SUB-MENU =============== */
document.querySelectorAll(".has-submenu").forEach((li) => {
    li.addEventListener("click", (e) => {
        // Prevent toggling when clicking inside the submenu itself
        if (!e.target.closest(".sub-menu")) {
            const submenu = li.querySelector(".sub-menu");

            // Close all other submenus
            document.querySelectorAll(".sub-menu.show").forEach((openSub) => {
                if (openSub !== submenu) {
                    openSub.classList.remove("show");
                }
            });

            // Toggle the clicked one
            submenu.classList.toggle("show");
        }
    });
});

/* ============= SIDEBAR TOGGLE =============== */
document.querySelectorAll(".toggle-arrow").forEach((arrow) => {
    arrow.addEventListener("click", (e) => {
        // Find the sidebar (adjust selector if needed)
        const sidebar = document.querySelector(".sidebar");
        const companyName = document.querySelector(".company-name");

        // Toggle the 'close' class
        sidebar.classList.toggle("close");
        companyName.classList.toggle("hide");
    });
});

const toggleBtn = document.getElementById("menu-toggle");
const sidebar = document.getElementById("sidebar");
const overlay = document.getElementById("overlay");

/* ============= TOGGLE SIDEBAR=============== */
toggleBtn.addEventListener("click", (e) => {
    sidebar.classList.toggle("active");
    overlay.classList.toggle("active");
});

// Close sidebar when clicking on overlay
overlay.addEventListener("click", () => {
    sidebar.classList.remove("active");
    overlay.classList.remove("active");
});
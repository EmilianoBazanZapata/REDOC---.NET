// Adds a "Show Documentation" button to the Swagger UI top bar that opens the ReDoc page.
// Swagger UI renders after the page loads, so wait until its top bar exists instead of using a fixed delay.
document.addEventListener("DOMContentLoaded", function () {
    var redocUrl = "/api-docs/index.html";

    function addButton() {
        var container = document.querySelector(".swagger-ui .topbar");
        if (!container || container.querySelector(".redoc-link")) return false;

        var button = document.createElement("button");
        button.innerText = "Show Documentation";
        button.className = "btn btn-primary redoc-link";
        button.addEventListener("click", function () {
            window.location.href = redocUrl;
        });

        container.appendChild(button);
        return true;
    }

    if (addButton()) return;

    var observer = new MutationObserver(function () {
        if (addButton()) observer.disconnect();
    });
    observer.observe(document.body, { childList: true, subtree: true });
});

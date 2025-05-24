window.AlertHelper = {
    showError: function (message, title = "Error") {
        Swal.fire({
            icon: 'error',
            title: title,
            text: message,
            confirmButtonText: 'OK',
            customClass: {
                confirmButton: "btn btn-danger"
            }
        });
    },

    showSuccess: function (message, title = "Success") {
        Swal.fire({
            icon: 'success',
            title: title,
            text: message,
            confirmButtonText: 'Nice!',
            customClass: {
                confirmButton: "btn btn-success"
            }
        });
    },

    showTempDataAlerts: function () {
        const success = document.getElementById("TempDataSuccess")?.value;
        const error = document.getElementById("TempDataError")?.value;

        if (error) AlertHelper.showError(error);
        if (success) AlertHelper.showSuccess(success);
    }
};

// Auto-invoke when DOM is ready
document.addEventListener("DOMContentLoaded", function () {
    AlertHelper.showTempDataAlerts();
});

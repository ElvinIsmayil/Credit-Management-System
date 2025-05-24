"use strict";

var KTSignupGeneral = function () {
    var form, submitButton;

    return {
        init: function () {
            form = document.querySelector("#kt_sign_up_form");
            submitButton = document.querySelector("#kt_sign_up_submit");

            if (!form || !submitButton) return;

            // Attach submit handler
            submitButton.addEventListener("click", function (e) {
                // Let jQuery validation handle the validation first
                if (!$(form).valid()) return;

                e.preventDefault(); // Prevent default only if valid
                submitButton.setAttribute("data-kt-indicator", "on");
                submitButton.disabled = true;

                setTimeout(function () {
                    submitButton.removeAttribute("data-kt-indicator");
                    submitButton.disabled = false;

                    // Removed alert popup here to avoid duplicates
                    form.submit(); // submit real form immediately
                }, 1000);
            });
        }
    };
}();

KTUtil.onDOMContentLoaded(function () {
    KTSignupGeneral.init();
});

// Password visibility toggle
window.togglePasswordVisibility = function (inputId, toggleIcon) {
    const input = document.getElementById(inputId);
    const icon = toggleIcon.querySelector("i");

    if (!input || !icon) return;

    if (input.type === "password") {
        input.type = "text";
        icon.classList.remove("bi-eye-slash");
        icon.classList.add("bi-eye");
    } else {
        input.type = "password";
        icon.classList.remove("bi-eye");
        icon.classList.add("bi-eye-slash");
    }
};

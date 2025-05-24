"use strict";

var KTSigninGeneral = function () {
    var form;
    var submitButton;
    var validator;

    return {
        init: function () {
            form = document.querySelector("#kt_sign_in_form");
            submitButton = document.querySelector("#kt_sign_in_submit");

            if (!form) return;

            validator = FormValidation.formValidation(form, {
                fields: {
                    email: {
                        validators: {
                            notEmpty: {
                                message: "Email address is required"
                            },
                            emailAddress: {
                                message: "Enter a valid email address"
                            }
                        }
                    },
                    password: {
                        validators: {
                            notEmpty: {
                                message: "Password is required"
                            }
                        }
                    }
                },
                plugins: {
                    trigger: new FormValidation.plugins.Trigger(),
                    bootstrap: new FormValidation.plugins.Bootstrap5({
                        rowSelector: ".fv-row"
                    })
                }
            });

            submitButton.addEventListener("click", function (e) {
                e.preventDefault();
                validator.validate().then(function (status) {
                    if (status === "Valid") {
                        form.submit(); // ?? Real submit
                    } else {
                        Swal.fire({
                            text: "Please fix the errors and try again.",
                            icon: "error",
                            buttonsStyling: false,
                            confirmButtonText: "Ok, got it!",
                            customClass: {
                                confirmButton: "btn btn-primary"
                            }
                        });
                    }
                });
            });
        }
    };
}();

KTUtil.onDOMContentLoaded(function () {
    KTSigninGeneral.init();
});

function togglePassword() {
    const input = document.getElementById('passwordInput');
    const icon = document.getElementById('toggleIcon');
    if (input.type === 'password') {
        input.type = 'text';
        icon.classList.remove('bi-eye-slash');
        icon.classList.add('bi-eye');
    } else {
        input.type = 'password';
        icon.classList.remove('bi-eye');
        icon.classList.add('bi-eye-slash');
    }
}

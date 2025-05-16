$(document).ready(function () {
    $('.delete-btn').click(function (e) {
        e.preventDefault();

        const button = $(this);
        const id = button.data('id');
        const name = button.data('name');
        const url = button.data('url');

        Swal.fire({
            title: 'Are you sure?',
            text: `You are about to delete ${name}`,
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Yes, delete!',
            cancelButtonText: 'Cancel'
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    url: url,
                    type: 'POST',
                    data: {
                        id: id,
                        __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val() // Anti-forgery token
                    },
                    success: function (response) {
                        if (response.success) {
                            Swal.fire('Deleted!', response.message, 'success');
                            // Remove the deleted item's row or element from UI
                            button.closest('tr').fadeOut(500, function () {
                                $(this).remove();
                            });
                        } else {
                            Swal.fire('Error!', response.message, 'error');
                        }
                    },
                    error: function () {
                        Swal.fire('Error!', 'Something went wrong. Try again.', 'error');
                    }
                });
            }
        });
    });
});

function handleActionConfirmation(form, action, name) {
    // Show SweetAlert for confirmation
    Swal.fire({
        title: 'Are you sure?',
        text: `You are about to ${action} ${name || 'all authors'}`,
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: `Yes, ${action}!`,
        cancelButtonText: 'Cancel'
    }).then((result) => {
        if (result.isConfirmed) {
            console.log("clicked")
            form.submit(); // Submit the form if confirmed
        } else {
            toastr.info(`${name || 'Action'} cancelled.`);
        }
    });
}

// Individual delete action
$('.delete-btn').click(function (e) {
    e.preventDefault(); // Prevent form submission
    const form = $(this).closest('form');
    const authorName = form.data('name'); // Get the author's name
    handleActionConfirmation(form, 'delete', authorName);function handleActionConfirmation(form, action, name) {
    // Show SweetAlert for confirmation
    Swal.fire({
        title: 'Are you sure?',
        text: `You are about to ${action} ${name || 'all authors'}`,
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: `Yes, ${action}!`,
        cancelButtonText: 'Cancel'
    }).then((result) => {
        if (result.isConfirmed) {
            form.submit(); // Submit the form if confirmed
        } else {
            toastr.info(`${name || 'Action'} cancelled.`);
        }
    });
}

// Individual delete action
$('.delete-btn').click(function (e) {
    e.preventDefault(); // Prevent form submission
    const form = $(this).closest('form');
    const authorName = form.data('name'); // Get the author's name
    handleActionConfirmation(form, 'delete', authorName);
});

// Delete all action
$('#delete-all-btn').click(function (e) {
    e.preventDefault(); // Prevent form submission
    handleActionConfirmation($('#delete-all-form'), 'delete all');
});

// Individual archive action
$('.archive-btn').click(function (e) {
    e.preventDefault(); // Prevent form submission
    const form = $(this).closest('form');
    const authorName = form.data('name'); // Get the author's name
    handleActionConfirmation(form, 'archive', authorName);
});

// Archive all action
$('#archive-all-btn').click(function (e) {
    e.preventDefault(); // Prevent form submission
    handleActionConfirmation($('#archive-all-form'), 'archive all');
});

});

// Delete all action
$('#delete-all-btn').click(function (e) {
    e.preventDefault(); // Prevent form submission
    handleActionConfirmation($('#delete-all-form'), 'delete all');
});

// Individual archive action
$('.archive-btn').click(function (e) {
    e.preventDefault(); // Prevent form submission
    const form = $(this).closest('form');
    const authorName = form.data('name'); // Get the author's name
    handleActionConfirmation(form, 'archive', authorName);
});

// Archive all action
$('#archive-all-btn').click(function (e) {
    e.preventDefault(); // Prevent form submission
    handleActionConfirmation($('#archive-all-form'), 'archive all');
});

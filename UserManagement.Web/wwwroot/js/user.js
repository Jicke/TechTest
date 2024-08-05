function SaveUser() {
    $('#errorMessage').addClass('d-none')
    var form = $('#SaveUserForm');
    $.post(form.attr("action"), form.serializeArray(), function (data) {
        if (data.ok) {
            window.location = $("#btnBack").attr("href");
        }
        else {
            $('#errorMessage').text(data.errorMessage).removeClass('d-none');
        }
    });
}
function ShowDeleteDialog(id) {
    $('#DeleteUserId').val(id);
    $('#deleteConfirmationModal').modal('show');
}
function DeleteUser() {
    var form = $('#DeleteUserForm');
    $.post(form.attr("action"), form.serializeArray(), function (data) {
        if (data.ok) {
            location.reload();
        }
    });
}

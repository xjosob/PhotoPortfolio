$(document).ready(function () {
    $('img[data-toggle="modal"]').click(function () {
        var src = $(this).attr('src');
        $('#modalImage').attr('src', src);
        $('#imageModal').modal('show');
    });
});

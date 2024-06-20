$(document).ready(function () {
    $('.img-fluid').click(function () {
        var src = $(this).attr('src');
        $('#modalImage').attr('src', src);
        $('#imageModal').modal('show');
    });
});


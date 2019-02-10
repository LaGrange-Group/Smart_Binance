$("#cancelButton").click(function () {
    alert("Trade Id: " + $(this).val());
    var url = $('#cancelconfrimmodal').data('url');
    $.get(url, function (data) {
        $("#cancelconfrimmodal").html(data);
        $("#cancelconfrimmodal").modal('show');
    });
});
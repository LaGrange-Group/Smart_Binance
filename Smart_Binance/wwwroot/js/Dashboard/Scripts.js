var container = $("#BuyContainer");  //quoteProcedureComponentContainer
var refreshComponent = function () {
    $.get("/Dashboard/GetMyViewComponent", { abcdef: $("#MarketName").val() }, function (data) { container.html(data); });
};
$(function () {
    $("#MarketName").on('change', function () {
        refreshComponent()
    });
})
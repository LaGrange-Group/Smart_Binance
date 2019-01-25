var container = $("#BuyContainer");  //quoteProcedureComponentContainer
var refreshComponent = function () {
    var percentBase = '';
    $('#PercentTypeGroup .active').each(function () {
        percentBase = $(this).attr('id');
    });
    $.get("/Dashboard/GetMyViewComponent", { marketPass: $("#MarketName").val(), percentTypePass: percentBase }, function (data) { container.html(data); });
};
$(function () {
    $("#MarketName").on('change', function () {
        refreshComponent();
        
    });
})

$(function () {
    $('#button-basepercent-25').addClass('active');
})

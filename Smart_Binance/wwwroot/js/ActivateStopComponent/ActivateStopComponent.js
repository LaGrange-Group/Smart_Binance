//------------------------------ Stop Loss Component
var marketName = document.getElementById('#MarketName');
var activateStop = document.getElementById("activatestop");
var stockMarket = "----Select Market----";
$(activateStop).click(function () {
    ActivateStop();
});

function ActivateStop() {
    var selectedTab = $("#tabtype li.active").attr("id");
    if ($(marketName).val() !== stockMarket && selectedTab == "markettab") {
        $.get("/Dashboard/LoadingGif", { typePass: "red" }, function (data) { stopLossContainer.html(data); });
        $.get("/Dashboard/UpdateStopLossViewComponent", { pricePass: $("#lastprice").val(), marketPass: $("#MarketName").val() }, function (data) { stopLossContainer.html(data); });
    } else if ($(marketName).val() !== stockMarket && selectedTab == "limittab") {
        $.get("/Dashboard/LoadingGif", { typePass: "red" }, function (data) { stopLossContainer.html(data); });
        $.get("/Dashboard/UpdateStopLossViewComponent", { pricePass: $("#lastprice-limit").val(), marketPass: $("#MarketName").val() }, function (data) { stopLossContainer.html(data); });
    } else if ($(marketName).val() !== stockMarket && selectedTab == "selltab") {
        $.get("/Dashboard/LoadingGif", { typePass: "red" }, function (data) { stopLossContainer.html(data); });
        $.get("/Dashboard/UpdateStopLossViewComponent", { pricePass: $("#lastprice-sell").val(), marketPass: $("#MarketName").val() }, function (data) { stopLossContainer.html(data); });
    }
}
﻿//------------------------------ Take Profit Component
var marketName = document.getElementById('#MarketName');
var activateTake = document.getElementById("activatetake");
var stockMarket = "----Select Market----";
var selectedTab = $("#tabtype li.active").attr("id");
if (selectedTab !== "limittab") {
    DisableActivateStopTake();
} else if (selectedTab === "limittab" && $('#MarketName').val() === defaultMarket) {
    DisableActivateStopTake();
}

$(activateTake).click(function () {
    ActivateTake();
});

function ActivateTake() {
    SetTakeTrue();
    var selectedTab = $("#tabtype li.active").attr("id");
    if ($(marketName).val() !== stockMarket && selectedTab == "markettab") {
        $.get("/Dashboard/LoadingGif", { typePass: "green" }, function (data) { takeProfitContainer.html(data); });
        $.get("/Dashboard/UpdateTakeProfitViewComponent", { pricePass: $("#lastprice").val(), marketPass: $("#MarketName").val() }, function (data) { takeProfitContainer.html(data); });
    } else if ($(marketName).val() !== stockMarket && selectedTab == "limittab") {
        $.get("/Dashboard/LoadingGif", { typePass: "green" }, function (data) { takeProfitContainer.html(data); });
        $.get("/Dashboard/UpdateTakeProfitViewComponent", { pricePass: $("#lastprice-limit").val(), marketPass: $("#MarketName").val() }, function (data) { takeProfitContainer.html(data); });
    } else if ($(marketName).val() !== stockMarket && selectedTab == "selltab") {
        $.get("/Dashboard/LoadingGif", { typePass: "green" }, function (data) { takeProfitContainer.html(data); });
        $.get("/Dashboard/UpdateTakeProfitViewComponent", { pricePass: $("#lastprice-sell").val(), marketPass: $("#MarketName").val() }, function (data) { takeProfitContainer.html(data); });
    }
}

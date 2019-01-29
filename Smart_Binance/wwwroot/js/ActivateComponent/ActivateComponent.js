//------------------------------ Take Profit Component

var activateStop = document.getElementById("activatetake");
$(activateStop).click(function () {
    ActivateTake();
});

function ActivateTake() {
    alert("Entered Update Take");
    if ($('#MarketName').val() !== "----Select Market----") {
        $.get("/Dashboard/UpdateTakeProfitViewComponent", { pricePass: $("#lastprice").val(), marketPass: $("#MarketName").val() }, function (data) { takeProfitContainer.html(data); });
    } else {
        $.get("/Dashboard/UpdateTakeProfitViewComponent", { pricePass: $("#lastprice").val(), marketPass: $("#MarketName").val() }, function (data) { takeProfitContainer.html(data); });
    }
}
//------------------------------ Stop Loss Component

var activateStop = document.getElementById("activatestop");
$(activateStop).click(function () {
    ActivateStop();
});

function ActivateStop() {
    alert("Entered Update Stop");
    if ($('#MarketName').val() !== "----Select Market----") {
        $.get("/Dashboard/UpdateStopLossViewComponent", { pricePass: $("#lastprice").val(), marketPass: $("#MarketName").val() }, function (data) { stopLossContainer.html(data); });
    } else {
        $.get("/Dashboard/UpdateStopLossViewComponent", { pricePass: $("#lastprice").val(), marketPass: $("#MarketName").val() }, function (data) { stopLossContainer.html(data); });
    }
}
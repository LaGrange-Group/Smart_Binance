// -------------------------- Tab Components
var marketName = document.getElementById('#MarketName');
var stockMarket = "----Select Market----";
var marketContainer = $("#MarketContainer");  //quoteProcedureComponentContainer
var limitContainer = $("#LimitContainer");  //quoteProcedureComponentContainer
var sellContainer = $("#SellContainer");  //quoteProcedureComponentContainer
var stopLossContainer = $("#StopLossContainer");  //quoteProcedureComponentContainer
var takeProfitContainer = $("#TakeProfitContainer");  //quoteProcedureComponentContainer
var tradesContainer = $("#SmartTradesContainer");  //quoteProcedureComponentContainer
var baseDecimalAmount = 0;
var priceDecimalAmount = 0;
var assetDecimalAmount = 0;
var minValue = 0;
var stopLossBool = 0;
var takeProfitBool = 0;
var trailingStopLossBool = 0;
var trailingTakeProfitBool = 0;
DisableCreateButton();

function CancelClicked(id) {
    $('#cancelconfirmmodal').val(id);
    var url = $('#cancelconfirmmodal').data('url');
    $.get(url, function (data) {
        $("#cancelconfirmmodal").html(data);
        $("#cancelconfirmmodal").modal('show');
    });
}

function SellClicked (id) {
    $('#sellconfirmmodal').val(id);
    var url = $('#sellconfirmmodal').data('url');
    $.get(url, function (data) {
        $("#sellconfirmmodal").html(data);
        $("#sellconfirmmodal").modal('show');
    });
}

var defaultMarket = "----Select Market----";
var refreshComponent = function (tab) {

    if (tab === "markettab") {
        var percentBase = '';
        $('#PercentTypeGroup .active').each(function () {
            percentBase = $(this).attr('id');
        });
        $.get("/Dashboard/LoadingGif", { typePass: "binance" }, function (data) { marketContainer.html(data); });
        $.get("/Dashboard/GetMyMarketViewComponent", { marketPass: $("#MarketName").val(), percentTypePass: percentBase }, function (data) { marketContainer.html(data); });
    } else if (tab === "limittab") {
        var percentBase = '';
        $('#PercentTypeGroup-limit .active').each(function () {
            percentBase = $(this).attr('id');
        });
        $.get("/Dashboard/LoadingGif", { typePass: "binance" }, function (data) { limitContainer.html(data); });
        $.get("/Dashboard/GetMyLimitViewComponent", { marketPass: $("#MarketName").val(), percentTypePass: percentBase }, function (data) { limitContainer.html(data); });
    } else if (tab === "selltab") {
        $.get("/Dashboard/LoadingGif", { typePass: "binance" } , function (data) { sellContainer.html(data); });
        $.get("/Dashboard/GetMySellViewComponent", { marketPass: $("#MarketName").val() }, function (data) { sellContainer.html(data); });
    }
};
$(function () {
    $("#MarketName").on('change', function () {
        DisableMarketSelect();
        DisableActivateStopTake();
        var selectedTab = $("#tabtype li.active").attr("id");
        UpdateStopTakeContainer();
        refreshComponent(selectedTab);
    });
})

$('#tabtype li').click(function () {
    if ($('#MarketName').val() !== "----Select Market----")
        DisableMarketSelect();
    var selectedTab = $(this).attr('id');
    UpdateStopTakeContainer();
    if ($('#MarketName').val() !== defaultMarket && selectedTab == "limittab") {
        var currentBaseAmount = $('#basetotal').val();
        var percentBase = '';
        $('#PercentTypeGroup .active').each(function () {
            percentBase = $(this).attr('id') + "-limit";
        });
        if (percentBase !== '') {
            currentBaseAmount = 0;
        }
        $.get("/Dashboard/LoadingGif", { typePass: "binance" }, function (data) { limitContainer.html(data); });
        $.get("/Dashboard/SwitchToLimitViewComponent", { marketPass: $("#MarketName").val(), percentTypePass: percentBase, currentBasePass: currentBaseAmount }, function (data) { limitContainer.html(data); });
    } else if ($('#MarketName').val() !== defaultMarket && selectedTab == "markettab") {
        var currentBaseAmount = $('#basetotal-limit').val();
        var percentBase = '';
        $('#PercentTypeGroup-limit .active').each(function () {
            percentBase = $(this).attr('id') + "-limit";
        });
        if (percentBase !== '') {
            currentBaseAmount = 0;
        }
        $.get("/Dashboard/LoadingGif", { typePass: "binance" }, function (data) { marketContainer.html(data); });
        $.get("/Dashboard/SwitchToMarketViewComponent", { marketPass: $("#MarketName").val(), percentTypePass: percentBase, currentBasePass: currentBaseAmount }, function (data) { marketContainer.html(data); });
    } else if ($('#MarketName').val() !== defaultMarket && selectedTab == "selltab") {
        $.get("/Dashboard/LoadingGif", { typePass: "binance" }, function (data) { sellContainer.html(data); });
        $.get("/Dashboard/GetMySellViewComponent", { marketPass: $("#MarketName").val() }, function (data) { sellContainer.html(data); });
    }
})

$(function () {
    $('#button-basepercent-25').addClass('active');
    $('#button-basepercent-25-limit').addClass('active');
});


$(".percTypeBtn-limit").click(function () {
    $(".percTypeBtn-limit").removeClass("active");
    var percent = $(this).val();
});

$(".percTypeBtn").click(function () {
    $(".percTypeBtn").removeClass("active");
    var percent = $(this).val();
});


// --------------------------- Reset Stop and Take

function UpdateStopTakeContainer() {
    $.get("/Dashboard/ResetStopLoss", function (data) { stopLossContainer.html(data); });
    $.get("/Dashboard/ResetTakeProfit", function (data) { takeProfitContainer.html(data); });
    SetStopFalse();
    SetTakeFalse();
    ResetStopTakeValues();
}

function ResetStopTakeValues() {
    stopLossBool = 0;
    takeProfitBool = 0;
    trailingStopLossBool = 0;
    trailingTakeProfitBool = 0;
}

// ---------------------------------- Utility Functions

function RefreshTradesComponent() {
    $.get("/Dashboard/UpdateTradesComponent", function (data) { tradesContainer.html(data); });
}

function DisableActivateStopTake() {
    $("#activatestop").attr("disabled", true);
    $("#activatetake").attr("disabled", true);
}

function EnableActivateStopTake() {
    $("#activatestop").attr("disabled", false);
    $("#activatetake").attr("disabled", false);
}

function DisableMarketSelect() {
    $("#MarketName").attr("disabled", true);
}

function EnableMarketSelect() {
    $("#MarketName").attr("disabled", false);
}

function DisableCreateButton() {
    $("#smarttradebutton").attr("disabled", true);
}

function EnableCreateButton() {
    $("#smarttradebutton").attr("disabled", false);
}

function CheckConditionValuesSell(pricePass, typeFrom) {
    var selectedTab = $("#tabtype li.active").attr("id");
    var amountGrab = 0;
    var baseCurrencyAmount = 0;
    if (selectedTab == "markettab") {
        amountGrab = $('#amount').val();
    } else if (selectedTab == "limittab") {
        amountGrab = $('#amount-limit').val();
    } else if (selectedTab == "selltab") {
        amountGrab = $('#amount-sell').val();
    }
    var conditionValue = pricePass * amountGrab;
    //alert("Min val: " + minValue + " Amount: " + amount + " Condition val: " + conditionValue);
    if (conditionValue < minValue) {
        DisableCreateButton();
        if (typeFrom === "stop") {
            $('#price-stoploss').css('color', 'red');
        } else if (typeFrom === "take") {
            $('#price-take').css('color', 'red');
        }
    } else {
        EnableCreateButton();
        if (typeFrom === "stop") {
            $('#price-stoploss').css('color', 'black');
        } else if (typeFrom === "take") {
            $('#price-take').css('color', 'black');
        }
    }
    if (takeProfitBool === 1 && stopLossBool === 1) {
        ConfirmBothTakeStopValidValues();
    }
    if (selectedTab == "markettab") {
        baseCurrencyAmount = $('#basetotal').val();
        if (baseCurrencyAmount < minValue || baseCurrencyAmount > baseTotal) {
            DisableCreateButton();
        }
    } else if (selectedTab == "limittab") {
        baseCurrencyAmount = $('#basetotal-limit').val();
        if (baseCurrencyAmount < minValue || baseCurrencyAmount > baseTotal) {
            DisableCreateButton();
        }
    } else if (selectedTab == "selltab") {
        if (amountGrab > amount) {
            DisableCreateButton();
        }
    }
}

function ConfirmBothTakeStopValidValues() {
    var selectedTab = $("#tabtype li.active").attr("id");
    var amount = 0;
    if (selectedTab == "markettab") {
        amount = $('#amount').val();
    } else if (selectedTab == "limittab") {
        amount = $('#amount-limit').val();
    } else if (selectedTab == "selltab") {
        amount = $('#amount-sell').val();
    }
    var takePriceGrab = $('#price-take').val();
    var stopPriceGrab = $('#price-stoploss').val();
    var takeConditionValue = takePriceGrab * amount;
    var stopConditionValue = stopPriceGrab * amount;
    if (takeConditionValue < minValue || stopConditionValue < minValue) {
        DisableCreateButton();
    } else {
        EnableCreateButton();
    }
}

function CallCheckCondtionFunc() {
    if (takeProfitBool == 1) {
        CheckConditionValuesSell($('#price-take').val(), "take");
    }
    if (stopLossBool == 1) {
        CheckConditionValuesSell($('#price-stoploss').val(), "stop");
    }
}

//------------------------------ Take Profit Component

var activateTake = document.getElementById("activatetake");

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

var slider = document.getElementById("myRange");
var output = document.getElementById("stocktakeprofitpercentbox");
var priceBox = document.getElementById("price-sell");
output.value = slider.value;



output.oninput = function () {
    slider.value = this.value;
    var newPrice = (initialPrice * 1) + (initialPrice * (this.value / 100));
    priceBox.value = newPrice.toFixed(priceDecimal);
}

slider.oninput = function () {
    output.value = this.value;
    var newPrice = (initialPrice * 1) + (initialPrice * (this.value / 100));
    priceBox.value = newPrice.toFixed(priceDecimal);
}

$(priceBox).on('change', function () {
    this.value = (this.value * 1).toFixed(priceDecimal);
    var priceDiff = this.value - initialPrice;
    var percentGain = (priceDiff / initialPrice) * 100;
    output.value = percentGain.toFixed(2);
});

function precision(a) {
    if (!isFinite(a)) return 0;
    var e = 1, p = 0;
    while (Math.round(a * e) / e !== a) { e *= 10; p++; }
    return p;
}

function InitialTPTake() {
    var newPrice = (initialPrice * 1) + (initialPrice * (output.value / 100));
    priceBox.value = newPrice.toFixed(priceDecimal);
};

function SetTakeTrue() {
    takeProfitBool = 1;
}

function SetTakeFalse() {
    takeProfitBool = 0;
}

function FlipTrailingTake() {
    if (trailingTakeProfitBool == 0) {
        trailingTakeProfitBool = 1;
    } else if (trailingTakeProfitBool == 1) {
        trailingTakeProfitBool = 0;
    }
}
//------------------------------ Stop Loss Component

var activateStop = document.getElementById("activatestop");
$('#activatestop').click(function () {
    ActivateStop();
});

function ActivateStop() {
    SetStopTrue();
    var internVal = $('#stoploss').attr('internalid');
    alert(internVal);
    var selectedTab = $("#tabtype li.active").attr("id");
    if ($(marketName).val() !== stockMarket && selectedTab == "markettab") {
        $.get("/Dashboard/LoadingGif", { typePass: "red" }, function (data) { sellContainer.html(data); });
        $.get("/Dashboard/UpdateStopLossViewComponent", { pricePass: $("#lastprice").val(), marketPass: $("#MarketName").val() }, function (data) { stopLossContainer.html(data); });
    } else if ($(marketName).val() !== stockMarket && selectedTab == "limittab") {
        $.get("/Dashboard/LoadingGif", { typePass: "red" }, function (data) { sellContainer.html(data); });
        $.get("/Dashboard/UpdateStopLossViewComponent", { pricePass: $("#lastprice-limit").val(), marketPass: $("#MarketName").val() }, function (data) { stopLossContainer.html(data); });
    } else if ($(marketName).val() !== stockMarket && selectedTab == "selltab") {
        $.get("/Dashboard/LoadingGif", { typePass: "red" }, function (data) { sellContainer.html(data); });
        $.get("/Dashboard/UpdateStopLossViewComponent", { pricePass: $("#lastprice-sell").val(), marketPass: $("#MarketName").val() }, function (data) { stopLossContainer.html(data); });
    }
}

var sliderStop = document.getElementById("myRange-stoploss");
var outputStop = document.getElementById("stockstoplosspercentbox");
var priceBoxStop = document.getElementById("price-stoploss");
outputStop.value = sliderStop.value;



outputStop.oninput = function () {
    sliderStop.value = this.value;
    var newPrice = (initialPrice * 1) + (initialPrice * (this.value / 100));
    priceBoxStop.value = newPrice.toFixed(priceDecimal);
}

sliderStop.oninput = function () {
    outputStop.value = this.value;
    var newPrice = (initialPrice * 1) + (initialPrice * (this.value / 100));
    priceBoxStop.value = newPrice.toFixed(priceDecimal);
}

$(priceBoxStop).on('change', function () {
    this.value = (this.value * 1).toFixed(priceDecimal);
    var priceDiff = this.value - initialPrice;
    var percentGain = (priceDiff / initialPrice) * 100;
    outputStop.value = percentGain.toFixed(2);
});

function precision(a) {
    if (!isFinite(a)) return 0;
    var e = 1, p = 0;
    while (Math.round(a * e) / e !== a) { e *= 10; p++; }
    return p;
}

function InitialTPStop() {
    var newPrice = (initialPrice * 1) + (initialPrice * (outputStop.value / 100));
    priceBoxStop.value = newPrice.toFixed(priceDecimal);
};

function SetStopTrue() {
    stopLossBool = 1;
}

function SetStopFalse() {
    stopLossBool = 0;
}

function FlipTrailingStop() {
    if (trailingStopLossBool == 0) {
        trailingStopLossBool = 1;
    } else if (trailingStopLossBool == 1) {
        trailingStopLossBool = 0;
    }
}
// ---------------------------------------- Trade Visual Buttons





// ---------------------------------- Create Smart Trade

function CreateSmartTrade() {
    var market = document.getElementById('modelmarket');
    var tradeType = document.getElementById('modeltradetype');
    var amount = document.getElementById('modelamount');
    var price = document.getElementById('modelprice');
    var baseAmount = document.getElementById('modelbaseamount');
    var takeProfitPrice = document.getElementById('modeltakeprofitprice');
    var stopLossPrice = document.getElementById('modelstoplossprice');
    var trailingTakePercent = document.getElementById('modeltrailingtakepercent');
    var stopLoss = document.getElementById('modelstoploss');
    var takeProfit = document.getElementById('modeltakeprofit');
    var trailingStopLoss = document.getElementById('modeltrailingstoploss');
    var trailingTakeProfit = document.getElementById('modeltrailingtakeprofit');
    var buildMarketPriceDecimal = document.getElementById('modelpricedecimal');
    var buildAmountDecimal = document.getElementById('modelamountdecimal');
    var buildBasePriceDecimal = document.getElementById('modelbasepricedecimal');
    var buildMinValue = document.getElementById('modelminvalue');
    $(buildBasePriceDecimal).val(baseDecimalAmount);
    $(buildMarketPriceDecimal).val(priceDecimalAmount);
    $(buildAmountDecimal).val(assetDecimalAmount);
    $(buildMinValue).val(minValue);
    $(market).val($('#MarketName').val());
    var selectedTab = $("#tabtype li.active").attr("id");

    if (selectedTab == 'markettab') {
        GatherMarketDetails(tradeType, amount, price, baseAmount);
    } else if (selectedTab == 'limittab') {
        GatherLimitDetails(tradeType, amount, price, baseAmount);
    } else if (selectedTab == 'selltab') {
        GatherSellDetails(tradeType, amount, price);
    }
    GatherStopLossDetails(stopLossPrice, stopLoss, trailingStopLoss);
    GatherTakeProfitDetails(takeProfitPrice, trailingTakePercent, takeProfit, trailingTakeProfit);
    $('#smartform').submit();
}

function GatherMarketDetails(tradeType, amount, price, baseAmount) {
    $(tradeType).val("market");
    $(amount).val($('#amount').val());
    $(price).val($('#lastprice').val());
    $(baseAmount).val($('#basetotal').val());
}

function GatherLimitDetails(tradeType, amount, price, baseAmount) {
    $(tradeType).val("limit");
    $(amount).val($('#amount-limit').val());
    $(price).val($('#lastprice-limit').val());
    $(baseAmount).val($('#basetotal-limit').val());
}

function GatherSellDetails(tradeType, amount, price) {
    $(tradeType).val("sell");
    $(price).val($('#lastprice-sell').val());
    $(amount).val($('#amount-sell').val());
}

function GatherStopLossDetails(stopLossPrice, stopLoss, trailingStopLoss) {
    if (stopLossBool == 1) {
        $(stopLoss).val(true);
        $(stopLossPrice).val($('#price-stoploss').val());
        if (trailingStopLossBool == 1) {
            $(trailingStopLoss).val(true);
        } else {
            $(trailingStopLoss).val(false);
        }
    } else {
        $(stopLoss).val(false);
    }
}

function GatherTakeProfitDetails(takeProfitPrice, trailingTakePercent, takeprofit, trailingTakeProfit) {
    if (takeProfitBool == 1) {
        $(takeprofit).val(true);
        $(takeProfitPrice).val($('#price-take').val());
        if (trailingTakeProfitBool == 1) {
            $(trailingTakeProfit).val(true);
            $(trailingTakePercent).val($('#trailingtakeprofitpercentbox').val());
        } else {
            $(trailingTakeProfit).val(false);
        }
    } else {
        $(takeprofit).val(false);
    }
}
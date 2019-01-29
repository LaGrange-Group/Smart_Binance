// -------------------------- Tab Components
var marketName = document.getElementById('#MarketName');
var stockMarket = "----Select Market----";
var marketContainer = $("#MarketContainer");  //quoteProcedureComponentContainer
var limitContainer = $("#LimitContainer");  //quoteProcedureComponentContainer
var sellContainer = $("#SellContainer");  //quoteProcedureComponentContainer
var stopLossContainer = $("#StopLossContainer");  //quoteProcedureComponentContainer
var takeProfitContainer = $("#TakeProfitContainer");  //quoteProcedureComponentContainer
  //quoteProcedureComponentContainer

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
        var selectedTab = $("#tabtype li.active").attr("id");
        UpdateStopTakeContainer();
        refreshComponent(selectedTab);
    });
})

$('#tabtype li').click(function () {
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
})


$(".percTypeBtn-limit").click(function () {
    $(".percTypeBtn-limit").removeClass("active");
    var percent = $(this).val();
});

$(".percTypeBtn").click(function () {
    $(".percTypeBtn").removeClass("active");
    var percent = $(this).val();
});



//------------------------------ Take Profit Component

var activateTake = document.getElementById("activatetake");

$(activateTake).click(function () {
    ActivateTake();
});

function ActivateTake() {
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


//------------------------------ Stop Loss Component

var activateStop = document.getElementById("activatestop");
$('#activatestop').click(function () {
    ActivateStop();
});

function ActivateStop() {
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


// --------------------------- Reset Stop and Take

function UpdateStopTakeContainer() {
    $.get("/Dashboard/ResetStopLoss", function (data) { stopLossContainer.html(data); });
    $.get("/Dashboard/ResetTakeProfit", function (data) { takeProfitContainer.html(data); });
}
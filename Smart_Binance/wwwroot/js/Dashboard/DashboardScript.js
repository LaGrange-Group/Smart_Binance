// -------------------------- Tab Components

var marketContainer = $("#MarketContainer");  //quoteProcedureComponentContainer
var limitContainer = $("#LimitContainer");  //quoteProcedureComponentContainer
var sellContainer = $("#SellContainer");  //quoteProcedureComponentContainer
  //quoteProcedureComponentContainer

var defaultMarket = "----Select Market----";
var refreshComponent = function (tab) {
    if (tab === "markettab") {
        var percentBase = '';
        $('#PercentTypeGroup .active').each(function () {
            percentBase = $(this).attr('id');
        });
        $.get("/Dashboard/GetMyMarketViewComponent", { marketPass: $("#MarketName").val(), percentTypePass: percentBase }, function (data) { marketContainer.html(data); });
    } else if (tab === "limittab") {
        var percentBase = '';
        $('#PercentTypeGroup-limit .active').each(function () {
            percentBase = $(this).attr('id');
        });
        $.get("/Dashboard/GetMyLimitViewComponent", { marketPass: $("#MarketName").val(), percentTypePass: percentBase }, function (data) { limitContainer.html(data); });
    } else if (tab === "selltab") {
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
    if ($('#MarketName').val() !== defaultMarket && selectedTab == "limittab") {
        var currentBaseAmount = $('#basetotal').val();
        var percentBase = '';
        $('#PercentTypeGroup .active').each(function () {
            percentBase = $(this).attr('id') + "-limit";
        });
        if (percentBase !== '') {
            currentBaseAmount = 0;
        }
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
        $.get("/Dashboard/SwitchToMarketViewComponent", { marketPass: $("#MarketName").val(), percentTypePass: percentBase, currentBasePass: currentBaseAmount }, function (data) { marketContainer.html(data); });
    } else if ($('#MarketName').val() !== defaultMarket && selectedTab == "selltab") {
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


var slider = document.getElementById("myRange");
var output = document.getElementById("stocktakeprofitpercentbox");
output.value = slider.value;

$('#stocktakeprofitpercentbox').on('input', function () {
    slider.value = this.value;
});

slider.oninput = function () {
    output.value = this.value;
}

// --------------------------- Reset Stop and Take

function UpdateStopTakeContainer() {
    $.get("/Dashboard/ResetStopLoss", function (data) { stopLossContainer.html(data); });
    $.get("/Dashboard/ResetTakeProfit", function (data) { takeProfitContainer.html(data); });
}
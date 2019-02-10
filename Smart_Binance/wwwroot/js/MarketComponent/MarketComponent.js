$(function () {
    if (percentType !== "") {
        $(percentType).addClass('active');
    }
})
var takeProfitContainer = $("#TakeProfitContainer");
var stopLossContainer = $("#StopLossContainer");
baseCurrencyTotalCheck(baseAmount);

$(".percTypeBtn").click(function () {
    $(".percTypeBtn").removeClass("active");
    var percent = $(this).val();
    var baseAmountCalc = (baseTotal * percent).toFixed(baseDecimalAmount);
    $('#basetotal').css('color', 'black');
    $('#basetotal').val(baseAmountCalc);
    $('#amount').val((baseAmountCalc / lastPrice).toFixed(assetDecimalAmount));
    baseCurrencyTotalCheck($('#basetotal').val());
});

$('body').on('change', '#basetotal', function () {
    removeActiveButton();
    var inputBaseAmount = $(this).val();
    inputBaseAmount = (inputBaseAmount * 1).toFixed(baseDecimalAmount);
    $(this).val(inputBaseAmount);
    baseCurrencyTotalCheck(inputBaseAmount);
    $('#amount').val((inputBaseAmount / lastPrice).toFixed(assetDecimalAmount));
});

$('body').on('change', '#amount', function () {
    removeActiveButton();
    var inputAmount = $('#amount').val();
    var newCost = (inputAmount * lastPrice).toFixed(baseDecimalAmount);
    $('#basetotal').val(newCost);
    var cba = $('#basetotal').val();
    baseCurrencyTotalCheck(cba);
});

// ----------------- Activate Take Profit



function removeActiveButton() {
    try {
        var percentBase = '';
        $('#PercentTypeGroup .active').each(function () {
            percentBase = $(this).attr('id');
        });
        $('#' + percentBase).removeClass("active");
    } catch{
        
    }
}

function baseCurrencyTotalCheck(currentBaseAmount) {
    if (currentBaseAmount > baseTotal || currentBaseAmount < minValue) {
        $('#basetotal').css('color', 'red');
        DisableCreateButton();
    } else {
        $('#basetotal').css('color', 'black');
        EnableCreateButton();
    }
    if (takeProfitBool == 1) {
        CheckConditionValuesSell($('#price-take').val(), "take");
    }
    if (stopLossBool == 1) {
        CheckConditionValuesSell($('#price-stoploss').val(), "stop");
    }
}



$('#smarttradebutton').click(function () {
    var selectedTab = $("#tabtype li.active").attr("id");
    if (selectedTab == 'markettab') {
        CreateSmartTrade();
    }
});

function ReturnMarketAssetDecimal() {
    return assetDecimalAmount;
}

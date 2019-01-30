$(function () {
    if (percentType !== "") {
        $(percentType).addClass('active');
    }
})
var takeProfitContainer = $("#TakeProfitContainer");
var stopLossContainer = $("#StopLossContainer");
baseCurrencyTotalColor(baseAmount);

$(".percTypeBtn").click(function () {
    $(".percTypeBtn").removeClass("active");
    var percent = $(this).val();
    var baseAmountCalc = (baseTotal * percent).toFixed(baseDecimalAmount);
    $('#basetotal').css('color', 'black');
    $('#basetotal').val(baseAmountCalc);
    $('#amount').val((baseAmountCalc / lastPrice).toFixed(assetDecimalAmount));
});

$('body').on('change', '#basetotal', function () {
    removeActiveButton();
    var inputBaseAmount = $(this).val();
    inputBaseAmount = (inputBaseAmount * 1).toFixed(baseDecimalAmount);
    $(this).val(inputBaseAmount);
    baseCurrencyTotalColor(inputBaseAmount);
    $('#amount').val((inputBaseAmount / lastPrice).toFixed(assetDecimalAmount));
});

$('body').on('change', '#amount', function () {
    removeActiveButton();
    var inputAmount = $('#amount').val();
    var newCost = (inputAmount * lastPrice).toFixed(baseDecimalAmount);
    $('#basetotal').val(newCost);
    var cba = $('#basetotal').val();
    baseCurrencyTotalColor(cba);
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

function baseCurrencyTotalColor(currentBaseAmount) {
    if (currentBaseAmount > baseTotal) {
        $('#basetotal').css('color', 'red');
    } else {
        $('#basetotal').css('color', 'black');
    }
}



$('#smarttradebutton').click(function () {
    var selectedTab = $("#tabtype li.active").attr("id");
    if (selectedTab == 'markettab') {
        alert("Triggered From Market Component");
        CreateSmartTrade();
    }
});
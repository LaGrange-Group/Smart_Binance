$(function () {
    if (percentType !== "") {
        $(percentType).addClass('active');
    }
})
baseCurrencyTotalCheck(baseAmount);


$(".percTypeBtn-limit").click(function () {
    $(".percTypeBtn-limit").removeClass("active");
    var percent = $(this).val();
    var baseAmountCalc = (baseTotal * percent).toFixed(baseDecimalAmount);
    $('#basetotal-limit').val(baseAmountCalc);
    $('#amount-limit').val((baseAmountCalc / $('#lastprice-limit').val()).toFixed(assetDecimalAmount));
    baseCurrencyTotalCheck($('#basetotal-limit').val());
});

$('#lastprice-limit').on('change', function () {
    var currentBaseAmount = $('#basetotal-limit').val();
    $('#amount-limit').val((currentBaseAmount / $(this).val()).toFixed(assetDecimalAmount));
    UpdateStopTakeContainer();
});


$('#basetotal-limit').on('change', function () {
    removeActiveButton();
    var inputBaseAmount = $(this).val();
    inputBaseAmount = (inputBaseAmount * 1).toFixed(baseDecimalAmount);
    $(this).val(inputBaseAmount);
    baseCurrencyTotalCheck(inputBaseAmount);
    $('#amount-limit').val((inputBaseAmount / $('#lastprice-limit').val()).toFixed(assetDecimalAmount));
});

$('#amount-limit').on('change', function () {
    removeActiveButton();
    var inputAmount = $(this).val();
    var newCost = (inputAmount * lastPrice).toFixed(baseDecimalAmount);
    $('#basetotal-limit').val(newCost);
    var cba = $('#basetotal-limit').val();
    baseCurrencyTotalCheck(cba);
});

function baseCurrencyTotalCheck(currentBaseAmount) {
    if (currentBaseAmount > baseTotal || currentBaseAmount < minValue) {
        $('#basetotal-limit').css('color', 'red');
        DisableCreateButton();
    } else {
        $('#basetotal-limit').css('color', 'black');
        EnableCreateButton();
    }
}

function removeActiveButton() {
    try {
        var percentBase = '';
        $('#PercentTypeGroup-limit .active').each(function () {
            percentBase = $(this).attr('id');
        });
        $('#' + percentBase).removeClass("active");
    } catch{
       
    }
}

$('#smarttradebutton').click(function () {
    var selectedTab = $("#tabtype li.active").attr("id");
    if (selectedTab == 'limittab') {
        CreateSmartTrade();
    }
});


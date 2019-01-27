$(function () {
    if (percentType !== "") {
        $(percentType).addClass('active');
    }
})
baseCurrencyTotalColor(baseAmount);


$(".percTypeBtn-limit").click(function () {
    $(".percTypeBtn-limit").removeClass("active");
    var percent = $(this).val();
    var baseAmountCalc = (baseTotal * percent).toFixed(baseDecimalAmount);
    $('#basetotal-limit').css('color', 'black');
    $('#basetotal-limit').val(baseAmountCalc);
    $('#amount-limit').val((baseAmountCalc / $('#lastprice-limit').val()).toFixed(assetDecimalAmount));
});

$('#lastprice-limit').on('change', function () {
    var currentBaseAmount = $('#basetotal-limit').val();
    $('#amount-limit').val((currentBaseAmount / $(this).val()).toFixed(assetDecimalAmount));
});


$('#basetotal-limit').on('change', function () {
    removeActiveButton();
    var inputBaseAmount = $(this).val();
    inputBaseAmount = (inputBaseAmount * 1).toFixed(baseDecimalAmount);
    $(this).val(inputBaseAmount);
    baseCurrencyTotalColor(inputBaseAmount);
    $('#amount-limit').val((inputBaseAmount / $('#lastprice-limit').val()).toFixed(assetDecimalAmount));
});

$('#amount-limit').on('change', function () {
    removeActiveButton();
    var inputAmount = $(this).val();
    var newCost = (inputAmount * lastPrice).toFixed(baseDecimalAmount);
    $('#basetotal-limit').val(newCost);
    var cba = $('#basetotal-limit').val();
    baseCurrencyTotalColor(cba);
});

function baseCurrencyTotalColor(currentBaseAmount) {
    if (currentBaseAmount > baseTotal) {
        $('#basetotal-limit').css('color', 'red');
    } else {
        $('#basetotal-limit').css('color', 'black');
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
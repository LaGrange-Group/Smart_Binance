$(function () {
    if (percentType !== "") {
        $(percentType).addClass('active');
    }
})
baseCurrencyTotalColor(baseAmount);
var decimalAmount = precision(parseFloat(amount));


$(".percTypeBtn-limit").click(function () {
    $(".percTypeBtn-limit").removeClass("active");
    var percent = $(this).val();
    var baseAmountCalc = baseTotal * percent;
    $('#basetotal-limit').css('color', 'black');
    $('#basetotal-limit').val(baseAmountCalc);
    $('#amount-limit').val((baseAmountCalc / $('#lastprice-limit').val()).toFixed(decimalAmount));
});

$('#lastprice-limit').on('change', function () {
    var currentBaseAmount = $('#basetotal-limit').val();
    $('#amount-limit').val((currentBaseAmount / $(this).val()).toFixed(decimalAmount));
});


$('#basetotal-limit').on('change', function () {
    removeActiveButton();
    var inputBaseAmount = $(this).val();
    baseCurrencyTotalColor(inputBaseAmount);
    $('#amount-limit').val((inputBaseAmount / $('#lastprice-limit').val()).toFixed(decimalAmount));
});

$('#amount-limit').on('change', function () {
    removeActiveButton();
    var inputAmount = $(this).val();
    var newCost = (inputAmount * lastPrice).toFixed(8);
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

function precision(a) {
    if (!isFinite(a)) return 0;
    var e = 1, p = 0;
    while (Math.round(a * e) / e !== a) { e *= 10; p++; }
    return p;
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
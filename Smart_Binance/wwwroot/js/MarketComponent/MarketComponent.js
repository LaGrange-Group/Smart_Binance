$(function () {
    if (percentType !== "") {
        $(percentType).addClass('active');
    }
})
baseCurrencyTotalColor(baseAmount);
var decimalAmount = precision(parseFloat(amount));


$(".percTypeBtn").click(function () {
    $(".percTypeBtn").removeClass("active");
    var percent = $(this).val();
    var baseAmountCalc = baseTotal * percent;
    $('#basetotal').css('color', 'black');
    $('#basetotal').val(baseAmountCalc);
    $('#amount').val((baseAmountCalc / lastPrice).toFixed(decimalAmount));
});

$('body').on('change', '#basetotal', function () {
    removeActiveButton();
    var inputBaseAmount = $(this).val();
    baseCurrencyTotalColor(inputBaseAmount);
    $('#amount').val((inputBaseAmount / lastPrice).toFixed(decimalAmount));
});

$('body').on('change', '#amount', function () {
    removeActiveButton();
    var inputAmount = $('#amount').val();
    var newCost = (inputAmount * lastPrice).toFixed(8);
    $('#basetotal').val(newCost);
    var cba = $('#basetotal').val();
    baseCurrencyTotalColor(cba);
});

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

function precision(a) {
    if (!isFinite(a)) return 0;
    var e = 1, p = 0;
    while (Math.round(a * e) / e !== a) { e *= 10; p++; }
    return p;
}
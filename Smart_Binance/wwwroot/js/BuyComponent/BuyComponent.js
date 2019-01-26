$(function () {
    $(percentType).addClass('active');
})

$(".percTypeBtn").click(function () {
    $(".percTypeBtn").removeClass("active");
    var percent = $(this).val();
    var baseAmountCalc = baseTotal * percent;
    $('#basetotal').val(baseAmountCalc);
    alert(countDecimals(amount));
    $('#amount').val(baseAmountCalc / lastPrice).toFixed(amount.countDecimals());
});

var countDecimals = function (value) {
    if (Math.floor(value) === value) return 0;
    return value.toString().split(".")[1].length || 0;
}
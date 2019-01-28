var slider = document.getElementById("myRange");
var output = document.getElementById("stocktakeprofitpercentbox");
var priceBox = document.getElementById("price-sell");
output.value = slider.value;
var priceDecimal = precision(initialPrice * 1);
alert(priceDecimal);

$('#stocktakeprofitpercentbox').on('input', function () {
    slider.value = this.value;
    var newPrice = (initialPrice * 1) + (initialPrice * (this.value / 100));
    $('#price-sell').val(newPrice.toFixed(priceDecimal));
});

slider.oninput = function () {
    output.value = this.value;
    var newPrice = (initialPrice * 1) + (initialPrice * (this.value / 100));
    $('#price-sell').val(newPrice.toFixed(priceDecimal));
}

priceBox.oninput = function () {
    var priceDiff = this.value - initialPrice;
    var percentGain = (priceDiff / initialPrice) * 100;
    output.value = percentGain.toFixed(2);
    alert(percentGain);
}

function precision(a) {
    if (!isFinite(a)) return 0;
    var e = 1, p = 0;
    while (Math.round(a * e) / e !== a) { e *= 10; p++; }
    return p;
}


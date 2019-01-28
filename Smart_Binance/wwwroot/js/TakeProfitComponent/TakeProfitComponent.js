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

function InitialTP() {
    var newPrice = (initialPrice * 1) + (initialPrice * (output.value / 100));
    priceBox.value = newPrice.toFixed(priceDecimal);
};

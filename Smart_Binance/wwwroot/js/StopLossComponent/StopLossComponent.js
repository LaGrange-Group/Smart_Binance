﻿var sliderStop = document.getElementById("myRange-stoploss");
var outputStop = document.getElementById("stockstoplosspercentbox");
var priceBoxStop = document.getElementById("price-stoploss");
var trailingStopBoolElement = document.getElementById("trailingstopbool");
outputStop.value = sliderStop.value;



outputStop.oninput = function () {
    sliderStop.value = this.value;
    var newPrice = (initialPrice * 1) + (initialPrice * (this.value / 100));
    priceBoxStop.value = newPrice.toFixed(priceDecimal);
    CheckConditionValuesSell($('#price-stoploss').val(), "stop");
}

sliderStop.oninput = function () {
    outputStop.value = this.value;
    var newPrice = (initialPrice * 1) + (initialPrice * (this.value / 100));
    priceBoxStop.value = newPrice.toFixed(priceDecimal);
    CheckConditionValuesSell($('#price-stoploss').val(), "stop");
}

$(priceBoxStop).on('change', function () {
    this.value = (this.value * 1).toFixed(priceDecimal);
    var priceDiff = this.value - initialPrice;
    var percentGain = (priceDiff / initialPrice) * 100;
    outputStop.value = percentGain.toFixed(2);
    CheckConditionValuesSell($('#price-stoploss').val(), "stop");
});

trailingStopBoolElement.onchange = function () {
    FlipTrailingStop();
}

function precision(a) {
    if (!isFinite(a)) return 0;
    var e = 1, p = 0;
    while (Math.round(a * e) / e !== a) { e *= 10; p++; }
    return p;
}

function InitialTPStop() {
    var newPrice = (initialPrice * 1) + (initialPrice * (outputStop.value / 100));
    priceBoxStop.value = newPrice.toFixed(priceDecimal);
};
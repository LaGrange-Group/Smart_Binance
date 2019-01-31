var amountSell = document.getElementById('amount-sell');
$('#button-basepercent-100-sell').addClass("active");
DisableCreateButton();

$(".percTypeBtn-sell").click(function () {
    $(".percTypeBtn-sell").removeClass("active");
    var percent = $(this).val();
    $(amountSell).val((amount * percent).toFixed(decimalAmount));
    CallCheckCondtionFunc();
});

$(amountSell).on('change', function () {
    removeActiveButton();
    if ($(this).val() > amount) {
        $(this).css('color', 'red');
    } else {
        $(this).css('color', 'black');
    }
    CallCheckCondtionFunc();
});



function removeActiveButton() {
    try {
        var percentBase = '';
        $('#PercentTypeGroup-sell .active').each(function () {
            percentBase = $(this).attr('id');
        });
        $('#' + percentBase).removeClass("active");
    } catch{

    }
}

$('#smarttradebutton').click(function () {
    var selectedTab = $("#tabtype li.active").attr("id");
    if (selectedTab == 'selltab') {
        CreateSmartTrade();
    }
});




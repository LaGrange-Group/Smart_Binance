
$('#button-basepercent-100-sell').addClass("active");

$(".percTypeBtn-sell").click(function () {
    $(".percTypeBtn-sell").removeClass("active");
    var percent = $(this).val();
    $('#amount-sell').val((amount * percent).toFixed(decimalAmount));
});

$('#amount-sell').on('change', function () {
    removeActiveButton();
    if ($(this).val() > amount) {
        $(this).css('color', 'red');
    } else {
        $(this).css('color', 'black');
    }
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




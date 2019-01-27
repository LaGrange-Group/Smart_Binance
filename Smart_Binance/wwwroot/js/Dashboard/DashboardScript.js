var marketContainer = $("#MarketContainer");  //quoteProcedureComponentContainer
var limitContainer = $("#LimitContainer");  //quoteProcedureComponentContainer
var refreshComponent = function (tab) {
    if (tab === "markettab") {
        var percentBase = '';
        $('#PercentTypeGroup .active').each(function () {
            percentBase = $(this).attr('id');
        });
        $.get("/Dashboard/GetMyMarketViewComponent", { marketPass: $("#MarketName").val(), percentTypePass: percentBase }, function (data) { marketContainer.html(data); });
    } else if (tab === "limittab") {
        var percentBase = '';
        $('#PercentTypeGroup-limit .active').each(function () {
            percentBase = $(this).attr('id');
        });
        $.get("/Dashboard/GetMyLimitViewComponent", { marketPass: $("#MarketName").val(), percentTypePass: percentBase }, function (data) { limitContainer.html(data); });
    }
    
};
$(function () {
    $("#MarketName").on('change', function () {
        //var selectedTab = $("#tabtype ul.nav-tabs li a.active");
        var selectedTab = $("#tabtype li.active").attr("id");
        refreshComponent(selectedTab);
    });
})

$('#tabtype li').click(function () {
    var selectedTab = $(this).attr('id');
    if ($('#MarketName').val() !== "----Select Market----" && selectedTab == "limittab") {
        var currentBaseAmount = $('#basetotal').val();
        var percentBase = '';
        $('#PercentTypeGroup .active').each(function () {
            percentBase = $(this).attr('id') + "-limit";
        });
        if (percentBase !== '') {
            currentBaseAmount = 0;
        }
        $.get("/Dashboard/SwitchToLimitViewComponent", { marketPass: $("#MarketName").val(), percentTypePass: percentBase, currentBasePass: currentBaseAmount }, function (data) { limitContainer.html(data); });
    } else if ($('#MarketName').val() !== "----Select Market----" && selectedTab == "markettab") {
        var currentBaseAmount = $('#basetotal-limit').val();
        var percentBase = '';
        $('#PercentTypeGroup-limit .active').each(function () {
            percentBase = $(this).attr('id') + "-limit";
        });
        if (percentBase !== '') {
            currentBaseAmount = 0;
        }
        $.get("/Dashboard/SwitchToMarketViewComponent", { marketPass: $("#MarketName").val(), percentTypePass: percentBase, currentBasePass: currentBaseAmount }, function (data) { marketContainer.html(data); });
    }
})

$(function () {
    $('#button-basepercent-25').addClass('active');
    $('#button-basepercent-25-limit').addClass('active');
})


$(".percTypeBtn-limit").click(function () {
    $(".percTypeBtn-limit").removeClass("active");
    var percent = $(this).val();
});

$(".percTypeBtn").click(function () {
    $(".percTypeBtn").removeClass("active");
    var percent = $(this).val();
});
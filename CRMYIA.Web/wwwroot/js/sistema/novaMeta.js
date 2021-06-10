$("#minValor").change(function () {
    var min = $("#minValor").val();
    var max = $("#maxValor").val();
    if (parseFloat(min) >= parseFloat(max)) {
        $("#maxValor").val("");
    }
});

$("#maxValor").change(function () {
    var min = $("#minValor").val();
    var max = $("#maxValor").val();
    if (parseFloat(min) >= parseFloat(max))  {
        $("#maxValor").val("");
    }
});

$("#minVidas").change(function () {
    var min = $("#minVidas").val();
    var max = $("#maxVidas").val();
    if (parseInt(min) >= parseInt(max)) {
        $("#maxVidas").val("");
    }
});

$("#maxVidas").change(function () {
    var min = $("#minVidas").val();
    var max = $("#maxVidas").val();
    if (parseInt(min) >= parseInt(max)) {
        $("#maxVidas").val("");
    }
});
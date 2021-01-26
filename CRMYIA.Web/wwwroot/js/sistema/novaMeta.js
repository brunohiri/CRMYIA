$("#DataMaxima").change(function () {
    var dataInicial = $("#DataMinima").val();
    var dataFinal = $("#DataMaxima").val();
    if (dataInicial > dataFinal) {
        $("#DataMaxima").val(dataInicial);
    }
});

$("#DataMinima").change(function () {
    var dataInicial = $("#DataMinima").val();
    var dataFinal = $("#DataMaxima").val();
    var parts = dataInicial.split("-");
    if (dataInicial > dataFinal) {
        $("#DataMaxima").val(dataInicial);
    }
    $("#MesValor").val(parts[1]);
    $("#AnoValor").val(parts[0]);
    $("#MesVida").val(parts[1]);
    $("#AnoVida").val(parts[0]);
});

$("#MesValor").change(function () {
    var parts = $("#DataMinima").val().split("-");
    var mes = $("#MesValor").val();
    if (mes < 1 || mes > 12) {
        if (parts != "")
            $("#MesValor").val(parts[1]);
        else
            $("#MesValor").val("");
    }
});

$("#minValor").change(function () {
    var min = $("#minValor").val();
    var max = $("#maxValor").val();

    alert(min)
    alert(max)
    if (parseFloat(min) >= parseFloat(max)) {
        $("#maxValor").val("");
    }
});

$("#maxValor").change(function () {
    var min = $("#minValor").val();
    var max = $("#maxValor").val();
    alert(min)
    alert(max)
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

$("#AnoValor").change(function () {
    var parts = $("#DataMinima").val().split("-");
    var ano = $("#AnoValor").val();
    if (ano.lengt != 4) {
        if (parts != "")
            $("#AnoValor").val(parts[0]);
        else
            $("#AnoValor").val("");
    }
});

$("#MesVida").change(function () {
    var parts = $("#DataMinima").val().split("-");
    var mes = $("#MesVida").val();
    if (mes < 1 || mes > 12) {
        if (parts != "")
            $("#MesVida").val(parts[1]);
        else
            $("#MesVida").val("");
    }
});

$("#AnoVida").change(function () {
    var parts = $("#DataMinima").val().split("-");
    var ano = $("#AnoVida").val();
    if (ano.lengt != 4) {
        if (parts != "")
            $("#AnoVida").val(parts[0]);
        else
            $("#AnoVida").val("");
    }
});

$("#DataMinima").change(function () {
    var dataInicial = $("#DataMinima").val();
    var dataFinal = $("#DataMaxima").val();
    if (dataInicial > dataFinal) {
        $("#DataMaxima").val(dataInicial);
    }
});
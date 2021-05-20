$(document).ready(function () {
    $('input:radio').change(function () {
        if ($("#radio1").prop("checked") == true) {
            $("#DataMinima").val("dd/mm/aaaa");
            $("#DtInicio").val("");
            $("#DtFinal").val("");
            $("#DataMaxima").val("dd/mm/aaaa");
            $("#DataMinima").prop("disabled", false);
            $("#DataMaxima").prop("disabled", false);
            $("#MesValor").val("");
            $("#AnoValor").val("");
            $("#MesVida").val("");
            $("#AnoVida").val("");
        }
        if ($("#radio2").prop("checked") == true) {
            var ter = getTuesdays();
            var seg = getMondays(1);
            if (verificar()) {
                $("#DataMinima").val(convertNormalDate(ter[0]));
                $("#DataMaxima").val(convertNormalDate(seg[0]));
                $("#DtInicio").val(convertNormalDate(ter[0]));
                $("#DtFinal").val(convertNormalDate(seg[0]));
                $("#DataMinima").prop("disabled", true);
                $("#DataMaxima").prop("disabled", true);
                $("#MesValor").val(ter[0].getMonth() + 1);
                $("#AnoValor").val(ter[0].getFullYear());
                $("#MesVida").val(ter[0].getMonth() + 1);
                $("#AnoVida").val(ter[0].getFullYear());
            } else
                alert("Verifique se data do seu computador está correta e recarregue a pagina!");

        }
        if ($("#radio3").prop("checked") == true) {
            var ter = getTuesdays();
            var seg = getMondays(3);
            if (verificar()) {
                $("#DataMinima").val(convertNormalDate(ter[0]));
                $("#DataMaxima").val(convertNormalDate(seg[0]));
                $("#DtInicio").val(convertNormalDate(ter[0]));
                $("#DtFinal").val(convertNormalDate(seg[0]));
                $("#DataMinima").prop("disabled", true);
                $("#DataMaxima").prop("disabled", true);
                $("#MesValor").val(ter[0].getMonth() + 1);
                $("#AnoValor").val(ter[0].getFullYear());
                $("#MesVida").val(ter[0].getMonth() + 1);
                $("#AnoVida").val(ter[0].getFullYear());
            } else
                alert("Verifique se data do seu computador está correta e recarregue a pagina!");
        }
        if ($("#radio4").prop("checked") == true) {
            var ter = getTuesdays();
            var seg = getMondays(6);
            if (verificar()) {
                $("#DataMinima").val(convertNormalDate(ter[0]));
                $("#DataMaxima").val(convertNormalDate(seg[0]));
                $("#DtInicio").val(convertNormalDate(ter[0]));
                $("#DtFinal").val(convertNormalDate(seg[0]));
                $("#DataMinima").prop("disabled", true);
                $("#DataMaxima").prop("disabled", true);
                $("#MesValor").val(ter[0].getMonth() + 1);
                $("#AnoValor").val(ter[0].getFullYear());
                $("#MesVida").val(ter[0].getMonth() + 1);
                $("#AnoVida").val(ter[0].getFullYear());
            } else
                alert("Verifique se data do seu computador está correta e recarregue a pagina!");
        }
    });
});

function verificar() {
    let mes = $("#Mes").val();
    var ter = getTuesdays();
    var m = ter[0].getMonth();
    if (mes != 1)
        mes = mes - 1;
    if (m === mes)
        return true;
    else
        return false;
}

function getTuesdays() {
    var d = new Date(),
        month = d.getMonth(),
        mondays = [];
    d.setDate(1);
    while (d.getDay() !== 2) {
        d.setDate(d.getDate() + 1);
    }
    while (d.getMonth() === month) {
        mondays.push(new Date(d.getTime()));
        d.setDate(d.getDate() + 7);
    }

    return mondays;
}

function getMondays(per) {
    var d = new Date(),
        month = d.getMonth(),
        tuesday = [];
    d.setDate(1);
    d.setMonth(d.getMonth() + per);
    while (d.getDay() !== 1) {
        d.setDate(d.getDate() + 1);
    }
    while (d.getMonth() === month + per) {
        tuesday.push(new Date(d.getTime()));
        d.setDate(d.getDate() + 7);
    }

    return tuesday;
}

function convertNormalDate(str) {
    var date = new Date(str),
        mnth = ("0" + (date.getMonth() + 1)).slice(-2),
        day = ("0" + date.getDate()).slice(-2);
    return [date.getFullYear(), mnth, day].join("-");
}
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

//$("#minValor").change(function () {
//    var min = $("#minValor").val();
//    var max = $("#maxValor").val();
//    if (parseFloat(min) >= parseFloat(max)) {
//        $("#maxValor").val("");
//    }
//});

//$("#maxValor").change(function () {
//    var min = parseFloat($("#minValor").val());
//    var max = parseFloat($("#maxValor").val());
//    if (min >= max) {
//        $("#maxValor").val("");
//    }
//});

//$("#minVidas").change(function () {
//    var min = $("#minVidas").val();
//    var max = $("#maxVidas").val();
//    if (parseInt(min) >= parseInt(max)) {
//        $("#maxVidas").val("");
//    }
//});

//$("#maxVidas").change(function () {
//    var min = $("#minVidas").val();
//    var max = $("#maxVidas").val();
//    if (parseInt(min) >= parseInt(max)) {
//        $("#maxVidas").val("");
//    }
//});

//$("#AnoValor").change(function () {
//    var parts = $("#DataMinima").val().split("-");
//    var ano = $("#AnoValor").val();
//    if (ano.lengt != 4) {
//        if (parts != "")
//            $("#AnoValor").val(parts[0]);
//        else
//            $("#AnoValor").val("");
//    }
//});

//$("#MesVida").change(function () {
//    var parts = $("#DataMinima").val().split("-");
//    var mes = $("#MesVida").val();
//    if (mes < 1 || mes > 12) {
//        if (parts != "")
//            $("#MesVida").val(parts[1]);
//        else
//            $("#MesVida").val("");
//    }
//});

//$("#AnoVida").change(function () {
//    var parts = $("#DataMinima").val().split("-");
//    var ano = $("#AnoVida").val();
//    if (ano.lengt != 4) {
//        if (parts != "")
//            $("#AnoVida").val(parts[0]);
//        else
//            $("#AnoVida").val("");
//    }
//});

//$("#DataMinima").change(function () {
//    var dataInicial = $("#DataMinima").val();
//    var dataFinal = $("#DataMaxima").val();
//    if (dataInicial > dataFinal) {
//        $("#DataMaxima").val(dataInicial);
//    }
//});
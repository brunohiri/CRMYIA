$(document).ready(function () {
    $('.sortable').sortable({
        connectWith: ".sortable",
        start: {},
        scroll: true,
        update: function (event, ui) {
        },
        change: function (event, ui) { },
        stop: function (event, ui) {
        },
        remove: function (event, ui) { }
    }).disableSelection();

    $('input[id="Data"]').daterangepicker({
        "locale": {
            "format": "DD/MM/YYYY",
            "separator": " - ",
            "applyLabel": "Aplicar",
            "cancelLabel": "Cancelar",
            "daysOfWeek": [
                "Dom",
                "Seg",
                "Ter",
                "Qua",
                "Qui",
                "Sex",
                "Sab"
            ],
            "monthNames": [
                "Janeiro",
                "Fevereiro",
                "Março",
                "Abril",
                "Maio",
                "Junho",
                "Julho",
                "Agosto",
                "Setembro",
                "Outubro",
                "Novembro",
                "Dezembro"
            ],
            "firstDay": 1
        }
    });


    $('.limpar-pesquisa').click(function () {
        location.reload();
    });

    //$('#btnPesquisar').click(function () {
    //    Pesquisa();
    //});
});
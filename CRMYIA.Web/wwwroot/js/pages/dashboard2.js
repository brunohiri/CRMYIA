$(function () {

    'use strict'

    /* ChartJS
     * -------
     * Here we will create a few charts using ChartJS
     */

    let d = new Date();
    let anoC = d.getFullYear();
    let mesC = d.getMonth();

    let DInicio = new Date(anoC, mesC, 1);
    let DFim = new Date(anoC, mesC + 1, 0);
    CarregarQuantificadores();
    $('#Data').val(GetDiaMesAno(DInicio) + ' - ' + GetDiaMesAno(DFim));
    $('#data-inicio').html(GetStringDiaMesAno(DInicio));
    $('#data-fim').html(GetStringDiaMesAno(DFim));
    CarregarProducao("", "");
    CarregarRankings();
    CarregarRankingUsuarioCorretoresAniversariantes();
    CarregarPropostasPendentes();


    

})

$(document).ready(function () {

    $('input[name="Data"]').daterangepicker({
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

    let d = new Date();
    let anoC = d.getFullYear();
    let mesC = d.getMonth();

    let DInicio = new Date(anoC, mesC, 1);
    let DFim = new Date(anoC, mesC + 1, 0);
});

$(document).on('click', '.limpar-pesquisa-dashboard', function () {

    let d = new Date();
    let anoC = d.getFullYear();
    let mesC = d.getMonth();

    let DInicio = new Date(anoC, mesC, 1);
    let DFim = new Date(anoC, mesC + 1, 0);
    CarregarProducao(GetDiaMesAno(DInicio), GetDiaMesAno(DFim));
    $("#Data").val(GetDiaMesAno(DInicio) + ' - ' + GetDiaMesAno(DFim));
    $('#data-inicio').html(GetStringDiaMesAno(DInicio));
    $('#data-fim').html(GetStringDiaMesAno(DFim));

});

$(document).on('click', '.exportar-pdf', function () {
    var Data = new Date();
    var diaM = Data.getDate();
    var mes = Data.getMonth();
    switch (mes) { 
        case 0: {
            mesF = mes + 1;
            mes = "Janeiro";
        }
            break;
        case 1: {
            mesF = mes + 1;
            mes = "Fevereiro";
        }
            break;
        case 2: {
            mesF = mes + 1;
            mes = "Março";
        }
            break;
        case 3: {
            mesF = mes + 1;
            mes = "Abril";
        }
            break;
        case 4: {
            mesF = mes + 1;
            mes = "Maio";
        }
            break;
        case 5: {
            mesF = mes + 1;
            mes = "Junho";
        }
            break;
        case 6: {
            mesF = mes + 1;
            mes = "Julho";
        }
            break;
        case 7: {
            mesF = mes + 1;
            mes = "Agosto";
        }
            break;
        case 8: {
            mesF = mes + 1;
            mes = "Setembro";
        }
            break;
        case 9: {
            mesF = mes + 1;
            mes = "Outubro";
        }
            break;
        case 10: {
            mesF = mes + 1;
            mes = "Novembro";
        }
            break;
        case 11: {
            mesF = mes + 1;
            mes = "Dezembro";
        }
            break;
    }

    if (diaM.toString().length == 1)
        diaM = "0" + diaM;

    var NomeArquivo = 'Dashboard_' + diaM + '_' + (Data.getMonth() + 1) + '_' + Data.getFullYear() + '_' + Data.getHours() + '_' + Data.getMinutes() + '_' + Data.getSeconds() + '.pdf'; //_dd_MM_yyyy_HH_mm_ss.pdf
    var options = {

    };
    var pdf = new jsPDF('p', 'pt', 'a4', true);
    pdf.addHTML($("#pdf-dow"), -1, -1, options, function () {
        pdf.save(NomeArquivo);
  	});
});

$(document).on('change', '.pesquisa-dashboard', function () {

    var d = new Date();
    var anoC = d.getFullYear();
    var mesC = d.getMonth();

    var DInicio = new Date(anoC, mesC, 1);
    var DFim = new Date(anoC, mesC + 1, 0);
    let Inicio = "";
    let Fim = "";

    GetDiaMesAno(DInicio);
    GetDiaMesAno(DFim);

    let vetData = $("#Data").val().split(' - ');
    if (vetData[0] == GetDiaAtual() && vetData[1] == GetDiaAtual()) {
        Inicio = GetDiaMesAno(DInicio);
        Fim = GetDiaMesAno(DFim);
    } else {
        Inicio = vetData[0];
        Fim = vetData[1];
    }

    CarregarProducao(Inicio, Fim);

    vetInicio = Inicio.split('/');
    vetFim = Fim.split('/');
    $('#data-inicio').html(GetStringData({ ano: vetInicio[2], mes: vetInicio[1], dia: vetInicio[0]} ));
    $('#data-fim').html(GetStringData({ ano: vetFim[2], mes: vetFim[1], dia: vetFim[0]} ));
});

function CarregarQuantificadores() {
    $.ajax({
        url: '/Index?handler=Quantificadores',
        cache: false,
        async: true,
        type: "GET",
        success: function (data) {
            if (data.status) {
                $('#ValorPropostasImplantadas').text(data.entityDashboard.valorPropostasImplantadas);
                $('#QtdOrcamentosEmNegociacao').text(data.entityDashboard.qtdOrcamentosEmNegociacao);
                $('#QtdCorretoresProduzindo').text(data.entityDashboard.qtdCorretoresProduzindo);
                $('#QtdCorretoresInativos').text(data.entityDashboard.qtdCorretoresInativos);


                //Corretores
                $('#QtdSegurosARenovar').text(data.entityDashboard.qtdSegurosARenovar);
                $('#QtdApolicesVigentes').text(data.entityDashboard.qtdApolicesVigentes);
            }
        }
    });
}

function CarregarProducao(Inicio, Fim) {
    $.ajax({
        url: '/Index?handler=Producao',
        data: { Inicio: Inicio, Fim: Fim },
        cache: false,
        async: true,
        type: "GET",
        success: function (data) {
            var result = '';
            var resultulOperadorasMaisVendidas = '';
            var resultulOperadoraMaisVendidasValor = '';
            if (data.status) {
                for (var i = 0; i < data.entityDashboard.operadorasMaisVendidas.length; i++) {
                    result += '<div class="progress-group">';
                    result += ' ' + data.entityDashboard.operadorasMaisVendidas[i].descricao + ' ';
                    result += '    <span class="float-right"><b>' + data.entityDashboard.operadorasMaisVendidas[i].valor + '</b></span>';
                    result += '    <div class="progress progress-sm">';
                    result += '        <div class="progress-bar" style="background-color:' + RetornarBackgroundColor(i) + ' !important;width: ' + (parseFloat(data.entityDashboard.operadorasMaisVendidas[i].valor.replace('R$', '').replaceAll('.', '').replaceAll(',', '.').trim())/2000000 * 100).toString() + '%"></div>';
                    result += '    </div>';
                    result += '</div>';
                    resultulOperadorasMaisVendidas += '<li><i class="far fa-circle ' + RetornarTextClass(i) + '"></i> ' + data.entityDashboard.operadorasMaisVendidas[i].descricao + '</li> ';

                    resultulOperadoraMaisVendidasValor += '<li class="nav-item">';
                    resultulOperadoraMaisVendidasValor += '  <a href = "#" class="nav-link"> ' + data.entityDashboard.operadorasMaisVendidas[i].descricao
                    resultulOperadoraMaisVendidasValor += '     <span class="float-right ' + RetornarTextClass(i) + '">';
                    resultulOperadoraMaisVendidasValor += '         <i class="fas fa-arrow-up text-sm"></i>' + data.entityDashboard.operadorasMaisVendidas[i].valor;
                    resultulOperadoraMaisVendidasValor += '     </span>';
                    resultulOperadoraMaisVendidasValor += '  </a>';
                    resultulOperadoraMaisVendidasValor += '</li>';
                }
                $('#DivProducaoMes').html(result);
                $('#ulOperadorasMaisVendidas').html(resultulOperadorasMaisVendidas);
                $('#ulOperadoraMaisVendidasValor').html(resultulOperadoraMaisVendidasValor);
                CarregarPieChart(data.entityDashboard);
                CarregarSalesChart(data.entityDashboard);

                $('#ValorSegurosFechados').text(data.entityDashboard.valorSegurosFechados);
                $('#ValorPlanoSaude').text(data.entityDashboard.valorPlanoSaude);
                $('#ValorMetaEstipulada').text(data.entityDashboard.valorMetaEstipulada);
                $('#QtdNegociosPerdidos').text(data.entityDashboard.qtdNegociosPerdidos);
            }
        }
    });
}

function CarregarRankings() {
    $.ajax({
        url: '/Index?handler=Rankings',
        cache: false,
        async: true,
        type: "GET",
        success: function (data) {
            var result = '';
            if (data.status) {
                result = '';
                if (data.entityDashboard.listRankingGerentes != null) {
                    if (data.entityDashboard.listRankingGerentes.length == 0) {
                        result += "<tr>";
                        result += "<td colspan=\"4\" style=\"text-align:center;\">Não há registros!</td>";
                        result += "</tr>";
                    }
                    else {
                        for (var i = 0; i < data.entityDashboard.listRankingGerentes.length; i++) {
                            result += '<tr>';
                            result += ' <td style="text-align:right;">' + (i + 1).toString() + 'º </td> ';
                            result += ' <td>' + data.entityDashboard.listRankingGerentes[i].nome + ' </td> ';
                            result += ' <td style="text-align:right;">' + data.entityDashboard.listRankingGerentes[i].valor + ' </td> ';
                            result += ' <td style="text-align:right;">' + data.entityDashboard.listRankingGerentes[i].numeroVidas + ' </td> ';
                            result += '</tr>';
                        }
                    }
                    $('#TBodyRankingGerentes').html(result);
                }

                if (data.entityDashboard.listRankingSupervisores != null) {
                    result = '';
                    if (data.entityDashboard.listRankingSupervisores.length == 0) {
                        result += "<tr>";
                        result += "<td colspan=\"4\" style=\"text-align:center;\">Não há registros!</td>";
                        result += "</tr>";
                    }
                    else {
                        for (var i = 0; i < data.entityDashboard.listRankingSupervisores.length; i++) {
                            result += '<tr>';
                            result += ' <td style="text-align:right;">' + (i + 1).toString() + 'º </td> ';
                            result += ' <td>' + data.entityDashboard.listRankingSupervisores[i].nome + ' </td> ';
                            result += ' <td style="text-align:right;">' + data.entityDashboard.listRankingSupervisores[i].valor + ' </td> ';
                            result += ' <td style="text-align:right;">' + data.entityDashboard.listRankingSupervisores[i].numeroVidas + ' </td> ';
                            result += '</tr>';
                        }
                    }
                    $('#TBodyRankingSupervisores').html(result);
                }

                if (data.entityDashboard.listRankingCorretores != null) {
                    result = '';
                    if (data.entityDashboard.listRankingCorretores.length == 0) {
                        result += "<tr>";
                        result += "<td colspan=\"4\" style=\"text-align:center;\">Não há registros!</td>";
                        result += "</tr>";
                    }
                    else {
                        for (var i = 0; i < data.entityDashboard.listRankingCorretores.length; i++) {
                            result += '<tr>';
                            result += ' <td style="text-align:right;">' + (i + 1).toString() + 'º </td> ';
                            result += ' <td>' + data.entityDashboard.listRankingCorretores[i].nome + ' </td> ';
                            result += ' <td style="text-align:right;">' + data.entityDashboard.listRankingCorretores[i].valor + ' </td> ';
                            result += ' <td style="text-align:right;">' + data.entityDashboard.listRankingCorretores[i].numeroVidas + ' </td> ';
                            result += '</tr>';
                        }
                    }
                    $('#TBodyRankingCorretores').html(result);
                }
            }
        }
    });
}

function CarregarRankingUsuarioCorretoresAniversariantes() {
    $.ajax({
        url: '/Index?handler=RankingUsuarioCorretoresAniversariantes',
        cache: false,
        async: true,
        type: "GET",
        success: function (data) {
            if (data.status) {
                $('#PosicaoRanking').text(data.entityDashboard.posicaoRanking + 'º');
                $('#CorretoresAniversariantesMes').text(data.entityDashboard.corretoresAniversariantesMes);

                $('#QuantidadeCorretoresCadastrados').text(data.entityDashboard.quantidadeCorretoresCadastrados);
                $('#ValorInadimplencia').text(data.entityDashboard.valorInadimplencia);
            }
        }
    });
}

function CarregarPropostasPendentes() {
    $.ajax({
        url: '/Index?handler=PropostasPendentes',
        cache: false,
        async: true,
        type: "GET",
        success: function (data) {
            var result = '';

            if (data.status) {        
                if (data.entityDashboard.propostasPendentes == null || data.entityDashboard.propostasPendentes.length == 0) {
                    result += "<tr>";
                    result += "<td colspan=\"5\" style=\"text-align:center;\">Não há registros!</td>";
                    result += "</tr>";
                }
                else {
                    for (var i = 0; i < data.entityDashboard.propostasPendentes.length; i++) {
                        result += "<tr>";
                        result += "    <td><a href=\"/NovaProposta?Id=" + data.entityDashboard.propostasPendentes[i].idPropostaCript + "\">" + data.entityDashboard.propostasPendentes[i].idProposta + "</a></td>";
                        result += "    <td>" + data.entityDashboard.propostasPendentes[i].operadora + "</td>";
                        result += "    <td>" + data.entityDashboard.propostasPendentes[i].supervisor + "</td>";
                        result += "    <td><span class=\"badge badge-warning\">" + data.entityDashboard.propostasPendentes[i].status + "</span></td>";
                        result += "    <td>" + data.entityDashboard.propostasPendentes[i].dataPendencia + "</td>";
                        result += "</tr>";

                    }
                }
                $('#tbodyPropostasPendentes').html(result);
            }
        }
    });
}

function CarregarPieChart(data) {
    //-------------
    //- PIE CHART -
    //-------------
    // Get context with jQuery - using jQuery's .get() method.

    var dataLabel = [];
    var datasetsData = [];
    var datasetColor = ['#3c8dbc', '#f56954', '#00a65a', '#f39c12', '#00c0ef', '#d2d6de'];
    for (var i = 0; i < data.operadorasMaisVendidas.length; i++) {
        dataLabel.push(data.operadorasMaisVendidas[i].descricao);
        datasetsData.push(data.operadorasMaisVendidas[i].valor.replace('R$', '').replaceAll('.', '').replaceAll(',', '.').trim());
    }


    var pieChartCanvas = $('#pieChart').get(0).getContext('2d')
    var pieData = {
        labels: dataLabel,
        datasets: [
            {
                data: datasetsData,
                backgroundColor: datasetColor,
            }
        ]
    }
    var pieOptions = {
        legend: {
            display: false
        }
    }
    //Create pie or douhnut chart
    // You can switch between pie and douhnut using the method below.
    var pieChart = new Chart(pieChartCanvas, {
        type: 'doughnut',
        data: pieData,
        options: pieOptions
    })

    //-----------------
    //- END PIE CHART -
    //-----------------
}

function CarregarSalesChart(data) {
    //-----------------------
    //- MONTHLY SALES CHART -
    //-----------------------

    // Get context with jQuery - using jQuery's .get() method.
    var salesChartCanvas = $('#salesChart').get(0).getContext('2d')

    var date = new Date();
    var firstDay = new Date(date.getFullYear(), date.getMonth(), 1);
    var lastDay = new Date(date.getFullYear(), date.getMonth() + 1, 0);

    var DiasAmil = [];
    var ValoresAmil = [];
    var DiasBradesco = [];
    var ValoresBradesco = [];
    var DiasGNDI = [];
    var ValoresGNDI = [];


    for (var i = 0; i < data.listValorProducaoPorDia.length; i++) {
        if (data.listValorProducaoPorDia[i].operadora == 'Amil') {
            DiasAmil.push(data.listValorProducaoPorDia[i].dia);
            ValoresAmil.push(data.listValorProducaoPorDia[i].valor);
        }
        else
            if (data.listValorProducaoPorDia[i].operadora == 'Bradesco Seguros') {
                DiasBradesco.push(data.listValorProducaoPorDia[i].dia);
                ValoresBradesco.push(data.listValorProducaoPorDia[i].valor);
            }
            else
                if (data.listValorProducaoPorDia[i].operadora == 'GNDI') {
                    DiasGNDI.push(data.listValorProducaoPorDia[i].dia);
                    ValoresGNDI.push(data.listValorProducaoPorDia[i].valor);
                }
    }

    console.log(data.listValorProducaoPorDia);

    var salesChartData = {
        labels: DiasAmil,
        datasets: [
            {
                label: 'GNDI',
                backgroundColor: RetornarBackgroundColor(0),
                borderColor: RetornarBackgroundColor(0),
                pointRadius: false,
                pointColor: RetornarBackgroundColor(0),
                pointStrokeColor: RetornarBackgroundColor(0),
                pointHighlightFill: '#fff',
                pointHighlightStroke: RetornarBackgroundColor(0),
                data: ValoresGNDI
            },
            {
                label: 'Bradesco Seguros',
                backgroundColor: RetornarBackgroundColor(1),
                borderColor: RetornarBackgroundColor(1),
                pointRadius: false,
                pointColor: RetornarBackgroundColor(1),
                pointStrokeColor: RetornarBackgroundColor(1),
                pointHighlightFill: '#fff',
                pointHighlightStroke: RetornarBackgroundColor(1),
                data: ValoresBradesco
            },
            {
                label: 'Amil',
                backgroundColor: RetornarBackgroundColor(2),
                borderColor: RetornarBackgroundColor(2),
                pointRadius: false,
                pointColor: RetornarBackgroundColor(2),
                pointStrokeColor: RetornarBackgroundColor(2),
                pointHighlightFill: '#fff',
                pointHighlightStroke: RetornarBackgroundColor(2),
                data: ValoresAmil
            }
        ]
    }

    var salesChartOptions = {
        maintainAspectRatio: false,
        responsive: true,
        legend: {
            display: false
        },
        scales: {
            xAxes: [{
                gridLines: {
                    display: false,
                }
            }],
            yAxes: [{
                gridLines: {
                    display: false,
                }
            }]
        }
    }

    // This will get the first returned node in the jQuery collection.
    var salesChart = new Chart(salesChartCanvas, {
        type: 'line',
        data: salesChartData,
        options: salesChartOptions
    }
    )

    //---------------------------
    //- END MONTHLY SALES CHART -
    //---------------------------
}

function RetornarTextClass(i) {
    var textClass = 'text-success';
    if (i == 0)
        textClass = 'text-danger';
    else
        if (i == 1)
            textClass = 'text-success';
        else
            if (i == 2)
                textClass = 'text-warning';
            else
                if (i == 3)
                    textClass = 'text-info';
                else
                    if (i == 4)
                        textClass = 'text-primary';
                    else
                        if (i == 5)
                            textClass = 'text-secondary';
    return textClass;
}

function RetornarBackgroundColor(i) {
    var datasetColor = ['#3c8dbc', '#f56954', '#00a65a', '#f39c12', '#00c0ef', '#d2d6de'];
    return datasetColor[i];
}
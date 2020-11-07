$(function () {

    'use strict'

    /* ChartJS
     * -------
     * Here we will create a few charts using ChartJS
     */


    CarregarQuantificadores();
    CarregarProducao();
    CarregarRankings();
    CarregarRankingUsuarioCorretoresAniversariantes();
    CarregarPropostasPendentes();

    //-----------------------
    //- MONTHLY SALES CHART -
    //-----------------------

    // Get context with jQuery - using jQuery's .get() method.
    var salesChartCanvas = $('#salesChart').get(0).getContext('2d')

    var salesChartData = {
        labels: ['January', 'February', 'March', 'April', 'May', 'June', 'July'],
        datasets: [
            {
                label: 'Digital Goods',
                backgroundColor: 'rgba(60,141,188,0.9)',
                borderColor: 'rgba(60,141,188,0.8)',
                pointRadius: false,
                pointColor: '#3b8bba',
                pointStrokeColor: 'rgba(60,141,188,1)',
                pointHighlightFill: '#fff',
                pointHighlightStroke: 'rgba(60,141,188,1)',
                data: [28, 48, 40, 19, 86, 27, 90]
            },
            {
                label: 'Electronics',
                backgroundColor: 'rgba(210, 214, 222, 1)',
                borderColor: 'rgba(210, 214, 222, 1)',
                pointRadius: false,
                pointColor: 'rgba(210, 214, 222, 1)',
                pointStrokeColor: '#c1c7d1',
                pointHighlightFill: '#fff',
                pointHighlightStroke: 'rgba(220,220,220,1)',
                data: [65, 59, 80, 81, 56, 55, 40]
            },
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



})


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

function CarregarProducao() {
    $.ajax({
        url: '/Index?handler=Producao',
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
                    result += '    <span class="float-right"><b>' + data.entityDashboard.operadorasMaisVendidas[i].quantidade + '</b></span>';
                    result += '    <div class="progress progress-sm">';
                    result += '        <div class="progress-bar bg-primary" style="width: ' + data.entityDashboard.operadorasMaisVendidas[i].quantidade + '%"></div>';
                    result += '    </div>';
                    result += '</div>';

                    resultulOperadorasMaisVendidas += '<li><i class="far fa-circle ' + RetornarTextClass(i) + '"></i> ' + data.entityDashboard.operadorasMaisVendidas[i].descricao + '</li> ';

                    resultulOperadoraMaisVendidasValor += '<li class="nav-item">';
                    resultulOperadoraMaisVendidasValor += '  <a href = "#" class="nav-link"> ' + data.entityDashboard.operadorasMaisVendidas[i].descricao
                    resultulOperadoraMaisVendidasValor += '     <span class="float-right ' + RetornarTextClass(i) + '">';
                    resultulOperadoraMaisVendidasValor += '         <i class="fas fa-arrow-up text-sm"></i>' + data.entityDashboard.operadorasMaisVendidas[i].quantidade;
                    resultulOperadoraMaisVendidasValor += '     </span>';
                    resultulOperadoraMaisVendidasValor += '  </a>';
                    resultulOperadoraMaisVendidasValor += '</li>';
                }
                $('#DivProducaoMes').html(result);
                $('#ulOperadorasMaisVendidas').html(resultulOperadorasMaisVendidas);
                $('#ulOperadoraMaisVendidasValor').html(resultulOperadoraMaisVendidasValor);
                CarregarPieChart(data.entityDashboard);



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
                if (data.entityDashboard.propostasPendentes.length == 0) {
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
    var datasetColor = ['#f56954', '#00a65a', '#f39c12', '#00c0ef', '#3c8dbc', '#d2d6de'];
    for (var i = 0; i < data.operadorasMaisVendidas.length; i++) {
        dataLabel.push(data.operadorasMaisVendidas[i].descricao);
        datasetsData.push(data.operadorasMaisVendidas[i].quantidade);
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
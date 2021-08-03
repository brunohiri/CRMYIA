
const formatter = new Intl.NumberFormat('pt-BR', {
    style: 'currency',
    currency: 'BRL'
});
var hashId = '';
var saltoSort1 = 20;
var saltoSort2 = 20;
var saltoSort3 = 20;
var saltoSort4 = 20;
var saltoSort5 = 20;
var saltoSort6 = 20;
var search = false;

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

    // Define o mês atual como valor default
    // let d = new Date();
    // $('#Data').val(`${new Date(d.getFullYear(), d.getMonth() - 3, 1).toLocaleDateString()} - ${new Date(d.getFullYear(), d.getMonth() + 1, 0).toLocaleDateString()}`);

    $('.limpar-pesquisa').click(function () {
        location.reload();
    });

    $('#btnPesquisar').click(function () {
        Pesquisa();
    });

    $('#btnRedirecionarCadastroLead').click(function () {
        location.href = "NovoCliente";
    });

    $('#gerenteMenuItems').change(function () {
        for (let id of ['supervisorMenuItems', 'corretorMenuItems']) {
            $(`#${id}`).attr('disabled', true);
            $(`#${id}`).empty();
            $(`#${id}`).append(new Option("Selecione...", null, null, true));
        }
        CarregarSlaves($(this).find(':selected'), 'supervisorMenuItems');
    });

    $('#supervisorMenuItems').change(function () {
        $('#corretorMenuItems').attr('disabled', true);
        $('#corretorMenuItems').empty();
        $('#corretorMenuItems').append(new Option("Selecione...", null, null, true));
        CarregarSlaves($(this).find(':selected'), 'corretorMenuItems');
    });

    CarregarOperadoras();
    //CarregarCorretores();
});
$("#sort1").on('scroll', function () {
    if (Math.round($(this).scrollTop() + $(this).innerHeight(), 10) >= Math.round($(this)[0].scrollHeight, 10)) {
        $(".loader").show(100);
        fase = $(this).data('statusId');
        Pesquisa(fase, saltoSort1, isSearch = false);
    }
});
$("#sort2").on('scroll', function () {
    if (Math.round($(this).scrollTop() + $(this).innerHeight(), 10) >= Math.round($(this)[0].scrollHeight, 10)) {
        $(".loader").show(100);
        fase = $(this).data('statusId');
        Pesquisa(fase, saltoSort2, isSearch = false)
    }
});
$("#sort3").on('scroll', function () {
    if (Math.round($(this).scrollTop() + $(this).innerHeight(), 10) >= Math.round($(this)[0].scrollHeight, 10)) {
        $(".loader").show(100);
        fase = $(this).data('statusId');
        Pesquisa(fase, saltoSort3, isSearch = false)
    }
});
$("#sort4").on('scroll', function () {
    if (Math.round($(this).scrollTop() + $(this).innerHeight(), 10) >= Math.round($(this)[0].scrollHeight, 10)) {
        $(".loader").show(100);
        fase = $(this).data('statusId');
        Pesquisa(fase, saltoSort4, isSearch = false)
    }
});
$("#sort5").on('scroll', function () {
    if (Math.round($(this).scrollTop() + $(this).innerHeight(), 10) >= Math.round($(this)[0].scrollHeight, 10)) {
        $(".loader").show(100);
        fase = $(this).data('statusId');
        Pesquisa(fase, saltoSort5, isSearch = false)
    }
});
$("#sort6").on('scroll', function () {
    if (Math.round($(this).scrollTop() + $(this).innerHeight(), 10) >= Math.round($(this)[0].scrollHeight, 10)) {
        $(".loader").show(100);
        fase = $(this).data('statusId');
        Pesquisa(fase, saltoSort6, isSearch = false)
    }
});
function BuscarFasesProposta(fase, salto) {
    toastr.info("Pesquisando...");
    let Data = $('#Data').val();
    var formData = new FormData();
    var d = new Date();
    var anoC = d.getFullYear();
    var mesC = d.getMonth();

    var DInicio = new Date(anoC, mesC, 1);
    var DFim = new Date(anoC, mesC + 1, 0);
    let Inicio = "";
    let Fim = "";
    let Descricao = "";
    let Nome = "";
    GetDiaMesAno(DInicio);
    GetDiaMesAno(DFim);

    let vetData = Data.split(' - ');
    if (vetData[0] == GetDiaAtual() && vetData[1] == GetDiaAtual()) {
        Inicio = GetDiaMesAno(DInicio);
        Fim = GetDiaMesAno(DFim);
    } else {
        Inicio = vetData[0];
        Fim = vetData[1] ? vetData[1] : "";
    }

    if ($('#operadoraMenuItems').val() != undefined && $('#operadoraMenuItems').val() != "Selecione...")
        Descricao = $('#operadoraMenuItems').val();

    if ($('#corretorMenuItems').val() != undefined && $('#corretorMenuItems').val() != "Selecione...")
        Nome = $('#corretorMenuItems').val();

    formData.append('Inicio', Inicio);
    formData.append('Fim', Fim);
    formData.append('Fase', fase);
    formData.append('Salto', salto);
    formData.append('Nome', Nome);
    formData.append('Descricao', Descricao);

    var res = $.ajax({
        type: "POST",
        url: "/Tarefa?handler=PesquisaTarefa",
        beforeSend: function (xhr) {
            xhr.setRequestHeader("XSRF-TOKEN",
                $('input:hidden[name="__RequestVerificationToken"]').val());
        },
        data: formData,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processData: false,
        contentType: false,
        success: function (result) {
            console.log(result)
            $("#periodoPesquisa").remove().html();
            $("#periodoPesquisa").append("Periodo: " + FormatarData(result.periodoA) + " até " + FormatarData(result.periodoB));
            if (result != null) {
                if (fase == 1 && result.status == true)
                    saltoSort1 = saltoSort1 + saltoSort1;
                if (fase == 2 && result.status == true)
                    saltoSort2 = saltoSort2 + saltoSort2;
                if (fase == 3 && result.status == true)
                    saltoSort3 = saltoSort3 + saltoSort3;
                if (fase == 4 && result.status == true)
                    saltoSort4 = saltoSort4 + saltoSort4;
                if (fase == 5 && result.status == true)
                    saltoSort5 = saltoSort5 + saltoSort5;
                if (fase == 6 && result.status == true)
                    saltoSort6 = saltoSort6 + saltoSort6;
                $(".loader").hide("fast", function () {
                    $(this).prev().hide("fast", arguments.callee);
                });
                if (result.propostas[0].length == 0)
                    toastr.warning("Nada encontrado!");
                else
                    toastr.success("Sucesso!");
                return result;
            }
        },
        failure: function (data) {
            console.log(response);
        }
    });
    AtualizarSortable(res);
}
function Pesquisa(fase = 0, salto = 0, isSearch = true) {
    toastr.info("Pesquisando...");
    search = isSearch;
    var formData = new FormData();
    let operadora, idGerente, idSupervisor, idCorretor, dataInicio, dataFim;

    operadora = $('#operadoraMenuItems').length && $('#operadoraMenuItems').val() != 'Selecione...' ? $('#operadoraMenuItems').val() : '';
    idGerente = $('#gerenteMenuItems').length && $('#gerenteMenuItems').val() != 'Selecione...' ? $('#gerenteMenuItems').find(':selected')[0].dataset.id : '';
    idSupervisor = $('#supervisorMenuItems').length && $('#supervisorMenuItems').val() != 'Selecione...' ? $('#supervisorMenuItems').find(':selected')[0].dataset.id : '';
    idCorretor = $('#corretorMenuItems').length && $('#corretorMenuItems').val() != 'Selecione...' ? $('#corretorMenuItems').find(':selected')[0].dataset.id : '';
    [dataInicio, dataFim] = $("#Data").val().includes(' - ') ? $("#Data").val().split(' - ') : [null, null];

    formData.append('operadora', operadora);
    formData.append('idGerente', idGerente);
    formData.append('idSupervisor', idSupervisor);
    formData.append('idCorretor', idCorretor);
    formData.append('dataInicio', dataInicio);
    formData.append('dataFim', dataFim);
    formData.append('fase', fase);
    formData.append('salto', salto)

    var res = $.ajax({
        type: "POST",
        url: "/Tarefa?handler=PesquisaPropostas",
        beforeSend: function (xhr) {
            xhr.setRequestHeader("XSRF-TOKEN",
                $('input:hidden[name="__RequestVerificationToken"]').val());
        },
        data: formData,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processData: false,
        contentType: false,
        success: function (result) {
            console.log(result);
            $("#periodoPesquisa").remove().html();
            $("#periodoPesquisa").append("Periodo: " + FormatarData(result.periodoA) + " até " + FormatarData(result.periodoB));
            if (result != null) {
                if (fase == 1 && result.status == true)
                    saltoSort1 = saltoSort1 + saltoSort1;
                if (fase == 2 && result.status == true)
                    saltoSort2 = saltoSort2 + saltoSort2;
                if (fase == 3 && result.status == true)
                    saltoSort3 = saltoSort3 + saltoSort3;
                if (fase == 4 && result.status == true)
                    saltoSort4 = saltoSort4 + saltoSort4;
                if (fase == 5 && result.status == true)
                    saltoSort5 = saltoSort5 + saltoSort5;
                if (fase == 6 && result.status == true)
                    saltoSort6 = saltoSort6 + saltoSort6;
                $(".loader").hide("fast", function () {
                    $(this).prev().hide("fast", arguments.callee);
                });

                result.propostas[0].length == 0 ? toastr.warning("Nada encontrado!") : toastr.success("Sucesso!");
                return result;
            }
        },
        failure: function (reason) {
            console.error(reason);
        }
    });

    AtualizarSortable(res);
}

function RedirecionarProposta(Id) {
    $.ajax({
        type: "GET",
        dataType: "json",
        url: '/Tarefa?handler=ObterHashId',
        data: { Id: Id },
        success: function (data) {
            window.open("/NovaProposta?id=" + data.hashId, "_blank");
        },
    });
}
function AtualizarSortable(resultado) {
    resultado.then(function (data) {
        let proximoContatoComCliente;
        let produto = '';
        let cliente = '';
        let corretor = '';
        let sortable = $('.sortable');
        if (search) {
            for (var k = 0; k < sortable.length; k++) {
                sortable[k].innerHTML = '';
            }
            saltoSort1 = 20;
            saltoSort2 = 20;
            saltoSort3 = 20;
            saltoSort4 = 20;
            saltoSort5 = 20;
            saltoSort6 = 20;
            search = false;
        }

        if (data.faseProposta != undefined) {
            if (data.fase > 0) {
                f = parseInt(data.fase) - 1;
                for (p in data.propostas[0]) {
                    proximoContatoComCliente = data.propostas[0][p].proximoContatoComCliente == undefined ? new Date(data.propostas[0][p].proximoContatoComCliente).toLocaleDateString('pt-br') : 'Não agendado';
                    produto = data.propostas[0][p].idCategoriaNavigation.idLinhaNavigation.idProdutoNavigation.descricao.length > 26 ? LimitaTexto(data.propostas[0][p].idCategoriaNavigation.idLinhaNavigation.idProdutoNavigation.descricao, 16) + '...' : data.propostas[0][p].idCategoriaNavigation.idLinhaNavigation.idProdutoNavigation.descricao;
                    cliente = data.propostas[0][p].idClienteNavigation.nome.length > 18 ? LimitaTexto(data.propostas[0][p].idClienteNavigation.nome, 16) : data.propostas[0][p].idClienteNavigation.nome;
                    corretor = data.propostas[0][p].idUsuarioCorretorNavigation.nome.length > 19 ? LimitaTexto(data.propostas[0][p].idUsuarioCorretorNavigation.nome, 16) : data.propostas[0][p].idUsuarioCorretorNavigation.nome;
                    html = '<li class="text-row ui-sortable-handle" data-task-id="' + data.propostas[0][p].idProposta + '" data-valorprevisto="' + data.propostas[0][p].valorPrevisto + '">\
                                        <a title="Ver Proposta" onclick=RedirecionarProposta(' + data.propostas[0][p].idProposta + ')>\
                                            <p style="margin-top:10px;" title="' + data.propostas[0][p].idCategoriaNavigation.idLinhaNavigation.idProdutoNavigation.descricao + '"><strong> ' + produto + ' </strong></p>\
                                            <p title="' + data.propostas[0][p].idClienteNavigation.nome + '"><strong>Cliente:</strong> ' + cliente + ' </p>\
                                            <p title="' + data.propostas[0][p].idUsuarioCorretorNavigation.nome + '"><strong>Corretor:</strong> ' + corretor + ' </p>\
                                            <p><strong>Valor Previsto:</strong>  <span id="ValorPrevisto_'+ data.faseProposta.idFaseProposta + "_" + data.propostas[0][p].idProposta + '"' + formatter.format(data.propostas[0][p].valorPrevisto) + ' "> ' + formatter.format(data.propostas[0][p].valorPrevisto) + '</p>\
                                            <p><strong>Data:</strong> ' + new Date(data.propostas[0][p].dataCadastro).toLocaleDateString('pt-br') + ' ' + new Date(data.propostas[0][p].dataCadastro).toLocaleTimeString('pt-br') + ' </p>\
                                            <p><strong>Retorno:</strong> ' + proximoContatoComCliente + ' </p>\
                                        </a>\
                                    </li>';
                    if (html != '')
                        $(sortable[f]).append(html).sortable({ connectWith: ".sortable" });
                }
            } else {
                for (i in data.faseProposta) {
                    for (j in data.propostas[i]) {
                        if (data.propostas[i][j].idFaseProposta == data.faseProposta[i].idFaseProposta) {
                            html = `<li class="text-row ui-sortable-handle" data-task-id="${data.propostas[i][j].idProposta}" data-valorprevisto="${data.propostas[i][j].valorPrevisto}">
                                        <a title="Ver Proposta" onclick=RedirecionarProposta(${data.propostas[i][j].idProposta})>
                                            <p class="text-truncate" style="margin-top:10px;" title="${data.propostas[i][j].idCategoriaNavigation.idLinhaNavigation.idProdutoNavigation.descricao}">
                                                <strong>${data.propostas[i][j].idCategoriaNavigation.idLinhaNavigation.idProdutoNavigation.descricao}</strong>
                                            </p>
                                            <hr />
                                            <p class="text-truncate" title="${data.propostas[i][j].idClienteNavigation.nome}">
                                                <strong>Cliente:</strong> ${data.propostas[i][j].idClienteNavigation.nome}
                                            </p>
                                            <p class="text-truncate" title="${data.propostas[i][j].idUsuarioCorretorNavigation == null ? "" : data.propostas[i][j].idUsuarioCorretorNavigation.nome}">
                                                <strong>Corretor:</strong> ${data.propostas[i][j].idUsuarioCorretorNavigation == null ? "" : data.propostas[i][j].idUsuarioCorretorNavigation.nome}
                                            </p>
                                            <p>
                                                <strong>Valor Previsto:</strong>  <span id="ValorPrevisto_${data.faseProposta.idFaseProposta}_${data.propostas[i][j].idProposta}">${formatter.format(data.propostas[i][j].valorPrevisto)}</pan>
                                            </p>
                                            <p><strong>Data:</strong> ${new Date(data.propostas[i][j].dataCadastro).toLocaleDateString('pt-br')} ${new Date(data.propostas[i][j].dataCadastro).toLocaleTimeString('pt-br')}</p>
                                            <p><strong>Retorno:</strong> ${data.propostas[i][j].proximoContatoComCliente == undefined ? new Date(data.propostas[i][j].proximoContatoComCliente).toLocaleDateString('pt-br') : 'Não agendado'}</p>
                                            ${data.propostas[i][j].idMotivoDeclinioLead ? '<p><strong>Motivo:</strong> ' + data.propostas[i][j].idMotivoDeclinioLeadNavigation.descricao + '</p>' : ''}
                                        </a>
                                    </li>`;
                        }
                        if (html != '')
                            $(sortable[i]).append(html).sortable({ connectWith: ".sortable" });
                    }
                }
            }
        }
        AtualizarCardsPropostas();
        AtualizarCardSomaPropostas();
    }, function () {
        console.log("Deu problema na requisição");
    });
}

function AtualizarCardsPropostas() {
    for (var ul = 0; ul < $('ul[id*="sort"]').length; ul++) {
        if ($('ul[id*="sort"]').eq(ul).find('li').length == 0) {
            $('ul[id*="sort"]').eq(ul).html('<li class="text-row-empty div-blocked" data-task-id="0">Nenhuma Proposta</li>');
        }
    }
}

function AtualizarCardSomaPropostas() {
    var soma = 0;
    for (var i = 1; i <= 6; i++) {
        soma = 0;
        for (var j = 0; j < $('#sort' + i + ' li a p span[id*="ValorPrevisto"]').length; j++) {
            var ValorPrevisto = $('#sort' + i + ' li a p span[id*="ValorPrevisto"]')[j];
            soma += parseFloat(ValorPrevisto.innerText.replace('R$', '').replaceAll('.', '').replaceAll(',', '.').trim());
        }
        $('#total-' + i).html(soma.toLocaleString("pt-BR", { style: "currency", currency: "BRL" }));
    }
}

function SomarTotalFaseProposta(data, codicao) {
    let array = 0;
    array = BuscarTodosValorPrevisto(data, codicao);
    let retorno;
    retorno = array.reduce(function (accumulator, item, index, array) {
        return (item + accumulator);
    }, 0);
    retorno == undefined ? retorno = formatter.format(0.00) : retorno = formatter.format(retorno);
    return retorno;
}

function BuscarTodosValorPrevisto(data, condicao) {
    let retorno = [];
    data.reduce(function (accumulator, item, index, array) {

        if (item.idFaseProposta == condicao) {

            if (isNaN(accumulator)) {
                retorno[index] = (0 + item.valorPrevisto);
                return accumulator;
            } else {
                retorno[index] = (accumulator + item.valorPrevisto);
                return accumulator;
            }
        }

    }, 0.00);
    console.log(retorno);
    return retorno;
}
function CadastroTarefas() {
    $('ul[id^="sort"]').sortable(
        {
            connectWith: ".sortable",
            receive: function (e, ui) {
                var status_id = $(ui.item).parent(".sortable").data("status-id");
                var task_id = $(ui.item).data("task-id");
                var declinioLeadId = '';

                if (status_id === 6) {
                    $('#modalMotivos').modal('show');

                    $('#motivoMenuItem').change(function () {
                        declinioLeadId = $('#motivoMenuItem').length && $('#motivoMenuItem').val() != 'Selecione...' ? $('#motivoMenuItem').select2('data')[0].id : '';
                    });

                    $('#btnSelecionarMotivoDesistencia').click(function () {
                        $('#modalMotivos').modal('hide');
                        $.ajax({
                            url: `/Tarefa?handler=Edit&statusId=${status_id}&taskId=${task_id}&declinioLeadId=${declinioLeadId}`,
                            success: function (data) {
                                if (data.status) {
                                    toastr.success("Sucesso!");
                                    for (var i = 0; i < $('#sort' + status_id + ' li').length; i++) {
                                        if ($('#sort' + status_id + ' li').eq(i).data('task-id') == "0") {
                                            $('#sort' + status_id + ' li').eq(i).remove();
                                        }
                                    }
                                    AtualizarCardsPropostas();
                                    AtualizarCardSomaPropostas();
                                }
                            }
                        });
                    });
                } else {
                    $.ajax({
                        url: `/Tarefa?handler=Edit&statusId=${status_id}&taskId=${task_id}`,
                        success: function (data) {
                            if (data.status) {
                                toastr.success("Sucesso!");
                                for (var i = 0; i < $('#sort' + status_id + ' li').length; i++) {
                                    if ($('#sort' + status_id + ' li').eq(i).data('task-id') == "0") {
                                        $('#sort' + status_id + ' li').eq(i).remove();
                                    }
                                }
                                AtualizarCardsPropostas();
                                AtualizarCardSomaPropostas();
                            }
                        }
                    });
                }
            }
        }).disableSelection();
}
function CarregarOperadoras() {
    $.getJSON("/Tarefa?handler=TodasOperadoras", function (data) {
        let contents = [];
        let i = 0;
        for (let name of data.operadora) {
            //contents.push('<input type="button" class="dropdown-item visitas-pesquisa-dropdown" role="option" aria-selected="true" type="button" value="' + name.nome.toUpperCase() + '"/>');
            contents.push('<option value="' + name.descricao.toUpperCase() + '">' + name.descricao.toUpperCase() + '</option>');
            i++;
        }
        $('#operadoraMenuItems').append(contents.join(""));

        //Esconder a linha que mostra que nenhum item foi encontrado
        $('#empty').hide();
    });
}

function CarregarCorretores() {
    $.getJSON("/Tarefa?handler=TodasCorretores", function (data) {
        let contents = [];
        let i = 0;
        for (let name of data.corretora) {
            //contents.push('<input type="button" class="dropdown-item visitas-pesquisa-dropdown" role="option" aria-selected="true" type="button" value="' + name.nome.toUpperCase() + '"/>');
            contents.push('<option value="' + name.nome.toUpperCase() + '">' + name.nome.toUpperCase() + '</option>');
            i++;
        }
        $('#corretorMenuItems').append(contents.join(""));

        //Esconder a linha que mostra que nenhum item foi encontrado
        $('#empty').hide();
    });
}

function CarregarSlaves(idMaster, idElement) {
    console.log(idMaster);
    $.ajax({
        type: "GET",
        dataType: "json",
        url: '/Tarefa?handler=UsuariosSlave',
        data: { IdMaster: idMaster[0].dataset.id },
        beforeSend: function () {
            $(`#${idElement}`).attr('disabled', true);
            $(`#${idElement}`).empty();
            $(`#${idElement}`).append(new Option("Carregando...", null, null, true));
        },
        success: function (data) {
            $(`#${idElement}`).empty();
            $(`#${idElement}`).append(new Option("Selecione...", null, null, true));
            
            for (let slave of data.result) {
                $(`#${idElement}`).append(`<option value="${slave.nome}" data-id="${slave.idUsuario}">${slave.nome.toUpperCase()}</option>`);
            }

            if (data.result.length > 0)
                $(`#${idElement}`).attr('disabled', false);
        },
    });
}

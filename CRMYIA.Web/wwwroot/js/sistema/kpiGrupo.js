
const formatter = new Intl.NumberFormat('pt-BR', {
    style: 'currency',
    currency: 'BRL'
});
var hashId = '';
$(document).ready(function () {
    
    $('.sortable').sortable({
        connectWith: ".sortable",
        start: {},
        update: function (event, ui) {},
        change: function (event, ui) {},
        stop: function (event, ui) {
            BuscarFasesProposta();
        },
        remove: function (event, ui) {}
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

    let d = new Date();
    let anoC = d.getFullYear();
    let mesC = d.getMonth();

    let DInicio = new Date(anoC, mesC, 1);
    let DFim = new Date(anoC, mesC + 1, 0);

    $('#Data').val('');
    
    $('.limpar-pesquisa').click(function () {
        var d = new Date();
        var anoC = d.getFullYear();
        var mesC = d.getMonth();

        var DInicio = new Date(anoC, mesC, 1);
        var DFim = new Date(anoC, mesC + 1, 0);

        $('#Data').val('');
        $('#corretorMenuItems').val('');
        $('#operadoraMenuItems').val('');

        $("#corretorMenuItems").select2('val', 'Selecione...');
        $("#operadoraMenuItems").select2('val', 'Selecione...');
        Pesquisa();
    });

    $('.pesquisa-tarefa').change(function () {
        Pesquisa();
    });

    CarregarOperadoras();
    CarregarCorretores();

});

function Pesquisa() {
    let Data = $('#Data').val();

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
    let Descricao = "";
    let Nome = "";
    if ($('#operadoraMenuItems').val() == undefined || $('#operadoraMenuItems').val() == "Selecione...")
        Descricao = "";
    else
        Descricao = $('#operadoraMenuItems').val();

    if ($('#corretorMenuItems').val() == undefined || $('#corretorMenuItems').val() == "Selecione...")
        Nome = "";
    else
        Nome = $('#corretorMenuItems').val();
    

    var res = $.ajax({
        url: '/Tarefa?handler=PesquisaTarefa',
        method: "GET",
        data: { Nome: Nome, Descricao: Descricao, Inicio: Inicio, Fim: Fim },
        dataType: 'json',
        success: function (data) {
            return data;
        },
        error: function () {
        }
    });

    AtualizarSortable(res);
}

function RedirecionarProposta(Id)  {
    $.ajax({
        type: "GET",
        dataType: "json",
        url: '/Tarefa?handler=ObterHashId',
        data: { Id: Id },
        success: function (data) {
            window.open("/NovaProposta?id=" + data.hashId , "_blank");
        },

    });

}


function AtualizarSortable(resultado) {
    resultado.then(function (data) {
        let html = '';
        let proximoContatoComCliente 
        let naoAgendado = 'Não agendado';
        let produto = '';
        let cliente = '';
        let corretor = '';
        let sortable = $('.sortable'); 
        for (var k = 0; k < sortable.length; k++) {
            sortable[k].innerHTML = '';
        }
        if (data.faseProposta != undefined) {
            for (i in data.faseProposta) {
                for (j in data.proposta) {
                   
                    if (data.proposta[j].idFaseProposta == data.faseProposta[i].idFaseProposta) {
                        proximoContatoComCliente = data.proposta[j].proximoContatoComCliente == undefined ? new Date(data.proposta[j].proximoContatoComCliente).toLocaleDateString('pt-br') : naoAgendado;
                        produto = data.proposta[j].idCategoriaNavigation.idLinhaNavigation.idProdutoNavigation.descricao.length > 26 ? LimitaTexto(data.proposta[j].idCategoriaNavigation.idLinhaNavigation.idProdutoNavigation.descricao, 16) + '...' : data.proposta[j].idCategoriaNavigation.idLinhaNavigation.idProdutoNavigation.descricao;
                        cliente = data.proposta[j].idClienteNavigation.nome.length > 18 ? LimitaTexto(data.proposta[j].idClienteNavigation.nome, 16) : data.proposta[j].idClienteNavigation.nome;
                        corretor = data.proposta[j].idUsuarioCorretorNavigation.nome.length > 19 ? LimitaTexto(data.proposta[j].idUsuarioCorretorNavigation.nome, 16) : data.proposta[j].idUsuarioCorretorNavigation.nome;
                        html = '<li class="text-row ui-sortable-handle" data-task-id="' + data.proposta[j].idProposta + '" data-valorprevisto="' + data.proposta[j].valorPrevisto + '">\
                                        <a title="Ver Proposta" onclick=RedirecionarProposta(' + data.proposta[j].idProposta + ')>\
                                            <p style="margin-top:10px;" title="' + data.proposta[j].idCategoriaNavigation.idLinhaNavigation.idProdutoNavigation.descricao + '"><strong> ' + produto + ' </strong></p>\
                                            <p title="' + data.proposta[j].idClienteNavigation.nome + '"><strong>Cliente:</strong> ' + cliente + ' </p>\
                                            <p title="' + data.proposta[j].idUsuarioCorretorNavigation.nome + '"><strong>Corretor:</strong> ' + corretor + ' </p>\
                                            <p><strong>Valor Previsto:</strong>  <span id="ValorPrevisto_'+ data.faseProposta.idFaseProposta + "_" + data.proposta[j].idProposta + '"' + formatter.format(data.proposta[j].valorPrevisto) + ' "> ' + formatter.format(data.proposta[j].valorPrevisto) + '</p>\
                                            <p><strong>Data:</strong> ' + new Date(data.proposta[j].dataCadastro).toLocaleDateString('pt-br') + ' ' + new Date(data.proposta[j].dataCadastro).toLocaleTimeString('pt-br') + ' </p>\
                                            <p><strong>Retorno:</strong> ' + proximoContatoComCliente + ' </p>\
                                        </a>\
                                    </li>';
                        
                    }
                    if(html != '')
                        $(sortable[i]).append(html).sortable({ connectWith: ".sortable" });//$("[href$='hashId']").data('url')
                    html = '';
                }
               
            }
        }
        AtualizarCardsPropostas();
        AtualizarCardSomaPropostas();
    }, function () {
        console.log("Deu problema na requisição");
    });

    function AtualizarCardsPropostas() {
        for (var ul = 0; ul < $('ul[id*="sort"]').length; ul++) {
            if ($('ul[id*="sort"]').eq(ul).find('li').length == 0) {
                $('ul[id*="sort"]').eq(ul).html('<li class="text-row-empty div-blocked" data-task-id="0">Nenhuma Proposta</li>');
            }
        }
    }

    function AtualizarCardSomaPropostas() {
        var soma = 0;
        for (var i = 0; i < 6; i++) {
            soma = 0;
            for (var j = 0; j < $('#sort' + i + ' li a p span[id*="ValorPrevisto"]').length; j++) {
                var ValorPrevisto = $('#sort' + i + ' li a p span[id*="ValorPrevisto"]')[j];
                soma += parseFloat(ValorPrevisto.innerText.replace('R$', '').replaceAll('.', '').replaceAll(',', '.').trim());
            }
            $('#total-' + i).html(soma.toLocaleString("pt-BR", { style: "currency", currency: "BRL" }));
        }
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



function BuscarFasesProposta() {
    var resultado = $.ajax({
        url: '/Tarefa?handler=BuscarFasesProposta',
        method: "GET",
        dataType: 'json',
        success: function (data) {
            return data;
        },
        error: function () {
        }
    });
    //AtualizarSortable(resultado);
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

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
    });       
});

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
    AtualizarSortable(resultado);
}

function AtualizarSortable(resultado) {
    resultado.then(function (data) {
        let html = '';
        $('.task-board').html('');
        if (data.faseProposta != undefined) {
            for (i in data.faseProposta) {
                html += '<div class="status-card">\
                            <div class="card-header">\
                                <span class="card-header-text">\
                                    '+ data.faseProposta[i].descricao + '\
                                <br>\
                                <div id="total-1">' + SomarTotalFaseProposta(data.proposta, data.faseProposta[i].idFaseProposta) + '</div>\
                                </span>\
                            </div>\
                            <ul class="sortable ui-sortable" id="sort' + data.faseProposta[i].idFaseProposta + '" data-status-id="' + data.faseProposta[i].idFaseProposta + '" data-total="1500,00">';

                for (j in data.proposta) {
                    hashId = ObterHashId(data.proposta[j].idProposta, RetornoObterHashId);
                    if (data.proposta[j].idFaseProposta == data.faseProposta[i].idFaseProposta) {
                        html += '<li class="text-row ui-sortable-handle" data-task-id="' + data.proposta[j].idProposta + '" data-valorprevisto="' + data.proposta[j].valorPrevisto + '">\
                                        <a title="Ver Proposta" href="/NovaProposta?id=' + hashId + '">\
                                            <p style="margin-top:10px;" title="' + data.proposta[j].idCategoriaNavigation.idLinhaNavigation.idProdutoNavigation.descricao + '"><strong>' + LimitaTexto(data.proposta[j].idCategoriaNavigation.idLinhaNavigation.idProdutoNavigation.descricao, 16) + '</strong></p>\
                                            <p title="' + data.proposta[j].idClienteNavigation.nome + '"><strong>Cliente:</strong>' + LimitaTexto(data.proposta[j].idClienteNavigation.nome, 16) + '</p>\
                                            <p title="' + data.proposta[j].idUsuarioCorretorNavigation.nome + '"><strong>Corretor:</strong> ' + LimitaTexto(data.proposta[j].idUsuarioCorretorNavigation.nome, 16) + '</p>\
                                            <p><strong>Valor Previsto:</strong> ' + formatter.format(data.proposta[j].valorPrevisto) + '</p>\
                                            <p><strong>Data:</strong>' + new Date(data.proposta[j].dataCadastro).toLocaleDateString('pt-br') + ' ' + new Date(data.proposta[j].dataCadastro).toLocaleTimeString('pt-br') + '</p>\
                                            <p><strong>Retorno:</strong> Não agendado</p>\
                                        </a>\
                                    </li>';
                    }
                }
                html += '</ul>\
                                   </div>';
            }
        }
        $('.task-board').html(html);
        CadastroTarefas();
    }, function () {
        console.log("Deu problema na requisição");
    });
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

function ObterHashId(Id, callback) {
    $.ajax({
        type: "GET",
        dataType: "json",
        url: '/Tarefa?handler=ObterHashId',
        data: { Id: Id },
        success: function (data) {
            var resultado = '';
            resultado = data.hashId;
            callback(resultado);
        },
       
    });

}

function RetornoObterHashId(Id) {
    hashId = Id;
}

const formatter = new Intl.NumberFormat('pt-BR', {
    style: 'currency',
    currency: 'BRL'
});
var hashId = '';
$(document).ready(function () {
    SalvarKPIGrupo();
    CadastroGrupos();

    $('.sortable').sortable({
        connectWith: ".sortable",
        start: {},
        update: function (event, ui) { },
        change: function (event, ui) { },
        stop: function (event, ui) {

        },
        remove: function (event, ui) { }
    }).disableSelection();
});



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
                    if (html != '')
                        $(sortable[i]).append(html).sortable({ connectWith: ".sortable" });//$("[href$='hashId']").data('url')
                    html = '';
                }

            }
        }
        AtualizarCardsKPIGrupo();
    }, function () {
        console.log("Deu problema na requisição");
    });

    function AtualizarCardsKPIGrupo() {
        for (var ul = 0; ul < $('ul[id*="sort"]').length; ul++) {
            if ($('ul[id*="sort"]').eq(ul).find('li').length == 0) {
                $('ul[id*="sort"]').eq(ul).html('<li class="text-row-empty div-blocked" data-task-id="0">Nenhum usuário atribuido</li>');
            }
        }
    }
}

function SalvarKPIGrupo() {
    $('#ButtonSalvarKPIGrupo').on('click', function (evt) {
        evt.preventDefault();
        $.post('/KPIGrupo?handler=KPIGrupo', $('#formKPIGrupo').serialize(), function (data) {
            $('#KPIGrupoMensagemDiv').show();
            $('#KPIGrupoMensagemDivAlert').removeClass();
            $('#KPIGrupoMensagemDivAlert').addClass(data.mensagem.cssClass);

            $('#KPIGrupoMensagemIcon').removeClass();
            $('#KPIGrupoMensagemIcon').addClass(data.mensagem.iconClass);
            $('#KPIGrupoMensagemSpan').text(data.mensagem.mensagem);
        });
        AtualizarCardsKPIGrupo();
    });
}

function CadastroGrupos() {
    $('ul[id^="sort"]').sortable(
        {
            connectWith: ".sortable",
            receive: function (e, ui) {
                var grupo_id = $(ui.item).parent(".sortable").data(
                    "grupo-id");
                console.log(grupo_id)
                var usuario_id = $(ui.item).data("user-id");

                console.log(ui.item)
                $.ajax({
                    url: '/KPIGrupo?handler=Edit&grupoId=' + grupo_id + '&usuarioId=' + usuario_id,
                    success: function (data) {
                        if (data.status) {
                            for (var i = 0; i < $('#sort' + grupo_id + ' li').length; i++) {
                                if ($('#sort' + grupo_id + ' li').eq(i).data('user-id') == "0") {
                                    $('#sort' + grupo_id + ' li').eq(i).remove();
                                }
                            }
                            AtualizarCardsKPIGrupo();
                        }
                    }
                });
            }

        }).disableSelection();

    function AtualizarCardsKPIGrupo() {
        for (var ul = 0; ul < $('ul[id*="sort"]').length; ul++) {
            if ($('ul[id*="sort"]').eq(ul).find('li').length == 0) {
                $('ul[id*="sort"]').eq(ul).html('<li class="text-row-empty div-blocked" data-task-id="0">Nenhum usuário atribuido</li>');
            }
        }
    }
}
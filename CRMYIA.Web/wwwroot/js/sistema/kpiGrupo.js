const formatter = new Intl.NumberFormat('pt-BR', {
    style: 'currency',
    currency: 'BRL'
});
var hashId = '';
var start = 20;

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

$('.TODO').on('scroll', function () {
    if (Math.round($(this).scrollTop() + $(this).innerHeight(), 10) >= Math.round($(this)[0].scrollHeight, 10)) {
        if ($("#searchKPIGrupo").val().length > 0) {
            CarregarCardsTODO();
        } else {
            var text = "";
            var formData = new FormData();
            formData.append('start', start);
            if ($("#IdPerfil").children("option:selected").val() == 3) {
                $(".loader").show(100);
                $.ajax({
                    type: "POST",
                    url: "/KPIGrupo?handler=Corretores",
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
                        $("#IdPerfil option:contains(" + 'Corretor' + ")").attr('selected', true)
                        $.each(result.listPerfil, function (i, d) {
                            text += '<li class="text-row ui-sortable-handle" data-user-id="' + this.idUsuario + '" id="id-"' + this.idUsuario + '">' +
                                '<div class="card card-color" style="position: relative; left: 0px; top: 0px;">' +
                                '<div class="card-header border-0 ui-sortable-handle" style="cursor: move;">' +
                                '<h3 class="card-title">' +
                                '<i class="fas fa-th mr-1"></i>' + this.nome.substr(0, 19) +
                                '</h3>' +
                                '<div class="card-tools mr-2 mb-1">' +
                                '<button type="button" class="btn btn-outline-light btn-sm" data-card-widget="collapse">' +
                                '<i class="fas fa-minus"></i>' +
                                '</button>' +
                                '<button type="button" class="btn btn-outline-light btn-sm" data-card-widget="remove">' +
                                '<i class="fas fa-times"></i>' +
                                '</button>' +
                                '</div>' +
                                '</div>' +
                                '<div class="card-body p-0 mb-0" style="display: block;">' +
                                '<div class="card card-widget widget-user mb-0">' +
                                '<div class="widget-user-header card-color">' +
                                '<h3 class="widget-user-username">' + this.descricaoPerfil + '</h3>' +
                                '</div>' +
                                '<div class="widget-user-image">' +
                                '<img class="img-circle elevation-2" src="' + this.caminhoFoto + this.nomeFoto + '" alt="' + this.nomeFoto + '">' +
                                '</div>' +
                                '<div class="card-footer bg-gradient-light">' +
                                '<div class="row">' +
                                '<div class="col-sm-4 border-right">' +
                                '<div class="description-block">' +
                                '<h5 class="description-header">0</h5>' +
                                '<span class="description-text">Realizado</span>' +
                                '</div>' +
                                '</div>' +
                                '<div class="col-sm-4 border-right">' +
                                '<div class="description-block">' +
                                '<h5 class="description-header">0</h5>' +
                                '<span class="description-text">Meta</span>' +
                                '</div>' +
                                '</div>' +
                                '<div class="col-sm-4">' +
                                '<div class="description-block">' +
                                '<h5 class="description-header">0</h5>' +
                                '<span class="description-text">Vidas</span>' +
                                '</div>' +
                                '</div>' +
                                '</div>' +
                                '</div>' +
                                '</div>' +
                                '</div>' +
                                '</div>' +
                                '</li>';
                        });
                        start += start;
                        $(".loader").hide("fast", function () {
                            $(this).prev().hide("fast", arguments.callee);
                        });
                        $('.TODO').append(text);
                    },
                    failure: function (data) {
                        console.log(response);
                    }
                });
            }
        }
    }
});
var ht = '';
var startSearch = 0;
var termoAnterior = "";
$("#btnSearchKPIGrupo").click(function () {
    CarregarCardsTODO();
});
function CarregarCardsTODO() {
    var text = "";
    var perfil = "";
    var termo = "";
    var take = 100;
    var formData = new FormData();
    if (startSearch == 0)
        ht = $('.TODO').html();
    termo = $("#searchKPIGrupo").val();
    if (termoAnterior != "") {
        if (termo != termoAnterior) {
            termoAnterior = termo;
            startSearch = 0;
        }
    }
    perfil = $("#IdPerfil").children("option:selected").val();
    if (termo.length > 4) {
        formData.append("termo", termo);
        formData.append("start", startSearch);
        formData.append("perfil", perfil);
        formData.append("take", take);

        $.ajax({
            type: "POST",
            url: "/KPIGrupo?handler=SearchByName",
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
                startSearch = result.start;
                $("#IdPerfil option:contains(" + 'Corretor' + ")").attr('selected', true)
                if (startSearch <= 100) {
                    if (result.listPerfil.length > 0) {
                        termoAnterior = termo;
                        $(".loader").show(100);
                        $.each(result.listPerfil, function (i, d) {
                            text += '<li class="text-row ui-sortable-handle" data-user-id="' + this.idUsuario + '" id="id-"' + this.idUsuario + '">' +
                                '<div class="card card-color" style="position: relative; left: 0px; top: 0px;">' +
                                '<div class="card-header border-0 ui-sortable-handle" style="cursor: move;">' +
                                '<h3 class="card-title">' +
                                '<i class="fas fa-th mr-1"></i>' + this.nome.substr(0, 19) +
                                '</h3>' +
                                '<div class="card-tools mr-2 mb-1">' +
                                '<button type="button" class="btn btn-outline-light btn-sm" data-card-widget="collapse">' +
                                '<i class="fas fa-minus"></i>' +
                                '</button>' +
                                '<button type="button" class="btn btn-outline-light btn-sm" data-card-widget="remove">' +
                                '<i class="fas fa-times"></i>' +
                                '</button>' +
                                '</div>' +
                                '</div>' +
                                '<div class="card-body p-0 mb-0" style="display: block;">' +
                                '<div class="card card-widget widget-user mb-0">' +
                                '<div class="widget-user-header card-color">' +
                                '<h3 class="widget-user-username">' + this.descricaoPerfil + '</h3>' +
                                '</div>' +
                                '<div class="widget-user-image">' +
                                '<img class="img-circle elevation-2" src="' + this.caminhoFoto + this.nomeFoto + '" alt="' + this.nomeFoto + '">' +
                                '</div>' +
                                '<div class="card-footer bg-gradient-light">' +
                                '<div class="row">' +
                                '<div class="col-sm-4 border-right">' +
                                '<div class="description-block">' +
                                '<h5 class="description-header">0</h5>' +
                                '<span class="description-text">Realizado</span>' +
                                '</div>' +
                                '</div>' +
                                '<div class="col-sm-4 border-right">' +
                                '<div class="description-block">' +
                                '<h5 class="description-header">0</h5>' +
                                '<span class="description-text">Meta</span>' +
                                '</div>' +
                                '</div>' +
                                '<div class="col-sm-4">' +
                                '<div class="description-block">' +
                                '<h5 class="description-header">0</h5>' +
                                '<span class="description-text">Vidas</span>' +
                                '</div>' +
                                '</div>' +
                                '</div>' +
                                '</div>' +
                                '</div>' +
                                '</div>' +
                                '</div>' +
                                '</li>';
                        });
                        $(".TODO li").remove();
                        startSearch += startSearch;
                    }
                } else {
                    if (result.listPerfil.length > 0) {
                        $(".loader").show(100);
                        $.each(result.listPerfil, function (i, d) {
                            text += '<li class="text-row ui-sortable-handle" data-user-id="' + this.idUsuario + '" id="id-"' + this.idUsuario + '">' +
                                '<div class="card card-color" style="position: relative; left: 0px; top: 0px;">' +
                                '<div class="card-header border-0 ui-sortable-handle" style="cursor: move;">' +
                                '<h3 class="card-title">' +
                                '<i class="fas fa-th mr-1"></i>' + this.nome.substr(0, 19) +
                                '</h3>' +
                                '<div class="card-tools mr-2 mb-1">' +
                                '<button type="button" class="btn btn-outline-light btn-sm" data-card-widget="collapse">' +
                                '<i class="fas fa-minus"></i>' +
                                '</button>' +
                                '<button type="button" class="btn btn-outline-light btn-sm" data-card-widget="remove">' +
                                '<i class="fas fa-times"></i>' +
                                '</button>' +
                                '</div>' +
                                '</div>' +
                                '<div class="card-body p-0 mb-0" style="display: block;">' +
                                '<div class="card card-widget widget-user mb-0">' +
                                '<div class="widget-user-header card-color">' +
                                '<h3 class="widget-user-username">' + this.descricaoPerfil + '</h3>' +
                                '</div>' +
                                '<div class="widget-user-image">' +
                                '<img class="img-circle elevation-2" src="' + this.caminhoFoto + this.nomeFoto + '" alt="' + this.nomeFoto + '">' +
                                '</div>' +
                                '<div class="card-footer bg-gradient-light">' +
                                '<div class="row">' +
                                '<div class="col-sm-4 border-right">' +
                                '<div class="description-block">' +
                                '<h5 class="description-header">0</h5>' +
                                '<span class="description-text">Realizado</span>' +
                                '</div>' +
                                '</div>' +
                                '<div class="col-sm-4 border-right">' +
                                '<div class="description-block">' +
                                '<h5 class="description-header">0</h5>' +
                                '<span class="description-text">Meta</span>' +
                                '</div>' +
                                '</div>' +
                                '<div class="col-sm-4">' +
                                '<div class="description-block">' +
                                '<h5 class="description-header">0</h5>' +
                                '<span class="description-text">Vidas</span>' +
                                '</div>' +
                                '</div>' +
                                '</div>' +
                                '</div>' +
                                '</div>' +
                                '</div>' +
                                '</div>' +
                                '</li>';
                        });
                        startSearch += startSearch;;
                    }
                }
                $(".loader").hide("fast", function () {
                    $(this).prev().hide("fast", arguments.callee);
                });
                $('.TODO').append(text);
            },
            failure: function (data) {
                console.log(response);
            }
        });
    }
    else {

    }
}
$("#searchKPIGrupo").change(function () {
    if ($("#searchKPIGrupo").val() != termoAnterior) {
        termoAnterior = $("#searchKPIGrupo").val();
        startSearch = 0;
    }
    if ($("#searchKPIGrupo").val().length == 0 && ht != "") {
        $(".TODO li").remove();
        $('.TODO').append(ht);
        ht = '';
    }
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

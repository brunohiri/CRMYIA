"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/notificacaohub").build();

connection.on("ReceberNotificacao", function (dados, status, id) {

    let tam = 0;
    let tamNotificacao = 0;
    var html = '';

    $.each(dados, function () {
        if (this.visualizado === true) {
            tam++;
        } else {
            tamNotificacao++;
        }
    });

    if (tamNotificacao > qtdNotificacao && status == true && id == $("#IdUsuario").val()) {
        if (tamNotificacao > 0) {
            html += '<a class="nav-link" data-toggle="dropdown" href="#">\
                    <i class="far fa-bell"></i >\
                    <span class="badge badge-warning navbar-badge">'+ tamNotificacao + '</span>\
                </a>';
        } else {
            html += '<a class="nav-link" data-toggle="dropdown" href="#">\
                    <i class="far fa-bell"></i>\
                </a>';
        }

        html += '<div class="dropdown-menu dropdown-menu-lg dropdown-menu-right">\
                    <span class="dropdown-item dropdown-header" >'+ tamNotificacao + ' Notificações</span>\
                    <div class="dropdown-divider"></div>';

        $.each(dados, function () {
            html += '<a href="#" class="dropdown-item notificacao-desativar" data-url="' + this.url + '" data-idnotificacao="' + this.idNotificacao + '">\
                        <i class="fas fa-file mr-2"></i>' + LimitaTexto(this.descricao, 16) + '\
                        <span class="float-right text-muted text-sm">' + FormataDatatime(this.dataCadastro) + '</span>\
                    </a>\
                    <div class="dropdown-divider"></div>';
        });
        html += '<a href="/ListarNotificacao" class="dropdown-item dropdown-footer">Ver todas as notificações</a>\
                     <div class="dropdown-divider"></div>\
                 </div>';
        qtdNotificacao = tamNotificacao;
        $("#lista-notificacoes").html(html);
        vazio = false;
    }
    else if (vazio && id == $("#IdUsuario").val()) {
        if (tamNotificacao > 0) {
            html += '<a class="nav-link" data-toggle="dropdown" href="#">\
                    <i class="far fa-bell"></i >\
                    <span class="badge badge-warning navbar-badge">'+ tamNotificacao + '</span>\
                </a>';
        } else {
            html += '<a class="nav-link" data-toggle="dropdown" href="#">\
                    <i class="far fa-bell"></i>\
                </a>';
        }
        //html += '<div class="dropdown-menu dropdown-menu-lg dropdown-menu-right">\
        //            <span class="dropdown-item dropdown-header" >0 Notificações</span>\
        //            <div class="dropdown-divider"></div>\
        //            <a href="#" class="dropdown-item dropdown-footer">Ver todas as notificações</a>\
        //            <div class="dropdown-divider"></div>\
        //        </div>';
        html += '<div class="dropdown-menu dropdown-menu-lg dropdown-menu-right">\
                    <span class="dropdown-item dropdown-header" >'+ tamNotificacao + ' Notificações</span>\
                    <div class="dropdown-divider"></div>';

        $.each(dados, function () {
            html += '<a href="#" class="dropdown-item notificacao-desativar" data-url="' + this.url + '" data-idnotificacao="' + this.idNotificacao + '">\
                        <i class="fas fa-file mr-2"></i>' + LimitaTexto(this.descricao, 16) + '\
                        <span class="float-right text-muted text-sm">' + FormataDatatime(this.dataCadastro) + '</span>\
                    </a>\
                    <div class="dropdown-divider"></div>';
        });
        html += '<a href="/ListarNotificacao" class="dropdown-item dropdown-footer">Ver todas as notificações</a>\
                     <div class="dropdown-divider"></div>\
                 </div>';
        vazio = false;
        $("#lista-notificacoes").html(html);
    }



});

connection.on("retornoPrivateMensagem", function (Para, De, Mensagem, Entity) {
    if (Para) {
        alert(Para)
    }

});

connection.start().then(function () {

}).catch(function (err) {
    return console.error(err.toString());
});

setInterval(function () {
    let id = $("#IdUsuario").val();
    connection.invoke("NotificacaoHub", id).catch(function (err) {
        //return console.log(err.toString());
    });
}, 500);


$(document).ready(function () {
    // Atribui evento e função para limpeza dos campos
    //$('#pesquisa-chat').on('input', limpaCampos);

    //// Dispara o Autocomplete a partir do segundo caracter
    //$("#pesquisa-chat").autocomplete({
    //    minLength: 2,
    //    source: function (request, response) {
    //        $.ajax({
    //            url: "/Index?handler=PesquisaChat",
    //            dataType: "json",
    //            data: {
    //                acao: 'autocomplete',
    //                parametro: $('#pesquisa-chat').val()
    //            },
    //            success: function (data) {
    //                response(data.entityUsuario);
    //            }
    //        });
    //    },
    //    focus: function (event, ui) {
    //        $("#pesquisa-chat").val(ui.item.titulo);
    //        carregarDados();
    //        return false;
    //    },
    //    select: function (event, ui) {
    //        $("#pesquisa-chat").val(ui.item.titulo);
    //        return false;
    //    }
    //}).autocomplete("instance")._renderItem = function (ul, item) {
    //    let url = "";
    //    let logado = "";
    //    if (item.caminhoFoto == null || item.nomeFoto == null) {
    //        url = "/img/fotoCadastro/foto-cadastro.jpeg";
    //    } else {
    //        url = item.caminhoFoto + item.nomeFoto;
    //    }

    //    if (item.logado == 0) {
    //        logado = "<span class='badge'><div class='bg-danger' style='border-radius: 50%; width: 7px; height: 7px;'></div></span>";
    //    } else if (item.logado == 1) {
    //        logado = "<span class='badge'><div class='bg-success' style='border-radius: 50%; width: 7px; height: 7px;'></div></span>";
    //    }

    //    return $("<li class='select-usuario' data-usuariopara='" + item.idUsuario +"' data-nome='" + item.nome + "' data-url='" + url + "'>")
    //        .append("<span><img src=" + url + " class='rounded-circle' width='40' height='51' /> " + logado + item.nome.toUpperCase() + " </span>")
    //        .appendTo(ul);
    //};

    //$('.btn-enviar-mensagem').on('click', function () {
    //    var Para = $("#Para").val();
    //    var De = $("#IdUsuario").val();
    //    var Mensagem = $("#Mensagem").val();

    //    connection.invoke("EnviarPrivateMensagem", Number(Para), Number(De), Mensagem).catch(function (err) {
    //        return console.log(err.toString());
    //    });

    //});

    //$('.select-usuario').on('click', function () {
    //    $('#Para').val($(this).data('usuariopara'));
    //    $('#Nome').val($(this).data('nome'));
    //    $('#Url').val($(this).data('url'));
    //    $('.direct-chat-messages').html('<div class="container text-center"><img class="rounded-circle" width="40" height="51" src="' + $(this).data('url') + '"/><br/><span>Escreva uma Mensagem para </span><br/><span>' + $(this).data('nome') + '</span></div>');
    //});
   
});

//// Função para carregar os dados da consulta nos respectivos campos
//function carregarDados() {
//    var busca = $('#pesquisa-chat').val();

//    if (busca != "" && busca.length >= 2) {
//        $.ajax({
//            url: "consulta.php",
//            dataType: "json",
//            data: {
//                acao: 'consulta',
//                parametro: $('#pesquisa-chat').val()
//            },
//            success: function (data) {
//                $('#codigo_barra').val(data[0].codigo_barra);
//                $('#titulo_livro').val(data[0].titulo);
//                $('#categoria').val(data[0].categoria);
//                $('#valor_compra').val(data[0].valor_compra);
//                $('#valor_venda').val(data[0].valor_venda);
//                $('#data_cadastro').val(data[0].data_cadastro);
//                $('#status').val(data[0].status);
//            }
//        });
//    }
//}

//// Função para limpar os campos caso a busca esteja vazia
//function limpaCampos() {
//    var busca = $('#pesquisa-chat').val();

//    if (busca == "") {
//        $('#codigo_barra').val('');
//        $('#titulo_livro').val('')
//        $('#categoria').val('');
//        $('#valor_compra').val('');
//        $('#valor_venda').val('');
//        $('#data_cadastro').val('');
//        $('#status').val('')
//    }
//}
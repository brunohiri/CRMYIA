"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/notificacaohub").build();

connection.on("ReceberNotificacao", function (dados, status, id) {
    let tam = dados.length;

    if (qtdNotificacao < tam && status == true && id == $("#IdUsuario").val()) {
        var html = '';
        html += '<a class="nav-link" data-toggle="dropdown" href="#">\
                    <i class="far fa-bell"></i >\
                    <span class="badge badge-warning navbar-badge">'+ tam + '</span>\
                </a>\
                <div class="dropdown-menu dropdown-menu-lg dropdown-menu-right">\
                    <span class="dropdown-item dropdown-header" >'+ tam + ' Notificações</span>\
                    <div class="dropdown-divider"></div>';

        $.each(dados, function () {
            html += '<a href="#" class="dropdown-item notificacao-desativar" data-url="' + this.url +'" data-idnotificacao="' + this.idNotificacao + '">\
                        <i class="fas fa-file mr-2"></i>' + LimitaTexto(this.descricao, 25) + '\
                        <span class="float-right text-muted text-sm">' + FormataDatatime(this.dataCadastro) + '</span>\
                    </a>\
                    <div class="dropdown-divider"></div>';
        });
        html += '<a href="#" class="dropdown-item dropdown-footer">Ver todas as notificações</a>\
                     <div class="dropdown-divider"></div>\
                 </div>';
        qtdNotificacao = tam;
        $("#lista-notificacoes").html(html);
        vazio = false;
    } 
    else if (vazio && id == $("#IdUsuario").val()) {
        var html = '';
        html += '<a class="nav-link" data-toggle="dropdown" href="#">\
                    <i class="far fa-bell"></i>\
                </a>\
                <div class="dropdown-menu dropdown-menu-lg dropdown-menu-right">\
                    <span class="dropdown-item dropdown-header" >0 Notificações</span>\
                    <div class="dropdown-divider"></div>\
                    <a href="#" class="dropdown-item dropdown-footer">Ver todas as notificações</a>\
                    <div class="dropdown-divider"></div>\
                </div>';
        vazio = false;
        $("#lista-notificacoes").html(html);
    }

    
  
});


connection.start().then(function () {
    
}).catch(function (err) {
    return console.error(err.toString());
});


setInterval(function () {
    let id = $("#IdUsuario").val();
    connection.invoke("NotificacaoHub", id ).catch(function (err) {
        return console.error(err.toString());
    });    
}, 500);

"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/notificacaohub").build();

connection.on("ReceberNotificacao", function (dados) {
    let tam = dados.length;
    
    if (qtdNotificacao < tam) {
        var html = '';
        html += '<a class="nav-link" data-toggle="dropdown" href="#">\
                    <i class="far fa-bell"></i >\
                    <span class="badge badge-warning navbar-badge">'+ tam + '</span>\
                </a>';
        $.each(dados, function () {
            html += '<div class="dropdown-menu dropdown-menu-lg dropdown-menu-right">\
                        <span class="dropdown-item dropdown-header" >'+ tam + ' Notifications</span>\
                        <div class="dropdown-divider"></div>\
                        <a href="#" class="dropdown-item">\
                            <i class="fas fa-file mr-2"></i> '+ tam + '' + '' + this.descricao + '\
                            <span class="float-right text-muted text-sm">'+ FormataDatatime(this.dataCadastro) + '</span>\
                        </a>\
                        <div class="dropdown-divider"></div>';
        });
        html += '<a href="#" class="dropdown-item dropdown-footer">See All Notifications</a>\
                    </div>';
        qtdNotificacao = tam;
    }

    $("#lista-notificacoes").html(html);
  
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
    $("body").removeClass('change-notificacao') 

    
}, 3000);




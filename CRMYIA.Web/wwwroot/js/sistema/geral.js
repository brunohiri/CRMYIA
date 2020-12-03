var qtdNotificacao = 0;
var vazio = true;
var statusNotificacao = true;
var atualizar = true;
$(document).ready(function () {

});

$(document).on('click', '.notificacao-desativar', function () {
    var url = $(this).data('url');
    $.ajax({
        url: '/Visita?handler=DesativarNotificacao',
        method: "GET",
        data: {IdNotificacao: $(this).data('idnotificacao')},
        dataType: 'json',
        success: function (data) {
            if (data.status) {
                location.href = url
            }
            
        },
        error: function () {

        }
    });

});

function FormataDatatime(data) {
    vetData = data.split('T');
    vetBr = vetData[0].split('-');
    vetHr = vetData[1].split('.');
    return vetBr[2] + '/' + vetBr[1] + '/' + vetBr[0] + '  ' + vetHr[0];
}

function LimitaTexto(data, qtdCaracter) {
    let texto = data.substring(0, qtdCaracter);//Novo Agendamento Janeiro...
    return texto + '...';
}


var qtdNotificacao = 0;
var vazio = true;
$(document).ready(function () {

});

function FormataDatatime(data) {
    //2020-11-27T14:16:25.937

    vetData = data.split('T');
    vetBr = vetData[0].split('-');
    vetHr = vetData[1].split('.');
    return vetBr[2] + '/' + vetBr[1] + '/' + vetBr[0] + '  ' + vetHr[0];
}
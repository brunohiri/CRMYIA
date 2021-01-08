var qtdNotificacao = 0;
var qtdNotificacaoMensagem = 0;
var vazio = true;
var notificacaoMensagemVazia = true;
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

function GetDiaMesAno(data) {
    var diaS = data.getDay();
    var diaM = data.getDate();
    var mes = data.getMonth();
    var ano = data.getFullYear();
    let mesF = "";

    switch (diaS) { //converte o numero em nome do dia
        case 0:
            diaS = "Domingo";
            break;
        case 1:
            diaS = "Segunda-feira";
            break;
        case 2:
            diaS = "Terça-feira";
            break;
        case 3:
            diaS = "Quarta-feira";
            break;
        case 4:
            diaS = "Quinta-feira";
            break;
        case 5:
            diaS = "Sexta-feira";
            break;
        case 6:
            diaS = "Sabado";
            break;
    }

    switch (mes) { //converte o numero em nome do mês
        case 0: {
            mesF = mes + 1;
            mes = "Janeiro";
        }
            break;
        case 1: {
            mesF = mes + 1;
            mes = "Fevereiro";
        }
            break;
        case 2: {
            mesF = mes + 1;
            mes = "Março";
        }
            break;
        case 3: {
            mesF = mes + 1;
            mes = "Abril";
        }
            break;
        case 4: {
            mesF = mes + 1;
            mes = "Maio";
        }
            break;
        case 5: {
            mesF = mes + 1;
            mes = "Junho";
        }
            break;
        case 6: {
            mesF = mes + 1;
            mes = "Julho";
        }
            break;
        case 7: {
            mesF = mes + 1;
            mes = "Agosto";
        }
            break;
        case 8: {
            mesF = mes + 1;
            mes = "Setembro";
        }
            break;
        case 9: {
            mesF = mes + 1;
            mes = "Outubro";
        }
            break;
        case 10: {
            mesF = mes + 1;
            mes = "Novembro";
        }
            break;
        case 11: {
            mesF = mes + 1;
            mes = "Dezembro";
        }
            break;
    }

    if (diaM.toString().length == 1)
        diaM = "0" + diaM;
    if (mes.toString().length == 1)
        mes = "0" + mes;

    //return diaS + ", " + diaM + "/" + mesF + "/" + ano;
    return diaM + "/" + mesF + "/" + ano;
}


function GetStringDiaMesAno(data) {
    var diaS = data.getDay();
    var diaM = data.getDate();
    var mes = data.getMonth();
    var ano = data.getFullYear();
    let mesF = "";

    switch (diaS) { //converte o numero em nome do dia
        case 0:
            diaS = "Domingo";
            break;
        case 1:
            diaS = "Segunda-feira";
            break;
        case 2:
            diaS = "Terça-feira";
            break;
        case 3:
            diaS = "Quarta-feira";
            break;
        case 4:
            diaS = "Quinta-feira";
            break;
        case 5:
            diaS = "Sexta-feira";
            break;
        case 6:
            diaS = "Sabado";
            break;
    }

    switch (mes) { //converte o numero em nome do mês
        case 0: {
            mesF = mes + 1;
            mes = "Janeiro";
        }
            break;
        case 1: {
            mesF = mes + 1;
            mes = "Fevereiro";
        }
            break;
        case 2: {
            mesF = mes + 1;
            mes = "Março";
        }
            break;
        case 3: {
            mesF = mes + 1;
            mes = "Abril";
        }
            break;
        case 4: {
            mesF = mes + 1;
            mes = "Maio";
        }
            break;
        case 5: {
            mesF = mes + 1;
            mes = "Junho";
        }
            break;
        case 6: {
            mesF = mes + 1;
            mes = "Julho";
        }
            break;
        case 7: {
            mesF = mes + 1;
            mes = "Agosto";
        }
            break;
        case 8: {
            mesF = mes + 1;
            mes = "Setembro";
        }
            break;
        case 9: {
            mesF = mes + 1;
            mes = "Outubro";
        }
            break;
        case 10: {
            mesF = mes + 1;
            mes = "Novembro";
        }
            break;
        case 11: {
            mesF = mes + 1;
            mes = "Dezembro";
        }
            break;
    }

    if (diaM.toString().length == 1)
        diaM = "0" + diaM;
    if (mes.toString().length == 1)
        mes = "0" + mes;

    //return diaS + ", " + diaM + "/" + mesF + "/" + ano;
    return diaM + " de " + mes + " de " + ano;
}

function GetStringData(data) {
    var dia = data.dia;
    var mes = data.mes - 1;
    var ano = data.ano;
    var mesString = '';
    var mesNum = 0;

    switch (mes) { //converte o numero em nome do mês
        case 0: {
            mesNum = mes + 1;
            mesString = "Janeiro";
        }
            break;
        case 1: {
            mesNum = mes + 1;
            mesString = "Fevereiro";
        }
            break;
        case 2: {
            mesNum = mes + 1;
            mesString = "Março";
        }
            break;
        case 3: {
            mesNum = mes + 1;
            mesString = "Abril";
        }
            break;
        case 4: {
            mesNum = mes + 1;
            mesString = "Maio";
        }
            break;
        case 5: {
            mesNum = mes + 1;
            mesString = "Junho";
        }
            break;
        case 6: {
            mesNum = mes + 1;
            mesString = "Julho";
        }
            break;
        case 7: {
            mesNum = mes + 1;
            mesString = "Agosto";
        }
            break;
        case 8: {
            mesNum = mes + 1;
            mesString = "Setembro";
        }
            break;
        case 9: {
            mesNum = mes + 1;
            mesString = "Outubro";
        }
            break;
        case 10: {
            mesNum = mes + 1;
            mesString = "Novembro";
        }
            break;
        case 11: {
            mesNum = mes + 1;
            mesString = "Dezembro";
        }
            break;
    }

    return dia + " de " + mesString + " de " + ano;
}

function GetDiaAtual() {
    var hoje = new Date();
    var dd = String(hoje.getDate()).padStart(2, '0');
    var mm = String(hoje.getMonth() + 1).padStart(2, '0');
    var yyyy = hoje.getFullYear();

    hoje = dd + '/' + mm + '/' + yyyy;
    return hoje;
}


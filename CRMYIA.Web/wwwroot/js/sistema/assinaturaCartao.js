$(document).ready(function () {

});

$(document).on('click', '.btn-redesocial', function () {
    //var a = this.children;
    //console.log(a);

    // console.log($(this).data('canvas') + ' ' + $(this).data('img') + ' ' + $(this).data('usuario'));
    var $this = $(this);
    htmlspinner = $this.html();
    corbutaoredesocial = $(this).css("background-color");
    var aria_expanded = $(this).attr('aria-expanded');
    var ativo = aria_expanded == 'true' ? true : false;
    console.log(corbutaoredesocial)


    //var spinner = '<div class="spinner-grow" style="background: #EE5A26;" role="status">\
    //                       <span class="sr-only"> Loading...</span>\
    //               </div>\
    //                <div class="spinner-grow" style="background: #494648;" role="status">\
    //                       <span class="sr-only"> Loading...</span>\
    //               </div>\
    //                <div class="spinner-grow" style="background: #EE5A26;" role="status">\
    //                       <span class="sr-only"> Loading...</span>\
    //               </div>\
    //                <div class="spinner-grow" style="background: #494648;" role="status">\
    //                       <span class="sr-only"> Loading...</span>\
    //               </div>';

    var eleCanvas = $(this).data('canvas');
    var eleImg = $(this).data('img');
    var vetUsuario = $(this).data('usuario').split('|');
    var vetDescricao = [];
    var vetEleCanvas = eleCanvas.split('|');
    var vetEleImg = eleImg.split('|');

    var obj = {}
    obj.nome = vetUsuario[0];
    obj.telefone = vetUsuario[1];
    obj.email = vetUsuario[2];

    //Indentifica se é  um array de canvas 
    if ($(this).data('canvas').indexOf('|') > - 1) {
        var vetEleCanvas = eleCanvas.split('|');
        var vetEleImg = eleImg.split('|');
        var i = 0;
        while (i < vetEleCanvas.length) {
            if ((vetEleCanvas[i] != '' && vetEleCanvas[i] != undefined) && (vetEleImg[i] != '' && vetEleImg[i] != undefined)) {
                GerarFotoContato(vetEleCanvas[i], vetEleImg[i], obj);
            }
            i++;
        }
    } else {
        GerarFotoContato(eleCanvas, eleImg, obj);
    }

});

function GerarFotoContato(Canvas, Img, usuario) {
    var canvas = document.getElementById(Canvas),

        ctx = canvas.getContext('2d');
    //1002 X 262 Assinatura
    if (canvas.width == 1002 && canvas.height == 262) {
        var dimensoes = $('#' + Img);
        canvas.crossOrigin = "Anonymous";

        ctx.drawImage($('#' + Img).get(0), 0, 0);
        ctx.clearRect(0, 0, canvas.width, canvas.height);
        ctx.font = "20px Verdana";
        ctx.drawImage($('#' + Img).get(0), 0, 0);
        ctx.fillStyle = "white";
        ctx.font = "bold 25px Verdana";
        ctx.fillStyle = '#F05A26';
        ctx.drawImage($('#' + Img).get(0), 0, 0);

        ctx.fillText(usuario.nome, (canvas.width - 650), (canvas.height - 130));
        ctx.fillText('E-mail: ' + usuario.email, (canvas.width - 650), (canvas.height - 95));
        ctx.fillText('   ' + usuario.telefone, (canvas.width - 650), (canvas.height - 65));

        var img = document.getElementById("wpp");
        ctx.drawImage(img, (canvas.width - 655), (canvas.height - 95));
        ctx.stroke();
    } else if (canvas.width == 826 && canvas.height == 1280) {    //826 X 1280 Cartão
        var dimensoes = $('#' + Img);
        canvas.crossOrigin = "Anonymous";

        ctx.drawImage($('#' + Img).get(0), 0, 0);
        ctx.clearRect(0, 0, canvas.width, canvas.height);
        ctx.font = "20px Verdana";
        ctx.drawImage($('#' + Img).get(0), 0, 0);
        ctx.fillStyle = "white";
        ctx.font = "bold 30px Verdana";
        ctx.fillStyle = '#F05A26';
        ctx.drawImage($('#' + Img).get(0), 0, 0);

        ctx.fillText(usuario.nome, (canvas.width - 760), (canvas.height - 940));
        ctx.fillText('' + usuario.email, (canvas.width - 550), (canvas.height - 525));
        ctx.fillText('   ' + usuario.telefone, (canvas.width - 620), (canvas.height - 460));

        //var img = document.getElementById("wpp");
        //ctx.drawImage(img, 26, (canvas.height - 72));
        ctx.stroke();
    }
}

function download_image(eleCanvas) {
    var canvas = document.getElementById(eleCanvas);
    image = canvas.toDataURL("image/png").replace("image/png", "image/octet-stream");
    var link = document.createElement('a');
    link.download = "download.png";
    link.href = image;
    link.click();
}
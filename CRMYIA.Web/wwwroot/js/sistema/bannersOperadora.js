$(document).ready(function () {

});

$(document).on('click', '.btn-redesocial', function () {

    var $this = $(this);
    htmlspinner = $this.html();
    corbutaoredesocial = $(this).css("background-color");
    var aria_expanded = $(this).attr('aria-expanded');
    var ativo = aria_expanded == 'true' ? true : false;
    console.log(corbutaoredesocial)

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
    if ($(this).data('canvas').indexOf('|') > - 1 && ativo) {
        var vetEleCanvas = eleCanvas.split('|');
        var vetEleImg = eleImg.split('|');
        var i = 0;
        while (i < vetEleCanvas.length) {
            if ((vetEleCanvas[i] != '' && vetEleCanvas[i] != undefined) && (vetEleImg[i] != '' && vetEleImg[i] != undefined)) {
                GerarFotoContato(vetEleCanvas[i], vetEleImg[i], obj);
            }
            i++;
        }
    } else if (ativo) {
        GerarFotoContato(eleCanvas, eleImg, obj);
    }

});

function GerarFotoContato(Canvas, Img, usuario) {
    var canvas = document.getElementById(Canvas),

        ctx = canvas.getContext('2d');

        var dimensoes = $('#' + Img);
        canvas.crossOrigin = "Anonymous";

        ctx.drawImage($('#' + Img).get(0), 0, 0);
        ctx.clearRect(0, 0, canvas.width, canvas.height);
        ctx.font = "20px Arial";
        ctx.drawImage($('#' + Img).get(0), 0, 0);
        ctx.fillStyle = "white";
        ctx.font = "bold 25px Arial";
        ctx.fillStyle = '#F05A26';
        ctx.drawImage($('#' + Img).get(0), 0, 0);

    //Icon Font Awesome
    const i = document.createElement('i');
    i.id = 'iClose';

    i.setAttribute('class', 'fas fa-phone-square-alt');
    document.body.appendChild(i);

    // get the styles for the icon you just made
    const iStyles = window.getComputedStyle(i);
    const iBeforeStyles = window.getComputedStyle(i, ':before');

    const fontFamily = iStyles.getPropertyValue('font-family');
    const fontWeight = iStyles.getPropertyValue('font-weight');
    const fontSize = '40px'; // just to make things a little bigger...

    const canvasFont = `${fontWeight} ${fontSize} ${fontFamily}`; // should be something like: '900 40px "Font Awesome 5 Pro"'
    const icon = String.fromCodePoint(iBeforeStyles.getPropertyValue('content').codePointAt(1)); // codePointAt(1) because the first character is a double quote

        ctx.font = canvasFont;
        ctx.fillStyle = '#F05A26';
        ctx.textAlign = 'center';
        //ctx.fillText(icon, (canvas.width / 2), (canvas.height - 265));

    //######################################################################################

    //Icon Font Awesome
    const e = document.createElement('i');
    e.id = 'eClose';

    e.setAttribute('class', 'fas fa-envelope-square');
    document.body.appendChild(e);

    // get the styles for the icon you just made
    const eiStyles = window.getComputedStyle(e);
    const eiBeforeStyles = window.getComputedStyle(e, ':before');

    const efontFamily = eiStyles.getPropertyValue('font-family');
    const efontWeight = eiStyles.getPropertyValue('font-weight');
    const efontSize = '40px'; // just to make things a little bigger...

    const ecanvasFont = `${efontWeight} ${efontSize} ${efontFamily}`; // should be something like: '900 40px "Font Awesome 5 Pro"'
    const eicon = String.fromCodePoint(eiBeforeStyles.getPropertyValue('content').codePointAt(1)); // codePointAt(1) because the first character is a double quote

    ctx.font = ecanvasFont;
    ctx.fillStyle = '#F05A26';
    ctx.textAlign = 'center';
    //#########################################################################################

        ctx.textAlign = 'center';
        ctx.fillText(usuario.nome, (canvas.width / 2), (canvas.height - 330));
        ctx.textAlign = 'center';
        ctx.fillText(`${eicon}` + " " + usuario.email, (canvas.width / 2), (canvas.height - 295));
        ctx.textAlign = 'center';
        ctx.fillText(`${icon}` + " "+ usuario.telefone.trim(), (canvas.width /2), (canvas.height - 253));
        
        //Imagem Operadora/Seguradora Centalizada
    var img = document.getElementById("imagem-operadora-seguradora");
    var parte = canvas.width / 4;
    var meio = (parte * 2) - (img.width / 2);
        ctx.drawImage(img, (meio), (canvas.height - 470));
    ctx.stroke();

    const iClose = document.getElementById('iClose');
    iClose.remove();
    const eClose = document.getElementById('eClose');
    eClose.remove();
}

function download_image(eleCanvas) {
    var canvas = document.getElementById(eleCanvas);
    image = canvas.toDataURL("image/png").replace("image/png", "image/octet-stream");
    var link = document.createElement('a');
    link.download = "download.png";
    link.href = image;
    link.click();
}
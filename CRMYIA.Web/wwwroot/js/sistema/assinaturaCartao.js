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
    } else if(ativo){
        GerarFotoContato(eleCanvas, eleImg, obj);
    }

});

$(document).on('click', '.download', function () {
    var canvas = document.getElementById($(this).data('elementocanvas'));
    image = canvas.toDataURL("image/png").replace("image/png", "image/octet-stream");
    var link = document.createElement('a');
    link.download = "download.png";
    link.href = image;
    link.click();

    if (link != undefined) {
        formData = new FormData();
        formData.append('IdCampanha', $(this).data('idcampanha'));
        $.ajax({
            type: 'POST',
            url: "/Banners?handler=ContadorDownload",
            data: formData,
            cache: false,
            contentType: false,
            processData: false,
            beforeSend: function (xhr) {
                xhr.setRequestHeader("XSRF-TOKEN",
                    $('input:hidden[name="__RequestVerificationToken"]').val());
            },
            success: function (data) {


            },
            error: function () {
                alert("Error occurs");
            }
        });
    }
});

function GerarFotoContato(Canvas, Img, usuario) {
    var canvas = document.getElementById(Canvas),

        ctx = canvas.getContext('2d');
    if (canvas.width == 1002 && canvas.height == 262) {//1002 X 262 Assinatura de Email Padrao
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


        //Icon Font Awesome phone-volume => Telefone Unicode => f2a0
        const phoneVolume = document.createElement('i');
        phoneVolume.id = 'phoneVolumeClose';

        phoneVolume.setAttribute('class', 'fas fa-phone-square-alt');
        document.body.appendChild(phoneVolume);

        // get the styles for the icon you just made
        const phoneVolumeStyles = window.getComputedStyle(phoneVolume);
        const phoneVolumeBeforeStyles = window.getComputedStyle(phoneVolume, ':before');

        const phoneVolumeFontFamily = phoneVolumeStyles.getPropertyValue('font-family');
        const phoneVolumeFontWeight = phoneVolumeStyles.getPropertyValue('font-weight');
        const phoneVolumeFontSize = '30px'; // just to make things a little bigger...

        const phoneVolumeCanvasFont = `${phoneVolumeFontWeight} ${phoneVolumeFontSize} ${phoneVolumeFontFamily}`; // should be something like: '900 40px "Font Awesome 5 Pro"'
        const iconPhoneVolume = String.fromCodePoint(phoneVolumeBeforeStyles.getPropertyValue('content').codePointAt(1)); // codePointAt(1) because the first character is a double quote

        ctx.font = phoneVolumeCanvasFont;
        ctx.fillStyle = '#303030';
        ctx.textAlign = 'center';

        //########################################################

        //Icon Font Awesome envelope => Email Unicode => f0e0
        const envelope = document.createElement('i');
        envelope.id = 'envelopeClose';

        envelope.setAttribute('class', 'fas fa-envelope-square');
        document.body.appendChild(envelope);

        // get the styles for the icon you just made
        const envelopeStyles = window.getComputedStyle(envelope);
        const envelopeBeforeStyles = window.getComputedStyle(envelope, ':before');

        const envelopeFontFamily = envelopeStyles.getPropertyValue('font-family');
        const envelopeFontWeight = envelopeStyles.getPropertyValue('font-weight');
        const envelopeFontSize = '30px'; // just to make things a little bigger...

        const envelopeCanvasFont = `${envelopeFontWeight} ${envelopeFontSize} ${envelopeFontFamily}`; // should be something like: '900 40px "Font Awesome 5 Pro"'
        const iconEnvelope = String.fromCodePoint(envelopeBeforeStyles.getPropertyValue('content').codePointAt(1)); // codePointAt(1) because the first character is a double quote

        ctx.font = envelopeCanvasFont;

        var nome = usuario.nome.split(' ');
        ctx.fillStyle = '#303030';
        ctx.fillText(usuario.nome.trim(), (canvas.width - 570), (canvas.height - 128));
        ctx.fillText(`${iconPhoneVolume}` + " " + usuario.telefone, (canvas.width - 530), (canvas.height - 88));
        ctx.fillText(`${iconEnvelope}` + " " + usuario.email, (canvas.width - 480), (canvas.height - 48));

        ctx.stroke();

        const phoneVolumeClose = document.getElementById('phoneVolumeClose');
        phoneVolumeClose.remove();
        const envelopeClose = document.getElementById('envelopeClose');
        envelopeClose.remove();
    }
    else if (canvas.width == 845 && canvas.height == 443) { //845 X 443 Assinatura de Email Sazonais
        var dimensoes = $('#' + Img);
        canvas.crossOrigin = "Anonymous";

        ctx.drawImage($('#' + Img).get(0), 0, 0);
        ctx.clearRect(0, 0, canvas.width, canvas.height);
        ctx.font = '20px "Didact Gothic, Arial"';
        ctx.drawImage($('#' + Img).get(0), 0, 0);
        ctx.fillStyle = "white";
        ctx.font = 'bold 25px "Didact Gothic, Arial"';
        ctx.fillStyle = '#F05A26';
        ctx.drawImage($('#' + Img).get(0), 0, 0);


        //Icon Font Awesome phone-volume => Telefone Unicode => f2a0
        const phoneVolume = document.createElement('i');
        phoneVolume.id = 'phoneVolumeClose';

        phoneVolume.setAttribute('class', 'fas fa-phone-square-alt');
        document.body.appendChild(phoneVolume);

        // get the styles for the icon you just made
        const phoneVolumeStyles = window.getComputedStyle(phoneVolume);
        const phoneVolumeBeforeStyles = window.getComputedStyle(phoneVolume, ':before');

        const phoneVolumeFontFamily = phoneVolumeStyles.getPropertyValue('font-family');
        const phoneVolumeFontWeight = phoneVolumeStyles.getPropertyValue('font-weight');
        const phoneVolumeFontSize = '30px'; // just to make things a little bigger...

        const phoneVolumeCanvasFont = `${phoneVolumeFontWeight} ${phoneVolumeFontSize} ${phoneVolumeFontFamily}`; // should be something like: '900 40px "Font Awesome 5 Pro"'
        const iconPhoneVolume = String.fromCodePoint(phoneVolumeBeforeStyles.getPropertyValue('content').codePointAt(1)); // codePointAt(1) because the first character is a double quote

        ctx.font = phoneVolumeCanvasFont;
        ctx.fillStyle = '#303030';
        ctx.textAlign = 'center';

        //########################################################

        //Icon Font Awesome envelope => Email Unicode => f0e0
        const envelope = document.createElement('i');
        envelope.id = 'envelopeClose';

        envelope.setAttribute('class', 'fas fa-envelope-square');
        document.body.appendChild(envelope);

        // get the styles for the icon you just made
        const envelopeStyles = window.getComputedStyle(envelope);
        const envelopeBeforeStyles = window.getComputedStyle(envelope, ':before');

        const envelopeFontFamily = envelopeStyles.getPropertyValue('font-family');
        const envelopeFontWeight = envelopeStyles.getPropertyValue('font-weight');
        const envelopeFontSize = '30px'; // just to make things a little bigger...

        const envelopeCanvasFont = `${envelopeFontWeight} ${envelopeFontSize} ${envelopeFontFamily}`; // should be something like: '900 40px "Font Awesome 5 Pro"'
        const iconEnvelope = String.fromCodePoint(envelopeBeforeStyles.getPropertyValue('content').codePointAt(1)); // codePointAt(1) because the first character is a double quote

        ctx.font = envelopeCanvasFont;

        var nome = usuario.nome.split(' ');
        ctx.fillStyle = '#303030';
        ctx.textAlign = 'center';
        ctx.fillText(usuario.nome.trim(), (canvas.width - 470), (canvas.height - 330));
        ctx.fillText(`${iconPhoneVolume}` + " " + usuario.telefone, (canvas.width - 430), (canvas.height - 295));
        ctx.fillText(`${iconEnvelope}` + " " + usuario.email, (canvas.width - 382), (canvas.height - 265));

        ctx.stroke();

        const phoneVolumeClose = document.getElementById('phoneVolumeClose');
        phoneVolumeClose.remove();
        const envelopeClose = document.getElementById('envelopeClose');
        envelopeClose.remove();
       
    } else if (canvas.width == 826 && canvas.height == 1280) { // 826 X 1280 Cartão de de Visita Digital
        var dimensoes = $('#' + Img);
        canvas.crossOrigin = "Anonymous";

        ctx.drawImage($('#' + Img).get(0), 0, 0);
        ctx.clearRect(0, 0, canvas.width, canvas.height);
        ctx.font = '20px "Montserrat, Arial"';
        ctx.drawImage($('#' + Img).get(0), 0, 0);
        ctx.fillStyle = "white";
        ctx.font = 'bold 106px "Montserrat, Arial"';
        ctx.fillStyle = '#F05A26';
        ctx.drawImage($('#' + Img).get(0), 0, 0);

        var nome = usuario.nome.split(' ');
        
        ctx.fillText(nome[0], (canvas.width - 760), (canvas.height - 1050));

        ctx.fillStyle = '#F05A26';
        ctx.font = "106px Arial";
        ctx.fillText(nome[1], (canvas.width - 760), (canvas.height - 940));


        ctx.font = 'bold 106px';
        ctx.fillStyle = '#FFFFFF';
        ctx.font = '34px "Montserrat, Arial"';
        ctx.fillText(usuario.email, (canvas.width - 550), (canvas.height - 525));
        ctx.fillText(usuario.telefone, (canvas.width - 590), (canvas.height - 460));
        ctx.stroke();
    }
}

function texter(ctx, str, x, y) {
    var espaco = true;
    for (var i = 0; i <= str.length; ++i) {
        var ch = str.charAt(i);
       
        if (!(ch == ' ') && espaco) {
            ctx.fillStyle = '#303030';
        } else {
            ctx.fillStyle = '#F05A26';
            espaco = false;
        }
        ctx.fillText(str.charAt(i), x, y);
        x += ctx.measureText(ch).width + 0.5;
    }
}


//function download_image(eleCanvas) {
//    var canvas = document.getElementById(eleCanvas);
//    image = canvas.toDataURL("image/png").replace("image/png", "image/octet-stream");
//    var link = document.createElement('a');
//    link.download = "download.png";
//    link.href = image;
//    link.click();
//}
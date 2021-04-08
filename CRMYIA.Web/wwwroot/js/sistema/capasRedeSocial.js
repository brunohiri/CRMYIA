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
    // 1081 X 274 LINKEDIN
    if (canvas.width == 1081 && canvas.height == 274) {
        var dimensoes = $('#' + Img);
        canvas.crossOrigin = "Anonymous";

        ctx.drawImage($('#' + Img).get(0), 0, 0);
        ctx.clearRect(0, 0, canvas.width, canvas.height);
        ctx.font = '20px "Didact Gothic, Arial"';
        ctx.drawImage($('#' + Img).get(0), 0, 0);
        ctx.fillStyle = "white";
        ctx.font = 'bold 30px "Didact Gothic, Arial"';
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
        const phoneVolumeFontSize = '15px'; // just to make things a little bigger...

        const phoneVolumeCanvasFont = `${phoneVolumeFontWeight} ${phoneVolumeFontSize} ${phoneVolumeFontFamily}`; // should be something like: '900 40px "Font Awesome 5 Pro"'
        const iconPhoneVolume = String.fromCodePoint(phoneVolumeBeforeStyles.getPropertyValue('content').codePointAt(1)); // codePointAt(1) because the first character is a double quote

        ctx.font = phoneVolumeCanvasFont;
        ctx.fillStyle = '#FFFFFF';
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
        const envelopeFontSize = '15px'; // just to make things a little bigger...

        const envelopeCanvasFont = `${envelopeFontWeight} ${envelopeFontSize} ${envelopeFontFamily}`; // should be something like: '900 40px "Font Awesome 5 Pro"'
        const iconEnvelope = String.fromCodePoint(envelopeBeforeStyles.getPropertyValue('content').codePointAt(1)); // codePointAt(1) because the first character is a double quote

        ctx.font = envelopeCanvasFont;
        ctx.fillStyle = '#FFFFFF';
        ctx.textAlign = 'center';

       
        ctx.fillText(`${iconEnvelope}` + " " + usuario.email, (canvas.width - 265), (canvas.height - 40));
        ctx.fillText(`${iconPhoneVolume}` + " " + usuario.telefone, (canvas.width - 290), (canvas.height - 10));
        ctx.fillText(usuario.nome, (canvas.width - 260), (canvas.height - 65));
        
        ctx.stroke();

        const phoneVolumeClose = document.getElementById('phoneVolumeClose');
        phoneVolumeClose.remove();
        const envelopeClose = document.getElementById('envelopeClose');
        envelopeClose.remove();

        // 1081 x 401 FACEBOOK
    } else if (canvas.width == 1081 && canvas.height == 401){
        var dimensoes = $('#' + Img);
        canvas.crossOrigin = "Anonymous";

        ctx.drawImage($('#' + Img).get(0), 0, 0);
        ctx.clearRect(0, 0, canvas.width, canvas.height);
        ctx.font = '20px "Didact Gothic, Arial"';
        ctx.drawImage($('#' + Img).get(0), 0, 0);
        ctx.fillStyle = "white";
        ctx.font = 'bold 30px "Didact Gothic, Arial"';
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
        const phoneVolumeFontSize = '15px'; // just to make things a little bigger...

        const phoneVolumeCanvasFont = `${phoneVolumeFontWeight} ${phoneVolumeFontSize} ${phoneVolumeFontFamily}`; // should be something like: '900 40px "Font Awesome 5 Pro"'
        const iconPhoneVolume = String.fromCodePoint(phoneVolumeBeforeStyles.getPropertyValue('content').codePointAt(1)); // codePointAt(1) because the first character is a double quote

        ctx.font = phoneVolumeCanvasFont;
        ctx.fillStyle = '#FFFFFF';
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
        const envelopeFontSize = '18px'; // just to make things a little bigger...

        const envelopeCanvasFont = `${envelopeFontWeight} ${envelopeFontSize} ${envelopeFontFamily}`; // should be something like: '900 40px "Font Awesome 5 Pro"'
        const iconEnvelope = String.fromCodePoint(envelopeBeforeStyles.getPropertyValue('content').codePointAt(1)); // codePointAt(1) because the first character is a double quote

        ctx.font = envelopeCanvasFont;
        ctx.fillStyle = '#FFFFFF';
        ctx.textAlign = 'center';

        ctx.fillText(usuario.nome, (canvas.width - 530), (canvas.height - 269));
        ctx.fillText(`${iconEnvelope}` + " " + usuario.email, (canvas.width - 535), (canvas.height - 242));
        ctx.fillText(`${iconPhoneVolume}` + " " + usuario.telefone, (canvas.width - 563), (canvas.height - 217));

        ctx.stroke();

        const phoneVolumeClose = document.getElementById('phoneVolumeClose');
        phoneVolumeClose.remove();
        const envelopeClose = document.getElementById('envelopeClose');
        envelopeClose.remove();
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
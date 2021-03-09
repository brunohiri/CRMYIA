var htmlspinner = '';
var corbutaoredesocial = '';
var canvas = '';
var img = '';

$(document).ready(function () {

    Campanhas();

    $(document).on('click', '[data-toggle="lightbox"]', function (event) {
        event.preventDefault();
        $(this).ekkoLightbox({
            alwaysShowClose: true
        });
    });

    $(document).on('click', '.img-fluid.mb-2', function () {
        let Descricao = $(this).data('descricao');
        let Caminho = $(this).data('caminho');
        setTimeout(function () {
            $('.modal-dialog').addClass('modal-lg');
            $('.modal-body').append('<div class="d-flex justify-content-center m-3"><a class="btn btn-success" href="' + Caminho + '" download="' + Caminho + '">Download</a></div>\
                                 <div class="form-group">\
                                    <label for="Descricao">Descrição</label>\
                                    <textarea class="form-control" id="TextDescricao" rows="3">' + Descricao + '</textarea>\
                                 </div>\
                                 <div class="d-flex justify-content-center m-3"><button class="btn btn-success copiar-descricao">COPIAR</button></div>');
        }, 500);
    });
   
});

$(document).on('click', '.copiar-descricao', function () {
    ///* Pega o texto do textArea */
    //var copyText = document.getElementById($(this).data('copiardescricao'));
    ///* Seleciona o textArea */
    //copyText.select();
    //copyText.setSelectionRange(0, 99999); /*Para dispositivos móveis*/
    ///* Copie o texto dentro do campo de texto */
    //document.execCommand("copy");



    var myCode = document.getElementById($(this).data('copiardescricao')).value;
    var fullLink = document.createElement('textarea');
    document.body.appendChild(fullLink);
    fullLink.value = myCode;
    fullLink.select();
    document.execCommand("copy", false);
    fullLink.remove();
    //alert("Copied the text: " + fullLink.value);

});

$(document).on('click', '.btn-redesocial', function () {
    //var a = this.children;
    //console.log(a);

    // console.log($(this).data('canvas') + ' ' + $(this).data('img') + ' ' + $(this).data('usuario'));
    var $this = $(this);
    htmlspinner = $this.html();
    corbutaoredesocial = $(this).css("background-color");
    var aria_expanded = $(this).attr('aria-expanded');
    var ativo = aria_expanded == 'true' ? true : false ;
    console.log(corbutaoredesocial)
    

    var spinner = '<div class="spinner-grow" style="background: #EE5A26;" role="status">\
                           <span class="sr-only"> Loading...</span>\
                   </div>\
                    <div class="spinner-grow" style="background: #494648;" role="status">\
                           <span class="sr-only"> Loading...</span>\
                   </div>\
                    <div class="spinner-grow" style="background: #EE5A26;" role="status">\
                           <span class="sr-only"> Loading...</span>\
                   </div>\
                    <div class="spinner-grow" style="background: #494648;" role="status">\
                           <span class="sr-only"> Loading...</span>\
                   </div>';

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

    //Desativa todos os bloco descrição
    $('.descricao').css('display', 'none');

    //Desativa todos os bloco descrição array
    if ($(this).data('descricao').indexOf('|') > - 1) {
        vetDescricao = $(this).data('descricao').split('|');
        var i = 0; 
        while (i < vetDescricao.length) {
            if (vetDescricao[i] != '' && vetDescricao[i] != undefined)
                $('#' + vetDescricao[i]).css('display', 'block');
            i++;
        }
       
    } else {
        $('#' + $(this).data('descricao')).css('display', 'block');
    }

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

   
    //if (vetEleCanvas.length == vetEleImg.length && ativo) {
    //    var i = 0;
    //    $this.css("background-color", '#FFFFFF');
    //    $this.css("border", '2px solid black');
    //    $this.html(spinner);
    //    $('.btn-redesocial').attr("disabled", true);
    //    $('#' + $(this).data('descricao')).css('display', 'block');
    //    while (i < vetEleCanvas.length - 1) {
    //        var imgCarregando = vetEleCanvas[i].split('-');

    //        $('#carregando-' + imgCarregando[imgCarregando.length - 1]).css('display', 'block');
    //        canvas = vetEleCanvas[i];
    //        img = vetEleImg[i];
    //        setTimeout(function () {
    //            GerarFotoContato(canvas, img, obj);
    //        }, 3000);
    //        $('#carregando-' + imgCarregando[imgCarregando.length - 1]).css('display', 'none');
    //        i++;
    //    }
    //    setTimeout(function () {
    //        $this.css("background-color", corbutaoredesocial);
    //        $this.css('border', '');
    //        $this.html(htmlspinner);
    //        $('.btn-redesocial').attr("disabled", false);

    //    }, 3000);
    //} else {
    //    $('#' + $(this).data('descricao')).css('display', 'none');
    //}

});

function Campanhas() {
    $.getJSON('/MaterialDivulgacao?handler=ListarCampanha', function (data) {
        console.log(data);
        var html = '';
        if (data.status) {

            console.log(data.campanhaArquivo);
            console.log(data.campanha);

            var vetnome = [];
            var vetwidth = [];
            var vetheight = [];
           
            displayBusyIndicator();
            for (var i = 0; i < data.campanhaArquivo.length; i++) {
                //vetnome = data.campanhaArquivo[j].nomeArquivo.split('|');
                //vetwidth = data.campanhaArquivo[j].width.split('|');
                //vetheight = data.campanhaArquivo[j].height.split('|');

                var btnfacebook = '';
                var btnwhatsapp = '';
                var btninstagram = '';
                var btnlinkedin = '';
                var k = 0;
                var canvas = '';
                var img = '';
                      html += '<section class="content">\
                                        <div class="container-fluid">\
                                            <div class="row">\
                                                <div class="col-12">\
                                                    <div class="card">\
                                                         <div class="card-header">\
                                                            <h4 class="card-title">' + data.campanhaArquivo[i].idCampanhaNavigation.descricao + '</h4>\
                                                        </div>\
                                                        <div class="card-body filtro-banners"><h3>Selecione a Rede Social:</h3>';
                                                html += 'btnfacebook' + 'btnwhatsapp' + 'btninstagram' + 'btnlinkedin';
                                                console.log(data.campanhaArquivo[i].nomeArquivo.split('|').length)
                                                if (data.campanhaArquivo[i].nomeArquivo.split('|').length > 1) {

                                                    vetnome = data.campanhaArquivo[i].nomeArquivo.split('|');
                                                    vetwidth = data.campanhaArquivo[i].width.split('|');
                                                    vetheight = data.campanhaArquivo[i].height.split('|');

                                                    html += '<div class="accordion" id="accordionExample' + data.campanhaArquivo[i].idCampanhaArquivo + '">';
                                                    //Inicio Multi

                                                    k = 0;
                                                    canvas = '';
                                                    img = '';
                                                    if (data.campanhaArquivo[i].nomeArquivo.toUpperCase().indexOf('FACEBOOK') > -1) {
                                                        html += '<div class="collapse" id="multiCollapseFacebook' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '" data-parent="#accordionExample' + data.campanhaArquivo[i].idCampanhaArquivo + '">';
                                                        html += '<!-- Inicio Bloco de Imagem -->\
                                                                <div class="d-flex justify-content-center m-3">';

                                                        while (k < vetnome.length) {
                                                            if (vetnome[k].toUpperCase().indexOf('FACEBOOK') > -1) {
                                                                html += '<div class="material-divulgacao-imagem p-2 flex-fill bd-highlight">\
                                                                            <canvas style="display:none" width="' + vetwidth[k] + '" height="' + vetheight[k] + '" id="canvas-facebook-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '-' + k + '"> </canvas>\
                                                                            <img style="display: none;" src="' + data.campanhaArquivo[i].caminhoArquivo + vetnome[k] + '" class="img-fluid mb-2" id="img-facebook-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '-' + k + '" alt="white sample"/>\
                                                                            <div class="text-center m-2">\
                                                                                <button onclick=download_image("' + 'canvas-facebook-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '-' + k + '") type="button" class="btn btn-success">Download</button>\
                                                                            </div>\
                                                                       </div>';
                                                                canvas += 'canvas-facebook-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '-' + k + '|';
                                                                img += 'img-facebook-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '-' + k + '|';
                                                            }
                                                            k++;
                                                        }
                                                        btnfacebook = '<button class="btn btn-redesocial btn-filtro-facebook botao-rede-social" type="button" data-canvas="' + canvas + '" data-img="' + img + '" data-descricao="descricao-facebook-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '" data-usuario="' + data.usuarioEntity.nome + ' | ' + data.usuarioEntity.telefone + ' |' + data.usuarioEntity.email + '" data-toggle="collapse" data-target="#multiCollapseFacebook' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '" aria-expanded="false" aria-controls="multiCollapseFacebook' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '"><i class="fab fa-facebook-f"></i> &nbsp; Banners Facebook &nbsp; <i class="fas fa-chevron-down"></i></button>';

                                                        html += '</div>';
                                                        html += '<!-- Fim Bloco de Imagem -->';
                                                        html += '<div class="form-group text-center descricao" style="display: none; font-size: 20px; color: #777 !important;" id="descricao-facebook-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '">\
                                                                <label for= "Descricao">Texto para a publicação no Feed</label >\
                                                                <textarea class="form-control text-dark font-weight-bold" style="font-size: 16px;" id="copiardescricao-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '" rows="3">\
                                                                '+ data.campanhaArquivo[i].descricao + '\
                                                                </textarea>\
                                                                <div class="d-flex justify-content-center m-3"><button class="btn btn-success copiar-descricao" data-copiardescricao="copiardescricao-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '">COPIAR</button></div>\
                                                            </div>\
                                                        </div>';
                                                    }

                                                    k = 0;
                                                    canvas = '';
                                                    img = '';
                                                    if (data.campanhaArquivo[i].nomeArquivo.toUpperCase().indexOf('WHATSAPP') > -1) {
                                                        html += '<div class="collapse" id="multiCollapseWhatsapp' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '" data-parent="#accordionExample' + data.campanhaArquivo[i].idCampanhaArquivo + '">';
                                                        html += '<!-- Inicio Bloco de Imagem -->\
                                                                <div class="d-flex justify-content-center m-3">';

                                                        while (k < vetnome.length) {
                                                            if (vetnome[k].toUpperCase().indexOf('WHATSAPP') > -1) {
                                                                html += '<div class="material-divulgacao-imagem p-2 flex-fill bd-highlight">\
                                                                            <canvas style="display:none" width="' + vetwidth[k] + '" height="' + vetheight[k] + '" id="canvas-whatsapp-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '-' + k + '"> </canvas>\
                                                                            <img style="display: none;" src="' + data.campanhaArquivo[i].caminhoArquivo + vetnome[k] + '" class="img-fluid mb-2" id="img-whatsapp-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '-' + k + '" alt="white sample"/>\
                                                                            <div class="text-center m-2">\
                                                                                <button onclick=download_image("' + 'canvas-whatsapp-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '-' + k + '") type="button" class="btn btn-success">Download</button>\
                                                                            </div>\
                                                                       </div>';
                                                                canvas += 'canvas-whatsapp-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '-' + k + '|';
                                                                img += 'img-whatsapp-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '-' + k + '|';
                                                            }
                                                            k++;
                                                        }
                                                        btnwhatsapp = '<button class="btn btn-redesocial btn-filtro-whatsapp botao-rede-social" type="button" data-canvas="' + canvas + '" data-img="' + img + '" data-descricao="descricao-whatsapp-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '" data-usuario="' + data.usuarioEntity.nome + ' | ' + data.usuarioEntity.telefone + ' |' + data.usuarioEntity.email + '" data-toggle="collapse" data-target="#multiCollapseWhatsapp' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '" aria-expanded="false" aria-controls="multiCollapseWhatsapp' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '"><i class="fab fa-whatsapp"></i> &nbsp; Banners WhatsApp &nbsp; <i class="fas fa-chevron-down"></i></button>';

                                                        html += '</div>';
                                                        html += '<!-- Fim Bloco de Imagem -->';
                                                        html += '<div class="form-group text-center descricao" style="display: none; font-size: 20px; color: #777 !important;" id="descricao-whatsapp-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '">\
                                                                <label for= "Descricao">Texto para a publicação no Feed</label >\
                                                                <textarea class="form-control text-dark font-weight-bold" style="font-size: 16px;" id="copiardescricao-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '" rows="3">\
                                                                '+ data.campanhaArquivo[i].descricao + '\
                                                                </textarea>\
                                                                <div class="d-flex justify-content-center m-3"><button class="btn btn-success copiar-descricao" data-copiardescricao="copiardescricao-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '">COPIAR</button></div>\
                                                            </div>\
                                                        </div>';
                                                    }

                                                    k = 0;
                                                    canvas = '';
                                                    img = '';
                                                    if (data.campanhaArquivo[i].nomeArquivo.toUpperCase().indexOf('INSTAGRAM') > -1) {
                                                        html += '<div class="collapse" id="multiCollapseInstagram' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '" data-parent="#accordionExample' + data.campanhaArquivo[i].idCampanhaArquivo + '">';
                                                        html += '<!-- Inicio Bloco de Imagem -->\
                                                                <div class="d-flex justify-content-center m-3">';

                                                        while (k < vetnome.length) {
                                                            if (vetnome[k].toUpperCase().indexOf('INSTAGRAM') > -1) {
                                                                html += '<div class="material-divulgacao-imagem p-2 flex-fill bd-highlight">\
                                                                            <canvas style="display:none" width="' + vetwidth[k] + '" height="' + vetheight[k] + '" id="canvas-instagram-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '-' + k + '"> </canvas>\
                                                                            <img style="display: none;" src="' + data.campanhaArquivo[i].caminhoArquivo + vetnome[k] + '" class="img-fluid mb-2" id="img-instagram-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '-' + k + '" alt="white sample"/>\
                                                                            <div class="text-center m-2">\
                                                                                <button onclick=download_image("' + 'canvas-instagram-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '-' + k + '") type="button" class="btn btn-success">Download</button>\
                                                                            </div>\
                                                                       </div>';
                                                                canvas += 'canvas-instagram-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '-' + k + '|';
                                                                img += 'img-instagram-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '-' + k + '|';
                                                            }
                                                            k++;
                                                        }
                                                        btninstagram = '<button class="btn btn-redesocial btn-filtro-instagram botao-rede-social" type="button" data-canvas="' + canvas + '" data-img="' + img + '" data-descricao="descricao-instagram-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '" data-usuario="' + data.usuarioEntity.nome + ' | ' + data.usuarioEntity.telefone + ' |' + data.usuarioEntity.email + '" data-toggle="collapse" data-target="#multiCollapseInstagram' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '" aria-expanded="false" aria-controls="multiCollapseInstagram' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '"><i class="fab fa-instagram"></i> &nbsp; Banners Instagram &nbsp; <i class="fas fa-chevron-down"></i></button>';

                                                        html += '</div>';
                                                        html += '<!-- Fim Bloco de Imagem -->';
                                                        html += '<div class="form-group text-center descricao" style="display: none; font-size: 20px; color: #777 !important;" id="descricao-instagram-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '">\
                                                                <label for= "Descricao">Texto para a publicação no Feed</label >\
                                                                <textarea class="form-control text-dark font-weight-bold" style="font-size: 16px;" id="copiardescricao-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '" rows="3">\
                                                                '+ data.campanhaArquivo[i].descricao + '\
                                                                </textarea>\
                                                                <div class="d-flex justify-content-center m-3"><button class="btn btn-success copiar-descricao" data-copiardescricao="copiardescricao-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '">COPIAR</button></div>\
                                                            </div>\
                                                        </div>';
                                                    }


                                                    k = 0;
                                                    canvas = '';
                                                    img = '';
                                                    if (data.campanhaArquivo[i].nomeArquivo.toUpperCase().indexOf('LINKEDIN') > -1) {
                                                        html += '<div class="collapse" id="multiCollapseLinkedin' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '" data-parent="#accordionExample' + data.campanhaArquivo[i].idCampanhaArquivo + '">';
                                                        html += '<!-- Inicio Bloco de Imagem -->\
                                                                <div class="d-flex justify-content-center m-3">';

                                                        while (k < vetnome.length) {
                                                            if (vetnome[k].toUpperCase().indexOf('LINKEDIN') > -1) {
                                                                html += '<div class="material-divulgacao-imagem p-2 flex-fill bd-highlight">\
                                                                            <canvas style="display:none" width="' + vetwidth[k] + '" height="' + vetheight[k] + '" id="canvas-linkedin-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '-' + k + '"> </canvas>\
                                                                            <img style="display: none;" src="' + data.campanhaArquivo[i].caminhoArquivo + vetnome[k] + '" class="img-fluid mb-2" id="img-linkedin-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '-' + k + '" alt="white sample"/>\
                                                                            <div class="text-center m-2">\
                                                                                <button onclick=download_image("' + 'canvas-linkedin-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '-' + k + '") type="button" class="btn btn-success">Download</button>\
                                                                            </div>\
                                                                       </div>';
                                                                canvas += 'canvas-linkedin-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '-' + k + '|';
                                                                img += 'img-linkedin-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '-' + k + '|';
                                                            }
                                                            k++;
                                                        }
                                                        btnlinkedin = '<button class="btn btn-redesocial btn-filtro-linkedin botao-rede-social" type="button" data-canvas="' + canvas + '" data-img="' + img + '" data-descricao="descricao-linkedin-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '" data-usuario="' + data.usuarioEntity.nome + ' | ' + data.usuarioEntity.telefone + ' |' + data.usuarioEntity.email + '" data-toggle="collapse" data-target="#multiCollapseLinkedin' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '" aria-expanded="false" aria-controls="multiCollapseLinkedin' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '"><i class="fab fa-linkedin-in"></i> &nbsp; Banners Linkedin &nbsp; <i class="fas fa-chevron-down"></i></button>';

                                                        html += '</div>';
                                                        html += '<!-- Fim Bloco de Imagem -->';
                                                        html += '<div class="form-group text-center descricao" style="display: none; font-size: 20px; color: #777 !important;" id="descricao-linkedin-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '">\
                                                                <label for= "Descricao">Texto para a publicação no Feed</label >\
                                                                <textarea class="form-control text-dark font-weight-bold" style="font-size: 16px;" id="copiardescricao-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '" rows="3">\
                                                                '+ data.campanhaArquivo[i].descricao + '\
                                                                </textarea>\
                                                                <div class="d-flex justify-content-center m-3"><button class="btn btn-success copiar-descricao" data-copiardescricao="copiardescricao-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '">COPIAR</button></div>\
                                                            </div>\
                                                        </div>';
                                                    }

                                                    html += '</div>';
                                                    //Fim Multi


                                                } else {
                                                    btnfacebook = '';
                                                    if (data.campanhaArquivo[i].nomeArquivo.toUpperCase().indexOf('FACEBOOK') > -1) {
                                                        btnfacebook = '<button class="btn btn-redesocial btn-filtro-facebook botao-rede-social" type="button" data-canvas="canvas-facebook-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '" data-img="img-facebook-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '" data-descricao="descricao-facebook-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '" data-usuario="' + data.usuarioEntity.nome + ' | ' + data.usuarioEntity.telefone + ' |' + data.usuarioEntity.email + '" data-toggle="collapse" data-target="#multiCollapseFacebook' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '" aria-expanded="false" aria-controls="multiCollapseFacebook' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '"><i class="fab fa-facebook-f"></i> &nbsp; Banners Facebook &nbsp; <i class="fas fa-chevron-down"></i></button>';

                                                        html += '<div class="collapse multi-collapse" id="multiCollapseFacebook' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '">\
                                                                        <div class="p-2 flex-fill bd-highlight" >\
                                                                            <div class="material-divulgacao-imagem">\
                                                                                <canvas style="display:none" width="' + data.campanhaArquivo[i].width + '" height="' + data.campanhaArquivo[i].height + '" id="canvas-facebook-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '"> </canvas>\
                                                                                <img style="display: none;" src="' + data.campanhaArquivo[i].caminhoArquivo + data.campanhaArquivo[i].nomeArquivo + '" class="img-fluid mb-2" id="img-facebook-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '" alt="white sample" />\
                                                                                <div class="text-center m-2">\
                                                                                    <button onclick=download_image("'+ 'canvas-facebook-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '") type="button" class="btn btn-success">Download</button>\
                                                                                </div>\
                                                                            </div>\
                                                                            <div class="form-group text-center descricao" style="display: none; font-size: 20px; color: #777 !important;" id="descricao-facebook-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '">\
                                                                                    <label for= "Descricao">Texto para a publicação no Feed</label >\
                                                                                    <textarea class="form-control text-dark font-weight-bold" style="font-size: 16px;" id="copiardescricao-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '" rows="3">\
                                                                                    ' + data.campanhaArquivo[i].descricao + '\
                                                                                    </textarea>\
                                                                                    <div class="d-flex justify-content-center m-3"><button class="btn btn-success copiar-descricao" data-copiardescricao="copiardescricao-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '">COPIAR</button></div>\
                                                                            </div>\
                                                                        </div>\
                                                                    </div>';
                                                    }

                                                    btnwhatsapp = '';
                                                    if (data.campanhaArquivo[i].nomeArquivo.toUpperCase().indexOf('WHATSAPP') > -1) {
                                                        btnwhatsapp = '<button class="btn btn-redesocial btn-filtro-whatsapp botao-rede-social" type="button" data-canvas="canvas-whatsapp-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '" data-img="img-whatsapp-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '" data-descricao="descricao-whatsapp-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '" data-usuario="' + data.usuarioEntity.nome + ' | ' + data.usuarioEntity.telefone + ' |' + data.usuarioEntity.email + '" data-toggle="collapse" data-target="#multiCollapseWhatsapp' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '" aria-expanded="false" aria-controls="multiCollapseWhatsapp' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '"><i class="fab fa-whatsapp"></i> &nbsp; Banners WhatsApp &nbsp; <i class="fas fa-chevron-down"></i></button>';

                                                        html += '<div class="collapse multi-collapse" id="multiCollapseWhatsapp' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '">\
                                                                        <div class="p-2 flex-fill bd-highlight" >\
                                                                            <div class="material-divulgacao-imagem">\
                                                                                <canvas style="display:none" width="' + data.campanhaArquivo[i].width + '" height="' + data.campanhaArquivo[i].height + '" id="canvas-whatsapp-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '"> </canvas>\
                                                                                <img style="display: none;" src="' + data.campanhaArquivo[i].caminhoArquivo + data.campanhaArquivo[i].nomeArquivo + '" class="img-fluid mb-2" id="img-whatsapp-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '" alt="white sample" />\
                                                                                <div class="text-center m-2">\
                                                                                    <button onclick=download_image("'+ 'canvas-whatsapp-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '") type="button" class="btn btn-success">Download</button>\
                                                                                </div>\
                                                                            </div>\
                                                                            <div class="form-group text-center descricao" style="display: none; font-size: 20px; color: #777 !important;" id="descricao-whatsapp-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '">\
                                                                                    <label for= "Descricao">Texto para a publicação no Feed</label >\
                                                                                    <textarea class="form-control text-dark font-weight-bold" style="font-size: 16px;" id="copiardescricao-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '" rows="3">\
                                                                                    ' + data.campanhaArquivo[i].descricao + '\
                                                                                    </textarea>\
                                                                                    <div class="d-flex justify-content-center m-3"><button class="btn btn-success copiar-descricao" data-copiardescricao="copiardescricao-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '">COPIAR</button></div>\
                                                                            </div>\
                                                                        </div>\
                                                                    </div>';
                                                    }

                                                    btninstagram = '';
                                                    if (data.campanhaArquivo[i].nomeArquivo.toUpperCase().indexOf('INSTAGRAM') > -1) {
                                                        btninstagram = '<button class="btn btn-redesocial btn-filtro-instagram botao-rede-social" type="button" data-canvas="canvas-instagram-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '" data-img="img-instagram-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '" data-descricao="descricao-instagram-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '" data-usuario="' + data.usuarioEntity.nome + ' | ' + data.usuarioEntity.telefone + ' |' + data.usuarioEntity.email + '" data-toggle="collapse" data-target="#multiCollapseInstagram' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '" aria-expanded="false" aria-controls="multiCollapseInstagram' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '"><i class="fab fa-instagram"></i> &nbsp; Banners Instagram &nbsp; <i class="fas fa-chevron-down"></i></button>';

                                                        html += '<div class="collapse multi-collapse" id="multiCollapseInstagram' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '">\
                                                                        <div class="p-2 flex-fill bd-highlight" >\
                                                                            <div class="material-divulgacao-imagem">\
                                                                                <canvas style="display:none" width="' + data.campanhaArquivo[i].width + '" height="' + data.campanhaArquivo[i].height + '" id="canvas-instagram-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '"> </canvas>\
                                                                                <img style="display: none;" src="' + data.campanhaArquivo[i].caminhoArquivo + data.campanhaArquivo[i].nomeArquivo + '" class="img-fluid mb-2" id="img-instagram-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '" alt="white sample" />\
                                                                                <div class="text-center m-2">\
                                                                                    <button onclick=download_image("'+ 'canvas-instagram-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '") type="button" class="btn btn-success">Download</button>\
                                                                                </div>\
                                                                            </div>\
                                                                            <div class="form-group text-center descricao" style="display: none; font-size: 20px; color: #777 !important;" id="descricao-instagram-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '">\
                                                                                    <label for= "Descricao">Texto para a publicação no Feed</label >\
                                                                                    <textarea class="form-control text-dark font-weight-bold" style="font-size: 16px;" id="copiardescricao-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '" rows="3">\
                                                                                    ' + data.campanhaArquivo[i].descricao + '\
                                                                                    </textarea>\
                                                                                    <div class="d-flex justify-content-center m-3"><button class="btn btn-success copiar-descricao" data-copiardescricao="copiardescricao-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '">COPIAR</button></div>\
                                                                            </div>\
                                                                        </div>\
                                                                    </div>';
                                                    }

                                                    btnlinkedin = '';
                                                    if (data.campanhaArquivo[i].nomeArquivo.toUpperCase().indexOf('LINKEDIN') > -1) {
                                                        btnlinkedin = '<button class="btn btn-redesocial btn-filtro-linkedin botao-rede-social" type="button" data-canvas="canvas-linkedin-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '" data-img="img-linkedin-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '" data-descricao="descricao-linkedin-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '" data-usuario="' + data.usuarioEntity.nome + ' | ' + data.usuarioEntity.telefone + ' |' + data.usuarioEntity.email + '" data-toggle="collapse" data-target="#multiCollapseLinkedin' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '" aria-expanded="false" aria-controls="multiCollapseLinkedin' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '"><i class="fab fa-linkedin-in"></i> &nbsp; Banners Linkedin &nbsp; <i class="fas fa-chevron-down"></i></button>';

                                                        html += '<div class="collapse multi-collapse" id="multiCollapseLinkedin' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '">\
                                                                        <div class="p-2 flex-fill bd-highlight" >\
                                                                            <div class="material-divulgacao-imagem">\
                                                                                <canvas style="display:none" width="' + data.campanhaArquivo[i].width + '" height="' + data.campanhaArquivo[i].height + '" id="canvas-linkedin-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '"> </canvas>\
                                                                                <img style="display: none;" src="' + data.campanhaArquivo[i].caminhoArquivo + data.campanhaArquivo[i].nomeArquivo + '" class="img-fluid mb-2" id="img-linkedin-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '" alt="white sample" />\
                                                                                <div class="text-center m-2">\
                                                                                    <button onclick=download_image("'+ 'canvas-linkedin-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '") type="button" class="btn btn-success">Download</button>\
                                                                                </div>\
                                                                            </div>\
                                                                            <div class="form-group text-center descricao" style="display: none; font-size: 20px; color: #777 !important;" id="descricao-linkedin-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '">\
                                                                                    <label for= "Descricao">Texto para a publicação no Feed</label >\
                                                                                    <textarea class="form-control text-dark font-weight-bold" style="font-size: 16px;" id="copiardescricao-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '" rows="3">\
                                                                                    ' + data.campanhaArquivo[i].descricao + '\
                                                                                    </textarea>\
                                                                                    <div class="d-flex justify-content-center m-3"><button class="btn btn-success copiar-descricao" data-copiardescricao="copiardescricao-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '">COPIAR</button></div>\
                                                                            </div>\
                                                                        </div>\
                                                                    </div>';
                                                    }
                                                }
                                               html += '</div>\
                                                    </div>\
                                                </div>\
                                            </div>\
                                        </div>\
                                </section >';
                if (btnfacebook != undefined)
                    html = html.replace('btnfacebook', btnfacebook);
                else
                    html = html.replace('btnfacebook', '');

                if (btnwhatsapp != undefined)
                    html = html.replace('btnwhatsapp', btnwhatsapp);
                else
                    html = html.replace('btnwhatsapp', '');

                if (btninstagram != undefined)
                    html = html.replace('btninstagram', btninstagram);
                else
                    html = html.replace('btninstagram', '');

                if (btnlinkedin != undefined)
                    html = html.replace('btnlinkedin', btnlinkedin);
                else
                    html = html.replace('btnlinkedin', '');
            }

            html += '<img id="wpp" style="display: none" src="img/marketing/wwpp.png">';
            //html += 'btnfacebook' + 'btnwhatsapp' + 'btninstagram' + 'btnlinkedin';
           

            //html = html.replace('htmlfacebook', htmlfacebook);
            //html = html.replace('htmllinkedin', htmllinkedin);

            $('#campanhas').append(html);
            displayBusyAvailable();
        }
        //$('#campanhas').append(html);

    });


}

function QuantidadeRedeSociais(data) {

    var i = 0;
    var facebook = 0;
    var whatsapp = 0;
    var instagram = 0;
    var linkedin = 0;

    while (i < data.length) {

        if (data[i].toUpperCase().indexOf('FACEBOOK') > -1) {
            facebook++;
        }
        if (data[i].toUpperCase().indexOf('WHATSAPP') > -1) {
            whatsapp++;
        }
        if (data[i].toUpperCase().indexOf('INSTAGRAM') > -1) {
            instagram++;
        }
        if (data[i].toUpperCase().indexOf('LINKEDIN') > -1) {
            linkedin++;
        }

        i++;
    }

    return { facebook: facebook, whatsapp: whatsapp, instagram: instagram, linkedin: linkedin };

}

function GerarFotoContato(Canvas, Img, usuario) {
    var canvas = document.getElementById(Canvas),
        ctx = canvas.getContext('2d');

    //var dimensoes = $('#img-whatsapp-31-212-2')
    var dimensoes = $('#' + Img);
    //dimensoes[0].height
    //dimensoes[0].width

    //canvas.width = $('#' + Img).width();
    //canvas.crossOrigin = "Anonymous";
    //canvas.height = $('#' + Img).height();

    //canvas.width = dimensoes[0].width;
    canvas.crossOrigin = "Anonymous";
    //canvas.height = dimensoes[0].height;

    ctx.drawImage($('#' + Img).get(0), 0, 0);
    ctx.clearRect(0, 0, canvas.width, canvas.height);
    ctx.font = "20px Verdana";
    ctx.drawImage($('#' + Img).get(0), 0, 0);
    ctx.fillStyle = "white";
    ctx.font = "bold 30px Verdana";
    ctx.fillStyle = '#282C34';
    ctx.drawImage($('#' + Img).get(0), 0, 0);
    //(canvas.height - 30)
    //ctx.fillText(usuario.nome, 30, 1780);
    //ctx.fillText('   ' + usuario.telefone, 30, 1810);
    //ctx.fillText('E-mail: ' + usuario.email, 30, 1840);
    
    ctx.fillText('E-mail: ' + usuario.email, 30, (canvas.height - 105));
    ctx.fillText('   ' + usuario.telefone, 30, (canvas.height - 75));
    ctx.fillText(usuario.nome, 30, (canvas.height - 45));
    var img = document.getElementById("wpp");
    //ctx.drawImage(img, 20, 1785);
    ctx.drawImage(img, 26, (canvas.height - 105));
    ctx.stroke();
    //$('#' + canvas.id).css('display', 'block');
    //ctx.fillText($(this).val(), 10, 50);
}

function download_image(eleCanvas) {
    var canvas = document.getElementById(eleCanvas);
    image = canvas.toDataURL("image/png").replace("image/png", "image/octet-stream");
    var link = document.createElement('a');
    link.download = "download.png";
    link.href = image;
    link.click();
}
$(document).ready(function () {
    BannersOperadora()
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

function BannersOperadora() {

    var urlString = window.location.href;
    var url = new URL(urlString);
    var hash = url.searchParams.get("id");
    if (hash != undefined && hash != '') {
        console.log(hash.toString());
        formData = new FormData();
        formData.append('Id', decodeURIComponent(hash));
        $.ajax({
            type: 'POST',
            url: "/BannersDeOperadora?handler=ListarBannersOperadora",
            data: formData,
            cache: false,
            contentType: false,
            processData: false,
            beforeSend: function (xhr) {
                xhr.setRequestHeader("XSRF-TOKEN",
                    $('input:hidden[name="__RequestVerificationToken"]').val());
            },
            success: function (data) {
                console.log(data);
                var html = '';
                if (data.status) {
                    $('#nome-da-pagina').html('');
                    $('#nome-da-pagina').html(data.campanhaArquivo[0].nomeCampanha);

                    $('#banner-operadora').html('');
                    console.log(data.campanhaArquivo);
                    console.log(data.campanha);

                    var vetnome = [];
                    var vetwidth = [];
                    var vetheight = [];

                    displayBusyIndicator();
                    for (var i = 0; i < data.campanhaArquivo.length; i++) {
                        var btnfacebook = '';
                        var btnwhatsapp = '';
                        var btninstagram = '';
                        var btnlinkedin = '';
                        var k = 0;
                        var canvas = '';
                        var img = '';
                        var aniversario = '';
                        var tipopostagem = '';

                        html += '<img src="' + data.campanhaArquivo[0].caminhoArquivoOperadora + data.campanhaArquivo[0].nomeArquivoOperadora + '" id="imagem-operadora-seguradora" name="imagem-operadora-seguradora" style="width:100%; display: none;" alt="">';
                        html += '<section class="content">\
                                        <div class="container-fluid">\
                                            <div class="row">\
                                                <div class="col-12">\
                                                    <div class="card">\
                                                         <div class="card-header">\
                                                            <h4 class="card-title">Operadora - ' + data.campanhaArquivo[0].nomeOperadora + '</h4>\
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
                            aniversario = '';
                            tipopostagem = '';
                            if (data.campanhaArquivo[i].nomeArquivo.toUpperCase().indexOf('FACEBOOK') > -1) {

                                html += '<div class="collapse" id="multiCollapseFacebook' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '" data-parent="#accordionExample' + data.campanhaArquivo[i].idCampanhaArquivo + '">';
                                html += '<!-- Inicio Bloco de Imagem -->\
                                                                <div class="d-flex justify-content-center m-3 banners-das-redes-sociais">';

                                while (k < vetnome.length) {
                                    if (vetnome[k].toUpperCase().indexOf('FACEBOOK') > -1) {

                                        if (vetnome[k].toUpperCase().indexOf('STORIES') > -1) {
                                            tipopostagem += 'STORIES|';
                                        } else if (vetnome[k].toUpperCase().indexOf('FEED') > -1) {
                                            tipopostagem += 'FEED|';
                                        }
                                        if (vetnome[k].toUpperCase().indexOf('ANIVERSARIO') > -1 || vetnome[k].toUpperCase().indexOf('ANIVERSÁRIO') > -1) {
                                            aniversario += 'true|';
                                        } else {
                                            aniversario += 'false|';
                                        }

                                        html += '<div class="material-divulgacao-imagem p-2 flex-fill bd-highlight">\
                                                                            <canvas style="display:none" width="' + vetwidth[k] + '" height="' + vetheight[k] + '" id="canvas-facebook-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '-' + k + '"> </canvas>\
                                                                            <img style="display: none;" src="' + data.campanhaArquivo[i].caminhoArquivo + vetnome[k] + '" class="img-fluid mb-2" id="img-facebook-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '-' + k + '" alt="white sample"/>\
                                                                            <div class="text-center m-2">\
                                                                                <button data-idcampanha="'+ data.campanhaArquivo[i].idCampanha + '" data-elementocanvas="' + 'canvas-facebook-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '-' + k +'" type="button" class="btn btn-success download">Download</button>\
                                                                            </div>\
                                                                       </div>';
                                        canvas += 'canvas-facebook-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '-' + k + '|';
                                        img += 'img-facebook-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '-' + k + '|';
                                    }
                                    k++;
                                }
                                btnfacebook = '<button class="btn btn-redesocial btn-filtro-facebook botao-rede-social" type="button" data-canvas="' + canvas + '" data-aniversario="' + aniversario + '" data-tipopostagem="' + tipopostagem + '" data-img="' + img + '" data-descricao="descricao-facebook-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '" data-usuario="' + data.usuarioEntity.nomeApelido + ' | ' + data.usuarioEntity.telefone + ' |' + data.usuarioEntity.email + '" data-toggle="collapse" data-target="#multiCollapseFacebook' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '" aria-expanded="false" aria-controls="multiCollapseFacebook' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '"><i class="fab fa-facebook-f"></i> &nbsp; Banners Facebook &nbsp; <i class="fas fa-chevron-down"></i></button>';

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
                            aniversario = '';
                            tipopostagem = '';
                            if (data.campanhaArquivo[i].nomeArquivo.toUpperCase().indexOf('WHATSAPP') > -1) {

                                html += '<div class="collapse" id="multiCollapseWhatsapp' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '" data-parent="#accordionExample' + data.campanhaArquivo[i].idCampanhaArquivo + '">';
                                html += '<!-- Inicio Bloco de Imagem -->\
                                                                <div class="d-flex justify-content-center m-3 banners-das-redes-sociais">';

                                while (k < vetnome.length) {
                                    if (vetnome[k].toUpperCase().indexOf('WHATSAPP') > -1) {

                                        if (vetnome[k].toUpperCase().indexOf('STORIES') > -1) {
                                            tipopostagem += 'STORIES|';
                                        } else if (vetnome[k].toUpperCase().indexOf('FEED') > -1) {
                                            tipopostagem += 'FEED|';
                                        }
                                        if (vetnome[k].toUpperCase().indexOf('ANIVERSARIO') > -1 || vetnome[k].toUpperCase().indexOf('ANIVERSÁRIO') > -1) {
                                            aniversario += 'true|';
                                        } else {
                                            aniversario += 'false|';
                                        }

                                        html += '<div class="material-divulgacao-imagem p-2 flex-fill bd-highlight">\
                                                                            <canvas style="display:none" width="' + vetwidth[k] + '" height="' + vetheight[k] + '" id="canvas-whatsapp-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '-' + k + '"> </canvas>\
                                                                            <img style="display: none;" src="' + data.campanhaArquivo[i].caminhoArquivo + vetnome[k] + '" class="img-fluid mb-2" id="img-whatsapp-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '-' + k + '" alt="white sample"/>\
                                                                            <div class="text-center m-2">\
                                                                                <button data-idcampanha="'+ data.campanhaArquivo[i].idCampanha + '" data-elementocanvas="' + 'canvas-whatsapp-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '-' + k +'" type="button" class="btn btn-success download">Download</button>\
                                                                            </div>\
                                                                       </div>';
                                        canvas += 'canvas-whatsapp-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '-' + k + '|';
                                        img += 'img-whatsapp-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '-' + k + '|';
                                    }
                                    k++;
                                }
                                btnwhatsapp = '<button class="btn btn-redesocial btn-filtro-whatsapp botao-rede-social" type="button" data-canvas="' + canvas + '" data-aniversario="' + aniversario + '" data-tipopostagem="' + tipopostagem + '" data-img="' + img + '" data-descricao="descricao-whatsapp-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '" data-usuario="' + data.usuarioEntity.nomeApelido + ' | ' + data.usuarioEntity.telefone + ' |' + data.usuarioEntity.email + '" data-toggle="collapse" data-target="#multiCollapseWhatsapp' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '" aria-expanded="false" aria-controls="multiCollapseWhatsapp' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '"><i class="fab fa-whatsapp"></i> &nbsp; Banners WhatsApp &nbsp; <i class="fas fa-chevron-down"></i></button>';

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
                            aniversario = ''
                            tipopostagem = '';
                            if (data.campanhaArquivo[i].nomeArquivo.toUpperCase().indexOf('INSTAGRAM') > -1) {

                                html += '<div class="collapse" id="multiCollapseInstagram' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '" data-parent="#accordionExample' + data.campanhaArquivo[i].idCampanhaArquivo + '">';
                                html += '<!-- Inicio Bloco de Imagem -->\
                                                                <div class="d-flex justify-content-center m-3 banners-das-redes-sociais">';

                                while (k < vetnome.length) {
                                    if (vetnome[k].toUpperCase().indexOf('INSTAGRAM') > -1) {

                                        if (vetnome[k].toUpperCase().indexOf('STORIES') > -1) {
                                            tipopostagem += 'STORIES|';
                                        } else if (vetnome[k].toUpperCase().indexOf('FEED') > -1) {
                                            tipopostagem += 'FEED|';
                                        }
                                        if (vetnome[k].toUpperCase().indexOf('ANIVERSARIO') > -1 || vetnome[k].toUpperCase().indexOf('ANIVERSÁRIO') > -1) {
                                            aniversario += 'true|';
                                        } else {
                                            aniversario += 'false|';
                                        }

                                        html += '<div class="material-divulgacao-imagem p-2 flex-fill bd-highlight">\
                                                                            <canvas style="display:none" width="' + vetwidth[k] + '" height="' + vetheight[k] + '" id="canvas-instagram-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '-' + k + '"> </canvas>\
                                                                            <img style="display: none;" src="' + data.campanhaArquivo[i].caminhoArquivo + vetnome[k] + '" class="img-fluid mb-2" id="img-instagram-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '-' + k + '" alt="white sample"/>\
                                                                            <div class="text-center m-2">\
                                                                                <button data-idcampanha="'+ data.campanhaArquivo[i].idCampanha + '" data-elementocanvas="' + 'canvas-instagram-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '-' + k +'" type="button" class="btn btn-success download">Download</button>\
                                                                            </div>\
                                                                       </div>';
                                        canvas += 'canvas-instagram-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '-' + k + '|';
                                        img += 'img-instagram-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '-' + k + '|';
                                    }
                                    k++;
                                }
                                btninstagram = '<button class="btn btn-redesocial btn-filtro-instagram botao-rede-social" type="button" data-canvas="' + canvas + '" data-aniversario="' + aniversario + '" data-tipopostagem="' + tipopostagem + '" data-img="' + img + '" data-descricao="descricao-instagram-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '" data-usuario="' + data.usuarioEntity.nomeApelido + ' | ' + data.usuarioEntity.telefone + ' |' + data.usuarioEntity.email + '" data-toggle="collapse" data-target="#multiCollapseInstagram' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '" aria-expanded="false" aria-controls="multiCollapseInstagram' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '"><i class="fab fa-instagram"></i> &nbsp; Banners Instagram &nbsp; <i class="fas fa-chevron-down"></i></button>';

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
                            aniversario = '';
                            tipopostagem = '';
                            if (data.campanhaArquivo[i].nomeArquivo.toUpperCase().indexOf('LINKEDIN') > -1) {

                                html += '<div class="collapse" id="multiCollapseLinkedin' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '" data-parent="#accordionExample' + data.campanhaArquivo[i].idCampanhaArquivo + '">';
                                html += '<!-- Inicio Bloco de Imagem -->\
                                                                <div class="d-flex justify-content-center m-3 banners-das-redes-sociais">';

                                while (k < vetnome.length) {
                                    if (vetnome[k].toUpperCase().indexOf('LINKEDIN') > -1) {

                                        if (vetnome[k].toUpperCase().indexOf('STORIES') > -1) {
                                            tipopostagem += 'STORIES|';
                                        } else if (vetnome[k].toUpperCase().indexOf('FEED') > -1) {
                                            tipopostagem += 'FEED|';
                                        }
                                        if (vetnome[k].toUpperCase().indexOf('ANIVERSARIO') > -1 || vetnome[k].toUpperCase().indexOf('ANIVERSÁRIO') > -1) {
                                            aniversario += 'true|';
                                        } else {
                                            aniversario += 'false|';
                                        }

                                        html += '<div class="material-divulgacao-imagem p-2 flex-fill bd-highlight">\
                                                                            <canvas style="display:none" width="' + vetwidth[k] + '" height="' + vetheight[k] + '" id="canvas-linkedin-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '-' + k + '"> </canvas>\
                                                                            <img style="display: none;" src="' + data.campanhaArquivo[i].caminhoArquivo + vetnome[k] + '" class="img-fluid mb-2" id="img-linkedin-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '-' + k + '" alt="white sample"/>\
                                                                            <div class="text-center m-2">\
                                                                                <button data-idcampanha="'+ data.campanhaArquivo[i].idCampanha + '" data-elementocanvas="' + 'canvas-linkedin-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '-' + k +'" type="button" class="btn btn-success download">Download</button>\
                                                                            </div>\
                                                                       </div>';
                                        canvas += 'canvas-linkedin-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '-' + k + '|';
                                        img += 'img-linkedin-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '-' + k + '|';
                                    }
                                    k++;
                                }
                                btnlinkedin = '<button class="btn btn-redesocial btn-filtro-linkedin botao-rede-social" type="button" data-canvas="' + canvas + '" data-aniversario="' + aniversario + '" data-tipopostagem="' + tipopostagem + '" data-img="' + img + '" data-descricao="descricao-linkedin-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '" data-usuario="' + data.usuarioEntity.nomeApelido + ' | ' + data.usuarioEntity.telefone + ' |' + data.usuarioEntity.email + '" data-toggle="collapse" data-target="#multiCollapseLinkedin' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '" aria-expanded="false" aria-controls="multiCollapseLinkedin' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '"><i class="fab fa-linkedin-in"></i> &nbsp; Banners Linkedin &nbsp; <i class="fas fa-chevron-down"></i></button>';

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
                                btnfacebook = '<button class="btn btn-redesocial btn-filtro-facebook botao-rede-social" type="button" data-canvas="canvas-facebook-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '" data-img="img-facebook-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '" data-descricao="descricao-facebook-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '" data-usuario="' + data.usuarioEntity.nomeApelido + ' | ' + data.usuarioEntity.telefone + ' |' + data.usuarioEntity.email + '" data-toggle="collapse" data-target="#multiCollapseFacebook' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '" aria-expanded="false" aria-controls="multiCollapseFacebook' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '"><i class="fab fa-facebook-f"></i> &nbsp; Banners Facebook &nbsp; <i class="fas fa-chevron-down"></i></button>';

                                html += '<div class="collapse multi-collapse" id="multiCollapseFacebook' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '">\
                                                                        <div class="p-2 flex-fill bd-highlight" >\
                                                                            <div class="material-divulgacao-imagem">\
                                                                                <canvas style="display:none" width="' + data.campanhaArquivo[i].width + '" height="' + data.campanhaArquivo[i].height + '" id="canvas-facebook-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '"> </canvas>\
                                                                                <img style="display: none;" src="' + data.campanhaArquivo[i].caminhoArquivo + data.campanhaArquivo[i].nomeArquivo + '" class="img-fluid mb-2" id="img-facebook-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '" alt="white sample" />\
                                                                                <div class="text-center m-2">\
                                                                                    <button data-idcampanha="'+ data.campanhaArquivo[i].idCampanha + '" data-elementocanvas="' + 'canvas-facebook-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '-' + k +'" type="button" class="btn btn-success download">Download</button>\
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
                                btnwhatsapp = '<button class="btn btn-redesocial btn-filtro-whatsapp botao-rede-social" type="button" data-canvas="canvas-whatsapp-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '" data-img="img-whatsapp-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '" data-descricao="descricao-whatsapp-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '" data-usuario="' + data.usuarioEntity.nomeApelido + ' | ' + data.usuarioEntity.telefone + ' |' + data.usuarioEntity.email + '" data-toggle="collapse" data-target="#multiCollapseWhatsapp' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '" aria-expanded="false" aria-controls="multiCollapseWhatsapp' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '"><i class="fab fa-whatsapp"></i> &nbsp; Banners WhatsApp &nbsp; <i class="fas fa-chevron-down"></i></button>';

                                html += '<div class="collapse multi-collapse" id="multiCollapseWhatsapp' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '">\
                                                                        <div class="p-2 flex-fill bd-highlight" >\
                                                                            <div class="material-divulgacao-imagem">\
                                                                                <canvas style="display:none" width="' + data.campanhaArquivo[i].width + '" height="' + data.campanhaArquivo[i].height + '" id="canvas-whatsapp-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '"> </canvas>\
                                                                                <img style="display: none;" src="' + data.campanhaArquivo[i].caminhoArquivo + data.campanhaArquivo[i].nomeArquivo + '" class="img-fluid mb-2" id="img-whatsapp-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '" alt="white sample" />\
                                                                                <div class="text-center m-2">\
                                                                                    <button data-idcampanha="'+ data.campanhaArquivo[i].idCampanha + '" data-elementocanvas="' + 'canvas-whatsapp-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '-' + k +'" type="button" class="btn btn-success download">Download</button>\
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
                                btninstagram = '<button class="btn btn-redesocial btn-filtro-instagram botao-rede-social" type="button" data-canvas="canvas-instagram-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '" data-img="img-instagram-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '" data-descricao="descricao-instagram-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '" data-usuario="' + data.usuarioEntity.nomeApelido + ' | ' + data.usuarioEntity.telefone + ' |' + data.usuarioEntity.email + '" data-toggle="collapse" data-target="#multiCollapseInstagram' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '" aria-expanded="false" aria-controls="multiCollapseInstagram' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '"><i class="fab fa-instagram"></i> &nbsp; Banners Instagram &nbsp; <i class="fas fa-chevron-down"></i></button>';

                                html += '<div class="collapse multi-collapse" id="multiCollapseInstagram' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '">\
                                                                        <div class="p-2 flex-fill bd-highlight" >\
                                                                            <div class="material-divulgacao-imagem">\
                                                                                <canvas style="display:none" width="' + data.campanhaArquivo[i].width + '" height="' + data.campanhaArquivo[i].height + '" id="canvas-instagram-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '"> </canvas>\
                                                                                <img style="display: none;" src="' + data.campanhaArquivo[i].caminhoArquivo + data.campanhaArquivo[i].nomeArquivo + '" class="img-fluid mb-2" id="img-instagram-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '" alt="white sample" />\
                                                                                <div class="text-center m-2">\
                                                                                    <button data-idcampanha="'+ data.campanhaArquivo[i].idCampanha + '" data-elementocanvas="' + 'canvas-instagram-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '-' + k +'" type="button" class="btn btn-success download">Download</button>\
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
                                btnlinkedin = '<button class="btn btn-redesocial btn-filtro-linkedin botao-rede-social" type="button" data-canvas="canvas-linkedin-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '" data-img="img-linkedin-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '" data-descricao="descricao-linkedin-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '" data-usuario="' + data.usuarioEntity.nomeApelido + ' | ' + data.usuarioEntity.telefone + ' |' + data.usuarioEntity.email + '" data-toggle="collapse" data-target="#multiCollapseLinkedin' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '" aria-expanded="false" aria-controls="multiCollapseLinkedin' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '"><i class="fab fa-linkedin-in"></i> &nbsp; Banners Linkedin &nbsp; <i class="fas fa-chevron-down"></i></button>';

                                html += '<div class="collapse multi-collapse" id="multiCollapseLinkedin' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '">\
                                                                        <div class="p-2 flex-fill bd-highlight" >\
                                                                            <div class="material-divulgacao-imagem">\
                                                                                <canvas style="display:none" width="' + data.campanhaArquivo[i].width + '" height="' + data.campanhaArquivo[i].height + '" id="canvas-linkedin-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '"> </canvas>\
                                                                                <img style="display: none;" src="' + data.campanhaArquivo[i].caminhoArquivo + data.campanhaArquivo[i].nomeArquivo + '" class="img-fluid mb-2" id="img-linkedin-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '" alt="white sample" />\
                                                                                <div class="text-center m-2">\
                                                                                    <button data-idcampanha="'+ data.campanhaArquivo[i].idCampanha + '" data-elementocanvas="' + 'canvas-linkedin-' + data.campanhaArquivo[i].idCampanha + '-' + data.campanhaArquivo[i].idCampanhaArquivo + '-' + k +'" type="button" class="btn btn-success download">Download</button>\
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
                   
                    displayBusyAvailable();
                }
                if (html == '') {
                    html = '<div class="jumbotron mt-3">\
                                <h1 class="text-center"> <font style="vertical-align: inherit;"><font style="vertical-align: inherit;">Não encontramos banners para essa Campanha.</font></font></h1 >\
                                <p class="lead text-center"><font style="vertical-align: inherit;"><font style="vertical-align: inherit;">Para mais informações contate o Administrador do Sistema.</font></font></p>\
                                <!--<a class="btn btn-lg btn-primary" href="/docs/4.6/components/navbar/" role="button"><font style="vertical-align: inherit;"><font style="vertical-align: inherit;">Voltar para pagina anterior »</font></font></a>-->\
                                <div class="d-flex justify-content-around">\
                                    <button type="reset" class="btn btn-lg btn-primary" onclick="javascript:history.go(-1)">Voltar para pagina anterior »</button>\
                                </div>\
                            </div >';
                }
                $('#banner-operadora').append(html);
            },
            error: function () {
                swal("Erro!", "Erro ao buscar o registro, contate o Administrador do Sistema.", "error");
            }
        });
    }

}

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


    //Icon Font Awesome phone-volume => Telefone Unicode => f2a0
    const phoneVolume = document.createElement('i');
    phoneVolume.id = 'phoneVolumeClose';

    phoneVolume.setAttribute('class', 'fas fa-phone-volume');
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

    envelope.setAttribute('class', 'fas fa-envelope');
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
    ctx.fillStyle = '#F05A26';
    ctx.textAlign = 'center';
    ctx.fillText(usuario.nome.trim(), (canvas.width - 570), (canvas.height - 128));
    ctx.fillText(`${iconPhoneVolume}` + " " + usuario.telefone, (canvas.width - 530), (canvas.height - 88));
    ctx.fillText(`${iconEnvelope}` + " " + usuario.email, (canvas.width - 485), (canvas.height - 48));

    //Imagem Operadora/Seguradora Centalizada
    var img = document.getElementById("imagem-operadora-seguradora");
    var parte = canvas.width / 4;
    var meio = (parte * 2) - (img.width / 2);
    ctx.drawImage(img, (meio), (canvas.height - 470));
    ctx.stroke();

    const phoneVolumeClose = document.getElementById('phoneVolumeClose');
    phoneVolumeClose.remove();
    const envelopeClose = document.getElementById('envelopeClose');
    envelopeClose.remove();
}

//function download_image(eleCanvas) {
//    var canvas = document.getElementById(eleCanvas);
//    image = canvas.toDataURL("image/png").replace("image/png", "image/octet-stream");
//    var link = document.createElement('a');
//    link.download = "download.png";
//    link.href = image;
//    link.click();
//}
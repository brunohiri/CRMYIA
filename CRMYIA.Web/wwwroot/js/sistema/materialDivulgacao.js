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
    $(document).on('click', '.copiar-descricao', function () {
        /* Pega o texto do textArea */
        var copyText = document.getElementById("TextDescricao");
        /* Seleciona o textArea */
        copyText.select();
        copyText.setSelectionRange(0, 99999); /*Para dispositivos móveis*/
        /* Copie o texto dentro do campo de texto */
        document.execCommand("copy");
    });
});

function Campanhas() {
    $.getJSON('/MaterialDivulgacao?handler=ListarCampanha', function (data) {
        console.log(data);
        if (data.status) {
            var html = '<img id="wpp" style="display: none" src="img/marketing/wwpp.png">';
           
            console.log(data.campanhaArquivo);
            console.log(data.campanha);
            var imagem = [];
            for (var i = 0; i < data.campanha.length; i++) {
                for (var j = 0; j < data.campanhaArquivo.length; j++) {
                    imagem = data.campanhaArquivo[0].nomeArquivo.split('|');
                    html += '<section class="content">\
                                <div class="container-fluid">\
                                    <div class="row">\
                                        <div class="col-12">\
                                            <div class="card">\
                                                <div class="card-header">\
                                                    <h4 class="card-title">' + data.campanhaArquivo[0].idCampanhaNavigation.descricao + '</h4>\
                                                </div>\
                                                <div class="card-body">\
                                                        <div class="d-flex flex-nowrap">';
                                                        var k = 0;
                                                        while (k < imagem.length) {
                                                            html += '<div>\
                                                                            <canvas style="width: 200px" id="canvas-' + data.campanhaArquivo[0].idCampanha + '"> </canvas>\
                                                                            <img style="display: none" src="' + data.campanhaArquivo[0].caminhoArquivo + imagem[0] + '" class="img-fluid mb-2 img-' + data.campanhaArquivo[0].idCampanha + '" alt="white sample" /><br>\
                                                                            <div class="text-center m-2"><button onclick=download_image("'+ 'canvas-' + data.campanhaArquivo[j].idCampanha + '") type="button" class="btn btn-success">Download</button></div>\
                                                                     </div>';
                                                            k++;
                                                        }
                                              html += '</div>\
                                                    </div>\
                                                </div>\
                                            </div>\
                                        </div>\
                                    </div>\
                            </section >';
                }
            }
                
            $('#campanhas').append(html);
            for (var j = 0; j < data.campanhaArquivo.length; j++) {
                GerarFotoContato('canvas-' + data.campanhaArquivo[j].idCampanha, '.img-' + data.campanhaArquivo[0].idCampanha)
            }
        }
      

        
    });
}


function GerarFotoContato(eleCanvas, eleImg){
    var clsEleImg = $(eleImg)[0];
    var canvas = document.getElementById(eleCanvas);

    //canvas.width = clsEleImg.width;
    //canvas.height = clsEleImg.height;
    var ctx = canvas.getContext('2d');
   
    ctx.canvas.width = clsEleImg.width;
    ctx.canvas.height = clsEleImg.height;
    canvas.width = $(eleImg).width();
    canvas.crossOrigin = "Anonymous";
    canvas.height = $(eleImg).height();
    ctx.drawImage($(eleImg).get(0), 0, 0);
    ctx.clearRect(0, 0, clsEleImg.width, clsEleImg.height);
    ctx.font = "bold 20px Verdana";
    ctx.fillStyle = '#282C34';
    ctx.drawImage($(eleImg).get(0), 0, 0);
    
    ctx.fillText('Marcelo N. Oliveira', 30, 1780);
    ctx.fillText('   (18) 98125-0252', 30, 1810);
    ctx.fillText('E-mail: marcelonorbertodeoliveira@hotmail.com', 30, 1840);
    var img = document.getElementById("wpp");
    ctx.drawImage(img, 20, 1785);
    ctx.stroke();
}


function download_image(eleCanvas) {
    var canvas = document.getElementById(eleCanvas);
    image = canvas.toDataURL("image/png").replace("image/png", "image/octet-stream");
    var link = document.createElement('a');
    link.download = "download.png";
    link.href = image;
    link.click();
}
$(document).ready(function () {
    Videos();
});

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


function Videos() {
    $.getJSON('/Videos?handler=ListarVideos', function (data) {
        console.log(data)
        var html = '<div class="d-flex justify-content-around">';
        var frase = "";
        $('#videos').html('');
        if (data.status) {
            $.each(data.entityVideo, function (index, value) {
                if (this.nomeVideo.toUpperCase().indexOf('FEED') > -1) {
                    frase = '<span class="font-weight-bold">Vídeo para FEED</span>';
                } else if (this.nomeVideo.toUpperCase().indexOf('STORES') > -1) {
                    frase = '<span class="font-weight-bold">Vídeo para Stories</span>';
                }
                html += '<div class="card" style="width: 20.5rem;" >\
                            <div class="p-3">\
                                <iframe src="https://www.youtube.com/embed/'+ this.identificadorVideo + '"?feature=oembed" frameborder="0" allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture" allowfullscreen="">\
                                </iframe>\
                            </div>\
                            <div class="card-body">\
                                <p class="card-text text-center">'+ frase +'</span></p>\
                            </div >\
                            <div class="d-flex justify-content-center"><a type="button" class="btn btn-success mb-4" href="' + this.caminhoArquivo + this.nomeVideo + '" download>Download do Vídeo</a></div>\
                        </div>';
           
            });
        }
        html += '</div>';
        $('#videos').html(html);
    });
}
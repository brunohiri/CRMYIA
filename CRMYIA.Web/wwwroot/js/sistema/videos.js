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

$(document).on('click', '.download', function () {
  
    formData = new FormData();
    formData.append('IdCampanha', $(this).data('idcampanha'));
    $.ajax({
        type: 'POST',
        url: "/Videos?handler=ContadorDownload",
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
});


function Videos() {
    var urlString = window.location.href;
    var url = new URL(urlString);
    var hash = url.searchParams.get("id");
    if (hash != undefined && hash != '') {
        console.log(hash.toString());
        formData = new FormData();
        formData.append('Id', decodeURIComponent(hash));
    $.ajax({
        type: 'POST',
        url: "/Videos?handler=ListarVideos",
        data: formData,
        cache: false,
        contentType: false,
        processData: false,
        beforeSend: function (xhr) {
            xhr.setRequestHeader("XSRF-TOKEN",
                $('input:hidden[name="__RequestVerificationToken"]').val());
        },
        success: function (data) {
            console.log(data)
            var html = '';
           
            var frase = "";
            if (data.status) {
                html = '<div class="d-flex justify-content-around">';
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
                                <p class="card-text text-center">'+ frase + '</span></p>\
                            </div >\
                            <div class="d-flex justify-content-center"><a type="button" data-idcampanha="' + this.idCampanha + '" class="btn btn-success mb-4 download" href="' + this.caminhoArquivo + this.nomeVideo + '" download>Download do Vídeo</a></div>\
                        </div>';

                });
                html += '</div>';
            }
            if (!data.status) {
                html = '<div class="jumbotron mt-3">\
                                <h1 class="text-center"> <font style="vertical-align: inherit;"><font style="vertical-align: inherit;">Não encontramos vídeos para essa Campanha.</font></font></h1 >\
                                <p class="lead text-center"><font style="vertical-align: inherit;"><font style="vertical-align: inherit;">Para mais informações contate o Administrador do Sistema.</font></font></p>\
                                <!--<a class="btn btn-lg btn-primary" href="/docs/4.6/components/navbar/" role="button"><font style="vertical-align: inherit;"><font style="vertical-align: inherit;">Voltar para pagina anterior »</font></font></a>-->\
                                <div class="d-flex justify-content-around">\
                                    <button type="reset" class="btn btn-lg btn-primary" onclick="javascript:history.go(-1)">Voltar para pagina anterior »</button>\
                                </div>\
                            </div>';
            }
            $('#videos').html(html);
        },
        error: function () {
            swal("Erro!", "Erro ao buscar o registro, contate o Administrador do Sistema.", "error");
        }
    });
}


    //$.getJSON('/Videos?handler=ListarVideos', function (data) {
    //    console.log(data)
    //    var html = '<div class="d-flex justify-content-around">';
    //    var frase = "";
    //    $('#videos').html('');
    //    if (data.status) {
    //        $.each(data.entityVideo, function (index, value) {
    //            if (this.nomeVideo.toUpperCase().indexOf('FEED') > -1) {
    //                frase = '<span class="font-weight-bold">Vídeo para FEED</span>';
    //            } else if (this.nomeVideo.toUpperCase().indexOf('STORES') > -1) {
    //                frase = '<span class="font-weight-bold">Vídeo para Stories</span>';
    //            }
    //            html += '<div class="card" style="width: 20.5rem;" >\
    //                        <div class="p-3">\
    //                            <iframe src="https://www.youtube.com/embed/'+ this.identificadorVideo + '"?feature=oembed" frameborder="0" allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture" allowfullscreen="">\
    //                            </iframe>\
    //                        </div>\
    //                        <div class="card-body">\
    //                            <p class="card-text text-center">'+ frase +'</span></p>\
    //                        </div >\
    //                        <div class="d-flex justify-content-center"><a type="button" class="btn btn-success mb-4" href="' + this.caminhoArquivo + this.nomeVideo + '" download>Download do Vídeo</a></div>\
    //                    </div>';
           
    //        });
    //    }
    //    html += '</div>';
    //    $('#videos').html(html);
    //});
}
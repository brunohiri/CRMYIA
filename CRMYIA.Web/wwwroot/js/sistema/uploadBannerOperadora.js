
//Alterar
var idbanner = '';
var nomearquivo = '';
var caminho = '';
$(document).ready(function () {

    // Get the template HTML and remove it from the doumenthe template HTML and remove it from the doument
    var previewNode = document.querySelector("#template");
    previewNode.id = "";
    var previewTemplate = previewNode.parentNode.innerHTML;
    previewNode.parentNode.removeChild(previewNode);

    $("#upload-banner-operadora").dropzone({
        paramName: "files", // The name that will be used to transfer the file
        maxFiles: 2000,
        parallelUploads: 128,
        url: "/UploadBannerOperadora?handler=UploadBannerOperadora",
        previewsContainer: "#imageUpload", // Define the container to display the previews
        //params: JSON.stringify(obj),
        acceptedFiles: '.png,.jpg,.jpeg,.gif',
        autoProcessQueue: false,
        uploadMultiple: true,
        previewTemplate: previewTemplate,
        clickable: ".fileinput-button", // Define the element that should be used as click trigger to select files.
        init: function () {

            var myDropzone = this;

            // Update selector to match your button
            $('#btn-upload-banner-operadora').click(function (e) {
                e.preventDefault();
                if ($('#Descricao').val() != "") {
                    myDropzone.processQueue();
                } else {
                    console.log(myDropzone)
                }
            });

            this.on('sending', function (file, xhr, formData) {
                if (VerificaNomeArquivo(file.name)) {
                    //formData.append('IdAssinaturaCartao', $('#IdAssinaturaCartao').val());
                    //formData.append('Titulo', $('#Titulo').val());
                    $('#Ativo').is(":checked") == true ? formData.append("Ativo", 'true') : formData.append("Ativo", 'false');

                    if (Number.isInteger(parseInt($('#IdCalendario').val()))) {
                        formData.append("IdCalendario", $('#IdCalendario').val());
                    } else {
                        formData.append("IdCalendario", 0);
                    }
                } else {
                    myDropzone.removeAllFiles(true);
                    swal("Erro!", "<span>Nome do arquivo não esta no padrão!</span><br><span>Exemplo:</span><br> <span>NOME_DO_ARQUIVO - REDES_SOCIAIS - [LOCAL_DA_POSTAGEM].EXTENSAO</span>", "error");
                }
            });

            //this.on("maxfilesexceeded", function (file) {
            //    this.removeAllFiles();
            //    this.addFile(file);
            //});

            this.on("success", function (files, data) {
                // Gets triggered when the files have successfully been sent.
                $('#modalAlterarImagem').modal('hide');
                if (data.status) {
                    myDropzone.removeAllFiles(true);
                    $('#tableCapaRedeSocial').html('');
                    CarregarTabela(data.entityLista);

                    $('#UploadCapaRedeSoCialMensagemDiv').show();
                    $('#UploadCapaRedeSoCialMensagemDivAlert').removeClass();
                    $('#UploadCapaRedeSoCialMensagemDivAlert').addClass(data.mensagem.cssClass);

                    $('#UploadCapaRedeSoCialMensagemIcon').removeClass();
                    $('#UploadCapaRedeSoCialMensagemIcon').addClass(data.mensagem.iconClass);
                    $('#UploadCapaRedeSoCialMensagemSpan').text(data.mensagem.mensagem);
                    document.getElementById("upload-banner-operadora").reset();
                    $("#IdOperadora").select2('val', '0');
                }
                setTimeout(function () { $('#UploadCapaRedeSoCialMensagemDiv').css('display', 'none'); }, 5000);

            });
        },
        //success: function (file, response) {
        //    console.log(response);
        //}
    });


    //Alterar
    // Get the template HTML and remove it from the doumenthe template HTML and remove it from the doument
    var previewNode = document.querySelector("#template-alterar");
    previewNode.id = "";
    var previewTemplate = previewNode.parentNode.innerHTML;
    previewNode.parentNode.removeChild(previewNode);

    $("#alterar-upload-banner-operadora").dropzone({
        paramName: "file", // The name that will be used to transfer the file
        maxFiles: 1,
        parallelUploads: 1,
        url: "/UploadBannerOperadora?handler=AlterarImagem",
        previewsContainer: "#imageUploadAlterar", // Define the container to display the previews
        //params: JSON.stringify(obj),
        acceptedFiles: '.png,.jpg,.jpeg,.gif',
        autoProcessQueue: false,
        uploadMultiple: false,
        previewTemplate: previewTemplate,
        clickable: ".fileinput-alterar", // Define the element that should be used as click trigger to select files.
        init: function () {

            var myAlterarDropzone = this;

            // Update selector to match your button
            $('#btn-alterar-upload-banner-operadora').click(function (e) {
                e.preventDefault();
                myAlterarDropzone.processQueue();
                //if ($('#Descricao').val() != "") {
                //    myAlterarDropzone.processQueue();
                //} else {
                //    console.log(myAlterarDropzone)
                //}
            });

            this.on('sending', function (file, xhr, formData) {
                formData.append("IdBanner", idbanner);
                formData.append("NomeArquivo", nomearquivo);
                $('#Ativo').is(":checked") == true ? formData.append("Ativo", 'true') : formData.append("Ativo", 'false');
            });
            this.on("success", function (files, data) {
                // Gets triggered when the files have successfully been sent.
                console.log(data);

                if (data.status) {
                    myAlterarDropzone.removeAllFiles(true);
                    $('#tableCamapanhaGenerica').html('');
                    CarregarTabela(data.entityLista);

                    $('#ModalAlterarImagemMensagemDiv').show();
                    $('#ModalAlterarImagemMensagemDivAlert').removeClass();
                    $('#ModalAlterarImagemMensagemDivAlert').addClass(data.mensagem.cssClass);

                    $('#ModalAlterarImagemMensagemIcon').removeClass();
                    $('#ModalAlterarImagemMensagemIcon').addClass(data.mensagem.iconClass);
                    $('#ModalAlterarImagemMensagemSpan').text(data.mensagem.mensagem);
                }
                setTimeout(function () { $('#modalAlterarImagem').modal('hide'); }, 3000);

            });
        },
        //success: function (file, response) {
        //    console.log(response);
        //}
    });

    ExisteDataSazonal();

});

$(document).on('change', '#ExisteDataSazonal', function () {
    ExisteDataSazonal();

});

$(document).on('click', '.alterar-imagem', function () {
    $('#modalAlterarImagem').modal();

    $('#modalLabelAlterarImagem').text('Alterar Imagem');

    $('#imagem').attr("src", $(this).data('caminho') + $(this).data('nomearquivo'));

    /*console.log($(this).data('idcampanhaarquivo') + ' | ' + $(this).data('nomearquivo') + ' | ' + $(this).data('caminhoimagem'))*/
    idbanner = $(this).data('idbanner');
    nomearquivo = $(this).data('nomearquivo');
    caminho = $(this).data('caminho');
});

$(document).on('click', '.alterar-titulo', function () {
    var idbanner = $(this).data('idbanner');
    formData = new FormData();
    formData.append('IdBanner', idbanner);
    if (idbanner != undefined) {
        $.ajax({
            type: 'POST',
            url: "/UploadBannerOperadora?handler=Obter",
            data: formData,
            cache: false,
            contentType: false,
            processData: false,
            beforeSend: function (xhr) {
                xhr.setRequestHeader("XSRF-TOKEN",
                    $('input:hidden[name="__RequestVerificationToken"]').val());
            },
            success: function (data) {
                if (data.status) {
                    console.log(data);
                    $('#IdInformacao').val(data.entityInformacao.idInformacao);
                    $('#Descricao').val(data.entityInformacao.descricao);
                    $('#IdBanner').val(data.entityLista.idBanner);
                    $("#IdCampanha").val(data.entityLista.idCampanha).trigger('change');
                    $('#Descricao').focus();
                    $('.btnsalvar').addClass('d-none');
                    $('#add-btn').html('');
                    $('#add-btn').html('<button type="button" id="btn-salvar-texto" class="btn btn-success">Salvar</button>');
                }
            },
            error: function () {
                alert("Error occurs");
            }
        });
    }

});

$(document).on('click', '#btn-salvar-texto', function () {

    formData = new FormData();
    formData.append('IdBanner', $('#IdBanner').val());
    formData.append('Descricao', $('#Descricao').val());
    formData.append('IdOperadora', $("#IdOperadora").val());
    formData.append('IdInformacao', $("#IdInformacao").val())
    $('#Ativo').is(":checked") == true ? formData.append("Ativo", 'true') : formData.append("Ativo", 'false');
    if ($('#IdBanner').val() != undefined && $('#Descricao').val() != undefined && $("#IdOperadora").val() != undefined)
    $.ajax({
        type: 'POST',
        url: "/UploadBannerOperadora?handler=UploadBannerOperadora",
        data: formData,
        cache: false,
        contentType: false,
        processData: false,
        beforeSend: function (xhr) {
            xhr.setRequestHeader("XSRF-TOKEN",
                $('input:hidden[name="__RequestVerificationToken"]').val());
        },
        success: function (data) {
            if (data.status) {

                $('#add-btn').html('');

                $('.btnsalvar').removeClass('d-none');
                $('.btnsalvar').addClass('d-block');
               
                $('#tableCapaRedeSocial').html('');
                CarregarTabela(data.entityLista);

                $('#UploadCapaRedeSoCialMensagemDiv').show();
                $('#UploadCapaRedeSoCialMensagemDivAlert').removeClass();
                $('#UploadCapaRedeSoCialMensagemDivAlert').addClass(data.mensagem.cssClass);

                $('#UploadCapaRedeSoCialMensagemIcon').removeClass();
                $('#UploadCapaRedeSoCialMensagemIcon').addClass(data.mensagem.iconClass);
                $('#UploadCapaRedeSoCialMensagemSpan').text(data.mensagem.mensagem);
                document.getElementById("upload-banner-operadora").reset();
                $("#IdOperadora").select2('val', '0');
                $("#IdOperadora").select2({
                    placeholder: "Selecione...",
                    allowClear: true
                });
            }
            setTimeout(function () { $('#UploadCapaRedeSoCialMensagemDiv').css('display', 'none'); }, 5000);
        },
        error: function () {
            alert("Error occurs");
        }
    });
});

$(document).on('click', '.excluir-imagem', function () {

    var idbanner = $(this).data('idbanner');
    swal({
        title: "Você tem certeza?",
        text: "Que deseja Excluir a Imagem.",
        type: "warning",
        showCancelButton: !0,
        confirmButtonText: "Sim!",
        cancelButtonText: "Não, cancelar!",
        reverseButtons: !0,
        confirmButtonColor: "#198754",
        cancelButtonColor: "#DC3545"
    }).then(function (e) {
        if (e.value === true) {
            formData = new FormData();
            formData.append('IdBanner', idbanner);
            $.ajax({
                type: 'POST',
                url: "/UploadBannerOperadora?handler=ExcluirImagem",
                data: formData,
                cache: false,
                contentType: false,
                processData: false,
                beforeSend: function (xhr) {
                    xhr.setRequestHeader("XSRF-TOKEN",
                        $('input:hidden[name="__RequestVerificationToken"]').val());
                },
                success: function (data) {
                    if (data.status) {
                        swal("Sucesso!", data.mensagem, "success");
                        CarregarTabela(data.entityLista);
                    }
                },
                error: function () {
                    alert("Error occurs");
                }
            });

        } else {
            e.dismiss;
        }

    }, function (dismiss) {
        return false;
    });
});

function ExisteDataSazonal() {
    if ($('#ExisteDataSazonal').is(":checked") == true) {
        $('#EstadoExisteDataSazonal').html('Sim');
        $('#BlocoDataSazonal').css('display', 'block');
    } else {
        $('#EstadoExisteDataSazonal').html('Não');
        $('#BlocoDataSazonal').css('display', 'none');
    }
}

function CarregarTabela(data) {
    var html = '';
    if (data) {
        html += '<div class="card-body table-responsive">\
                        <table id="tableListagem" class="table table-striped table-hover dt-responsive display nowrap datatable" style = "width:100%">\
                            <thead>\
                                <tr>\
                                    <th></th>\
                                    <th>Imagem</th>\
                                    <!--<th>Descricao</th>-->\
                                    <th>Cadastro</th>\
                                    <th>Status</th>\
                                    <th></th>\
                                </tr>\
                            </thead>\
                            <tbody>';
        $.each(data, function (index, value) {
            var ativo = this.ativo = 'true' ? '<span class="badge badge-pill badge-success">ATIVO</span>' : '<span class="badge badge-pill badge-warning">DESATIVADO</span>';
            html += '<tr>\
                        <td></td>\
                        <td><img src="' + this.caminhoArquivo + this.nomeArquivo + '" style="max-width: 100px!important;" alt="Capa"></td>\
                        <!--<td>'+ this.descricao + '</td>-->\
                        <td>'+ this.dataCadastro + '</td>\
                        <td>'+ ativo + '</td>\
                        <td>\
                            <span class="btn alterar-titulo" title="Editar Titulo" data-idbanner="' + this.idBanner + '"><i class="icon fas fa-edit"></i></span>\
                            <span class="btn alterar-imagem" title="Editar Imagem" data-idbanner="' + this.idBanner + '" data-nomearquivo="' + this.nomeArquivo + '" data-caminho="' + this.caminhoArquivo + '"><i class="fas fa-images"></i></span>\
                            <span class="btn excluir-imagem" title="Excluir Imagem" data-idbanner="' + this.idBanner + '" data-nomearquivo="' + this.nomeArquivo + '"><i class="fas fa-trash-alt"></i></span>\
                        </td >\
                    </tr>';
        });
        html += '</tbody>\
                    </table>\
                </div>';

        $('#tableBannerOperadora').html(html);
        InitDatatables();
    }
}
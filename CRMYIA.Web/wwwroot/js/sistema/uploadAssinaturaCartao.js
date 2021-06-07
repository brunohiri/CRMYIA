
//Alterar
var idassinaturacartao = '';
var nomearquivo = '';
var caminho = '';

$(document).ready(function () {


    // Get the template HTML and remove it from the doumenthe template HTML and remove it from the doument
    var previewNode = document.querySelector("#template");
    previewNode.id = "";
    var previewTemplate = previewNode.parentNode.innerHTML;
    previewNode.parentNode.removeChild(previewNode);

    $("#upload-assinatura-cartao").dropzone({
        paramName: "files", // The name that will be used to transfer the file
        maxFiles: 1,
        parallelUploads: 128,
        url: "/UploadAssinaturaCartao?handler=UploadAssinaturaCartao",
        previewsContainer: "#imageUpload", // Define the container to display the previews
        //params: JSON.stringify(obj),
        acceptedFiles: '.png,.jpg,.jpeg,.gif',
        autoProcessQueue: false,
      /*  uploadMultiple: true,*/
        previewTemplate: previewTemplate,
        clickable: ".fileinput-button", // Define the element that should be used as click trigger to select files.
        init: function () {

            var myDropzone = this;

            // Update selector to match your button
            $('#btn-upload-assinatura-cartao').click(function (e) {
                e.preventDefault();
                if ($('#Descricao').val() != "") {
                    myDropzone.processQueue();
                } else {
                    console.log(myDropzone)
                }
            });

            this.on('sending', function (file, xhr, formData) {
                if (VerificaNomeArquivoAssinaturaCartao(file.name)) {
                    //formData.append('IdAssinaturaCartao', $('#IdAssinaturaCartao').val());
                    //formData.append('Titulo', $('#Titulo').val());
                    //formData.append("IdCampanha", $('#IdCampanha').val());
                    $('#Ativo').is(":checked") == true ? formData.append("Ativo", 'true') : formData.append("Ativo", 'false');

                    if (Number.isInteger(parseInt($('#IdCalendario').val()))) {
                        formData.append("IdCalendario", $('#IdCalendario').val());
                    } else {
                        formData.append("IdCalendario", 0);
                    }
                } else {
                    myDropzone.removeAllFiles(true);
                    swal("Erro!", "<span>Nome do arquivo não esta no padrão!</span><br><span>Exemplo:</span><br> <span>NOME_DO_ARQUIVO - TEMA_DA_ASSINATURA.EXTENSAO</span>", "error");
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
                    document.getElementById("upload-assinatura-cartao").reset();
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

    $("#alterar-upload-assinatura-cartao").dropzone({
        paramName: "file", // The name that will be used to transfer the file
        maxFiles: 1,
        parallelUploads: 1,
        url: "/UploadAssinaturaCartao?handler=AlterarImagem",
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
            $('#btn-alterar-upload-assinatura-cartao').click(function (e) {
                e.preventDefault();
                myAlterarDropzone.processQueue();
                //if ($('#Descricao').val() != "") {
                //    myAlterarDropzone.processQueue();
                //} else {
                //    console.log(myAlterarDropzone)
                //}
            });

            this.on('sending', function (file, xhr, formData) {
                formData.append("IdAssinaturaCartao", idassinaturacartao);
                formData.append("NomeArquivo", nomearquivo);
                formData.append("IdCampanha", $('#IdCampanha').val());
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
    idassinaturacartao = $(this).data('idassinaturacartao');
    nomearquivo = $(this).data('nomearquivo');
    caminho = $(this).data('caminho');
});

$(document).on('click', '.alterar-titulo', function () {
    var idassinaturacartao = $(this).data('idassinaturacartao');
    formData = new FormData();
    formData.append('IdAssinaturaCartao', idassinaturacartao);
    if (idassinaturacartao != undefined) {
        $.ajax({
            type: 'POST',
            url: "/UploadAssinaturaCartao?handler=Obter",
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
                    $('#IdAssinaturaCartao').val(data.entityLista.idAssinaturaCartao);
                    $('#Titulo').val(data.entityLista.titulo);
                    $("#IdCampanha").val(data.entityLista.idCampanha).trigger('change');
                    $('#Titulo').focus();

                    $('.salvar-texto').css('display', 'block');
                    $('.upload-assinatura-cartao').css('display', 'none');
                    $('.btn-adicionar-imagem').css('display', 'none');
                }
            },
            error: function () {
                swal("Erro!", "Erro ao buscar o registro, contate o Administrador do Sistema.", "error");
            }
        });
    }

});

$(document).on('click', '#btn-salvar-texto', function () {

    formData = new FormData();
    formData.append('IdAssinaturaCartao', $('#IdAssinaturaCartao').val())
    formData.append('Titulo', $('#Titulo').val());
    $('#Ativo').is(":checked") == true ? formData.append("Ativo", 'true') : formData.append("Ativo", 'false');
    if ($("#Titulo").val() != undefined)
        displayBusyIndicator()
    $.ajax({
        type: 'POST',
        url: "/UploadAssinaturaCartao?handler=UploadAssinaturaCartao",
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

                $('#tableAssinaturaCartao').html('');
                CarregarTabela(data.entityLista);

                $('#UploadCapaRedeSoCialMensagemDiv').show();
                $('#UploadCapaRedeSoCialMensagemDivAlert').removeClass();
                $('#UploadCapaRedeSoCialMensagemDivAlert').addClass(data.mensagem.cssClass);

                $('#UploadCapaRedeSoCialMensagemIcon').removeClass();
                $('#UploadCapaRedeSoCialMensagemIcon').addClass(data.mensagem.iconClass);
                $('#UploadCapaRedeSoCialMensagemSpan').text(data.mensagem.mensagem);
                document.getElementById("upload-assinatura-cartao").reset();
                $("#IdCampanha").select2('val', '0');
                $("#IdCampanha").select2({
                    placeholder: "Selecione...",
                    allowClear: true
                });

                $('.salvar-texto').css('display', 'none');
                $('.upload-assinatura-cartao').css('display', 'block');
                $('.btn-adicionar-imagem').css('display', 'block');
            }
            setTimeout(function () { $('#UploadCapaRedeSoCialMensagemDiv').css('display', 'none'); }, 5000);
        },
        error: function () {
            swal("Erro!", "Erro ao alterar o registro, contate o Administrador do Sistema.", "error");
        }
    });
    displayBusyAvailable()
});

$(document).on('click', '.excluir-imagem', function () {

    var idassinaturacartao = $(this).data('idassinaturacartao');
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
            formData.append('IdAssinaturaCartao', idassinaturacartao);
            $.ajax({
                type: 'POST',
                url: "/UploadAssinaturaCartao?handler=ExcluirImagem",
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
        $('#tableAssinaturaCartao').html(html);
        html += '<div class="card-body table-responsive">\
                        <table id="tableListagem" class="table table-striped table-hover dt-responsive display nowrap datatable" style = "width:100%">\
                            <thead>\
                                <tr>\
                                    <th></th>\
                                    <th>Imagem</th>\
                                    <th>Titulo</th>\
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
                        <td>'+ this.titulo + '</td>\
                        <td>'+ this.dataCadastro + '</td>\
                        <td>'+ ativo + '</td>\
                        <td>\
                            <span class="btn alterar-titulo" title="Editar Titulo" data-idassinaturacartao="' + this.idAssinaturaCartao + '"><i class="icon fas fa-edit"></i></span>\
                            <span class="btn alterar-imagem" title="Editar Imagem" data-idassinaturacartao="' + this.idAssinaturaCartao + '" data-nomearquivo="' + this.nomeArquivo + '" data-caminho="' + this.caminhoArquivo + '"><i class="fas fa-images"></i></span>\
                            <span class="btn excluir-imagem" title="Excluir Imagem" data-idassinaturacartao="' + this.idAssinaturaCartao + '" data-nomearquivo="' + this.nomeArquivo + '"><i class="fas fa-trash-alt"></i></span>\
                        </td >\
                    </tr>';
        });
        html += '</tbody>\
                    </table>\
                </div>';

        $('#tableAssinaturaCartao').html(html);
        InitDatatables();
    }
}
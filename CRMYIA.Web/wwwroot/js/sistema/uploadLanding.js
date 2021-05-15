//Alterar
var IdLandingPageCarrossel = '';
var nomearquivo = '';
var caminhoimagem = '';

$(document).ready(function () {
    $(".select").select2({
        minimumResultsForSearch: Infinity
    });
    //Dropzone.autoDiscover = false;


    $("#btnUploadLandingCarrossel").click(function (e) {
        e.preventDefault();
        //myDropzone.autoProcessQueue();
    });

    // Get the template HTML and remove it from the doumenthe template HTML and remove it from the doument
    var previewNode = document.querySelector("#template");
    previewNode.id = "";
    var previewTemplate = previewNode.parentNode.innerHTML;
    previewNode.parentNode.removeChild(previewNode);

    var obj = {};
    $("#uploadLandingCarrossel").dropzone({
        paramName: "files", // The name that will be used to transfer the file
        maxFiles: 2000,
        parallelUploads: 128,
        url: "/UploadLanding?handler=UploadLandingPage",
        previewsContainer: "#imageUpload", // Define the container to display the previews
        params: JSON.stringify(obj),
        acceptedFiles: '.png,.jpg,.jpge,.gif',
        autoProcessQueue: false,
        uploadMultiple: true,
        previewTemplate: previewTemplate,
        clickable: ".fileinput-button", // Define the element that should be used as click trigger to select files.
        init: function () {

            var myDropzone = this;

            // Update selector to match your button
            $('#btnUploadLandingCarrossel').click(function (e) {
                e.preventDefault();
                if ($('#Descricao').val() != "") {
                    myDropzone.processQueue();
                } else {
                    console.log(myDropzone)
                }
            });

            this.on('sending', function (file, xhr, formData) {
                //if (VerificaNomeArquivo(file.name)) {
                    formData.append("IdLandingPageCarrossel", $('#IdLandingPageCarrossel').val());
                formData.append("IdUsuario", $('#IdUsuarioLand').val());
                    formData.append("Titulo", $('#Titulo').val());
                    $('#Ativo').is(":checked") == true ? formData.append("Ativo", 'true') : formData.append("Ativo", 'false');
                //} else {
                   // myDropzone.removeAllFiles(true);
                    //swal("Erro!", "<span>Nome do arquivo não esta no padrão!</span><br><span>Exemplo:</span><br> <span>NOME_DO_ARQUIVO - REDES_SOCIAIS - [LOCAL_DA_POSTAGEM].EXTENSAO</span>", "error");
                //}
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
                    $('#tableLandingCarrossel').html('');
                    CarregarTabela(data.entityLista);

                    $('#MensagemDiv').show();
                    $('#MensagemDivAlert').removeClass();
                    $('#MensagemDivAlert').addClass(data.mensagem.cssClass);

                    $('#MensagemIcon').removeClass();
                    $('#MensagemIcon').addClass(data.mensagem.iconClass);
                    $('#MensagemSpan').text(data.mensagem.mensagem);
                    document.getElementById("uploadLandingCarrossel").reset();
                    $("#IdUsuarioLand").select2('val', '0');
                }
                setTimeout(function () { $('#MensagemDiv').css('display', 'none'); $('#MensagemDiv').css('display', 'none'); }, 5000);

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

    $("#alterarUploadLandingCarrossel").dropzone({
        paramName: "file", // The name that will be used to transfer the file
        maxFiles: 2000,
        parallelUploads: 1,
        url: "/UploadLanding?handler=AlterarImagem",
        previewsContainer: "#imageUploadAlterar", // Define the container to display the previews
        //params: JSON.stringify(obj),
        acceptedFiles: '.png,.jpg,.jpge,.gif',
        autoProcessQueue: false,
        uploadMultiple: true,
        previewTemplate: previewTemplate,
        clickable: ".fileinput-alterar", // Define the element that should be used as click trigger to select files.
        init: function () {

            var myAlterarDropzone = this;

            // Update selector to match your button
            $('#btnAlterar').click(function (e) {
                e.preventDefault();
                myAlterarDropzone.processQueue();
                if ($('#Descricao').val() != "") {
                    myAlterarDropzone.processQueue();
                } else {
                    console.log(myAlterarDropzone)
                }
            });

            this.on('sending', function (file, xhr, formData) {
                //params: { IdLandingPageCarrossel: IdLandingPageCarrossel, NomeArquivo: nomearquivo },
                formData.append("IdLandingPageCarrossel", IdLandingPageCarrossel);
                formData.append("NomeArquivo", nomearquivo);
            });
            this.on("success", function (files, data) {
                // Gets triggered when the files have successfully been sent.
                console.log(data);

                if (data.status) {
                    myAlterarDropzone.removeAllFiles(true);
                    $('#tableLandingCarrossel').html('');
                    CarregarTabela(data.entityLista);

                    $('#ModalAlterarImagemMensagemDiv').show();
                    $('#ModalAlterarImagemMensagemDivAlert').removeClass();
                    $('#ModalAlterarImagemMensagemDivAlert').addClass(data.mensagem.cssClass);

                    $('#ModalAlterarImagemMensagemIcon').removeClass();
                    $('#ModalAlterarImagemMensagemIcon').addClass(data.mensagem.iconClass);
                    $('#ModalAlterarImagemMensagemSpan').text(data.mensagem.mensagem);

                }
                setTimeout(function () { $('#ModalAlterarImagemMensagemDiv').css('display', 'none'); $('#modalAlterarImagem').modal('hide'); }, 3000);

            });
        },
        //success: function (file, response) {
        //    console.log(response);
        //}
    });

});


$(document).on('click', '.alterar-imagem', function () {
    $('#modalAlterarImagem').modal();

    $('#modalLabelAlterarImagem').text('Alterar Imagem');

    $('#imagem').attr("src", $(this).data('caminhoimagem'));

    /*    console.log($(this).data('IdLandingPageCarrossel') + ' | ' + $(this).data('nomearquivo') + ' | ' + $(this).data('caminhoimagem'))*/
    IdLandingPageCarrossel = $(this).data('idlandcarrossel');
    nomearquivo = $(this).data('nomearquivo');
    caminhoimagem = $(this).data('caminhoimagem');
});

$(document).on('click', '.alterar-titulo', function () {
    var IdLandingPageCarrossel = $(this).data('IdLandingPageCarrossel');
    formData = new FormData();
    formData.append('IdLandingPageCarrossel', IdLandingPageCarrossel);
    if (IdLandingPageCarrossel != undefined) {
        displayBusyIndicator()
        $.ajax({
            type: 'POST',
            url: "/UploadLanding?handler=Obter",
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
                    $('#IdLandingPageCarrossel').val(data.entityLista.IdLandingPageCarrossel);
                    $('#Titulo').val(data.entityInformacao.titulo);
                    $("#IdUsuarioLand").val(data.entityLista.IdLandingPageCarrossel).trigger('change');
                    $('#Titulo').focus();

                    $('.salvar-texto').css('display', 'block');
                    $('.uploadLandingCarrossel').css('display', 'none');
                    $('.btn-adicionar-imagem').css('display', 'none');

                }
            },
            error: function () {
                swal("Erro!", "Erro ao buscar o registro, contate o Administrador do Sistema.", "error");
            }
        });
        displayBusyAvailable()
    }

});

$(document).on('click', '#btn-salvar-texto', function () {

    formData = new FormData();
    formData.append('IdLandingPageCarrossel', $('#IdLandingPageCarrossel').val());
    formData.append('IdUsuario', $("#IdUsuarioLand").val());
    formData.append('Titulo', $("#Titulo").val());
    $('#Ativo').is(":checked") == true ? formData.append("Ativo", 'true') : formData.append("Ativo", 'false');
    if ($('#IdLandingPageCarrossel').val() != undefined && $("#IdUsuarioLand").val() != undefined && $("#Titulo").val() != undefined)
        displayBusyIndicator()
    $.ajax({
        type: 'POST',
        url: "/UploadLanding?handler=UploadLandingPage",
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

                $('#tableLandingCarrossel').html('');
                CarregarTabela(data.entityLista);

                $('#MensagemDiv').show();
                $('#MensagemDivAlert').removeClass();
                $('#MensagemDivAlert').addClass(data.mensagem.cssClass);

                $('#MensagemIcon').removeClass();
                $('#MensagemIcon').addClass(data.mensagem.iconClass);
                $('#MensagemSpan').text(data.mensagem.mensagem);
                document.getElementById("uploadLandingCarrossel").reset();
                $("#IdUsuarioLand").select2('val', '0');
                $("#IdUsuarioLand").select2({
                    placeholder: "Selecione...",
                    allowClear: true
                });

                $('.salvar-texto').css('display', 'none');
                $('.uploadLandingCarrossel').css('display', 'block');
                $('.btn-adicionar-imagem').css('display', 'block');
            }
            setTimeout(function () { $('#MensagemDiv').css('display', 'none'); $('#MensagemDiv').css('display', 'none'); }, 5000);
        },
        error: function () {
            swal("Erro!", "Erro ao alterar o registro, contate o Administrador do Sistema.", "error");
        }
    });
    displayBusyAvailable()
});

$(document).on('click', '.excluir-imagem', function () {
    var idlandcarrossel = $(this).data('idlandcarrossel');
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
            formData.append('idlandcarrossel', idlandcarrossel);
            $.ajax({
                type: 'POST',
                url: "/UploadLanding?handler=ExcluirImagem",
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
                    swal("Erro!", "Erro ao excluir o registro, contate o Administrador do Sistema.", "error");
                }
            });

        } else {
            e.dismiss;
        }

    }, function (dismiss) {
        return false;
    });
});

function TiraEspaco(data) {
    var vet = data.split(" ");
    var nome = "";
    for (var i = 0; i < vet.length; i++) {
        if (vet.length > i + 1)
            nome += vet[i] + "-";
        else
            nome += vet[i];
    }
    return nome;
}

function CarregarTabela(data) {
    var html = '';
    if (data) {
        $('#tableLandingCarrossel').html(html);
        html += '<div class="card-body table-responsive">\
                        <table id="tableListagem" class="table table-striped table-hover dt-responsive display nowrap datatable" style = "width:100%">\
                            <thead>\
                                <tr>\
                                    <th></th>\
                                    <th>Imagem</th>\
                                    <th>Usuário</th>\
                                    <th>Cadastro</th>\
                                    <th>Status</th>\
                                    <th></th>\
                                </tr>\
                            </thead>\
                            <tbody>';
        $.each(data, function (index, value) {
            var ativo = this.ativo = 'true' ? '<span class="badge badge-pill badge-success">ATIVO</span>' : '<span class="badge badge-pill badge-warning">DESATIVADO</span>';
            html += '<tr>\
                        <td class="text-center"></td>\
                        <td class="text-center"><img src="' + this.caminhoArquivo + this.nomeArquivo + '" style="max-width: 100px!important;" alt=""></td>\
                        <td class="text-center">'+ this.idUsuarioNavigation.nome + '</td>\
                        <td class="text-center">'+ this.dataCadastro + '</td>\
                        <td class="text-center">'+ ativo + '</td>\
                        <td class="text-center">\
                            <span class="btn alterar-titulo" title="Editar Texto" data-idlandcarrossel="' + this.idLandingPageCarrossel + '"><i class="icon fas fa-edit"></i></span>\
                            <span class="btn alterar-imagem" title="Editar Imagem" data-idlandcarrossel="' + this.idLandingPageCarrossel + '" data-nomearquivo="' + this.nomeArquivo + '" data-caminhoimagem="' + this.caminhoArquivo + this.nomeArquivo + '"><i class="fas fa-images"></i></span>\
                            <span class="btn excluir-imagem" title="Excluir Imagem" data-idlandcarrossel="' + this.idLandingPageCarrossel + '" data-nomearquivo="' + this.nomeArquivo + '"><i class="fas fa-trash-alt"></i></span>\
                        </td >\
                    </tr>';
        });
        html += '</tbody>\
                    </table>\
                </div>';

        $('#tableLandingCarrossel').html(html);
        InitDatatables();
    }
}
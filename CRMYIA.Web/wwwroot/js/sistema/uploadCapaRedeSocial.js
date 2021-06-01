var IdRedeSocial = 0;
//Alterar
var idcapa = '';
var nomearquivo = '';
var caminho = '';

$(document).ready(function () {


    // Get the template HTML and remove it from the doumenthe template HTML and remove it from the doument
    var previewNode = document.querySelector("#template");
    previewNode.id = "";
    var previewTemplate = previewNode.parentNode.innerHTML;
    previewNode.parentNode.removeChild(previewNode);

    $("#upload-capa-rede-social").dropzone({
        paramName: "files", // The name that will be used to transfer the file
        maxFiles: 1,
        parallelUploads: 128,
        url: "/UploadCapaRedeSocial?handler=UploadCapaRedeSocial",
        previewsContainer: "#imageUpload", // Define the container to display the previews
        //params: JSON.stringify(obj),
        acceptedFiles: '.png,.jpg,.jpeg,.gif',
        autoProcessQueue: false,
       /* uploadMultiple: true,*/
        previewTemplate: previewTemplate,
        clickable: ".fileinput-button", // Define the element that should be used as click trigger to select files.
        init: function () {

            var myDropzone = this;

            // Update selector to match your button
            $('#btn-upload-capa-rede-social').click(function (e) {
                e.preventDefault();
                if ($('#Descricao').val() != "") {
                    myDropzone.processQueue();
                } else {
                    console.log(myDropzone)
                }
            });

            this.on('sending', function (file, xhr, formData) {
                if (VerificaNomeArquivo(file.name) && IdRedeSocial > 0) {
                    formData.append('IdRedeSocial', IdRedeSocial);
                    formData.append('Titulo', $('#Titulo').val());
                    $('#Ativo').is(":checked") == true ? formData.append("Ativo", 'true') : formData.append("Ativo", 'false');
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
                    document.getElementById("upload-capa-rede-social").reset();
                    $("#IdRedeSocial").select2('val', '0');
                    $("#IdRedeSocial").select2({
                        placeholder: "Selecione...",
                        allowClear: true
                    });

                    $("#IdCampanha").select2('val', '0');
                    $("#IdCampanha").select2({
                        placeholder: "Selecione...",
                        allowClear: true
                    });

                    $("#IdRedeSocial").select2();
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

    $("#alterar-upload-capa-rede-social").dropzone({
        paramName: "file", // The name that will be used to transfer the file
        maxFiles: 2000,
        parallelUploads: 1,
        url: "/UploadCapaRedeSocial?handler=AlterarImagem",
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
            $('#btn-alterar-upload-capa-rede-social').click(function (e) {
                e.preventDefault();
                myAlterarDropzone.processQueue();
                //if ($('#Descricao').val() != "") {
                //    myAlterarDropzone.processQueue();
                //} else {
                //    console.log(myAlterarDropzone)
                //}
            });

            this.on('sending', function (file, xhr, formData) {
                formData.append("IdCapa", idcapa);
                formData.append("NomeArquivo", nomearquivo);
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
                setTimeout(function () { $('#ModalAlterarImagemMensagemDiv').css('display', 'none'); }, 3000);
                setTimeout(function () { $('#modalAlterarImagem').modal('hide'); }, 5000);

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

    $('#imagem').attr("src", $(this).data('caminho') + $(this).data('nomearquivo'));

    /*console.log($(this).data('idcampanhaarquivo') + ' | ' + $(this).data('nomearquivo') + ' | ' + $(this).data('caminhoimagem'))*/
    idcapa = $(this).data('idcapa');
    nomearquivo = $(this).data('nomearquivo');
    caminho = $(this).data('caminho');
});

$(document).on('click', '.alterar-titulo', function () {
    var idcapa = $(this).data('idcapa');
    formData = new FormData();
    formData.append('IdCapa', idcapa);
    if (idcapa != undefined) {
        $.ajax({
            type: 'POST',
            url: "/UploadCapaRedeSocial?handler=Obter",
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
                    $('#IdCapa').val(data.entityCapa.idCapa);
                    $('#Titulo').val(data.entityCapa.titulo);
                    $("#IdRedeSocial").val(data.entityRedeSocial.idRedeSocial).trigger('change');
                    $("#IdCampanha").val(data.idCampanha).trigger('change');
                    $('#Titulo').focus();

                    $('.salvar-texto').css('display', 'block');
                    $('.upload-capa-rede-social').css('display', 'none');
                    $('.btn-adicionar-imagem').css('display', 'none');

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
    formData.append('IdCapa', $('#IdCapa').val());
    formData.append('Titulo', $('#Titulo').val());
    formData.append('IdRedeSocial', $("#IdRedeSocial").val());
    formData.append('IdCalendario', $('#IdCalendario').val());
    $('#Ativo').is(":checked") == true ? formData.append("Ativo", 'true') : formData.append("Ativo", 'false');
    if ($('#IdCapa').val() != undefined && $('#Titulo').val() != undefined && $("#IdRedeSocial").val() != undefined)
        displayBusyIndicator()
    $.ajax({
        type: 'POST',
        url: "/UploadCapaRedeSocial?handler=UploadCapaRedeSocial",
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

                $('#tableCapaRedeSocial').html('');
                CarregarTabela(data.entityLista);

                $('#UploadCapaRedeSoCialMensagemDiv').show();
                $('#UploadCapaRedeSoCialMensagemDivAlert').removeClass();
                $('#UploadCapaRedeSoCialMensagemDivAlert').addClass(data.mensagem.cssClass);

                $('#UploadCapaRedeSoCialMensagemIcon').removeClass();
                $('#UploadCapaRedeSoCialMensagemIcon').addClass(data.mensagem.iconClass);
                $('#UploadCapaRedeSoCialMensagemSpan').text(data.mensagem.mensagem);
                document.getElementById("upload-capa-rede-social").reset();
                $("#IdRedeSocial").select2('val', '0');
                $("#IdRedeSocial").select2({
                    placeholder: "Selecione...",
                    allowClear: true
                });

                $("#IdCampanha").select2('val', '0');
                $("#IdCampanha").select2({
                    placeholder: "Selecione...",
                    allowClear: true
                });

                $('.salvar-texto').css('display', 'none');
                $('.upload-capa-rede-social').css('display', 'block');
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


$(document).on('change','#IdRedeSocial', function () {
    IdRedeSocial = $(this).val();
});

$(document).on('click', '#btn-rede-social', function () {
    //var obj = {};
    //obj.IdRedeSocial = $('#IdRedeSocial').val();
    //obj.Nome = $('#Nome').val();
    //$('#RedeSocialAtivo').is(":checked") == true ? obj.Ativo = 'true' : obj.Ativo = 'false';
    formData = new FormData();
    formData.append('IdRedeSocial', $('#IdRedeSocial').val());
    formData.append('NomeRedeSocial', $('#NomeRedeSocial').val());
    $('#RedeSocialAtivo').is(":checked") == true ? formData.append('Ativo', 'true') : formData.append('Ativo', 'false');
    if ($('#NomeRedeSocial').val() != undefined && $('#NomeRedeSocial').val() != '') {
        $.ajax({
            type: 'POST',
            url: "/UploadCapaRedeSocial?handler=SalvarRedeSocial",
            data: formData,
            cache: false,
            contentType: false,
            processData: false,
            beforeSend: function (xhr) {
                xhr.setRequestHeader("XSRF-TOKEN",
                    $('input:hidden[name="__RequestVerificationToken"]').val());
            },
            success: function (data) {
                var html = '';
                
                if (data.status && data.entityRetorno != null) {
                    $('.idRedeSocial').html('');
                        html += '<div class="form-group">\
                                        <label for="IdRedeSocial">Selecione uma Rede</label>\
                                        <select class="form-control select2" style="width: 100%;" name="IdRedeSocial" id="IdRedeSocial" required>\
                                            <option selected disabled value="">Selecione...</option>';
                                    $.each(data.entityRetorno, function (key, value) {
                                        html += '<option value="' + this.idRedeSocial + '">' + this.nome + '</option>';
                                    });

                                html += '</select>\
                                        <div class="invalid-feedback">\
                                            * O Campo é Obrigatório. \
                                        </div >\
                                </div>';

                    //html += '<div class="col-sm-6">\
                    //            <div class="form-group">\
                    //                <label>&emsp;</label>\
                    //                <div class="custom-control custom-switch custom-switch-off-danger custom-switch-on-success">\
                    //                    <input type="checkbox" class="custom-control-input remover" id="' + TiraEspaco(Descricao) + '" name="' + TiraEspaco(Descricao) + '">\
                    //                    <label class="custom-control-label" for="' + TiraEspaco(Descricao) + '">Remover</label>\
                    //                </div>\
                    //            </div>\
                    //        </div >';
                    $('.idRedeSocial').append(html);
                    $('.select2').select2();
                    $('#modalCadastrarRedeSocial').modal('hide');
                    IdRedeSocial = 0;
                }
               
                document.getElementById("cadastrar-upload-capa-rede-social").reset();
               
            },
            error: function () {
                alert("Error occurs");
            }
        });
    }

});

$(document).on('click', '.excluir-imagem', function () {

    var idcapa = $(this).data('idcapa');
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
            formData.append('IdCapa', idcapa);
            $.ajax({
                type: 'POST',
                url: "/UploadCapaRedeSocial?handler=ExcluirImagem",
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

function CarregarTabela(data) {
   var html = '';
    if (data) {
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
                        <td>'+ ativo +'</td>\
                        <td>\
                            <span class="btn alterar-titulo" title="Editar Titulo e Rede Social" data-idcapa="' + this.idCapa + '"><i class="icon fas fa-edit"></i></span>\
                            <span class="btn alterar-imagem" title="Editar Imagem" data-idcapa="' + this.idCapa + '" data-nomearquivo="' + this.nomeArquivo + '" data-caminho="' + this.caminhoArquivo + '"><i class="fas fa-images"></i></span>\
                            <span class="btn excluir-imagem" title="Excluir Imagem" data-idcapa="' + this.idCapa + '" data-nomearquivo="' + this.nomeArquivo + '"><i class="fas fa-trash-alt"></i></span>\
                        </td >\
                    </tr>';
        });
                 html += '</tbody>\
                    </table>\
                </div>';

        $('#tableCapaRedeSocial').html(html);
            InitDatatables();
    }
}
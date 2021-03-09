var idvideo;
var nomevideo;

$(document).ready(function () {

    // Get the template HTML and remove it from the doumenthe template HTML and remove it from the doument
    var previewNode = document.querySelector("#template");
    previewNode.id = "";
    var previewTemplate = previewNode.parentNode.innerHTML;
    previewNode.parentNode.removeChild(previewNode);

    $("#upload-video").dropzone({
        paramName: "files", // The name that will be used to transfer the file
        maxFilesize: 40960, // MB
        maxFiles: 1,
        parallelUploads: 128,
        url: "/NovoVideo?handler=UploadVideo",
        previewsContainer: "#videoUpload", // Define the container to display the previews
        //params: JSON.stringify(obj),
        acceptedFiles: '.mp3,.mp4,.avi',
        autoProcessQueue: false,
        uploadMultiple: true,
        previewTemplate: previewTemplate,
        clickable: ".fileinput-video", // Define the element that should be used as click trigger to select files.
        init: function () {

            var myDropzone = this;

            // Update selector to match your button
            $('#btn-upload-video').click(function (e) {
                e.preventDefault();
                if ($('#IdentificadorVideo').val() != "") {
                    myDropzone.processQueue();
                } else {
                    console.log(myDropzone)
                }
            });

            this.on('sending', function (file, xhr, formData) {

                formData.append("IdentificadorVideo", $('#IdentificadorVideo').val());
                $('#Ativo').is(":checked") == true ? formData.append("Ativo", 'true') : formData.append("Ativo", 'false');
            });
            this.on("success", function (files, data) {
                // Gets triggered when the files have successfully been sent.
                $('#modalAlterarImagem').modal('hide');
                if (data.status) {
                    $("#upload-video")[0].reset();
                    $("#upload-video input:hidden").val('');
                    $('#Ativo').prop('checked', true);

                    $('#tableCamapanhaGenerica').html('');
                    CarregarTabela(data.entityLista);

                    $('#UploadCampanhaGenericaMensagemDiv').show();
                    $('#UploadCampanhaGenericaMensagemDivAlert').removeClass();
                    $('#UploadCampanhaGenericaMensagemDivAlert').addClass(data.mensagem.cssClass);
                    $('#UploadCampanhaGenericaMensagemIcon').removeClass();
                    $('#UploadCampanhaGenericaMensagemIcon').addClass(data.mensagem.iconClass);
                    $('#UploadCampanhaGenericaMensagemSpan').text(data.mensagem.mensagem);
                }
                setTimeout(function () { $('#UploadCampanhaGenericaMensagemDiv').css('display', 'none'); }, 5000);
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

    $("#alterar-upload-video").dropzone({
        paramName: "file", // The name that will be used to transfer the file
        maxFiles: 2000,
        parallelUploads: 1,
        url: "/NovoVideo?handler=AlterarVideo",
        previewsContainer: "#videoUploadAlterar", // Define the container to display the previews
        //params: JSON.stringify(obj),
        acceptedFiles: '.mp3,.mp4,.avi',
        autoProcessQueue: false,
        uploadMultiple: true,
        previewTemplate: previewTemplate,
        clickable: ".fileinput-alterar", // Define the element that should be used as click trigger to select files.
        init: function () {

            var myAlterarDropzone = this;

            // Update selector to match your button
            $('#btn-alterar-upload-video').click(function (e) {
                e.preventDefault();
                myAlterarDropzone.processQueue();
                if ($('#IdentificadorVideo').val() != "") {
                    myAlterarDropzone.processQueue();
                } else {
                    console.log(myAlterarDropzone)
                }
            });

            this.on('sending', function (file, xhr, formData) {
                //params: { IdCampanhaArquivo: idcampanhaarquivo, NomeArquivo: nomearquivo },
                formData.append("IdVideo", idvideo);
                formData.append("NomeVideo", nomevideo);
            });
            this.on("success", function (files, data) {
                // Gets triggered when the files have successfully been sent.
                console.log(data);

                if (data.status) {
                    $('#tableVideo').html('');
                    CarregarTabela(data.entityLista);

                    $('#ModalAlterarVideoMensagemDiv').show();
                    $('#ModalAlterarVideoMensagemDivAlert').removeClass();
                    $('#ModalAlterarVideoMensagemDivAlert').addClass(data.mensagem.cssClass);

                    $('#ModalAlterarVideoMensagemIcon').removeClass();
                    $('#ModalAlterarVideoMensagemIcon').addClass(data.mensagem.iconClass);
                    $('#ModalAlterarVideoMensagemSpan').text(data.mensagem.mensagem);
                }
                setTimeout(function () { $('#modalAlterarVideo').modal('hide'); }, 3000);

            });
        },
        //success: function (file, response) {
        //    console.log(response);
        //}
    });

});

$(document).on('click', '.alterar-video', function () {
    $('#modalAlterarVideo').modal();

    $('#modalLabelAlterarVideo').text('Alterar Video');

    $('#video').html('<iframe src="https://www.youtube.com/embed/' + $(this).data('identificadorvideo') + '"?feature=oembed" frameborder="0" allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture" allowfullscreen=""></iframe>')

    idvideo = $(this).data('idvideo');
    nomevideo = $(this).data('nomevideo');
});

$(document).on('click', '.excluir-video', function () {
    var obj = {};
    obj.IdVideo = $(this).data('idvideo');
    swal({
        title: "Você tem certeza?",
        text: "Que deseja Excluir o Video.",
        type: "warning",
        showCancelButton: !0,
        confirmButtonText: "Sim!",
        cancelButtonText: "Não, cancelar!",
        reverseButtons: !0,
        confirmButtonColor: "#198754",
        cancelButtonColor: "#DC3545"
    }).then(function (e) {
        if (e.value === true) {
            $.ajax({
                type: "POST",
                url: "/NovoVideo?handler=ExcluirVideo",
                beforeSend: function (xhr) {
                    xhr.setRequestHeader("XSRF-TOKEN",
                        $('input:hidden[name="__RequestVerificationToken"]').val());
                },
                data: JSON.stringify(obj),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    if (data.status) {
                        $('#tableVideo').html('');
                        CarregarTabela(data.entityLista);
                        swal("Sucesso!", data.mensagem, "success");
                    } else {
                        swal("Erro!", data.message, "error");
                    }
                },
                failure: function (data) {
                    console.log(data);
                }
            });
           
        } else {
            e.dismiss;
        }

    }, function (dismiss) {
        return false;
    });
});

$(document).on('click', '.editar-formulario', function () {
    //alert($(this).data('idvideo'));
    var obj = {};
    obj.IdVideo = $(this).data('idvideo');
    displayBusyIndicator();
    $.ajax({
        type: "POST",
        url: "/NovoVideo?handler=EditarFormulario",
        beforeSend: function (xhr) {
            xhr.setRequestHeader("XSRF-TOKEN",
                $('input:hidden[name="__RequestVerificationToken"]').val());
        },
        data: JSON.stringify(obj),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            if (data.status) {
               
                $('#IdVideo').val(data.entity.idVideo);
                $('#IdentificadorVideo').val(data.entity.identificadorVideo);

                if (data.entity.ativo == true) {
                    $('#Ativo').prop('checked', true);
                } else {
                    $('#Ativo').prop('checked', false);
                }
                $('#btn-upload-video').hide();
                $('.fileinput-video').hide();
                $('#salvar-alteracao-video').html('<button type="button" class="btn btn-primary" id="btn-salvar-alteracao-video">Salvar Alteração</button>' + $('#salvar-alteracao-video').html());
            }
            displayBusyAvailable()
        },
        failure: function (data) {
            console.log(data);
        }
    });
});


$(document).on('click', '#btn-salvar-alteracao-video', function () {

    var obj = {};
    obj.IdVideo = $('#IdVideo').val();
    obj.IdentificadorVideo = $('#IdentificadorVideo').val();
    $('#Ativo').is(":checked") == true ? obj.Ativo = 'true' : obj.Ativo = 'false';
    $.ajax({
        type: "POST",
        url: "/NovoVideo?handler=SalvarAlteracaoVideo",
        beforeSend: function (xhr) {
            xhr.setRequestHeader("XSRF-TOKEN",
                $('input:hidden[name="__RequestVerificationToken"]').val());
        },
        data: JSON.stringify(obj),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            if (data.status) {
                $("#upload-video")[0].reset();
                $("#upload-video input:hidden").val('');
                $('#Ativo').prop('checked', true);

                $('#tableVideo').html('');
                CarregarTabela(data.entityLista);
                $('#UploadCampanhaGenericaMensagemDiv').show();
                $('#UploadCampanhaGenericaMensagemDivAlert').removeClass();
                $('#UploadCampanhaGenericaMensagemDivAlert').addClass(data.mensagem.cssClass);
                $('#UploadCampanhaGenericaMensagemIcon').removeClass();
                $('#UploadCampanhaGenericaMensagemIcon').addClass(data.mensagem.iconClass);
                $('#UploadCampanhaGenericaMensagemSpan').text(data.mensagem.mensagem);
                $('.fileinput-video').show();
                $('#btn-salvar-alteracao-video').hide();
                $('#btn-upload-video').show();
                myDropzone.removeAllFiles(true);

                setTimeout(function () { $('#UploadCampanhaGenericaMensagemDiv').hide(); }, 3000);
            }
        },
        failure: function (data) {
            console.log(data);
        }
    });

});

function CarregarTabela(data) {
    var html = '<div class="card-body table-responsive">\
        <table id = "tableListagem" class="table table-striped table-hover dt-responsive display nowrap datatable" style = "width:100%">\
                        <thead>\
                            <tr>\
                                <th></th>\
                                <th>Identificador do Video</th >\
                                <th>Rede Sociais</th>\
                                <th>Local</th>\
                                <th>Nome do Arquivo</th>\
                                <th>Cadastro</th>\
                                <th>Status</th>\
                                <th></th>\
                            </tr>\
                        </thead>\
                        <tbody>';
    $.each(data, function (index, value) {
        ativo = this.ativo == true ? '<span class="badge badge-pill badge-success">ATIVADO</span>' : '<span class="badge badge-pill badge-warning">DESATIVADO</span>';
        html += '<tr>\
                    <td></td>\
                    <td>'+ this.identificadorVideo + '</td>\
                    <td>Facebook</td>\
                    <td>Stories</td>\
                    <td>'+ this.nomeVideo + '</td>\
                    <td>' + this.dataCadastro + '</td>\
                    <td>'+ ativo +'</td >\
                    <td>\
                        <span class="text-info editar-formulario"  title="Editar" data-idvideo="' + this.idVideo + '"><i class="fas fa-edit"></i></span>\
                        <span class="alterar-video" title="Editar Video" data-idvideo="' + this.idVideo + '" data-nomevideo="' + this.nomeVideo + '" data-identificadorvideo="' + this.identificadorVideo + '"><i class="fab fa-youtube"></i></span>\
                        <span class="btn excluir-video" title="Excluir Imagem" data-idvideo="' + this.idVideo + '"><i class="fas fa-trash-alt"></i></span>\
                    </td>\
                 </tr>';
    });
    html += '</tbody >\
          </table >\
        </div >';
    $('#tableVideo').html(html);
    InitDatatables();
}

function VideoUpload() {
    $('#videoUpload').html('');
    var html = '<div id="template" class="file-row dz-image-preview">\
                    <div>\
                        <p class="name" data-dz-name></p>\
                        <strong class="error text-danger" data-dz-errormessage></strong>\
                    </div >\
                    <div>\
                        <p class="size" data-dz-size></p>\
                        <div class="progress progress-striped active" role="progressbar" aria-valuemin="0" aria-valuemax="100" aria-valuenow="0">\
                            <div class="progress-bar progress-bar-success" style="width:0%;" data-dz-uploadprogress></div>\
                        </div>\
                    </div>\
                    <div>\
                        <button data-dz-remove class="btn btn-danger delete">\
                            <i class="glyphicon glyphicon-trash"></i>\
                            <span>Excluir</span>\
                        </button>\
                    </div>\
                </div>';
    $('#videoUpload').html(html);
}
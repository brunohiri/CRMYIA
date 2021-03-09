﻿var IdCampanhaReferencia = 0;
var IdCampanha = 0;

//Alterar
var idcampanhaarquivo = '';
var nomearquivo = '';
var caminhoimagem = '';

$(document).ready(function () {
    $(".select").select2({
        minimumResultsForSearch: Infinity
    });
    //Dropzone.autoDiscover = false;

    var obj = {};
    obj.IdCampanha = $('#IdCampanha').val();
    $('#Ativo').is(":checked") == true ? obj.Ativo = 'true' : obj.Ativo = 'false';
    obj.Descricao = $('#Descricao').val();
    obj.Observacao = $('#Observacao').val(); 

        $('#imageUpload').on('change', function () {
            alert('foi')
        });

        $("#IdCampanha").change(function () {
            IdCampanha = this.value;
        });

        $("#btn-upload-campanha-generica").click(function (e) {
            e.preventDefault();
            //myDropzone.autoProcessQueue();
        });

    // Get the template HTML and remove it from the doumenthe template HTML and remove it from the doument
    var previewNode = document.querySelector("#template");
    previewNode.id = "";
    var previewTemplate = previewNode.parentNode.innerHTML;
    previewNode.parentNode.removeChild(previewNode);

        $("#upload-campanha-generica").dropzone({
            paramName: "files", // The name that will be used to transfer the file
            maxFiles: 2000,
            parallelUploads: 128,
            url: "/UploadCampanhaGenerica?handler=UploadCampanhaGenerica",
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
                $('#btn-upload-campanha-generica').click(function (e) {
                    e.preventDefault();
                    if ($('#Descricao').val() != "") {
                        myDropzone.processQueue();
                    } else {
                        console.log(myDropzone)
                    }
                });

                this.on('sending', function (file, xhr, formData) {
                    if (VerificaNomeArquivo(file.name)) {
                        formData.append("IdCampanhaArquivo", $('#IdCampanhaArquivo').val());
                        formData.append("IdCampanha", $('#IdCampanha').val());
                        formData.append("Descricao", $('#Descricao').val());
                        formData.append("Observacao", $('#Observacao').val());
                        $('#Ativo').is(":checked") == true ? formData.append("Ativo", 'true') : formData.append("Ativo", 'false');
                        //formData.append("Width", file.width);
                        //formData.append("Height", file.height);
                    } else {
                        myDropzone.removeAllFiles(true);
                        swal("Erro!", "<span>Nome do arquivo não esta no padrão!</span><br><span>Exemplo:</span><br> <span>NOME_DO_ARQUIVO,REDES_SOCIAIS,LOCAL_DA_POSTAGEM,.EXTENSAO</span>", "error");
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
                        $('#tableCamapanhaGenerica').html('');
                        CarregarTabela(data.entityLista);

                        $('#UploadCampanhaGenericaMensagemDiv').show();
                        $('#UploadCampanhaGenericaMensagemDivAlert').removeClass();
                        $('#UploadCampanhaGenericaMensagemDivAlert').addClass(data.mensagem.cssClass);

                        $('#UploadCampanhaGenericaMensagemIcon').removeClass();
                        $('#UploadCampanhaGenericaMensagemIcon').addClass(data.mensagem.iconClass);
                        $('#UploadCampanhaGenericaMensagemSpan').text(data.mensagem.mensagem);
                        document.getElementById("upload-campanha-generica").reset();
                        $("#IdCampanha").select2(); 
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

    $("#alterar-upload-campanha-generica").dropzone({
        paramName: "file", // The name that will be used to transfer the file
        maxFiles: 2000,
        parallelUploads: 1,
        url: "/UploadCampanhaGenerica?handler=AlterarImagem",
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
            $('#btn-alterar-upload-campanha-generica').click(function (e) {
                e.preventDefault();
                myAlterarDropzone.processQueue();
                if ($('#Descricao').val() != "") {
                    myAlterarDropzone.processQueue();
                } else {
                    console.log(myAlterarDropzone)
                }
            });

            this.on('sending', function (file, xhr, formData) {
                //params: { IdCampanhaArquivo: idcampanhaarquivo, NomeArquivo: nomearquivo },
                formData.append("IdCampanhaArquivo", idcampanhaarquivo);
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
                setTimeout(function () { $('#modalAlterarImagem').modal('hide'); }, 3000);
                
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

    console.log($(this).data('idcampanhaarquivo') + ' | ' + $(this).data('nomearquivo') + ' | ' + $(this).data('caminhoimagem'))
    idcampanhaarquivo = $(this).data('idcampanhaarquivo');
    nomearquivo = $(this).data('nomearquivo');
    caminhoimagem = $(this).data('caminhoimagem');
});

$(document).on('click', '.excluir-imagem', function () {
    console.log($(this).data('idcampanhaarquivo') + ' | ' + $(this).data('nomearquivo'));
    var idcampanhaarquivo = $(this).data('idcampanhaarquivo');
    var nomearquivo = $(this).data('nomearquivo');
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
            $.ajax({
                type: "GET",
                url: "/UploadCampanhaGenerica?handler=ExcluirImagem&IdCampanhaArquivo=" + idcampanhaarquivo + "&NomeArquivo=" + nomearquivo,
                contentType: "application/json",
                dataType: "json",
                success: function (data) {
                    if (data.status) {
                        swal("Sucesso!", data.mensagem, "success");
                        CarregarTabela(data.entityLista);
                    }
                }
            });
        } else {
            e.dismiss;
        }

    }, function (dismiss) {
        return false;
    });

   

});

//function createButton(text, cb) {
//    return $('<button>' + text + '</button>').on('click', cb);
//}

function VerificaImagem() {
    var verifica;
   $.ajax("/UploadCampanhaGenerica?handler=VerificaImagem&id=" + 111).done(function (json) {
       //console.log(json);
       verifica = json;
    });

    //$.ajax({
    //    type: "GET",
    //    url: "/UploadCampanhaGenerica?handler=VerificaImagem&Id=" + Id,
    //    contentType: "application/json",
    //    dataType: "json",
    //    success: function (data) {
           
    //    }
    //});

    console.log(verifica.retorno);
}

$(document).on('change', '.IdCampanhaReferencia', function () {
    var obj = {};
    obj.IdCampanhaReferencia = this.value;
    IdCampanhaReferencia = this.value;
    var Descricao = this.selectedOptions[0].text;
   
    $.ajax({
        type: "POST",
        url: "/NovaCampanhaGenerica?handler=GetSubCategoria",
        beforeSend: function (xhr) {
            xhr.setRequestHeader("XSRF-TOKEN",
                $('input:hidden[name="__RequestVerificationToken"]').val());
        },
        data: JSON.stringify(obj),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var html = '';
            if (data.status && data.retorno != null) {
                html += '<div class="row ' + TiraEspaco(Descricao) + '">\
                        <div class="col-sm-6">\
                            <div class="form-group">\
                                        <label for="' + TiraEspaco(Descricao) + '">Sub-Categoria de '+ Descricao +'</label>\
                                        <select class="form-control select2 IdCampanhaReferencia" style="width: 100%;" id=' + TiraEspaco(Descricao) + '" required>\
                                        <option selected disabled value="">Selecione...</option>';
                                    $.each(data.retorno, function (key, value) {
                                        html += '<option value="' + this.idCampanha + '">' + this.descricao + '</option>';
                                    });

                html += '</select>\
                                    <div class="invalid-feedback">\
                                        * O Campo Sub-Categoria de '+ Descricao + ' é Obrigatório. \
                                         Caso não deseje informar o campo click em remover.\
                                    </div >\
                            </div >\
                        </div >';

                html += '<div class="col-sm-6">\
                            <div class="form-group">\
                                <label>&emsp;</label>\
                                <div class="custom-control custom-switch custom-switch-off-danger custom-switch-on-success">\
                                    <input type="checkbox" class="custom-control-input remover" id="' + TiraEspaco(Descricao) + '" name="' + TiraEspaco(Descricao) + '">\
                                    <label class="custom-control-label" for="' + TiraEspaco(Descricao) + '">Remover</label>\
                                </div>\
                            </div>\
                        </div >';

                html += '<div>';
            }
            $('#sub-categoria').append(html);
            $('.select2').select2();
        },
        failure: function (data) {
            console.log(response);
        }
    });
});



$(document).on('change', '.remover', function (e) {
    //alert(this.name)
    if ($('.remover').is(":checked")) {
        $('.' + this.name).remove();
        var classIdCampanhaReferencia = $('.IdCampanhaReferencia');
        var ele = classIdCampanhaReferencia[classIdCampanhaReferencia.length - 1];
        IdCampanhaReferencia = ele.value;
    }
   
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
    var html = '<div class="card-body table-responsive">\
        <table id = "tableListagem" class="table table-striped table-hover dt-responsive display nowrap datatable" style = "width:100%">\
                        <thead>\
                            <tr>\
                                <th></th>\
                                <th>Imagem</th>\
                                <th>Campanha</th>\
                                <th>Descrição</th>\
                                <th>Nome do Arquivo</th>\
                                <th>Cadastro</th>\
                                <th>Status</th>\
                                <th></th>\
                            </tr>\
                        </thead>\
                        <tbody>';
    $.each(data, function (index, value) {
        console.log(this.idCampanhaArquivo)

        var vetNomeImagem = this.nomeArquivo.split('|');
        var caminhoImagem = "";
        var categoria = "";
        var descricao = "";
        var ativo = "";

        if (vetNomeImagem.length == 1) {
            if (this.caminhoArquivo != undefined && this.nomeArquivo != undefined) {
                caminhoImagem = this.caminhoArquivo + this.nomeArquivo;
            }
            else {
                caminhoImagem = "/img/fotoCadastro/foto-cadastro.jpeg";
            }

            html += '<tr>\
                        <td class="text-center"></td>\
                        <td class="text-center">' + "<img src='" + caminhoImagem + "'  style=\"width: 50px!important;\" >" + '</td>\
                        <td class="text-center">' + this.idCampanhaNavigation.descricao + '</td>\
                        <td class="text-center" title="' + this.descricao + '">' + this.descricao == "" ? "<span class=\"badge badge-pill badge-warning\" title=\"NÃO INFORMADA\">NÃO INFORMADA</span>" : this.descricao + '</td>\
                        <td>' + this.nomeArquivo + '</td>\
                        <td>' + this.dataCadastro + '</td>\
                        <td class="text-center">' + this.ativo == true ? "<span title=\"ATIVADO\" class=\"badge badge-pill badge-success\">ATIVADO</span>" : " < span title =\"DESATIVADO\" class=\"badge badge-pill badge-warning\">DESATIVADO</span>" + '</td >\
                        <td class="text-center">\
                            <a href="/UploadCampanhaGenerica?id=' + this.idCampanhaArquivo + '" title="Editar" class="text-info"><i class="icon fas fa-edit"></i></a>\
                            <span class="alterar-imagem" title="Editar Imagem" data-idcampanhaarquivo="' + this.idCampanhaArquivo + '" data-nomearquivo="' + this.nomeArquivo + '" data-caminhoimagem="' + caminhoImagem + '"><i class="fas fa-images"></i></span>\
                            <span class="btn excluir-imagem" title="Excluir Imagem" data-idcampanhaarquivo="' + this.idCampanhaArquivo + '"><i class="fas fa-trash-alt"></i></span>\
                        </td>\
                      </tr >';
        }
        else if (vetNomeImagem.length > 1) {
            for (var i = 0; i < vetNomeImagem.length; i++) {

                var itemComTralha = "";
                if (vetNomeImagem[i] == "") {
                    itemComTralha = "#";
                }
                else {
                    itemComTralha = vetNomeImagem[i].substring(0, 1);
                }
                if (!(itemComTralha == "#")) {

                    if (this.caminhoArquivo != null && vetNomeImagem[i] != null) {
                        caminhoImagem = this.caminhoArquivo + vetNomeImagem[i];
                    }
                    else {
                        caminhoImagem = "/img/fotoCadastro/foto-cadastro.jpeg";
                    }

                    categoria = this.idCampanhaNavigation.idCampanhaReferencia != undefined ? "SubCategoria " + this.idCampanhaNavigation.descricao : "Categoria " + this.idCampanhaNavigation.descricao;
                    descricao = this.descricao == "" ? '<span class="badge badge-pill badge-warning" title="NÃO INFORMADA">NÃO INFORMADA</span>' : this.descricao;
                    ativo = this.ativo == true ? '<span title="ATIVADO" class="badge badge-pill badge-success">ATIVADO</span>' : '<span title ="DESATIVADO" class="badge badge-pill badge-warning">DESATIVADO</span>';
                    html += '<tr>\
                                                <td class="text-center"></td>\
                                                <td class="text-center"><img src="' + caminhoImagem + '"  style="width: 50px!important;"></td>\
                                                <td class="text-center">' + categoria + '</td>\
                                                <td class="text-center" title="' + vetNomeImagem[i].descricao + '">' + descricao + '</td>\
                                                <td>' + vetNomeImagem[i] + '</td>\
                                                <td>' + this.dataCadastro + '</td>\
                                                <td class="text-center">' + ativo + '</td >\
                                                <td class="text-center">\
                                                    <a href="/UploadCampanhaGenerica?id=' + this.idCampanhaArquivo + '" title="Editar" class="text-info"><i class="icon fas fa-edit"></i></a>\
                                                    <span class="btn alterar-imagem" title="Editar Imagem" data-idcampanhaarquivo="' + this.idCampanhaArquivo + '" data-nomearquivo="' + vetNomeImagem[i] + '" data-caminhoimagem="' + caminhoImagem + '"><i class="fas fa-images"></i></span>\
                                                    <span class="btn excluir-imagem" title="Excluir Imagem" data-idcampanhaarquivo="' + this.idCampanhaArquivo + '"><i class="fas fa-trash-alt"></i></span>\
                                                </td>\
                                            </tr >';
                }
            }//fim for
        }
    });
    html += '</tbody >\
          </table >\
        </div >';
    $('#tableCamapanhaGenerica').html(html);
    InitDatatables();
}
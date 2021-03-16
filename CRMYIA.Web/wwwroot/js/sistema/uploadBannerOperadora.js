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
        uploadMultiple: false,
        previewTemplate: previewTemplate,
        clickable: ".fileinput-button", // Define the element that should be used as click trigger to select files.
        init: function () {

            var myDropzone = this;

            // Update selector to match your button
            $('#btn-banner-operadora').click(function (e) {
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
                    document.getElementById("upload-assinatura-cartao").reset();
                }
                setTimeout(function () { $('#UploadCapaRedeSoCialMensagemDiv').css('display', 'none'); }, 5000);

            });
        },
        //success: function (file, response) {
        //    console.log(response);
        //}
    });

});
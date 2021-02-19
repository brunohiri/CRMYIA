var IdCampanhaReferencia = 0;
var IdCampanha = 0;
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
       
        $("#upload-campanha-generica").dropzone({
            paramName: "files", // The name that will be used to transfer the file
            maxFiles: 2000,
            parallelUploads: 128,
            url: "/UploadCampanhaGenerica?handler=UploadCampanhaGenerica",
            previewsContainer: "#imageUpload", // Define the container to display the previews
            params: JSON.stringify(obj),
            acceptedFiles: '.png,.jpg,.jpge,.gif',
            //autoProcessQueue: false,
            uploadMultiple: true,
            init: function () {

                var myDropzone = this;

                // Update selector to match your button
                $('#btn-upload-campanha-generica').click(function (e) {
                    e.preventDefault();
                    myDropzone.processQueue();
                });

                this.on('sending', function (file, xhr, formData) {

                    formData.append("IdCampanhaArquivo", $('#IdCampanhaArquivo').val());
                    formData.append("IdCampanha", $('#IdCampanha').val());
                    formData.append("Descricao", $('#Descricao').val());
                    formData.append("Observacao", $('#Observacao').val());
                    $('#Ativo').is(":checked") == true ? formData.append("Ativo", 'true') : formData.append("Ativo", 'false');

                    // Append all form inputs to the formData Dropzone will POST
                    //var data = $('#upload-campanha-generica').serializeArray();
                    //console.log(data)
                    //var obj = {};
                    //obj.IdCampanha = $('#IdCampanha').val();
                    //$('#Ativo').is(":checked") == true ? obj.Ativo = 'true' : obj.Ativo = 'false';
                    //obj.Descricao = $('#Descricao').val();
                    //obj.Observacao = $('#Observacao').val(); 
                    //$.ajax({
                    //    type: "POST",
                    //    url: "/UploadCampanhaGenerica?handler=UploadCampanhaGenerica",
                    //    beforeSend: function (xhr) {
                    //        xhr.setRequestHeader("XSRF-TOKEN",
                    //            $('input:hidden[name="__RequestVerificationToken"]').val());
                    //    },
                    //    data: JSON.stringify(obj),
                    //    contentType: "application/json; charset=utf-8",
                    //    dataType: "json",
                    //    success: function (data) {
                            
                    //    },
                    //    failure: function (data) {
                    //        console.log(response);
                    //    }
                    //});

                    //$.each(data, function (key, el) {
                    //    formData.append(el.name, el.value);
                    //});
                });
            },
            //success: function (file, response) {
            //    console.log(response);
            //}
        });

       
});

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

$(document).on('change', '#eSubCategoria', function () {
    if ($('#eSubCategoria').is(":checked")) {
        $('.categoria').removeClass('d-none');
        //$('.categoria').addClass('d-block');

        $.ajax({
            type: "POST",
            url: "/NovaCampanhaGenerica?handler=ListCampanha",
            beforeSend: function (xhr) {
                xhr.setRequestHeader("XSRF-TOKEN",
                    $('input:hidden[name="__RequestVerificationToken"]').val());
            },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                var html = '';
                if (data.status) {
                    html += '<div class="col-sm-6">\
                        <div class="form-group">\
                                    <label for="IdCampanhaReferencia">Campanha</label>\
                                    <select class="form-control select2 IdCampanhaReferencia" style="width: 100%;" id="IdCampanhaReferencia" name="IdCampanhaReferencia" required>\
                                        <option selected disabled value="">Selecione...</option>';
                                        $.each(data.retorno, function (key, value) {
                                            html += '<option value="' + this.idCampanha + '">' + this.descricao + '</option>';
                                        });
                            html += '</select>\
                                    <div class="invalid-feedback">\
                                * O Campo Campanha é Obrigatório. \
                                Caso não deseje informar o campo clique em remover.\
                                    </div>\
                                </div >\
                            </div >';

                    html += '<div class="col-sm-3">\
                                <div class="form-group">\
                                    <label>&emsp;</label>\
                                    <div class="custom-control custom-switch custom-switch-off-danger custom-switch-on-success">\
                                        <input type="checkbox" class="custom-control-input remover" id="categoria" name="categoria">\
                                            <label class="custom-control-label" for="categoria">Remover</label>\
                                        </div>\
                                    </div>\
                            </div>';
                    $('.categoria').append(html);
                    $('.select2').select2();
                }
            },
            failure: function (data) {
                console.log(response);
            }
        });
    } else {
        //$('.categoria').removeClass('d-block');
        $('.categoria').addClass('d-none');
    }
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

(function () {
    'use strict';
    window.addEventListener('load', function () {
        // Fetch all the forms we want to apply custom Bootstrap validation styles to
        var forms = document.getElementsByClassName('needs-validation');
        // Loop over them and prevent submission
        var validation = Array.prototype.filter.call(forms, function (form) {
            form.addEventListener('submit', function (event) {
                if (form.checkValidity() === false) {
                    event.preventDefault();
                    event.stopPropagation();
                } else {
                    
                    var dados = {};

                    dados.Descricao = $('#Descricao').val();
                    //dados.DataCadastro = $('#DataCadastro').val();
                    $('#Ativo').is(":checked") == true ? dados.Ativo = 'true' : dados.Ativo = 'false';
                    dados.IdCampanha = $("#IdCampanha").val();
                    dados.IdCampanhaReferencia = $('#IdCampanhaReferencia').val();
                    if (IdCampanhaReferencia > 0) {
                        dados.IdCampanhaReferencia = IdCampanhaReferencia;
                    }
                    //if ($('#eSubCategoria').is(":checked") && $("#IdCampanhaReferencia").length > 0){
                    //    obj.IdCampanhaReferencia = IdCampanhaReferencia;
                    //} else {
                    //    obj.IdCampanhaReferencia = undefined;
                    //}
                    $.ajax({
                        type: "POST",
                        url: "/NovaCampanhaGenerica?handler=Salvar",
                        beforeSend: function (xhr) {
                            xhr.setRequestHeader("XSRF-TOKEN",
                                $('input:hidden[name="__RequestVerificationToken"]').val());
                        },
                        data: JSON.stringify(dados),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (data) {
                            if (data.status) {
                                //$(".needs-validation").each(function () {
                                //    this.reset();
                                //});
                                $('.categoria').html('');
                                $('#sub-categoria').html('');
                                $('#CampanhaMensagemDiv').show();
                                $('#CampanhaMensagemDivAlert').removeClass();
                                $('#CampanhaMensagemDivAlert').addClass(data.mensagem.cssClass);

                                $('#CampanhaMensagemIcon').removeClass();
                                $('#CampanhaMensagemIcon').addClass(data.mensagem.iconClass);
                                $('#CampanhaMensagemSpan').text(data.mensagem.mensagem);
                                $('#form-salvar input').val("");

                                //setTimeout(function () { location.href = '/NovaCampanha/' }, 3000);
                            }
                        },
                        failure: function (data) {
                            console.log(response);
                        }
                    });

                    
                }
                form.classList.add('was-validated');
            }, false);
        });
    }, false);
})();
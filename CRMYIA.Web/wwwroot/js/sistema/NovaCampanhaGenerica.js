$(document).ready(function () {
});

$(document).on('click', '.btn-salvar-campanha',function (evt) {
    evt.preventDefault();

    var $form = $('#salvar-campanha'),
    params = $form.serializeArray(),
    formData = new FormData();

    if ($('#NomeArquivoCampanha')[0].files.length === 1) {
        var arquivo = $form.find('[name="NomeArquivoCampanha"]')[0].files;
        var arquivo_img = arquivo[0];
        
        formData.append('File', arquivo_img);
    }

    if ($("#foto").val() == "") {
        formData.append('File', '');
    }

    $.each(params, function (i, val) {
        formData.append(val.name, val.value);
    });

    var obj = {};
    obj.File = arquivo;
    $('#Ativo').is(":checked") == true ? obj.Ativo = 'true' : obj.Ativo = 'false';
    obj.Descricao = $('#Descricao').val()
 
    $.ajax({
        type: 'POST',
        url: "/NovaCampanhaGenerica?handler=Salvar",
        data: formData,
        cache: false,
        contentType: false,
        processData: false,
        beforeSend: function (xhr) {
            xhr.setRequestHeader("XSRF-TOKEN",
                $('input:hidden[name="__RequestVerificationToken"]').val());
        },
        success: function (repo) {
            document.getElementById("salvar-campanha").reset();
        },
        error: function () {
            alert("Error occurs");
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
                    $('.categoria').html(html);
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
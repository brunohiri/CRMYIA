$(document).ready(function () {

    $('.btn-salvar').on('click', function () {
        var form_data = new FormData();
        if ($('#Descricao').val() != undefined && $('#Descricao').val() != '') {
            form_data.append('Descricao', $('#Descricao').val());
            $('#Ativo').is(":checked") == true ? formData.append("Ativo", 'true') : formData.append("Ativo", 'false');
            $.ajax({
                type: 'POST',
                url: "/NovoGrupoCorretor?handler=Salvar",
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

                        $('#MensagemDiv').show();
                        $('#MensagemDivAlert').removeClass();
                        $('#MensagemDivAlert').addClass(data.mensagem.cssClass);

                        $('#MensagemIcon').removeClass();
                        $('#MensagemIcon').addClass(data.mensagem.iconClass);
                        $('#MensagemSpan').text(data.mensagem.mensagem);

                        document.getElementById("form-salvar").reset();


                    }
                    setTimeout(function () { $('#MensagemDiv').css('display', 'none'); }, 5000);
                },
                error: function () {
                    swal("Erro!", "Erro ao alterar o registro, contate o Administrador do Sistema.", "error");
                }
            });
        }
    });

});
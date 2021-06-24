$(document).ready(function () {
    $("#txtTelefone").inputmask({
        mask: ["(99) 9999-9999", "(99) 99999-9999"],
        keepStatic: true
    });
});
$("#btnRedesSociais").click(function () {
    formData = new FormData();
    formData.append('Facebook', $("#txtFacebook").val());
    formData.append('Twitter', $("#txtTwitter").val());
    formData.append('Instagram', $("#txtInstagram").val());
    formData.append('Linkedin', $("#txtLinkedin").val());
    $.ajax({
        type: 'POST',
        url: "/MinhaConta?handler=RedesSociais",
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
                location.reload();
            }
            else {
                swal("Erro!", data.mensagem, "error");
            }
        },
        error: function () {
            alert("Error occurs");
        }
    });
});
$("#btnAlterarSenha").click(function () {
    if ($("#txtNovaSenha").val() != $("#txtConfSenha").val()) {
        swal("Erro!", "As senhas não coincidem!", "error");
    } else {
        formData = new FormData();
        formData.append('SenhaAtual', $("#txtSenhaAtual").val());
        formData.append('Senha', $("#txtNovaSenha").val());
        swal({
            title: "Você tem certeza?",
            text: "Que deseja Alterar sua senha?",
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
                    type: 'POST',
                    url: "/MinhaConta?handler=AlterarSenha",
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
                            location.reload();
                        }
                        else {
                            swal("Erro!", data.mensagem, "error");
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
    }

});
$("#btnLimparSenha").click(function () {
    $("#txtSenhaAtual").val("")
    $("#txtNovaSenha").val("")
    $("#txtConfSenha").val("")
    toastr.info("Campos limpos!");
});
$("#btnDadosPessoais").click(function () {
    formData = new FormData();
    formData.append('Nome', $("#txtNome").val());
    formData.append('NomeApelido', $("#txtApelido").val());
    formData.append('Telefone', $("#txtTelefone").val());
    if ($("#txtNome").val().length <= 3) {
        swal("Erro!", "Seu nome é muito pequeno!", "error");
    } else {
        $.ajax({
            type: 'POST',
            url: "/MinhaConta?handler=AlterarDados",
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
                    location.reload();
                }
                else {
                    swal("Erro!", data.mensagem, "error");
                }
            },
            error: function () {
                alert("Error occurs");
            }
        });
    }

});
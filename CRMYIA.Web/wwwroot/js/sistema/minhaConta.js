$(document).ready(function () {

});
$("#btnRedesSociais").click(function () {
    alert("oi");
});
$("#btnAlterarSenha").click(function () {
    if ($("#txtNovaSenha").val() != $("#txtConfSenha").val()) {
        toastr.error("As senhas não coincidem!");
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
                            swal("Erro!", data.mensagem, "Error");
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
    alert("oi");
});
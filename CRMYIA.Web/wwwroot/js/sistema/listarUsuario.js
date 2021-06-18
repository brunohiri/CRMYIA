$(document).ready(function () {
    $('input[name="Inicio"]').daterangepicker({
        "locale": {
            "format": "DD/MM/YYYY",
            "separator": " - ",
            "applyLabel": "Aplicar",
            "cancelLabel": "Cancelar",
            "daysOfWeek": [
                "Dom",
                "Seg",
                "Ter",
                "Qua",
                "Qui",
                "Sex",
                "Sab"
            ],
            "monthNames": [
                "Janeiro",
                "Fevereiro",
                "Março",
                "Abril",
                "Maio",
                "Junho",
                "Julho",
                "Agosto",
                "Setembro",
                "Outubro",
                "Novembro",
                "Dezembro"
            ],
            "firstDay": 1,
        }
    });

    $('input[name="Inicio"]').on('cancel.daterangepicker', function (ev, picker) {
        SetData();
    });

    SetData();

    $('.visitas-pesquisa').on('change', function () {
        Pesquisar();
    });
});

$(document).on('click', '.limpar-pesquisa', function () {
    $("#IdPerfil").select2('val', '0');
    $("#IdPerfil").select2({
        placeholder: "Selecione...",
        allowClear: true
    });

    $('#Ativo').val('Selecione...');

    SetData();

    Pesquisar();
});

$(document).on('click', '.redirect', function () {

    if ($(this).data('redirect') != undefined && $(this).data('redirect') != '') {
        location.replace('/NovoUsuario?id=' + encodeURIComponent($(this).data('redirect')));
    }

});

function Pesquisar() {
    var formData = new FormData();

    if ($('#Ativo').val() != undefined && $('#Ativo').val() != '' && $('#Ativo').val() != 'Selecione...') {
        if ($('#Ativo').val() == "0") {
            formData.append("Ativo", 'false');
        }
        else if ($('#Ativo').val() == "1") {
            formData.append("Ativo", 'true');
        }
    } else {
        formData.append("Ativo", null);
    }

    if ($('#IdPerfil').select2('data') != undefined && $('#IdPerfil').select2('data') != '') {
        formData.append("Descricao", $('#IdPerfil').select2('data')[0].text);
    } else {
        formData.append("Descricao", null);
    }

    if ($('#Inicio').val() != undefined && $('#Inicio').val() != '') {
        var date_range = $('#Inicio').val(); var dates = date_range.split(" - ");
        var start = dates[0];
        var end = dates[1];
        var start = moment(dates[0], 'D MMMM YY');
        var end = moment(dates[1], 'D MMMM YY');

        formData.append("DataInicio", start._i);
        formData.append("DataFim", end._i);
    } else {
        formData.append("DataInicio", null);
        formData.append("DataFim", null);
    }

    displayBusyIndicator();
    $.ajax({
        type: 'POST',
        url: "/ListarUsuario?handler=Pesquisa",
        data: formData,
        cache: false,
        contentType: false,
        processData: false,
        beforeSend: function (xhr) {
            xhr.setRequestHeader("XSRF-TOKEN",
                $('input:hidden[name="__RequestVerificationToken"]').val());
        },
        success: function (data) {
                CarregarTabela(data);
        },
        error: function () {
            swal("Erro!", "Erro ao buscar o registro, contate o Administrador do Sistema.", "error");
        }
    });
    displayBusyAvailable();
}

function SetData() {
    var DataHoje = new Date();
    if (DataHoje.getMonth() < 9)
        var DataHojeFormatada = ((DataHoje.getDate())) + "/" + ("0" + (DataHoje.getMonth() + 1)) + "/" + DataHoje.getFullYear();
    else
        var DataHojeFormatada = ((DataHoje.getDate())) + "/" + ((DataHoje.getMonth() + 1)) + "/" + DataHoje.getFullYear();

    var DataInicio = new Date();
    DataInicio.setDate(DataInicio.getDate() - 120);
    if (DataInicio.getMonth() < 9)
        var DataInicioFormatada = ((DataInicio.getDate())) + "/" + ("0" + (DataInicio.getMonth() + 1)) + "/" + DataInicio.getFullYear();
    else
        var DataInicioFormatada = ((DataInicio.getDate())) + "/" + ((DataInicio.getMonth() + 1)) + "/" + DataInicio.getFullYear();

    if (DataHojeFormatada != undefined && DataInicioFormatada != undefined) {
        $('input[name="Inicio"]').val(DataInicioFormatada + ' - ' + DataHojeFormatada);
    }
}

function CarregarTabela(data) {
    var html = '';
    if (data.status) {
        $('#tabelaListarUsuario').html(html);
        html += '<div class="card-body table-responsive">\
                        <table id="tableListagem" class="table table-striped table-hover dt-responsive display nowrap datatable" style = "width:100%">\
                            <thead>\
                                <tr>\
                                    <th></th>\
                                    <th>Perfil</th>\
                                    <th>Nome</th>\
                                    <th>Email</th>\
                                    <th>Telefone</th>\
                                    <th>Cadastro</th>\
                                    <th>Status</th>\
                                    <th></th>\
                                </tr>\
                            </thead>\
                            <tbody>';
        $.each(data.lista, function (index, value) {
            var ativo = this.ativo == true ? '<span class="badge badge-pill badge-success">ATIVO</span>' : '<span class="badge badge-pill badge-warning">DESATIVADO</span>';
            var usuarioPerfil = '<span class="badge badge-pill badge-warning">NÃO INFORMADO</span>';
            var telefone = this.telefone == null ? '<span class="badge badge-pill badge-warning">NÃO INFORMADO</span>' : this.telefone ;
            if (this.usuarioPerfil != null && this.usuarioPerfil.length > 0)
                var usuarioPerfil = this.usuarioPerfil[0].idPerfilNavigation.descricao;
            html += '<tr>\
                        <td></td>\
                        <td>' + usuarioPerfil + '</td>\
                        <td>' + this.nome + '</td>\
                        <td>' + this.email + '</td>\
                        <td>' + telefone + '</td>\
                        <td>' + FormataDatatime(this.dataCadastro) + '</td>\
                        <td>' + ativo + '</td >\
                        <td class="text-center">\
                            <span class="btn redirect" title="Editar" data-redirect="' + this.idUsuarioString + '"><i class="icon fas fa-edit"></i></span>\
                            <span class="btn redirect" title="Vincular" data-redirect="' + this.idUsuarioString + '"><i class="icon fas fa-link"></i></span>\
                        </td>\
                    </tr>';
        });
        html += '</tbody>\
                    </table>\
                </div>';

        $('#tabelaListarUsuario').html(html);
        
    } else {
        $('#tabelaListarUsuario').html(html);
        html += '<div class="card-body table-responsive">\
                        <table id="tableListagem" class="table table-striped table-hover dt-responsive display nowrap datatable" style = "width:100%">\
                            <thead>\
                                <tr>\
                                    <th></th>\
                                    <th>Perfil</th>\
                                    <th>Nome</th>\
                                    <th>Email</th>\
                                    <th>Telefone</th>\
                                    <th>Cadastro</th>\
                                    <th>Status</th>\
                                    <th></th>\
                                </tr>\
                            </thead>\
                            <tbody>';
       
        html += '</tbody>\
                    </table>\
                </div>';

        $('#tabelaListarUsuario').html(html);
    }
    InitDatatables();
}
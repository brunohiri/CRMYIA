$(function () {
    var items = [];
    var res = ObterCliente();

    if (res.status == 200) {
        $.each(res.responseJSON.data, function (index, value) {
            items.push({ id: this.idCliente, text: this.nome.toUpperCase() });
        });
    }

    pageSize = 50

    jQuery.fn.select2.amd.require(["select2/data/array", "select2/utils"],

        function (ArrayData, Utils) {
            function CustomData($element, options) {
                CustomData.__super__.constructor.call(this, $element, options);
            }
            Utils.Extend(CustomData, ArrayData);

            CustomData.prototype.query = function (params, callback) {

                results = [];
                if (params.term && params.term !== '') {
                    results = _.filter(items, function (e) {
                        return e.text.toUpperCase().indexOf(params.term.toUpperCase()) >= 0;
                    });
                } else {
                    results = items;
                }

                if (!("page" in params)) {
                    params.page = 1;
                }
                var data = {};
                data.results = results.slice((params.page - 1) * pageSize, params.page * pageSize);
                data.pagination = {};
                data.pagination.more = params.page * pageSize < results.length;
                callback(data);
            };

            $(document).ready(function () {
                $('#IdCliente').select2({
                    ajax: {},
                    dataAdapter: CustomData
                });
            });
        })
});

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
    $("#IdOrigem").select2('val', '0');
    $("#IdOrigem").select2({
        placeholder: "Selecione...",
        allowClear: true
    });

    $("#IdCliente").select2('val', '0');
    $("#IdCliente").select2({
        placeholder: "Selecione...",
        allowClear: true
    });

    $("#IdCidade").select2('val', '0');
    $("#IdCidade").select2({
        placeholder: "Selecione...",
        allowClear: true
    });

    $('#StatusPlanoLead').val('Selecione...');

    SetData();

    Pesquisar();
});

$(document).on('click', '.redirect', function () {

    if ($(this).data('redirect') != undefined && $(this).data('redirect') != '') {
        location.replace('/NovoCliente?id=' + encodeURIComponent($(this).data('redirect')));
    }

});

function ObterCliente() {
    var result;
    result = $.ajax({
                type: 'POST',
                url: "/ListarCliente?handler=ObterCliente",
                cache: false,
                contentType: false,
                processData: false,
                async: false,  
                beforeSend: function (xhr) {
                    xhr.setRequestHeader("XSRF-TOKEN",
                        $('input:hidden[name="__RequestVerificationToken"]').val());
                },
                success: function (data) {
                    if (data.status) {
                        return data.data;
                    }
                },
                error: function () {
                    swal("Erro!", "Erro ao buscar o registro, contate o Administrador do Sistema.", "error");
                }
            });
    return result;
}

function Pesquisar() {
    var formData = new FormData();

    if ($('#IdOrigem').val() != undefined && $('#IdOrigem').val() != '' && $('#IdOrigem').val() != 'Selecione...') {
        formData.append("IdOrigem", $('#IdOrigem').val());
    } else {
        formData.append("IdOrigem", null);
    }

    if ($('#IdCliente').select2('data') != undefined && $('#IdCliente').select2('data') != '' && $('#IdCliente').select2('data') != 'Selecione...') {
        formData.append("Nome", $('#IdCliente').select2('data')[0].text);
    } else {
        formData.append("Nome", null);
    }

    if ($('#IdCidade').select2('data') != undefined && $('#IdCidade').select2('data') != '' && $('#IdCidade').select2('data') != 'Selecione...') {
        formData.append("NomeCidade", $('#IdCidade').select2('data')[0].text);
    } else {
        formData.append("NomeCidade", null);
    }

    if ($('.daterange').data('daterangepicker').startDate._d != undefined && $('.daterange').data('daterangepicker').endDate._d != undefined) {
        formData.append("DataInicio", $('.daterange').data('daterangepicker').startDate._d);
        formData.append("DataFim", $('.daterange').data('daterangepicker').endDate._d);
    } else {
        formData.append("DataInicio", null);
        formData.append("DataFim", null);
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

    if (Number.isInteger(parseInt($('#StatusPlanoLead').val()))) {
        if ($('#StatusPlanoLead').val() == 0) {
            formData.append("StatusPlanoLead", "false");
        } else if ($('#StatusPlanoLead').val() == 1) {
            formData.append("StatusPlanoLead", "true");
        }
    } else {
        formData.append("StatusPlanoLead", null);
    }

    displayBusyIndicator();
    $.ajax({
        type: 'POST',
        url: "/ListarCliente?handler=Pesquisa",
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
                CarregarTabela(data.lista);
            }
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

    if (DataHojeFormatada != undefined) {
        $('input[name="Inicio"]').val('01/01/2020 - ' + DataHojeFormatada);
    }
}

function CarregarTabela(data) {
    var html = '';
    if (data) {
        $('#tabelaListarCliente').html(html);
        html += '<div class="card-body table-responsive">\
                        <table id="tableListagem" class="table table-striped table-hover dt-responsive display nowrap datatable" style = "width:100%">\
                            <thead>\
                                <tr>\
                                    <th></th>\
                                    <th width="15%"> Nome</th>\
                                    <th width="10%">Origem</th>\
                                    <th width="10%">Corretor</th>\
                                    <th width="15%">Cidade/UF</th>\
                                    <th width="15%">Cadastro</th>\
                                    <th width="10%">Status</th>\
                                    <th width="5%"></th>\
                                </tr>\
                            </thead>\
                            <tbody>';
        $.each(data, function (index, value) {
            var ativo = this.ativo = 'true' ? '<span class="badge badge-pill badge-success">ATIVO</span>' : '<span class="badge badge-pill badge-warning">DESATIVADO</span>';
            html += '<tr>\
                        <td class="text-center"></td>\
                        <td class="text-center">' + this.nome + '</td>\
                        <td class="text-center">' + this.origemDescricao + '</td>\
                        <td class="text-center">' + this.corretorNome + '</td>\
                        <td class="text-center">' + this.cidadeNome + '</td>\
                        <td class="text-center">' + FormataDatatime(this.dataCadastro) + '</td>\
                        <td class="text-center">'+ ativo + '</td>\
                        <td class="text-center">\
                            <span class="btn redirect" title="Editar Texto" data-redirect="' + this.idClienteString + '"><i class="icon fas fa-edit"></i></span>\
                        </td>\
                    </tr>';
        });
        html += '</tbody>\
                    </table>\
                </div>';

        $('#tabelaListarCliente').html(html);
        InitDatatables();
    }
}
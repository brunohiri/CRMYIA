var itemsSelecionados = [];

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

    $('.btn-vincular').on('click', function () {
        $('#modalVincular').modal('show');
    });


    $('.selecionar').on('click', function () {
       //$(this).data('selecionar')
        var $this = $(this);
        if ($this[0].checked == true) {
            $('.btn-vincular').prop('disabled', false);
            //true => disabled, false => liberado
            if ($this[0].disabled == false) {
                $this[0].checked = true;
                itemsSelecionados.push($this[0].dataset.selecionar.toString());
            }
        } else {
            var $this = $(this);

            //true => disabled, false => liberado]
            if ($this[0].disabled == false) {
                $this[0].checked = false;

                for (var i = 0; i < itemsSelecionados.length; i++) {

                    if (itemsSelecionados[i] === $this[0].dataset.selecionar) {
                        itemsSelecionados.splice(i, 1);
                        i--;
                    }
                }
            }
            var todos = $('.selecionar');
            var j = 0;
            var achou = true;
            while (j < todos.length && achou == true) {
                if (todos[j].checked == true)
                    achou = false;
                j++;
            }
            if (achou) {
                $('.btn-vincular').prop('disabled', true);
                $('#selecionar-todos').prop('checked', false);
            }
              
        }

    });

    $('#selecionar-todos').on('click', function () {
        if ($('#selecionar-todos').is(":checked") == true) {
            ApagarTodos();
            var todos = $('.selecionar');
            if (todos.length > 0) {
                $('.btn-vincular').prop('disabled', false);
            }
            //true => disabled, false => liberado
            for (var i = 0; i < todos.length; i++){
                if (todos[i].disabled != true) {
                    todos[i].checked = true;
                    itemsSelecionados.push(todos[i].dataset.selecionar.toString());
                }
            }
       
        } else {
            ApagarTodos();
        }

    });

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

    $('.btn-salvar').on('click', function () {
        formData = new FormData();
        formData.append('IdUsuarioCorretor', $('#IdUsuarioCorretor').val());
        formData.append('Itens', itemsSelecionados);
        $.ajax({
            type: 'POST',
            url: "/ListarCliente?handler=VincularCorretor",
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
                    $('#modalVincular').modal('hide');
                }
            },
            error: function () {
                swal("Erro!", "Erro ao alterar o registro, contate o Administrador do Sistema.", "error");
            }
        });

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

function ApagarTodos() {
    $('.btn-vincular').prop('disabled', true);
    var todos = $('.selecionar');
    //true => disabled, false => liberado
    for (var i = 0; i < todos.length; i++) {
        if (todos[i].disabled != true) {
            todos[i].checked = false;
            itemsSelecionados.splice(0, itemsSelecionados.length);
        }
    }
}

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

    if ($('#IdCliente').select2('data') != undefined && $('#IdCliente').select2('data') != '' && $('#IdCliente').select2('data').text != 'Selecione...') {
        formData.append("Nome", $('#IdCliente').select2('data')[0].text);
    } else {
        formData.append("Nome", null);
    }

    if ($('#IdCidade').select2('data') != undefined && $('#IdCidade').select2('data') != '' && $('#IdCidade').select2('data') != 'Selecione...') {
        formData.append("NomeCidade", $('#IdCidade').select2('data')[0].text);
    } else {
        formData.append("NomeCidade", null);
    }

    //if ($('.daterange').data('daterangepicker').startDate._d != undefined && $('.daterange').data('daterangepicker').endDate._d != undefined) {
    //    formData.append("DataInicio", $('.daterange').data('daterangepicker').startDate._d);
    //    formData.append("DataFim", $('.daterange').data('daterangepicker').endDate._d);
    //} else {
    //    formData.append("DataInicio", null);
    //    formData.append("DataFim", null);
    //}

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
    if (data) {
        $('#tabelaListarCliente').html(html);
        html += '<div class="card-body table-responsive">\
                        <table id="tableListagem" class="table table-striped table-hover dt-responsive display nowrap datatable" style = "width:100%">\
                            <thead>\
                                <tr>\
                                    <th></th>\
                                    <th></th>\
                                    <th>Nome</th>\
                                    <th>Origem</th>\
                                    <th>Corretor</th>\
                                    <th>Cidade/UF</th>\
                                    <th>Cadastro</th>\
                                    <th>Status</th>\
                                    <th></th>\
                                </tr>\
                            </thead>\
                            <tbody>';
        $.each(data, function (index, value) {
            var ativo = this.ativo = 'true' ? '<span class="badge badge-pill badge-success">ATIVO</span>' : '<span class="badge badge-pill badge-warning">DESATIVADO</span>';
            var checkbox = this.corretorNome == "" ? '<input type="checkbox" name="selecionar" id="selecionar" style="background-color: red" class="form-check-input selecionar" data-selecionar="' + this.idCliente + '">' : '<input type="checkbox" name="selecionar-todos" id="selecionar-todos" class="form-check-input selecionar-todos" data-selecionar="' + this.idCliente + '" disabled>' ;
            html += '<tr>\
                        <td></td>\
                        <td class="text-center">' + checkbox + '</td>\
                        <td class="text-center">' + this.nome.toUpperCase() + '</td>\
                        <td class="text-center">' + this.origemDescricao.toUpperCase() + '</td>\
                        <td class="text-center">' + this.corretorNome.toUpperCase() + '</td>\
                        <td class="text-center">' + this.cidadeNome.toUpperCase() + '</td>\
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
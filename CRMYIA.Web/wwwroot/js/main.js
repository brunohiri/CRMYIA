
$(document).ready(function () {

    //Carregar DataTable
    InitDatatables();

    //Initialize Select2 Elements
    $('.select2').select2();

    //Timepicker
    $('#timepicker').datetimepicker({
        format: 'H:m',
    });


    $.fn.datetimepicker.Constructor.Default = $.extend({}, $.fn.datetimepicker.Constructor.Default, {
        format: 'DD/MM/YYYY HH:mm',
        locale: moment.locale('pt-br'),
        date: moment()
    });

    //Masks
    $('.cep').blur(function () {
        $.ajax({
            type: "GET",
            url: "https://viacep.com.br/ws/" + $(this).val().replace('-', '') + "/json/",
            dataType: "json",
            success: function (data) {
                $('.endereco').attr('disabled', false);
                $('.endereco').val('');
                $('select[id="IdEstado"]').val(' ');
                if (data !== '') {
                    $('.endereco').val(data.logradouro);
                    $('.bairro').val(data.bairro);
                    $('.cidade').val(data.ibge);
                    $('.uf').val(data.uf.toUpperCase());
                    $('.numero').focus();
                }
            }
        });
    });

    $(".telefone").inputmask({
        mask: ["(99) 9999-9999", "(99) 99999-9999"],
        keepStatic: true
    });

    $(".onlyNumber").inputmask({
        mask: ["99999999", "99999999"],
        keepStatic: true
    });

    $(".cep").inputmask({
        mask: ["99999-999"],
        keepStatic: true
    });

    $(".data").inputmask({
        mask: ["99/99/9999"],
        keepStatic: true
    });

    $('.money').mask("#.##0,00", { reverse: true });

    $('.quantidade').mask('##0.000', { reverse: true });


    //================ Máscara CPF/CNPJ ====================
    var CpfCnpjMaskBehavior = function (val) {
        mascara = val !== '' ? (val.replace(/\D/g, '').length <= 11 ? '000.000.000-009' : '00.000.000/0000-00') : '00.000.000/0000-00';
        return mascara;
    },
        cpfCnpjpOptions = {
            onKeyPress: function (val, e, field, options) {
                field.mask(CpfCnpjMaskBehavior.apply({}, arguments), options);
            }
        };

    $('.cpf-cnpj').mask(CpfCnpjMaskBehavior, cpfCnpjpOptions);

    //================ Máscara CPF ====================
    var CpfMaskBehavior = function (val) {
        mascara = '000.000.000-00';
        return mascara;
    },
        CpfOptions = {
            onKeyPress: function (val, e, field, options) {
                field.mask(CpfMaskBehavior.apply({}, arguments), options);
            }
        };

    $('.cpf').mask(CpfMaskBehavior, CpfOptions);


    //================ Máscara CNPJ ====================
    var CnpjMaskBehavior = function (val) {
        mascara = '00.000.000/0000-00';
        return mascara;
    },
        CnpjOptions = {
            onKeyPress: function (val, e, field, options) {
                field.mask(CnpjMaskBehavior.apply({}, arguments), options);
            }
        };

    $('.cnpj').mask(CnpjMaskBehavior, CnpjOptions);


    $('#UsuarioIdPerfil').change(function () {
        if ($(this).val() == 3 || $(this).val() == 4) //Perfil: Supervisor ou Corretor
        {
            $('#IdUsuarioHierarquia').removeAttr('disabled');
            $.ajax({
                url: '/NovoUsuario?handler=Perfil',
                data: { idPerfil: $(this).val() },
                cache: false,
                async: true,
                type: "GET",
                success: function (data) {
                    var result = '';
                    if (data.status) {
                        result += '<option value="0" selected>Selecione...</option>';
                        for (var i = 0; i < data.list.length; i++) {
                            result += '<option value="' + data.list[i].idUsuario + '">' + data.list[i].nome + '</option>';
                        }
                        $('#IdUsuarioHierarquia').html(result);
                    }
                }
            });
        }
        else
            $('#IdUsuarioHierarquia').attr('disabled', 'disabled');
    });


    // Cadastro de Leads
    CarregarListaTelefone();
    SalvarCadastroTelefone();
    CarregarListaEmail();
    SalvarCadastroEmail();

    if (window.location.href.indexOf('NovaProposta') > 0) {
        //Cadastro de Propostas
        CadastroPropostas();
    }
    if (window.location.href.indexOf('Tarefa') > 0) {
        //Cadastro de Tarefas
        CadastroTarefas();
    }
});

function InitDatatables() {
    //Datatables
    var uiDatatable = function () {
        if ($(".datatable").length > 0) {
            $(".datatable").dataTable({
                "language": {
                    "sEmptyTable": "Nenhum registro encontrado",
                    "sInfo": "Mostrando de _START_ até _END_ de _TOTAL_ registros",
                    "sInfoEmpty": "Mostrando 0 até 0 de 0 registros",
                    "sInfoFiltered": "(Filtrados de _MAX_ registros)",
                    "sInfoPostFix": "",
                    "sInfoThousands": ".",
                    "sLengthMenu": "_MENU_ resultados por página",
                    "sLoadingRecords": "Carregando...",
                    "sProcessing": "Processando...",
                    "sZeroRecords": "Nenhum registro encontrado",
                    "sSearch": "Pesquisar",
                    "oPaginate": {
                        "sNext": "Próximo",
                        "sPrevious": "Anterior",
                        "sFirst": "Primeiro",
                        "sLast": "Último"
                    },
                    "oAria": {
                        "sSortAscending": ": Ordenar colunas de forma ascendente",
                        "sSortDescending": ": Ordenar colunas de forma descendente"
                    }
                    ,
                    "decimal": ",",
                    "thousands": "."
                }
            });
            //$(".datatable").on('page.dt', function () {
            //    onresize(100);
            //});
        }

        if ($(".datatable_order").length > 0) {
            var oTable = $(".datatable_order").dataTable({
                "language": {
                    "sEmptyTable": "Nenhum registro encontrado",
                    "sInfo": "Mostrando de _START_ até _END_ de _TOTAL_ registros",
                    "sInfoEmpty": "Mostrando 0 até 0 de 0 registros",
                    "sInfoFiltered": "(Filtrados de _MAX_ registros)",
                    "sInfoPostFix": "",
                    "sInfoThousands": ".",
                    "sLengthMenu": "_MENU_ resultados por página",
                    "sLoadingRecords": "Carregando...",
                    "sProcessing": "Processando...",
                    "sZeroRecords": "Nenhum registro encontrado",
                    "sSearch": "Pesquisar",
                    "oPaginate": {
                        "sNext": "Próximo",
                        "sPrevious": "Anterior",
                        "sFirst": "Primeiro",
                        "sLast": "Último"
                    },
                    "oAria": {
                        "sSortAscending": ": Ordenar colunas de forma ascendente",
                        "sSortDescending": ": Ordenar colunas de forma descendente"
                    }
                    ,
                    "decimal": ",",
                    "thousands": "."
                }
            });

            oTable.fnSort([[0, 'desc'], [1, 'asc']]);
        }


        if ($(".datatable_nopage").length > 0) {
            $(".datatable_nopage").dataTable({
                "language": {
                    "sEmptyTable": "Nenhum registro encontrado",
                    "sInfo": "Mostrando de _START_ até _END_ de _TOTAL_ registros",
                    "sInfoEmpty": "Mostrando 0 até 0 de 0 registros",
                    "sInfoFiltered": "(Filtrados de _MAX_ registros)",
                    "sInfoPostFix": "",
                    "sInfoThousands": ".",
                    "sLengthMenu": "_MENU_ resultados por página",
                    "sLoadingRecords": "Carregando...",
                    "sProcessing": "Processando...",
                    "sZeroRecords": "Nenhum registro encontrado",
                    "sSearch": "Pesquisar",
                    "oPaginate": {
                        "sNext": "Próximo",
                        "sPrevious": "Anterior",
                        "sFirst": "Primeiro",
                        "sLast": "Último"
                    },
                    "oAria": {
                        "sSortAscending": ": Ordenar colunas de forma ascendente",
                        "sSortDescending": ": Ordenar colunas de forma descendente"
                    }
                    ,
                    "decimal": ",",
                    "thousands": "."
                },
                "paging": false,
                "searching": false,
                "ordering": false,
                "info": false
            });
        }

        if ($(".datatable_simple").length > 0) {
            $(".datatable_simple").dataTable({ "ordering": false, "info": false, "lengthChange": false, "searching": false });
            $(".datatable_simple").on('page.dt', function () {
                onresize(100);
            });
        }

    }//END Datatable

    uiDatatable();
}

/* ========================== Telefone =================================== */
function CarregarListaTelefone() {
    var Id = $('#ClienteIdCliente').val();
    if (Id !== undefined) {
        $.ajax({
            type: "GET",
            url: "/NovoCliente?handler=ListTelefone&Id=" + Id,
            contentType: "application/json",
            dataType: "json",
            success: function (data) {
                $('#tableListagemTelefone tbody').html('');
                var tabela = '';
                for (var i = 0; i < data.listEntityTelefone.length; i++) {
                    tabela += '<tr>';
                    tabela += '<td>(' + data.listEntityTelefone[i].ddd + ') ' + data.listEntityTelefone[i].telefone1 + '</td>';
                    tabela += '<td>' + (data.listEntityTelefone[i].whatsApp ? 'Sim' : 'Não') + '</td>';
                    tabela += '<td>' + (data.listEntityTelefone[i].idOperadoraTelefoneNavigation.descricao) + '</td>';
                    tabela += '<td>' + new Date(data.listEntityTelefone[i].dataCadastro).toLocaleDateString('pt-br') + ' ' + new Date(data.listEntityTelefone[i].dataCadastro).toLocaleTimeString('pt-br') + '</td>';
                    tabela += '<td>' + (data.listEntityTelefone[i].ativo ? 'Ativo' : 'Inativo') + '</td>';
                    tabela += '<td><a href="javascript:void(0)" name="LinkEditarTelefone" data-id="' + data.listEntityTelefone[i].idTelefone + '" title="Editar" class="text-info"><i class="icon fas fa-edit" data-toggle="modal" data-target="#modal-telefone"></i></a></td>';
                    tabela += '</tr>';
                }

                $('#tableListagemTelefone tbody').html(tabela);
                CarregarModalTelefone();
            }
        });
    }
}

function CarregarModalTelefone() {
    $('a[name="LinkEditarTelefone"]').click(function () {
        var IdTelefone = $(this).data('id');
        $.ajax({
            type: "GET",
            url: "/NovoCliente?handler=Telefone&Id=" + IdTelefone,
            contentType: "application/json",
            dataType: "json",
            success: function (data) {
                $('#TelefoneMensagemDiv').hide();
                LimparCadastroTelefone();

                $('#TelefoneIdTelefone').val(data.entityEditarTelefone.idTelefone);
                $('#TelefoneNumero').val(data.entityEditarTelefone.ddd.toString() + data.entityEditarTelefone.telefone1.toString());
                $('#TelefoneOperadora').val(data.entityEditarTelefone.idOperadoraTelefone);
                if (data.entityEditarTelefone.whatsApp)
                    $('#TelefoneWhatsApp').attr('checked', true);
                else
                    $('#TelefoneWhatsApp').attr('checked', false);
                $('#TelefoneDataCadastro').val(new Date(data.entityEditarTelefone.dataCadastro).toLocaleDateString('pt-br') + ' ' + new Date(data.entityEditarTelefone.dataCadastro).toLocaleTimeString('pt-br'));
                if (data.entityEditarTelefone.ativo)
                    $('#TelefoneAtivo').attr('checked', true);
                else
                    $('#TelefoneAtivo').attr('checked', false);
            }
        });
    });
}

function LimparCadastroTelefone() {
    $('#TelefoneIdTelefone').val('');
    $('#TelefoneNumero').val('');
    $('#TelefoneOperadora').val('0');
    $('#TelefoneWhatsApp').attr('checked', false);
    $('#TelefoneAtivo').attr('checked', true);
}

function SalvarCadastroTelefone() {
    $('#ButtonAdicionarModalTelefone').click(function () {
        LimparCadastroTelefone();
    });

    $('#ButtonCancelarTelefone').click(function () {
        LimparCadastroTelefone();
    });

    $('#ButtonSalvarTelefone').on('click', function (evt) {
        evt.preventDefault();
        $.post('/NovoCliente?handler=Telefone', $('#formTelefone').serialize(), function (data) {
            $('#TelefoneMensagemDiv').show();
            $('#TelefoneMensagemDivAlert').removeClass();
            $('#TelefoneMensagemDivAlert').addClass(data.mensagem.cssClass);

            $('#TelefoneMensagemIcon').removeClass();
            $('#TelefoneMensagemIcon').addClass(data.mensagem.iconClass);
            $('#TelefoneMensagemSpan').text(data.mensagem.mensagem);

            CarregarListaTelefone();
        });
    });
}

/* ========================== Email =================================== */
function CarregarListaEmail() {
    var Id = $('#ClienteIdCliente').val();
    if (Id !== undefined) {
        $.ajax({
            type: "GET",
            url: "/NovoCliente?handler=ListEmail&Id=" + Id,
            contentType: "application/json",
            dataType: "json",
            success: function (data) {
                $('#tableListagemEmail tbody').html('');
                var tabela = '';
                for (var i = 0; i < data.listEntityEmail.length; i++) {
                    tabela += '<tr>';
                    tabela += '<td>' + data.listEntityEmail[i].emailConta + '</td>';
                    tabela += '<td>' + new Date(data.listEntityEmail[i].dataCadastro).toLocaleDateString('pt-br') + ' ' + new Date(data.listEntityEmail[i].dataCadastro).toLocaleTimeString('pt-br') + '</td>';
                    tabela += '<td>' + (data.listEntityEmail[i].ativo ? 'Ativo' : 'Inativo') + '</td>';
                    tabela += '<td><a href="javascript:void(0)" name="LinkEditarEmail" data-id="' + data.listEntityEmail[i].idEmail + '" title="Editar" class="text-info"><i class="icon fas fa-edit" data-toggle="modal" data-target="#modal-email"></i></a></td>';
                    tabela += '</tr>';
                }

                $('#tableListagemEmail tbody').html(tabela);
                CarregarModalEmail();
            }
        });
    }
}

function CarregarModalEmail() {
    $('a[name="LinkEditarEmail"]').click(function () {
        var IdEmail = $(this).data('id');
        $.ajax({
            type: "GET",
            url: "/NovoCliente?handler=Email&Id=" + IdEmail,
            contentType: "application/json",
            dataType: "json",
            success: function (data) {
                $('#EmailMensagemDiv').hide();
                LimparCadastroEmail();
                $('#EmailIdEmail').val(data.entityEditarEmail.idEmail);
                $('#EmailConta').val(data.entityEditarEmail.emailConta);
                $('#EmailDataCadastro').val(new Date(data.entityEditarEmail.dataCadastro).toLocaleDateString('pt-br') + ' ' + new Date(data.entityEditarEmail.dataCadastro).toLocaleTimeString('pt-br'));
                if (data.entityEditarEmail.ativo)
                    $('#EmailAtivo').attr('checked', true);
                else
                    $('#EmailAtivo').attr('checked', false);
            }
        });
    });
}

function LimparCadastroEmail() {
    $('#EmailIdEmail').val('');
    $('#EmailConta').val('');
    $('#EmailAtivo').attr('checked', true);
}

function SalvarCadastroEmail() {
    $('#ButtonAdicionarModalEmail').click(function () {
        LimparCadastroEmail();
    });

    $('#ButtonCancelarEmail').click(function () {
        LimparCadastroEmail();
    });

    $('#ButtonSalvarEmail').on('click', function (evt) {
        evt.preventDefault();
        $.post('/NovoCliente?handler=Email', $('#formEmail').serialize(), function (data) {
            $('#EmailMensagemDiv').show();
            $('#EmailMensagemDivAlert').removeClass();
            $('#EmailMensagemDivAlert').addClass(data.mensagem.cssClass);

            $('#EmailMensagemIcon').removeClass();
            $('#EmailMensagemIcon').addClass(data.mensagem.iconClass);
            $('#EmailMensagemSpan').text(data.mensagem.mensagem);

            CarregarListaEmail();
        });
    });
}

/* ========================== Cadastro de Propostas =================================== */
function CadastroPropostas() {

    if ($('#Entity_IdProposta').val() != 0) {
        var IdCliente = $('#PropostaIdClienteHidden').val();
        CarregarClientePropostaDocumento(IdCliente, null);
        CarregarPropostaLinhaCategoria($('#PropostaIdCategoria').val());
    }

    $('#PropostaIdCliente').change(function () {
        var IdCliente = $(this).val();
        CarregarClientePropostaDocumento(IdCliente, null);
    });

    $('#PropostaDocumentoCliente').blur(function () {
        var Documento = $(this).val();
        CarregarClientePropostaDocumento(null, Documento);
    });

    $('#PropostaIdMotivoDeclinio').attr('disabled', 'disabled');
    $('#PropostaIdStatusProposta').change(function () {
        if ($(this).val() == 3) //Declinar
        {
            $('#PropostaIdMotivoDeclinio').removeAttr('disabled');
            $('#PropostaIdMotivoDeclinio').val('0');
        }
        else {
            $('#PropostaIdMotivoDeclinio').val('0');
            $('#PropostaIdMotivoDeclinio').attr('disabled', 'disabled');
        }
    });

    $('#PropostaPossuiPlano').removeAttr('checked');
    CarregarPossuiPlano();
    $('#PropostaPossuiPlano').click(function () {
        CarregarPossuiPlano();
        if ($(this).is(':checked')) {
            $('#PropostaPlanoJaUtilizado').removeAttr('disabled');
            $('#PropostaTempoPlano').removeAttr('disabled');
            $('#PropostaPreferenciaHospitalar').removeAttr('disabled');
        }
    });

    $('#PropostaIdOperadora').change(function () {
        var IdOperadora = $(this).val();
        CarregarPropostaOperadoraProduto(IdOperadora);
    });

    $('#PropostaIdProduto').change(function () {
        var IdProduto = $(this).val();
        CarregarPropostaProdutoLinha(IdProduto);
    });

    $('#PropostaIdLinha').change(function () {
        var IdLinha = $(this).val();
        CarregarPropostaLinhaCategoria(IdLinha);
    });

    CalcularQuantidadeVidas();
}

function CarregarPossuiPlano() {
    $('#PropostaPlanoJaUtilizado').attr('disabled', 'disabled');
    $('#PropostaTempoPlano').attr('disabled', 'disabled');
    $('#PropostaPreferenciaHospitalar').attr('disabled', 'disabled');
    $('#PropostaPlanoJaUtilizado').val('');
    $('#PropostaTempoPlano').val('');
    $('#PropostaPreferenciaHospitalar').val('');
}


function CarregarClientePropostaDocumento(IdCliente, Documento) {
    $.ajax({
        type: "GET",
        url: "/NovaProposta?handler=Cliente&Id=" + IdCliente + "&Documento=" + Documento,
        contentType: "application/json",
        dataType: "json",
        success: function (data) {
            if (data.entityCliente != null) {
                $('#PropostaIdClienteHidden').val(data.entityCliente.idCliente);
                $('#PropostaDocumentoCliente').val(data.entityCliente.documento);
                $('#PropostaClienteNome').val(data.entityCliente.nome);
                $('#PropostaClienteDataNascAbertura').val(data.entityCliente.dataNascAbertura);
                $('#PropostaClienteSituacao').val(data.entityCliente.situacao);
                $('#PropostaClienteEndereco').val(data.entityCliente.endereco);
                $('#PropostaClienteCelular').val(data.entityCliente.celular);
                $('#PropostaClienteTelefone').val(data.entityCliente.telefone);
                $('#PropostaClienteEmail').val(data.entityCliente.email);
            }
        }
    });
}

function CarregarPropostaOperadoraProduto(IdOperadora) {
    var IdProduto = 0;
    if (IdOperadora == "0") {
        IdProduto = $('#PropostaIdProdutoHidden').val();
    }

    $.ajax({
        type: "GET",
        url: "/NovaProposta?handler=Produto&IdOperadora=" + IdOperadora + "&IdProduto=" + IdProduto,
        contentType: "application/json",
        dataType: "json",
        success: function (data) {
            var result = '';
            if (data.status) {
                if (data.listProduto.length > 0) {
                    result = '<option value="0" selected>Selecione...</option>';
                    for (var i = 0; i < data.listProduto.length; i++) {
                        result += '<option value="' + data.listProduto[i].idProduto + '">' + data.listProduto[i].descricao + '</option>';
                    }
                    $('#PropostaIdProduto').html(result);
                }

                if (IdOperadora == "0") {
                    console.log(data.idOperadora);
                    $('#PropostaIdProduto').val(IdProduto);
                    $('#PropostaIdOperadora').val(data.idOperadora);
                    $('.select2').select2();
                }
            }
        }
    });
}

function CarregarPropostaProdutoLinha(IdProduto) {
    var IdLinha = 0;
    if (IdProduto == "0") {
        IdLinha = $('#PropostaIdLinhaHidden').val();
    }

    $.ajax({
        type: "GET",
        url: "/NovaProposta?handler=Linha&IdProduto=" + IdProduto + "&IdLinha=" + IdLinha,
        contentType: "application/json",
        dataType: "json",
        success: function (data) {
            var result = '';
            if (data.status) {
                if (data.listLinha.length > 0) {
                    result = '<option value="0" selected>Selecione...</option>';
                    for (var i = 0; i < data.listLinha.length; i++) {
                        result += '<option value="' + data.listLinha[i].idLinha + '">' + data.listLinha[i].descricao + '</option>';
                    }
                    $('#PropostaIdLinha').html(result);
                }
                if (IdProduto == "0") {
                    $('#PropostaIdLinha').val(IdLinha);
                    $('#PropostaIdProduto').val(data.idProduto);
                    $('#PropostaIdProdutoHidden').val(data.idProduto);

                    CarregarPropostaOperadoraProduto(0);

                    $('.select2').select2();
                }
            }
        }
    });
}

function CarregarPropostaLinhaCategoria(IdLinha) {
    var IdCategoria = 0;
    if (IdLinha == "0") {
        IdCategoria = $('#PropostaIdCategoriaHidden').val();
    }

    $.ajax({
        type: "GET",
        url: "/NovaProposta?handler=Categoria&IdLinha=" + IdLinha + "&IdCategoria=" + IdCategoria,
        contentType: "application/json",
        dataType: "json",
        success: function (data) {
            var result = '';
            if (data.status) {
                if (data.listCategoria.length > 0) {
                    result = '<option value="0" selected>Selecione...</option>';
                    for (var i = 0; i < data.listCategoria.length; i++) {
                        result += '<option value="' + data.listCategoria[i].idCategoria + '">' + data.listCategoria[i].descricao + '</option>';
                    }
                    $('#PropostaIdCategoria').html(result);
                }
                if (IdLinha == "0") {
                    $('#PropostaIdCategoria').val(IdCategoria);
                    $('#PropostaIdLinha').val(data.idLinha);
                    $('#PropostaIdLinhaHidden').val(data.idLinha);

                    CarregarPropostaProdutoLinha(0);

                    $('.select2').select2();
                }
            }
        }
    });
}


function CalcularQuantidadeVidas() {
    $('.faixaetaria').change(function () {
        var qtdSoma = 0;
        for (var i = 0; i < $('.faixaetaria').length; i++) {
            if ($('.faixaetaria')[i].value != '')
                qtdSoma += parseInt($('.faixaetaria')[i].value);
        }
        $('#PropostaQuantidadeVidas').val(qtdSoma);
    });
}

function CadastroTarefas() {
    $('ul[id^="sort"]').sortable(
        {
            connectWith: ".sortable",
            receive: function (e, ui) {
                var status_id = $(ui.item).parent(".sortable").data(
                    "status-id");
                var task_id = $(ui.item).data("task-id");
                $.ajax({
                    url: '/Tarefa?handler=Edit&statusId=' + status_id + '&taskId=' + task_id,
                    success: function (data) {
                        if (data.status) {
                            for (var i = 0; i < $('#sort' + status_id + ' li').length; i++) {
                                if ($('#sort' + status_id + ' li').eq(i).data('task-id') == "0") {
                                    $('#sort' + status_id + ' li').eq(i).remove();
                                }
                            }
                            AtualizarCardsPropostas();
                        }
                    }
                });
            }

        }).disableSelection();

    function AtualizarCardsPropostas() {
        for (var ul = 0; ul < $('ul[id*="sort"]').length; ul++) {
            if ($('ul[id*="sort"]').eq(ul).find('li').length == 0) {
                $('ul[id*="sort"]').eq(ul).html('<li class="text-row-empty div-blocked" data-task-id="0">Nenhuma Proposta</li>');
            }
        }
    }
}
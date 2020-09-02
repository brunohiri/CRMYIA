$(document).ready(function () {

    //Carregar DataTable
    InitDatatables();

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
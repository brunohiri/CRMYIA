const formatter = new Intl.NumberFormat('pt-BR', {
    style: 'currency',
    currency: 'BRL'
});
var hashId = '';
var start = 20;

$(document).ready(function () {
    SalvarKPIGrupo();
    CadastroGrupos();
    AtualizarRealizado();
    $('.sortable').sortable({
        connectWith: ".sortable",
        start: {

        },
        update: function (event, ui) {
            AtualizarRealizado();
        },
        change: function (event, ui) {
            //var gId = ui.item.attr("data-grupo-id");
            //var mId = ui.item.attr("data-meta-id");
        },
        stop: function (event, ui) {
            AtualizarRealizado();
        },
        remove: function (event, ui) {
        }
    }).disableSelection();
});
$(document).on('keypress', function (e) {
    if (e.which == 13) {
        $('.loading').delay(20).queue(function (next) {
            $(this).hide();
            next();
        });
        CarregarCardsTODO();
    }
});
$("#IdUsuarioKPIGrupo").change(function () {
    $("#txtNomeKPIGrupo").val($("#IdUsuarioKPIGrupo").children("option:selected").text())
});
function AtualizarRealizado() {
    var grupos = CalcularGrupos();
    var valor = 0.00;
    var vidas = 0;
    $.each(grupos, function (i, d) {
        $.each(d.itens, function (i, da) {
            if (da.vidas == 0 && da.valores == 0) {
                $("#valor-" + d.grupo).html("");
                $("#valor-" + d.grupo).append(da.valores);

                $("#vidas-" + d.grupo).html("");
                $("#vidas-" + d.grupo).append(da.vidas);
            } else {
                var v = da.valores;
                var vf = v.replace(",", ".");
                vidas += parseInt(da.vidas);
                valor += parseFloat(vf);
            }
        });

        if (valor != 0) {
            $("#valor-" + d.grupo).html("");
            $("#valor-" + d.grupo).append(valor.toLocaleString("pt-BR", { style: "currency", currency: "BRL" }));
        }

        if (vidas != 0) {
            $("#vidas-" + d.grupo).html("");
            $("#vidas-" + d.grupo).append(vidas);
        }

        valor = 0.00;
        vidas = 0;
    });
}
function CalcularGrupos() {
    allGrupos = $(".grupo");
    var grupos = [];
    $.each(allGrupos, function (i, d) {
        grupos.push({
            'grupo': d.className.slice(27),
            'itens': eachChildren(d.children)
        })
    });
    return grupos;
}
function eachChildren(itens) {
    var result = [];
    if (itens.length > 0) {
        $.each(itens, function (i, d) {
            result.push({
                'vidas': d.dataset.vidas,
                'valores': d.dataset.valores
            }
            );
        });
    } else {
        result.push({
            'vidas': 0,
            'valores': 0
        })
    }
    return result;
}

$('.TODO').on('scroll', function () {
    if (Math.round($(this).scrollTop() + $(this).innerHeight(), 10) >= Math.round($(this)[0].scrollHeight, 10)) {
        if ($("#searchKPIGrupo").val().length > 0) {
            CarregarCardsTODO();
        } else {
            var text = "";
            var formData = new FormData();
            formData.append('start', start);
            $(".loader").show(100);
            $.ajax({
                type: "POST",
                url: "/KPIGrupo?handler=Corretores",
                beforeSend: function (xhr) {
                    xhr.setRequestHeader("XSRF-TOKEN",
                        $('input:hidden[name="__RequestVerificationToken"]').val());
                },
                data: formData,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                processData: false,
                contentType: false,
                success: function (result) {
                    $.each(result.listCorretor, function (i, d) {
                        text += '<li data-realizado="0" data-vidas="0" data-valores="0" class="text-row ui-sortable-handle" data-user-id="' + this.idUsuario + '" id="id-"' + this.idUsuario + '">' +
                            '<div class="card card-color" style="position: relative; left: 0px; top: 0px;">' +
                            '<div class="card-header border-0 ui-sortable-handle" style="cursor: move;">' +
                            '<h3 class="card-title">' +
                            '<i class="fas fa-th mr-1"></i>' + this.nome.substr(0, 16) +
                            '</h3>' +
                            '<div class="card-tools mr-2 mb-1">' +
                            '<button type="button" class="btn btn-outline-light btn-sm" data-card-widget="collapse">' +
                            '<i class="fas fa-minus"></i>' +
                            '</button>' +
                            '<button type="button" class="btn btn-outline-light btn-sm" >' +
                            '<i class="fas fa-times"></i>' +
                            '</button>' +
                            '</div>' +
                            '</div>' +
                            '<div class="card-body p-0 mb-0" style="display: block;">' +
                            '<div class="card card-widget widget-user mb-0">' +
                            '<div class="widget-user-header card-color">' +
                            '<h3 class="widget-user-username">' + this.descricaoPerfil + '</h3>' +
                            '</div>' +
                            '<div class="widget-user-image">' +
                            '<img class="img-circle elevation-2" src="' + this.caminhoFoto + this.nomeFoto + '" alt="' + this.nomeFoto + '">' +
                            '</div>' +
                            '<div class="card-footer bg-gradient-light">' +
                            '<div class="row">' +
                            '<div class="col-sm-6 border-right">' +
                            '<div class="description-block">' +
                            '<h5 class="description-header">0</h5>' +
                            '<span class="description-text">Meta</span>' +
                            '</div>' +
                            '</div>' +
                            '<div class="col-sm-6">' +
                            '<div class="description-block">' +
                            '<h5 class="description-header">0</h5>' +
                            '<span class="description-text">Vidas</span>' +
                            '</div>' +
                            '</div>' +
                            '</div>' +
                            '</div>' +
                            '</div>' +
                            '</div>' +
                            '</div>' +
                            '</li>';
                    });
                    start += start;
                    $(".loader").hide("fast", function () {
                        $(this).prev().hide("fast", arguments.callee);
                    });
                    $('.TODO').append(text);
                },
                failure: function (data) {
                    console.log(response);
                }
            });

        }
    }
});
var ht = '';
var startSearch = 0;
var termoAnterior = "";
$("#btnSearchKPIGrupo").click(function () {
    CarregarCardsTODO();
});
function CarregarCardsTODO() {
    var text = "";
    var termo = "";
    var take = 100;
    var formData = new FormData();
    if (startSearch == 0)
        ht = $('.TODO').html();
    termo = $("#searchKPIGrupo").val();
    if (termoAnterior != "") {
        if (termo != termoAnterior) {
            termoAnterior = termo;
            startSearch = 0;
        }
    }
    if (termo.length > 2) {
        formData.append("termo", termo);
        formData.append("start", startSearch);
        formData.append("take", take);

        $.ajax({
            type: "POST",
            url: "/KPIGrupo?handler=SearchByName",
            beforeSend: function (xhr) {
                xhr.setRequestHeader("XSRF-TOKEN",
                    $('input:hidden[name="__RequestVerificationToken"]').val());
            },
            data: formData,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            processData: false,
            contentType: false,
            success: function (result) {
                if (result.listCorretor.length > 0) {
                    toastr.info("Busca realizada com sucesso!");
                } else {
                    toastr.warning("Nada encontrado!");
                }
                startSearch = result.start;
                if (startSearch <= 100) {
                    if (result.listCorretor.length > 0) {
                        termoAnterior = termo;
                        $(".loader").show(100);
                        $.each(result.listCorretor, function (i, d) {
                            text += '<li data-realizado="0" data-vidas="0" data-valores="0" class="text-row ui-sortable-handle" data-user-id="' + this.idUsuario + '" id="id-"' + this.idUsuario + '">' +
                                '<div class="card card-color" style="position: relative; left: 0px; top: 0px;">' +
                                '<div class="card-header border-0 ui-sortable-handle" style="cursor: move;">' +
                                '<h3 class="card-title">' +
                                '<i class="fas fa-th mr-1"></i>' + this.nome.substr(0, 16) +
                                '</h3>' +
                                '<div class="card-tools mr-2 mb-1">' +
                                '<button type="button" class="btn btn-outline-light btn-sm" data-card-widget="collapse">' +
                                '<i class="fas fa-minus"></i>' +
                                '</button>' +
                                '<button type="button" class="btn btn-outline-light btn-sm" >' +
                                '<i class="fas fa-times"></i>' +
                                '</button>' +
                                '</div>' +
                                '</div>' +
                                '<div class="card-body p-0 mb-0" style="display: block;">' +
                                '<div class="card card-widget widget-user mb-0">' +
                                '<div class="widget-user-header card-color">' +
                                '<h3 class="widget-user-username">' + this.descricaoPerfil + '</h3>' +
                                '</div>' +
                                '<div class="widget-user-image">' +
                                '<img class="img-circle elevation-2" src="' + this.caminhoFoto + this.nomeFoto + '" alt="' + this.nomeFoto + '">' +
                                '</div>' +
                                '<div class="card-footer bg-gradient-light">' +
                                '<div class="row">' +
                                '<div class="col-sm-6 border-right">' +
                                '<div class="description-block">' +
                                '<h5 class="description-header">0</h5>' +
                                '<span class="description-text">Valores</span>' +
                                '</div>' +
                                '</div>' +
                                '<div class="col-sm-6">' +
                                '<div class="description-block">' +
                                '<h5 class="description-header">0</h5>' +
                                '<span class="description-text">Vidas</span>' +
                                '</div>' +
                                '</div>' +
                                '</div>' +
                                '</div>' +
                                '</div>' +
                                '</div>' +
                                '</div>' +
                                '</li>';
                        });
                        $(".TODO li").remove();
                        startSearch += startSearch;
                    }
                } else {
                    if (result.listCorretor.length > 0) {
                        $(".loader").show(100);
                        $.each(result.listCorretor, function (i, d) {
                            text += '<li data-realizado="0" data-vidas="0" data-valores="0" class="text-row ui-sortable-handle" data-user-id="' + this.idUsuario + '" id="id-"' + this.idUsuario + '">' +
                                '<div class="card card-color" style="position: relative; left: 0px; top: 0px;">' +
                                '<div class="card-header border-0 ui-sortable-handle" style="cursor: move;">' +
                                '<h3 class="card-title">' +
                                '<i class="fas fa-th mr-1"></i>' + this.nome.substr(0, 16) +
                                '</h3>' +
                                '<div class="card-tools mr-2 mb-1">' +
                                '<button type="button" class="btn btn-outline-light btn-sm" data-card-widget="collapse">' +
                                '<i class="fas fa-minus"></i>' +
                                '</button>' +
                                '<button type="button" class="btn btn-outline-light btn-sm" >' +
                                '<i class="fas fa-times"></i>' +
                                '</button>' +
                                '</div>' +
                                '</div>' +
                                '<div class="card-body p-0 mb-0" style="display: block;">' +
                                '<div class="card card-widget widget-user mb-0">' +
                                '<div class="widget-user-header card-color">' +
                                '<h3 class="widget-user-username">' + this.descricaoPerfil + '</h3>' +
                                '</div>' +
                                '<div class="widget-user-image">' +
                                '<img class="img-circle elevation-2" src="' + this.caminhoFoto + this.nomeFoto + '" alt="' + this.nomeFoto + '">' +
                                '</div>' +
                                '<div class="card-footer bg-gradient-light">' +
                                '<div class="row">' +
                                '<div class="col-sm-6 border-right">' +
                                '<div class="description-block">' +
                                '<h5 class="description-header">0</h5>' +
                                '<span class="description-text">Valores</span>' +
                                '</div>' +
                                '</div>' +
                                '<div class="col-sm-6">' +
                                '<div class="description-block">' +
                                '<h5 class="description-header">0</h5>' +
                                '<span class="description-text">Vidas</span>' +
                                '</div>' +
                                '</div>' +
                                '</div>' +
                                '</div>' +
                                '</div>' +
                                '</div>' +
                                '</div>' +
                                '</li>';
                        });

                        startSearch += startSearch;
                    }
                }
                $(".loader").hide("fast", function () {
                    $(this).prev().hide("fast", arguments.callee);
                });
                $('.TODO').append(text);
            },
            failure: function (data) {
                console.log(response);
            }
        });
    }
    else {
        toastr.error('Pesquisa deve conter ao menos 3 caracteres!')
    }
}
$("#searchKPIGrupo").change(function () {
    if ($("#searchKPIGrupo").val() != termoAnterior) {
        termoAnterior = $("#searchKPIGrupo").val();
        startSearch = 0;
    }
    if ($("#searchKPIGrupo").val().length == 0 && ht != "") {
        $(".TODO li").remove();
        $('.TODO').append(ht);
        ht = '';
    }
});
function ExcluirKPICard(id) {
    swal({
        title: "Você tem certeza?",
        text: "Que deseja Excluir o grupo.",
        type: "warning",
        showCancelButton: !0,
        confirmButtonText: "Sim!",
        cancelButtonText: "Não, cancelar!",
        reverseButtons: !0,
        confirmButtonColor: "#198754",
        cancelButtonColor: "#DC3545"
    }).then(function (e) {
        if (e.value === true) {
            formData = new FormData();
            formData.append('id', id);
            $.ajax({
                type: 'POST',
                url: "/KPIGrupo?handler=ExcluirKPIGrupo",
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
                        $('#btnRemove-' + id).CardWidget("remove");
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
function ExcluirKPICardUsuario(id) {
    swal({
        title: "Você tem certeza?",
        text: "Que deseja Excluir o corretor do grupo.",
        type: "warning",
        showCancelButton: !0,
        confirmButtonText: "Sim!",
        cancelButtonText: "Não, cancelar!",
        reverseButtons: !0,
        confirmButtonColor: "#198754",
        cancelButtonColor: "#DC3545"
    }).then(function (e) {
        if (e.value === true) {
            formData = new FormData();
            formData.append('id', id);
            $.ajax({
                type: 'POST',
                url: "/KPIGrupo?handler=ExcluirKPIGrupoUsuario",
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
                        $('#btnRemove-' + id).CardWidget("remove");
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

function SalvarKPIGrupo() {
    $('#ButtonSalvarKPIGrupo').on('click', function (evt) {
        evt.preventDefault();
        $.post('/KPIGrupo?handler=KPIGrupo', $('#formKPIGrupo').serialize(), function (data) {
            $('#KPIGrupoMensagemDiv').show();
            $('#KPIGrupoMensagemDivAlert').removeClass();
            $('#KPIGrupoMensagemDivAlert').addClass(data.mensagem.cssClass);

            $('#KPIGrupoMensagemIcon').removeClass();
            $('#KPIGrupoMensagemIcon').addClass(data.mensagem.iconClass);
            $('#KPIGrupoMensagemSpan').text(data.mensagem.mensagem);
        });

    });
}

function CadastroGrupos() {
    $('ul[id^="sort"]').sortable(
        {
            connectWith: ".sortable",
            revert: 300,
            receive: function (e, ui) {
                var grupo_id = $(ui.item).parent(".sortable").data("grupo-id");
                var meta_id = $(ui.item).parent(".sortable").data("meta-id");
                var usuario_id = $(ui.item).data("user-id");
                if (grupo_id == 0 || meta_id == 0 || usuario_id == 0) {
                    $(ui.sender).sortable('cancel', 'revert');
                    toastr.warning("Meta não foi cadastrada!");
                } else {
                    $.ajax({
                        url: '/KPIGrupo?handler=Edit&grupoId=' + grupo_id + '&usuarioId=' + usuario_id + '&metaId=' + meta_id,
                        success: function (data) {
                            if (data.status) {
                                for (var i = 0; i < $('#sort' + grupo_id + ' li').length; i++) {
                                    if ($('#sort' + grupo_id + ' li').eq(i).data('user-id') == "0") {
                                        $('#sort' + grupo_id + ' li').eq(i).remove();
                                    }
                                }
                                toastr.success("Salvo com sucesso!");
                            }
                        }
                    });
                }
            }

        }).disableSelection();
}


$(function () {

    /* initialize the external events
     -----------------------------------------------------------------*/
    function ini_events(ele) {
        ele.each(function () {

            // create an Event Object (http://arshaw.com/fullcalendar/docs/event_data/Event_Object/)
            // it doesn't need to have a start or end
            var eventObject = {
                title: $.trim($(this).text()) // use the element's text as the event title
            }

            // store the Event Object in the DOM element so we can get to it later
            $(this).data('eventObject', eventObject)

            // make the event draggable using jQuery UI
            $(this).draggable({
                zIndex: 1070,
                revert: true, // will cause the event to go back to its
                revertDuration: 0  //  original position after the drag
            })

        })
    }

    ini_events($('#external-events div.external-event'))

    /* initialize the calendar
     -----------------------------------------------------------------*/
    //Date for the calendar events (dummy data)
    var date = new Date()
    var d = date.getDate(),
        m = date.getMonth(),
        y = date.getFullYear()

    var Calendar = FullCalendar.Calendar;
    var Draggable = FullCalendarInteraction.Draggable;

    var containerEl = document.getElementById('external-events');
    var checkbox = document.getElementById('drop-remove');
    var calendarEl = document.getElementById('calendar');

    // initialize the external events
    // -----------------------------------------------------------------

    new Draggable(containerEl, {
        itemSelector: '.external-event',
        eventData: function (eventEl) {
            console.log(eventEl);
            return {
                title: eventEl.innerText,
                backgroundColor: window.getComputedStyle(eventEl, null).getPropertyValue('background-color'),
                borderColor: window.getComputedStyle(eventEl, null).getPropertyValue('background-color'),
                textColor: window.getComputedStyle(eventEl, null).getPropertyValue('color'),
            };
        }
    });


    if ($('#IdPerfil').val() == 6) {
        $('.help-block').attr('style', 'color: red !important;');
        CarregarCalendarMkt(Calendar, calendarEl);
    }
    else
    {
        CarregarCalendar(Calendar, calendarEl);
    }

    /* ADDING EVENTS */
    var currColor = '#3c8dbc' //Red by default
    //Color chooser button
    var colorChooser = $('#color-chooser-btn')
    $('#color-chooser > li > a').click(function (e) {
        e.preventDefault()
        //Save color
        currColor = $(this).css('color')
        //Add color effect to button
        $('#add-new-event').css({
            'background-color': currColor,
            'border-color': currColor
        })
        $('#VisitaIdStatusVisita').val($(this).data('id'));
    })
    $('#add-new-event').click(function (e) {
        e.preventDefault()
        //Get value and make sure it is not null
        var val = $('#VisitaEventoDataHora').val().split(' ')[1] + ' - ' + $('#VisitaTitulo').val();
        if (val.length == 0) {
            return
        }

        //Create events
        var event = $('<div />')
        event.css({
            'background-color': currColor,
            'border-color': currColor,
            'color': '#fff'
        }).addClass('external-event')
        event.html(val)
        $('#external-events').prepend(event)

        $.post('/Visita?handler=Visitas', $('#formVisita').serialize(), function (data) {
            if (data.status) {
                CarregarCalendar(Calendar, calendarEl);
            }
            else {
                alert(data.mensagem);
            }
        });

        //Add draggable funtionality
        ini_events(event)

        //Remove event from text input
        $('#VisitaTitulo').val('');
        $('#VisitaObservacao').val('')
    })
    $().on()
    $(".btn-confirmar").on('click', function () {
        alert('foi')
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
            "firstDay": 1
        }
    });

    //Date range picker with time picker
    $('#DataInicioFim').daterangepicker({
        timePicker: true,
        timePickerIncrement: 30,
           "locale": {
            "format": "DD/MM/YYYY hh:mm A",
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
            "firstDay": 1
        }
    });

    //$('input[name="DataInicioFim"]').daterangepicker({
    //    timePicker: true,
    //    timePicker24Hour: true,
    //    timePickerIncrement: 30,
    //    startDate: moment().startOf('hour'),
    //    endDate: moment().startOf('hour').add(32, 'hour'),
    //    "locale": {
    //        "format": "DD/MM/YYYY hh:mm A",
    //        "separator": " - ",
    //        "applyLabel": "Aplicar",
    //        "cancelLabel": "Cancelar",
    //        "daysOfWeek": [
    //            "Dom",
    //            "Seg",
    //            "Ter",
    //            "Qua",
    //            "Qui",
    //            "Sex",
    //            "Sab"
    //        ],
    //        "daysOfWeek": [
    //            "Dom",
    //            "Seg",
    //            "Ter",
    //            "Qua",
    //            "Qui",
    //            "Sex",
    //            "Sab"
    //        ],
    //        "monthNames": [
    //            "Janeiro",
    //            "Fevereiro",
    //            "Março",
    //            "Abril",
    //            "Maio",
    //            "Junho",
    //            "Julho",
    //            "Agosto",
    //            "Setembro",
    //            "Outubro",
    //            "Novembro",
    //            "Dezembro"
    //        ],
    //        "firstDay": 1
    //    }
    //});

    //$('input[name="DataSazonal"]').daterangepicker({
    //    singleDatePicker: true,
    //    showDropdowns: true,
    //    //minYear: 1901,
    //    "locale": {
    //        "format": "DD/MM/YYYY",
    //        "applyLabel": "Aplicar",
    //        "cancelLabel": "Cancelar",
    //        "daysOfWeek": [
    //            "Dom",
    //            "Seg",
    //            "Ter",
    //            "Qua",
    //            "Qui",
    //            "Sex",
    //            "Sab"
    //        ],
    //        cancelLabel: 'Clear',
    //        "daysOfWeek": [
    //            "Dom",
    //            "Seg",
    //            "Ter",
    //            "Qua",
    //            "Qui",
    //            "Sex",
    //            "Sab"
    //        ],
    //        "monthNames": [
    //            "Janeiro",
    //            "Fevereiro",
    //            "Março",
    //            "Abril",
    //            "Maio",
    //            "Junho",
    //            "Julho",
    //            "Agosto",
    //            "Setembro",
    //            "Outubro",
    //            "Novembro",
    //            "Dezembro"
    //        ],
    //        "firstDay": 1
    //    }
    //});

    PreencherRadio();
    BuscarVisita();

    $('.visitas-pesquisa').change(function () {

        VisitasPesquisa();
    });
   

    $('#menuItems').on('click', '.dropdown-item', function () {
        $('#dropdown_coins').text($(this)[0].value)
        $("#dropdown_coins").dropdown('toggle');
    })

    PreencherDropdown();

})

$(document).ready(function () {
    
    $('#formVisita')
        .bootstrapValidator({
            message: 'This value is not valid',
            framework: 'bootstrap',
            feedbackIcons: {
                valid: 'glyphicon glyphicon-ok',
                invalid: 'glyphicon glyphicon-remove',
                validating: 'glyphicon glyphicon-refresh'
            },
            fields: {
                Descricao: {
                    validators: {
                        notEmpty: {
                            message: 'Descrição é um campo obrigatório e não pode estar vazio.'
                        }
                    }
                },
                Cor: {
                    validators: {
                        color: {
                            type: ['hex', 'rgb', 'hsl', 'keyword'],
                            message: 'Deve ser uma cor %s válida.'
                        },
                        notEmpty: {
                            message: 'Cor é um campo obrigatório e não pode estar vazio.'
                        }
                    }
                },
                Tipo: {
                    validators: {
                        notEmpty: {
                            message: 'Tipo é um campo obrigatório e não pode estar vazio.'
                        }
                    }
                },
                DataSazonal: {
                    validators: {
                        notEmpty: {
                            message: 'Data Sazonal é um campo obrigatório e não pode estar vazio.'
                        }
                    }
                },
                DataInicioFim: {
                    validators:
                    {
                        notEmpty:
                        {
                            message: 'Data Início e Data Fim é um campo obrigatório e não pode estar vazio.'
                        },
                        //callback:
                        //{
                        //    message: 'The password is not valid',
                        //    callback: function (value, validator, $field) {
                        //        if (value === '') {
                        //            return {
                        //                valid: false,    // or false
                        //                message: 'Data Início e Data Fim é um campo obrigatório e não pode estar vazio.'
                        //            };
                        //        }
                        //    }
                        //}
                    }
                },
                Ativo: {
                    validators: {
                        notEmpty: {
                            message: 'Please specify at least one language you can speak'
                        }
                    }
                },
            }
        }).on('success.form.bv', function (e) {
            // Prevent form submission
            e.preventDefault();

            // Get the form instance
            var $form = $(e.target),
                form_data = new FormData();
            // Get the BootstrapValidator instance
            var bv = $form.data('bootstrapValidator');

            // Use Ajax to submit form data
            //$.post($form.attr('action'), $form.serialize(), function (result) {
            //    console.log(result);
            //}, 'json');


            //Verificacao de Repetição
            if ($('input[type="radio"][name="Tipo[]"]:checked').length > 0) {
                form_data.append('Tipo', $('input[type="radio"][name="Tipo[]"]:checked').val())
            }

            if ($('input[type="radio"][name="Repete[]"]:checked').length > 0) {
                form_data.append('Repete', $('input[type="radio"][name="Repete[]"]:checked').val())
            }

            if ($('input[type="radio"][name="Frequencia[]"]:checked').length > 0) {
                form_data.append('Frequencia', $('input[type="radio"][name="Frequencia[]"]:checked').val())
            }

            if ($('input[type="radio"][name="Termina[]"]:checked').length > 0) {
                form_data.append('Termina', $('input[type="radio"][name="Termina[]"]:checked').val())
            }



            form_data.append('IdVisita', $('#IdVisita').val());
            form_data.append('IdCalendarioSazonal', $('#IdCalendarioSazonal').val());
            form_data.append('Descricao', $('#Descricao').val());
            form_data.append('Cor', $('#Cor').val());
            //form_data.append('Tipo', $('input[type="radio"][name="Tipo[]"]:checked').val());
            $('input[name=ExisteCampanha]:checked').val() == 'true' ? form_data.append("ExisteCampanha", 'true') : form_data.append("ExisteCampanha", 'false');

            form_data.append('Repetir', $('#Repetir').val());

            form_data.append('DataSazonal', $('#DataSazonal').val());
            form_data.append('DataEm', $('#DataEm').val());
            var DataInicioFim = $('#DataInicioFim').val().split('-')
            form_data.append('DataInicio', DataInicioFim[0]);
            form_data.append('DataFim', DataInicioFim[1]);
            $('#Ativo').is(":checked") == true ? form_data.append("Ativo", 'true') : form_data.append("Ativo", 'false');
            form_data.append('Observacao', $('#Observacao').val());



            $.ajax({
                type: 'POST',
                url: "/Visita?handler=Visitas",
                data: form_data,
                cache: false,
                contentType: false,
                processData: false,
                beforeSend: function (xhr) {
                    xhr.setRequestHeader("XSRF-TOKEN",
                        $('input:hidden[name="__RequestVerificationToken"]').val());
                },
                success: function (data) {
                    if (data.status) {
                        //    $('#IdAssinaturaCartao').val(data.entityLista.idAssinaturaCartao);
                        //    $('#Titulo').val(data.entityLista.titulo);
                        //    $("#IdCampanha").val(data.entityLista.idCampanha).trigger('change');
                        //    $('#Titulo').focus();

                        //    $('.salvar-texto').css('display', 'block');
                        //    $('.upload-assinatura-cartao').css('display', 'none');
                        //    $('.btn-adicionar-imagem').css('display', 'none');
                    }
                },
                error: function () {
                    swal("Erro!", "Erro ao buscar o registro, contate o Administrador do Sistema.", "error");
                }
            });
        })
        .find('.Tipo')
        .on('change', function () {
            //input[type="radio"][name="Repete[]"]
            //var topic = $(this).val(),
            //    $container = $('[data-repete="' + topic + '"]');
            //$container.toggle();

            //var display = $container.css('display');
            //alert($(this).data('radio'))

            var topic = $(this).val();

            //Tipo
            if ($(this).data('radio') == 'Tipo') {
                
                   var $container = $('[data-tipo="' + topic + '"]');
                $container.toggle();

                var display = $container.css('display');
                if (3 == topic && 'block' == display) {
                    $('#formVisita').bootstrapValidator('addField', 'Frequencia[]', {
                        validators: {
                            notEmpty: {
                                message: 'O Campo Frequência é um campo obrigatório.'
                            }
                        }
                    });
                }
                else if (1 == topic || 2 == topic) {
                    $('#Frequencia').css('display', 'none');
                    $('#formVisita').bootstrapValidator('removeField', 'Frequencia[]');
                }
            }//

            //Repete
            if ($(this).data('radio') == 'Repete') {
                var $container = $('[data-repete="' + topic + '"]');
                $container.toggle();

                var display = $container.css('display');
                if (7 == topic && 'block' == display) {
                    $('#formVisita').bootstrapValidator('addField', 'Frequencia[]', {
                        validators: {
                            notEmpty: {
                                message: 'O Campo Frequência é um campo obrigatório.'
                            }
                        }
                    });
                }
                else if (1 == topic || 2 == topic || 3 == topic || 4 == topic || 5 == topic || 6 == topic) {
                    $('#Frequencia').css('display', 'none');
                    $('.BlocoRepetir').css('display', 'none');
                    $('#Termina').css('display', 'none');
                    $('input[type="radio"][name="Frequencia[]"]:checked').prop('checked', false); 
                    $('input[type="radio"][name="Termina[]"]:checked').prop('checked', false); 
                    $('#formVisita').bootstrapValidator('removeField', 'Frequencia[]');
                    $('#formVisita').bootstrapValidator('removeField', 'Repetir');
                    $('#formVisita').bootstrapValidator('removeField', 'Periodo');
                    $('#formVisita').bootstrapValidator('removeField', 'Termina[]');
                }
            }

            var tam = $('input[type="radio"][name="Frequencia[]"]:checked');
            if (tam.length > 0 && $('input[type="radio"][name="Repete[]"]:checked').val() == 7) {
                $('.BlocoRepetir').css('display', 'block');
                $('#Termina').css('display', 'block');

                $('#formVisita').bootstrapValidator('addField', 'Repetir', {
                    validators: {
                        notEmpty: {
                            message: 'O Campo Repete a Cada é um campo obrigatório.'
                        }
                    }
                });
                $('#formVisita').bootstrapValidator('addField', 'Periodo', {
                    validators: {
                        notEmpty: {
                            message: 'O Campo é um campo obrigatório.'
                        }
                    }
                });
                $('#formVisita').bootstrapValidator('addField', 'Termina[]', {
                    validators: {
                        notEmpty: {
                            message: 'O Campo é um campo obrigatório.'
                        }
                    }
                });
            } 
            //else {
            //    $('#formVisita').bootstrapValidator('removeField', 'Frequencia[]'); $('#formVisita').bootstrapValidator('removeField', 'Frequencia[]');
            //}

            //switch (true) {
            //    case (7 == topic && 'block' == display):
            //        $('#formVisita').bootstrapValidator('addField', 'Frequencia[]', {
            //            validators: {
            //                notEmpty: {
            //                    message: 'Please choose at least 1 framework'
            //                }
            //            }
            //        });
            //        break;
            //    case (7 == topic && 'none' == display):
            //        $('#formVisita').bootstrapValidator('removeField', 'Frequencia[]');
            //        break;
            //case ('javascript' == topic && 'block' == display):
            //    $('#interviewForm').bootstrapValidator('addField', 'js_frameworks[]', {
            //        validators: {
            //            notEmpty: {
            //                message: 'The name of framework is required'
            //            }
            //        }
            //    });
            //    break;
            //case ('javascript' == topic && 'none' == display):
            //    $('#interviewForm').bootstrapValidator('removeField', 'js_frameworks[]');
            //    break;
            //}
        });

     $(document).on('click', '.select2-results__option', function () {
        $("#select2-menuItems-container").val($(this).html());
    });

});

$('.select2').on('change', function () {
    //var data = $(".select2 option:selected").text();
    //$("#test").val(data);
    VisitasPesquisa();
})

//$(document).ready(function () {
   
//});

$(document).on('click', 'input[name="Tipo[]"]', function () {
    if ($(this).val() == 3) {
        $('.Observacao').css('display', 'block');
    } else {
        $('.Observacao').css('display', 'none');
    }

    if ($(this).val() == 1) {
        $('.DataSazonal').html('Data do Feriado');
        $('#DataSazonal').attr("disabled", false);
        $('#Sim').attr("disabled", false);
        $('#Nao').attr("disabled", false);
    }
    else if ($(this).val() == 2) {
        $('.DataSazonal').html('Data Comemorativa');
        $('#DataSazonal').attr("disabled", false);
        $('#Sim').attr("disabled", false);
        $('#Nao').attr("disabled", false);
    }
    else if ($(this).val() == 3) {
        $('.DataSazonal').html('Data do Evento');
        $('#DataSazonal').attr("disabled", false);
        $('#Sim').attr("disabled", true);
        $('#Nao').attr("disabled", true);
        $('#Sim').prop('checked', false);
        $('#Nao').prop('checked', false);
    } else {
        $('.DataSazonal').html('Data');
        $('#DataSazonal').attr("disabled", false);
        $('#Sim').attr("disabled", true);
        $('#Nao').attr("disabled", true);
    }

});

$(document).on('click', 'input[type="radio"][name="Frequencia[]"]:checked', function () {

    if ($(this).data('radio') == 'Diariamente') {
        $('#SelecaoRepete').html('Repete a Cada Dia');
    }
    else if ($(this).data('radio') == 'Semanamente') {
        $('#SelecaoRepete').html('Repete a Cada Semana');
    }
    else if ($(this).data('radio') == 'Mensalmente') {
        $('#SelecaoRepete').html('Repete a Cada Mês');
    }
    else if ($(this).data('radio') == 'Anualmente') {
        $('#SelecaoRepete').html('Repete a Cada Ano');
    }

});

$(document).on('click', '.todos-perfil', function () {

    VisitasPesquisa();
});

function BuscarVisita() {
    
    var UrlParametro = window.location.search;
    if (UrlParametro) {
        var ParametroIdNotificacao = UrlParametro.split('IdNotificacao');
        var IdNotificacao = ParametroIdNotificacao[1].substring(1, ParametroIdNotificacao[1].length);
        var ParametroId = ParametroIdNotificacao[0].split('Id');
        var Id = ParametroId[1].substring(1, ParametroId[1].length - 1)
        var Calendar = FullCalendar.Calendar;
        var calendarEl = document.getElementById('calendar');

        $.ajax({
            url: '/Visita?handler=BuscarVisita',
            method: "Get",
            data: { Id: Id, IdNotificacao: IdNotificacao },
            dataType: 'json',
            success: function (data) {
                if (data.status == true) {
                    var events = [];
                    events.push({
                        sourceId: data.listVisita.sourceId,
                        title: data.listVisita.title,
                        backgroundColor: data.listVisita.backgroundColor,
                        borderColor: data.listVisita.borderColor,
                        start: data.listVisita.start,
                        allDay: data.listVisita.allDay
                    });
                }
                CarregarCalendarPesquisa(Calendar, calendarEl, events);
            },
            error: function () {

            }
        });
    }
}

function VisitasPesquisa() {
    var d = new Date();
    var anoC = d.getFullYear();
    var mesC = d.getMonth();

    var DInicio = new Date(anoC, mesC, 1);
    var DFim = new Date(anoC, mesC + 1, 0);
    let DataInicial = "";
    let DataFinal = "";

    GetDiaMesAno(DInicio);
    GetDiaMesAno(DFim);

    var Calendar = FullCalendar.Calendar;
    var calendarEl = document.getElementById('calendar');

    let vetData = $("#Inicio").val().split(' - ');

    if (vetData[0] == GetDiaAtual() && vetData[1] == GetDiaAtual()) {
        DataInicial = GetDiaMesAno(DInicio);
        DataFinal = GetDiaMesAno(DFim);
    } else {
        DataInicial = vetData[0];
        DataFinal = vetData[1];
    }


    $.ajax({
        url: '/Visita?handler=VisitasPesquisa',
        method: "GET",
        data: { IdPerfil: $("#PreencherOption option:selected").val(), Nome: $("#select2-menuItems-container").html(), DataInicial: DataInicial, DataFinal: DataFinal, IdPerfilUsuario: $("#IdPerfilUsuario").val() },
        dataType: 'json',
        success: function (data) {
            if (data.status == true) {
                var events = [];
                $.map(data.listVisita, function (r) {
                    events.push({
                        sourceId: r.sourceId,
                        title: r.title,
                        backgroundColor: r.backgroundColor,
                        borderColor: r.borderColor,
                        start: r.start,
                        allDay: r.allDay
                    });
                });
            }

            CarregarCalendarPesquisa(Calendar, calendarEl, events)

        },
        error: function () {

        }
    });
}

function CarregarCalendar(Calendar, calendarEl) {
    $('#calendar').html('');
    var calendar = new Calendar(calendarEl, {
        plugins: ['bootstrap', 'interaction', 'dayGrid', 'timeGrid'],
        defaultView: 'dayGridMonth',
        locale: 'pt-br',
    /*    selectable: true,*/
        eventLimit: true, // permitir link a mais quando muitos eventos
        header: {
            left: 'prev,next today',
            center: 'title',
            right: 'dayGridMonth,timeGridWeek,timeGridDay'
        },
        //dayClick: function (date) {
        //    alert('clicked ' + date.format());
        //},
        //select: function (startDate, endDate) {
        //    alert(startDate.startStr + ' ' + startDate.endStr);
        //},
        'themeSystem': 'bootstrap',
        //Random default events
        events: function (info, successCallback, failureCallback) {
            $.ajax({
                url: '/Visita?handler=Visitas',
                cache: false,
                async: false,
                type: "GET",
                success: function (data) {
                    var events = [];
                    $.map(data.listVisita, function (r) {
                        if (r.end != '0001-01-01T00:00:00') {
                            events.push({
                                sourceId: r.sourceId,
                                title: r.title,
                                backgroundColor: r.backgroundColor,
                                borderColor: r.borderColor,
                                start: r.start,
                                end: r.end,
                                allDay: r.allDay
                            });
                        } else {
                            events.push({
                                sourceId: r.sourceId,
                                title: r.title,
                                backgroundColor: r.backgroundColor,
                                borderColor: r.borderColor,
                                start: r.start,
                                allDay: r.allDay
                            });
                        }
                    });
                    successCallback(events);
                }
            });
        },
        eventRender(info) {
            let tooltip = new Tooltip(info.el, {
                title: info.event.title,
                placement: 'top',
                trigger: 'hover',
                container: 'body',
                html: true,
            });
        },
        editable: true,
        droppable: true, // this allows things to be dropped onto the calendar !!!
        drop: function (info) {
            // is the "remove after drop" checkbox checked?
            if (checkbox.checked) {
                // if so, remove the element from the "Draggable Events" list
                info.draggedEl.parentNode.removeChild(info.draggedEl);
            }
        },
        eventClick: function (info) {
            var eventObj = info.event;
            console.log(eventObj.extendedProps.sourceId);
            $.ajax({
                url: '/Visita?handler=ByIdVisita&IdVisita=' + eventObj.extendedProps.sourceId,
                cache: false,
                async: false,
                contentType: "application/json",
                dataType: "json",
                type: "GET",
                success: function (data) {
                    if (data.status) {
                        $('#VisitaTitulo').val(data.entityVisita.descricao);
                        $('#VisitaEventoDataHora').val(new Date(data.entityVisita.dataAgendamento).toLocaleDateString('pt-br') + ' ' + new Date(data.entityVisita.dataAgendamento).toLocaleTimeString('pt-br'));
                        $('#VisitaObservacao').val(data.entityVisita.observacao);
                        $('#VisitaIdVisita').val(data.entityVisita.idVisita);
                    }
                }
            });
        }
    });

    calendar.render();
    // $('#calendar').fullCalendar()
}

/*#########################################################################*/

function CarregarCalendarMkt(Calendar, calendarEl) {
    $('#calendar').html('');
    var calendar = new Calendar(calendarEl, {
        plugins: ['bootstrap', 'interaction', 'dayGrid', 'timeGrid'],
        defaultView: 'dayGridMonth',
        locale: 'pt-br',
        selectable: true,
        //eventLimit: true, // permitir link a mais quando muitos eventos
        header: {
            left: 'prev,next today',
            center: 'title',
            right: 'dayGridMonth,timeGridWeek,timeGridDay'
        },
        dayClick: function (date) {
            alert('clicked ' + date.format());
        },
        select: function (startDate, endDate) {
            //alert(startDate.startStr.length + ' ' + startDate.endStr.length);
            $('#StartStr').val(startDate.startStr);
            $('#EndStr').val(startDate.endStr);
            //var VetStartStr = startDate.startStr.split('-');
            //console.log(VetStartStr[2] + '/' + VetStartStr[1] + '/' + VetStartStr[0])
            $('#DataEm').val(startDate.startStr);
            $('#FeriadoDataComemorativa').modal('show');
            //$('#DataSazonal').attr("disabled", true);
            $('.help-block').attr('style', 'display: none !important; color: red !important;');

            $('#Feriado').prop('checked', false);
            $('#DataComemorativa').prop('checked', false);
            $('#Evento').prop('checked', false);

            $('#Sim').prop('checked', false);
            $('#Nao').prop('checked', false);
            
        },
        'themeSystem': 'bootstrap',
        //Random default events
        events: function (info, successCallback, failureCallback) {
            $.ajax({
                url: '/Visita?handler=Visitas',
                cache: false,
                async: false,
                type: "GET",
                success: function (data) {
                    var events = [];
                    $.map(data.listVisita, function (r) {
                        if (r.end != null) {
                            var s = r.start.split('T');
                            var e = r.end.split('T');
                            events.push({
                                //sourceId: r.sourceId,
                                title: r.title,
                                //backgroundColor: r.backgroundColor,
                                //borderColor: r.borderColor,
                                start: s[0],
                                end: e[0],
                                //allDay: r.allDay,
                                overlap: false,
                                rendering: 'background',
                                color: '#ff9f89'
                            });
                        } else {
                            events.push({
                                sourceId: r.sourceId,
                                title: r.title,
                                backgroundColor: r.backgroundColor,
                                borderColor: r.borderColor,
                                start: r.start,
                                allDay: r.allDay,
                                //overlap: false,
                                //rendering: 'background',
                                //color: '#ff9f89'
                            });
                        }
                    });
                    successCallback(events);
                }
            });
        },
        eventRender(info) {
            let tooltip = new Tooltip(info.el, {
                title: info.event.title,
                placement: 'top',
                trigger: 'hover',
                container: 'body',
                html: true,
            });
            $(info.el).bind('dblclick', function (e) {
                var eventObj = info.event;
                console.log(eventObj.extendedProps.sourceId);
                
                if (eventObj.extendedProps.sourceId != undefined)
                {
                    $('#FeriadoDataComemorativa').modal('show');
                    $('#Descricao').focus();
                    $('#DataSazonal').attr("disabled", true);
                    $('.help-block').attr('style', 'display: none !important; color: red !important;');

                    $('#Feriado').prop('checked', false);
                    $('#DataComemorativa').prop('checked', false);
                    $('#Evento').prop('checked', false);

                    $('#Sim').prop('checked', false);
                    $('#Nao').prop('checked', false);


                }
                    //$.ajax({
                    //    url: '/Visita?handler=ByIdVisita&IdVisita=' + eventObj.extendedProps.sourceId,
                    //    cache: false,
                    //    async: false,
                    //    contentType: "application/json",
                    //    dataType: "json",
                    //    type: "GET",
                    //    success: function (data) {
                    //        if (data.status) {
                    //            $('#VisitaTitulo').val(data.entityVisita.descricao);
                    //            $('#VisitaEventoDataHora').val(new Date(data.entityVisita.dataAgendamento).toLocaleDateString('pt-br') + ' ' + new Date(data.entityVisita.dataAgendamento).toLocaleTimeString('pt-br'));
                    //            $('#VisitaObservacao').val(data.entityVisita.observacao);
                    //            $('#VisitaIdVisita').val(data.entityVisita.idVisita);
                    //        }
                    //    }
                    //});
            });
        },
        editable: true,
        droppable: true, // this allows things to be dropped onto the calendar !!!
        drop: function (info) {
            // is the "remove after drop" checkbox checked?
            if (checkbox.checked) {
                // if so, remove the element from the "Draggable Events" list
                info.draggedEl.parentNode.removeChild(info.draggedEl);
            }
        },
        /*eventClick: function (info) {
            var eventObj = info.event;
            console.log(eventObj.extendedProps.sourceId);
            if (eventObj.extendedProps.sourceId != undefined)
            $.ajax({
                url: '/Visita?handler=ByIdVisita&IdVisita=' + eventObj.extendedProps.sourceId,
                cache: false,
                async: false,
                contentType: "application/json",
                dataType: "json",
                type: "GET",
                success: function (data) {
                    if (data.status) {
                        $('#VisitaTitulo').val(data.entityVisita.descricao);
                        $('#VisitaEventoDataHora').val(new Date(data.entityVisita.dataAgendamento).toLocaleDateString('pt-br') + ' ' + new Date(data.entityVisita.dataAgendamento).toLocaleTimeString('pt-br'));
                        $('#VisitaObservacao').val(data.entityVisita.observacao);
                        $('#VisitaIdVisita').val(data.entityVisita.idVisita);
                    }
                }
            });
        }*/
    });

    calendar.render();
    // $('#calendar').fullCalendar()
}

function CarregarCalendarPesquisa(Calendar, calendarEl, Event) {
    $('#calendar').html('');
    var calendar = new Calendar(calendarEl, {
        plugins: ['bootstrap', 'interaction', 'dayGrid', 'timeGrid'],
        defaultView: 'dayGridMonth',
        locale: 'pt-br',
        header: {
            left: 'prev,next today',
            center: 'title',
            right: 'dayGridMonth,timeGridWeek,timeGridDay'
        },
        'themeSystem': 'bootstrap',
        //Random default events
        events: Event,
        eventRender(info) {
            let tooltip = new Tooltip(info.el, {
                title: info.event.title,
                placement: 'top',
                trigger: 'hover',
                container: 'body',
                html: true,
            });
        },
        editable: true,
        droppable: true, // this allows things to be dropped onto the calendar !!!
        drop: function (info) {
            // is the "remove after drop" checkbox checked?
            if (checkbox.checked) {
                // if so, remove the element from the "Draggable Events" list
                info.draggedEl.parentNode.removeChild(info.draggedEl);
            }
        },
        eventClick: function (info) {
            var eventObj = info.event;
            console.log(eventObj.extendedProps.sourceId);
            $.ajax({
                url: '/Visita?handler=ByIdVisita&IdVisita=' + eventObj.extendedProps.sourceId,
                cache: false,
                async: false,
                contentType: "application/json",
                dataType: "json",
                type: "GET",
                success: function (data) {
                    if (data.status) {
                        $('#VisitaTitulo').val(data.entityVisita.descricao);
                        $('#VisitaEventoDataHora').val(new Date(data.entityVisita.dataAgendamento).toLocaleDateString('pt-br') + ' ' + new Date(data.entityVisita.dataAgendamento).toLocaleTimeString('pt-br'));
                        $('#VisitaObservacao').val(data.entityVisita.observacao);
                        $('#VisitaIdVisita').val(data.entityVisita.idVisita);
                    }
                }
            });
        }
    });

    calendar.render();
    // $('#calendar').fullCalendar()
}

function PreencherRadio() {
    $.getJSON("/Visita?handler=TodosPerfil", function (data) {
        let html = '<option selected value="0">Todos</option>';
        if (data.status) {
            $.each(data.listVisita, function () {
                html += '<option class="todos-perfil" value="' + this.idPerfil + '">' + this.descricao + '</option>';
            });

            $("#IdPerfilUsuario").val(data.idPerfil);
        }
        $("#PreencherOption").html(html);
    });
}

function PreencherDropdown() {
    $.getJSON("/Visita?handler=TodosNomes", function (data) {
        let contents = [];
        let i = 0;
        for (let name of data.listVisita) {
            //contents.push('<input type="button" class="dropdown-item visitas-pesquisa-dropdown" role="option" aria-selected="true" type="button" value="' + name.nome.toUpperCase() + '"/>');
            contents.push('<option value="' + i + '">' + name.nome.toUpperCase() + '</option>');
            i++;
        }
        $('#menuItems').append(contents.join(""));

        //Esconder a linha que mostra que nenhum item foi encontrado
        $('#empty').hide();
    });
}



function Filtro(word) {
    let items = $(".dropdown-item");
    let length = items.length
    let collection = []
    let hidden = 0
    for (let i = 0; i < length; i++) {
        if ($(items[i]).val().toLowerCase().startsWith(word)) {
            $(items[i]).show()
        }
        else {
            $(items[i]).hide()
            hidden++
        }
    }

    //If all items are hidden, show the empty view
    if (hidden === length) {
        $('#empty').show()
    }
    else {
        $('#empty').hide()
    }
}

function InicializaCor() {
    var toread = document.getElementById('Cor');
    console.log("Red Value - " + parseInt("0x" + toread.value.slice(1, 3)));
}

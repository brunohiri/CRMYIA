var DataTerminaEm = '';
var Calendar = FullCalendar.Calendar;
var obj = {};
var submit = true;

window.mobilecheck = function () {
    var check = false;
    (function (a) { if (/(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|mobile.+firefox|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows ce|xda|xiino/i.test(a) || /1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-/i.test(a.substr(0, 4))) check = true; })(navigator.userAgent || navigator.vendor || window.opera);
    return check;
};

var calendarEl = document.getElementById('calendar');

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
                DataSazonal: {
                    validators: {
                        notEmpty: {
                            message: 'Data Sazonal é um campo obrigatório e não pode estar vazio.'
                        }
                    }
                },
                DataInicio: {
                    validators: {
                        callback: {
                            message: 'Data Início é maior que Data Fim.',
                            callback: function (DataInicio, validator) {
                                var DataFim = $('#DataFim').val();
                                return DataInicio < DataFim;
                            }
                        }
                    }
                },
                DataFim: {
                    validators: {
                        callback: {
                            message: 'Data Fim é menor que Data Início.',
                            callback: function (DataFim, validator) {
                                var DataInicio = $('#DataInicio').val();
                                return DataFim > DataInicio;
                            }
                        }
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

            //Verificacao de Repetição
            if ($('input[type="radio"][name="Tipo[]"]:checked').length > 0) {
                form_data.append('Tipo', $('input[type="radio"][name="Tipo[]"]:checked').data('tipo'));
            }

            if ($('input[type="radio"][name="Repete[]"]:checked').length > 0) {
                form_data.append('Repete', $('input[type="radio"][name="Repete[]"]:checked').val());
            }

            if ($('input[type="radio"][name="Frequencia[]"]:checked').length > 0) {
                form_data.append('Frequencia', $('input[type="radio"][name="Frequencia[]"]:checked').val());
            }

            if ($('input[type="radio"][name="Termina[]"]:checked').length > 0) {
                form_data.append('Termina', $('input[type="radio"][name="Termina[]"]:checked').val());
            }

            if ($('input[type="radio"][name="Frequencia[]"]:checked').val() == 3) {
                form_data.append('MesDataColocacao', $('#MesDataColocacao').val());
                form_data.append('MesDiaDaSemana', $('#DiaDaSemanaOriginal').val());
                form_data.append('MesDia', $('#MesDia').val());
                form_data.append('SelectMensalmente', $('#SelectMensalmente').val());
            }
            form_data.append('IdVisita', $('#IdVisita').val());
            form_data.append('IdCalendarioSazonal', $('#IdCalendarioSazonal').val());
            form_data.append('Descricao', $('#Descricao').val());
            form_data.append('Cor', $('#Cor').val());
            form_data.append('OpExcluirAlterar', $('#OpExcluirAlterar').val());
            form_data.append('GuidId', $('#GuidId').val());
            //form_data.append('Tipo', $('input[type="radio"][name="Tipo[]"]:checked').val());
            $('input[type="radio"][name="ExisteCampanha[]"]:checked').val() == 'true' ? form_data.append("ExisteCampanha", 'true') : form_data.append("ExisteCampanha", 'false');

            form_data.append('Repetir', $('#Repetir').val());

            form_data.append('DataSazonal', $('#DataSazonal').val());
            form_data.append('DataTerminaEm', $('#DataTerminaEm').val());

            //var DataInicioFim = $('#DataInicioFim').val().split('-')
            //var DataInicio = DataInicioFim[0];
            //var DataFim = DataInicioFim[1];
            form_data.append('DataInicio', $('#DataInicio').val());
            form_data.append('DataFim', $('#DataFim').val());

            $('#Ativo').is(":checked") == true ? form_data.append("Ativo", 'true') : form_data.append("Ativo", 'false');
            form_data.append('Observacao', $('#Observacao').val());

            if ($('input[type="radio"][name="Frequencia[]"]:checked').val() == 2) {//Semanalmente
                var semamadia = [];
                var Semana = $('.Semana');
                for (var i = 0; i < Semana.length; i++) {
                    if (Semana[i].value == "true")
                        semamadia.push(Semana[i].dataset.posicao.toString());
                }
                form_data.append('Semana', semamadia);
            }
            $('#calendar').html('');
            var calendar = new Calendar(calendarEl, {
                plugins: ['bootstrap', 'interaction', 'dayGrid', 'timeGrid'],
                defaultView: window.mobilecheck() ? 'timeGridWeek' : 'dayGridMonth',
                //timeZone: 'UTC',
                eventLimit: true, // allow "more" link when too many events
                views: {
                    timelineFourDays: {
                        type: 'timeline',
                    }
                },
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
                    $('#formVisita')
                        .bootstrapValidator('disableSubmitButtons', false)  // Enable the submit buttons
                        .bootstrapValidator('resetForm', true);             // Reset the form

                    $('#Ativo').prop('checked', true);
                    $('#Repetir').val('1');

                    //startDate.start
                    $('#StartStr').val(startDate.startStr);
                    $('#EndStr').val(startDate.endStr);

                    var StartStr = startDate.startStr.split('-');
                    var Hora;
                    var Minutos;

                    /*if (AnoBissexto(StartStr[0])) {*/
                    var DataInicio = new Date();
                    Hora = DataInicio.getHours();
                    Minutos = DataInicio.getMinutes();
                    DataInicio = startDate.start;
                    DataInicio.setHours(Hora);
                    DataInicio.setMinutes(Minutos);
                    DataInicio.setSeconds(00);

                    DataInicio.setMinutes(DataInicio.getMinutes() - DataInicio.getTimezoneOffset());
                    document.getElementById('DataInicio').value = DataInicio.toISOString().slice(0, 16);

                    var DataFim = new Date();
                    Hora = DataFim.getHours();
                    Minutos = DataFim.getMinutes();
                    DataFim = startDate.start;
                    DataFim.setHours(Hora);
                    DataFim.setMinutes(Minutos);
                    DataFim.setSeconds(00);
                    DataFim.setDate(DataInicio.getDate());

                    DataFim.setMinutes(DataFim.getMinutes() - DataFim.getTimezoneOffset()); DataFim.setMinutes(DataFim.getMinutes() + 30);
                    document.getElementById('DataFim').value = DataFim.toISOString().slice(0, 16);

                    var DataTerminaEm = new Date();
                    Hora = DataTerminaEm.getHours();
                    Minutos = DataTerminaEm.getMinutes();
                    DataTerminaEm = startDate.start;
                    DataTerminaEm.setHours(Hora);
                    DataTerminaEm.setMinutes(Minutos);
                    DataTerminaEm.setSeconds(00);
                    DataTerminaEm.setMinutes(DataTerminaEm.getMinutes() - DataTerminaEm.getTimezoneOffset());
                    document.getElementById('DataTerminaEm').value = DataTerminaEm.toISOString().slice(0, 16);
                    $('#DataTerminaEm').val(startDate.startStr);
                    ObterDataColocacaoDiaDaSemana(startDate.startStr);

                    $('#formVisita').bootstrapValidator('addField', 'Tipo[]', {
                        validators: {
                            notEmpty: {
                                message: 'Tipo é um campo obrigatório.'
                            }
                        }
                    });

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
                   
                    form_data.append('StartStr', info.startStr);
                    form_data.append('EndStr', info.endStr);
                    //Ajax
                    if ($('input[type="radio"][name="Repete[]"]:checked').val() != undefined) {
                        swal({
                            title: 'Aguarde',
                            text: 'Estamos agendando seus eventos...',
                            allowOutsideClick: false,
                            showConfirmButton: false,
                            onOpen: () => {

                                //Inicia o Loading
                                swal.showLoading();

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
                                            var events = [];
                                            $.map(data.listVisita, function (r) {
                                                if (r.tipo == 1) {
                                                    //Feriado
                                                    events.push({
                                                       /* sourceId: r.sourceId,*/
                                                        title: r.title,
                                                        backgroundColor: r.backgroundColor,
                                                        borderColor: r.borderColor,
                                                        start: r.start,
                                                        //end: r.end,
                                                        allDay: r.allDay,
                                                        //overlap: false,
                                                        //rendering: 'background',
                                                        //color: '#ff9f89'
                                                    });

                                                    var s = r.start.split('T');
                                                    var e = r.end.split('T');
                                                    events.push({
                                                        title: r.title,
                                                        start: s[0],
                                                        end: e[0],
                                                        overlap: false,
                                                        rendering: 'background'
                                                    });
                                                } else if (r.tipo == 2) {
                                                    //Data Comemorativa
                                                    events.push({
                                                        /*sourceId: r.sourceId,*/
                                                        title: r.title,
                                                        backgroundColor: r.backgroundColor,
                                                        borderColor: r.borderColor,
                                                        start: r.start,
                                                        //end: r.end,
                                                        allDay: r.allDay,
                                                        //overlap: false,
                                                        //rendering: 'background',
                                                        //color: '#ff9f89'
                                                    });

                                                    var s = r.start.split('T');
                                                    var e = r.end.split('T');
                                                    events.push({
                                                        title: r.title,
                                                        start: s[0],
                                                        end: e[0],
                                                        overlap: false,
                                                        rendering: 'background'
                                                    });
                                                } else if (r.tipo == 3) {
                                                    //Evento
                                                    events.push({
                                                        sourceId: r.sourceId,
                                                        title: r.title,
                                                        backgroundColor: r.backgroundColor,
                                                        borderColor: r.borderColor,
                                                        start: r.start,
                                                        end: r.end,
                                                        allDay: r.allDay,
                                                        //overlap: false,
                                                        //rendering: 'background',
                                                        //color: '#ff9f89'
                                                    });
                                                }

                                                //Fecha e finaliza o Loading
                                                swal.close(); swal.hideLoading();
                                            });
                                            $('input[type="radio"][name="Repete[]"]:checked').prop('checked', false);
                                            $('#FeriadoDataComemorativa').modal('hide');
                                            form_data.delete('Tipo');
                                            form_data.delete('Repete');
                                            form_data.delete('Frequencia');
                                            form_data.delete('Termina');
                                            form_data.delete('MesDataColocacao');
                                            form_data.delete('MesDiaDaSemana');
                                            form_data.delete('MesDia');
                                            form_data.delete('SelectMensalmente');
                                            form_data.delete('IdCalendarioSazonal');
                                            form_data.delete('Descricao');
                                            form_data.delete('Cor');
                                            form_data.delete('OpExcluirAlterar');
                                            form_data.delete('GuidId');
                                            form_data.delete('ExisteCampanha');
                                            form_data.delete('Repetir');
                                            form_data.delete('DataSazonal');
                                            form_data.delete('DataTerminaEm');
                                            form_data.delete('DataInicio');
                                            form_data.delete('DataFim');
                                            form_data.delete('Ativo');
                                            form_data.delete('Observacao');
                                            form_data.delete('Semana');
                                            //form_data.delete('StartStr');
                                            //form_data.delete('EndStr');
                                            successCallback(events);
                                            location.reload(true);
                                        }

                                    },
                                    error: function () {
                                        swal("Erro!", "Erro ao buscar o registro, contate o Administrador do Sistema.", "error");
                                    }
                                });
                            }

                        });
                    }

                    
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

                        if (eventObj.extendedProps.sourceId != undefined) {
                            //Aciona no duplo Clique

                            $('#Descricao').focus();
                            $('#DataSazonal').attr("disabled", true);
                            $('.help-block').attr('style', 'display: none !important; color: red !important;');

                            $('#Feriado').prop('checked', false);
                            $('#DataComemorativa').prop('checked', false);
                            $('#Evento').prop('checked', false);

                            $('#Sim').prop('checked', false);
                            $('#Nao').prop('checked', false);

                            var form_data = new FormData();
                            form_data.append('IdVisita', eventObj.extendedProps.sourceId);
                            $.ajax({
                                type: 'POST',
                                url: "/Visita?handler=Obter",
                                data: form_data,
                                cache: false,
                                contentType: false,
                                processData: false,
                                beforeSend: function (xhr) {
                                    xhr.setRequestHeader("XSRF-TOKEN",
                                        $('input:hidden[name="__RequestVerificationToken"]').val());
                                },
                                success: function (data) {
                                    var html = '';
                                    //2021-05-09T05:24:00


                                    if (data.status) {
                                        var StrData = '';

                                        var VetDataInicio = data.entityVisita.dataInicio.split('T');
                                        var VetDataFim = data.entityVisita.dataFim.split('T');

                                        var DataHoje = new Date();
                                        if (DataHoje.getMonth() < 9)
                                            var DataHojeFormatada = (DataHoje.getFullYear() + "-" + ("0" + (DataHoje.getMonth() + 1)) + "-" + (DataHoje.getDate()));
                                        else
                                            var DataHojeFormatada = (DataHoje.getFullYear() + "-" + ((DataHoje.getMonth() + 1)) + "-" + (DataHoje.getDate()));

                                        var DataInicio = new Date(data.entityVisita.dataInicio);
                                        var DataFim = new Date(data.entityVisita.dataFim);
                                        var DataMoment = moment(data.entityVisita.dataInicio, "MM-DD-YYYY");

                                        //DataMoment._locale._months[11] mes
                                        //DataMoment._locale._weekdays[6] dia da semana

                                        if (VetDataInicio[0] == VetDataFim[0] && DataHojeFormatada == VetDataInicio[0] && DataHojeFormatada == VetDataFim[0]) {
                                            //Hoje
                                            StrData = 'Hoje, ' + VetDataInicio[1].trim().substring(0, 5) + ' - ' + VetDataFim[1].trim().substring(0, 5);
                                        } else if (VetDataInicio[0] == VetDataFim[0] && data.entityVisita.dataInicio < data.entityVisita.dataFim) {
                                            StrData = DataMoment._locale._weekdays[DataInicio.getDay()] + ', ' + DataInicio.getDate() + ' de ' + DataMoment._locale._months[DataInicio.getMonth()] + ', ' + VetDataInicio[1].trim().substring(0, 5) + ' - ' + VetDataFim[1].trim().substring(0, 5);
                                        } else if (VetDataInicio[0] < VetDataFim[0] && data.entityVisita.dataInicio < data.entityVisita.dataFim) {
                                            StrData = DataMoment._locale._weekdays[DataInicio.getDay()] + ', ' + DataInicio.getDate() + ' de ' + DataMoment._locale._months[DataInicio.getMonth()] + ', ' + VetDataInicio[1].trim().substring(0, 5) + ' - '
                                                + DataMoment._locale._weekdays[DataFim.getDay()] + ', ' + DataFim.getDate() + ' de ' + DataMoment._locale._months[DataFim.getMonth()] + ', ' + VetDataFim[1].trim().substring(0, 5);
                                        }

                                        html += '<div class="card text-center">\
                                        <div class="card-header">Titulo: '+ data.entityVisita.descricao.toUpperCase() + '<br> ' + StrData + '</div>\
                                        <div class="card-body">\
                                            <div class="form-group">\
                                                <label class="col-lg-12 control-label" > Selecione uma opção</label>\
                                                <div class="col-lg-12">\
                                                    <div class="checkbox">\
                                                        <label><input type="radio" class="Tipo" name="ExcluirAlterar[]" value="1" /> Alterar </label>\
                                                    </div>\
                                                    <div class="checkbox">\
                                                        <label><input type="radio" class="Tipo" name="ExcluirAlterar[]" value="2" /> Excluir </label>\
                                                    </div>\
                                                </div>\
                                            </div>\
                                            <div class="d-flex justify-content-center">\
                                                <div class="col-sm-6" id="BlocoExcluirAlterar" style="display: none">\
                                                    <label id="TituloExcluirAlterar"></label>\
                                                    <div class="form-group">\
                                                        <div class="form-check form-check-inline">\
                                                            <input class="form-check-input Tipo" type="radio" name="OpExcluirAlterar[]" id="EsteEvento" value="1" checked>\
                                                                <label class="form-check-label" for="EsteEvento">Este Evento</label>\
                                                                    </div><br />';
                                        if (data.existeMaisQueUm) {
                                            html += '<div class="form-check form-check-inline">\
                                                                <input class="form-check-input Tipo" type="radio" name="OpExcluirAlterar[]" id="EventosSeguintes" value="2">\
                                                                    <label class="form-check-label" for="EventosSeguintes">Este e os eventos Seguintes</label>\
                                                                    </div><br />\
                                                                <div class="form-check form-check-inline">\
                                                                    <input class="form-check-input Tipo" type="radio" name="OpExcluirAlterar[]" id="TodosEventos" data-tipo="3" data-radio="Tipo" value="3">\
                                                                        <label class="form-check-label" for="TodosEventos">Todos os Eventos</label>\
                                                                    </div><br />';
                                        }
                                        html += '</div>\
                                                            </div>\
                                                            <!--<div class="col-sm-6">\
                                                            </div>-->\
                                                        </div>\
                                                    </div>\
                                                    <!--<div class="card-footer text-muted">\
                                                        2 days ago\
                                                    </div>-->\
                                                </div>\
                                                <div class="modal-footer d-flex justify-content-around">\
                                                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Cancelar</button>\
                                                    <div id="BtnAlterar" style="display: none"><button type="button" class="btn btn-warning btn-alterar-excluir" data-btn="alterar">Alterar</button></div>\
                                                    <div id="BtnExcluir" style="display: none"><button type="button" class="btn btn-danger btn-alterar-excluir" data-btn="excluir">Excluir</button></div>\
                                                </div>';
                                        $('#AlterarExcluirIdVisita').val(data.entityVisita.idVisita);
                                        $('#GuidId').val(data.entityVisita.guidId);
                                        $('#AlterarExcluir').modal('show');
                                    }
                                    $('#modalAlterarExcluir').html(html);
                                },
                                error: function () {
                                    swal("Erro!", "Erro ao buscar o registro, contate o Administrador do Sistema.", "error");
                                }
                            });


                        }
                    });
                },
                editable: false,
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

            $form
                .bootstrapValidator('disableSubmitButtons', false)  // Enable the submit buttons
                .bootstrapValidator('resetForm', true);             // Reset the form
        })
        .find('.Tipo')
        .on('change', function () {
            var $this = this;
            AdicionarValidacao($this);
        });

});


$('.select2').on('change', function () {
    VisitasPesquisa();
});

$(document).on('click', 'input[name="Tipo[]"]', function () {
   
    if ($(this).val() == 3 && $(this).data('tipo') == 1) {
        $('.DataSazonal').html('Data do Feriado');
        $('#DataSazonal').attr("disabled", false);
        $('#Sim').attr("disabled", false);
        $('#Nao').attr("disabled", false);
        $('.Observacao').css('display', 'block');
    }
    else if ($(this).val() == 3 && $(this).data('tipo') == 2) {
        $('.DataSazonal').html('Data Comemorativa');
        $('#DataSazonal').attr("disabled", false);
        $('#Sim').attr("disabled", false);
        $('#Nao').attr("disabled", false);
        $('.Observacao').css('display', 'block');

    }
    else if ($(this).val() == 3 && $(this).data('tipo') == 3) {
        $('.DataSazonal').html('Data do Evento');
        $('#DataSazonal').attr("disabled", false);
        $('#Sim').attr("disabled", true);
        $('#Nao').attr("disabled", true);
        $('#Sim').prop('checked', false);
        $('#Nao').prop('checked', false);
        $('.Observacao').css('display', 'block');
    }
    else {
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
    else if ($(this).data('radio') == 'Semanalmente') {
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

$(document).on('change', '#DataInicioFim', function () {
    var data = $(this).val();

    var dataVet = data.split('-');
    var dataVet1 = dataVet[1].trim().split(' ');
    var dataVet2 = dataVet1[0].split('/');//2021-04-06
    DataTerminaEm = dataVet2[2] + '-' + dataVet2[1] + '-' + dataVet2[0];
    $('#DataTerminaEm').val(DataTerminaEm);
});

$(document).on('click', 'input[type="radio"][name="ExcluirAlterar[]"]:checked', function () {
    $('#BlocoExcluirAlterar').css('display', 'block');

    if ($(this).val() == 1) {
        $('#TituloExcluirAlterar').html('Selecione uma opção para ser alterada.');
        $('#BtnAlterar').css('display', 'block');
        $('#BtnExcluir').css('display', 'none');
    } else if ($(this).val() == 2) {
        $('#TituloExcluirAlterar').html('Selecione uma opção para ser excluida.');
        $('#BtnAlterar').css('display', 'none');
        $('#BtnExcluir').css('display', 'block');
    }

});

$(document).on('click', '.btn-alterar-excluir', function () {
    if ($(this).data('btn') == 'alterar') {
        $('#AlterarExcluir').modal('hide');
        var startDate = new Date(obj.DataInicio);
        var endDate = new Date(obj.DataFim);
        $('#FeriadoDataComemorativa').modal('show');
        $('#exampleModalLabel').html('Calendário Marketing - Alterar');
        document.getElementById('DataInicio').value = obj.DataInicio;
        document.getElementById('DataFim').value = obj.DataFim;
        var DataTerminaEm = obj.DataInicio.toString().slice(0, 10);
        var VetDataTerminaEm = DataTerminaEm.split('-');
        VetDataTerminaEm[1] = ("0" + (parseInt(VetDataTerminaEm[1]) + 1) ).slice(-2);
        document.getElementById('DataTerminaEm').value = VetDataTerminaEm[0] + '-' + VetDataTerminaEm[1] + '-' + VetDataTerminaEm[2];

        $('#IdVisita').val(obj.IdVisita);
        $('#Descricao').val(obj.Descricao);
        $('#Cor').val(obj.Cor);
        $('#OpExcluirAlterar').val(obj.OpExcluirAlterar);
        $('#GuidId').val(obj.GuidId);
        $('#Repetir').val(obj.Repetir);

        if (obj.Tipo == 1) {
            $("#Feriado").prop("checked", true);
            $('#Repete').css('display', 'block');

            $('.DataSazonal').html('Data do Feriado');
            $('#DataSazonal').attr("disabled", false);
            $('#Sim').attr("disabled", false);
            $('#Nao').attr("disabled", false);
            $('.Observacao').css('display', 'block');
        } else if (obj.Tipo == 2) {
            $("#DataComemorativa").prop("checked", true);
            $('#Repete').css('display', 'block');

            $('.DataSazonal').html('Data Comemorativa');
            $('#DataSazonal').attr("disabled", false);
            $('#Sim').attr("disabled", false);
            $('#Nao').attr("disabled", false);
            $('.Observacao').css('display', 'block');
        } else if (obj.Tipo == 3) {
            $("#Evento").prop("checked", true);
            $('#Repete').css('display', 'block');

            $('.DataSazonal').html('Data do Evento');
            $('#DataSazonal').attr("disabled", false);
            $('#Sim').attr("disabled", true);
            $('#Nao').attr("disabled", true);
            $('#Sim').prop('checked', false);
            $('#Nao').prop('checked', false);
            $('.Observacao').css('display', 'none');
        }

        if (obj.Repete == 1) {
            $('#Nunca').prop("checked", true);
        } else if (obj.Repete == 2) {
            $('#TodosDias').prop("checked", true);
        } else if (obj.Repete == 3) {
            $('#ACadaSemana').prop("checked", true);
        } else if (obj.Repete == 4) {
            $('#ACada2Semanas').prop("checked", true);
        } else if (obj.Repete == 5) {
            $('#ACadaMeses').prop("checked", true);
        } else if (obj.Repete == 6) {
            $('#ACadaAno').prop("checked", true);
        } else if (obj.Repete == 7) {
            $('#Personalizado').prop("checked", true);
            $('#Frequencia').css('display', 'block');
        }


        if (obj.Frequencia == 1) {
            $('#Diariamente').prop("checked", true);
            $('.BlocoRepetir').css('display', 'block');
        } else if (obj.Frequencia == 2) {
            $('#Semanalmente').prop("checked", true);
            $('.BlocoRepetir').css('display', 'block');
        } else if (obj.Frequencia == 3) {
            $('#Mensalmente').prop("checked", true);
            $('.BlocoRepetir').css('display', 'block');
        } else if (obj.Frequencia == 4) {
            $('#Anualmente').prop("checked", true);
            $('.BlocoRepetir').css('display', 'block');
        }

        if (obj.Termina == 1) {
            $('#TerminaNunca').prop("checked", true);
            $('#Termina').css('display', 'block');
        } else if (obj.Termina == 2) {
            $('#Em').prop("checked", true);
            $('#Termina').css('display', 'block');
            var vetDataTerminaEm = obj.DataTerminaEm.split('T');
            $('#DataTerminaEm').val(vetDataTerminaEm[0].toString());
        }

        if (obj.Semana != undefined && obj.Semana != '') {
            $('#Semana').css('display', 'block');
            var HtmlSemana = $('.Semana');
            for (var j = 0; j < HtmlSemana.length; j++) {
                    HtmlSemana[j].classList.remove('btn-success');
                    HtmlSemana[j].classList.add('btn-secondary');
                    HtmlSemana[j].value = false;
            }
            if (obj.Semana.indexOf(',') > -1) {
                var Semana = obj.Semana.split(',');
                
                for (var i = 0; i < Semana.length; i++) {
                    for (var j = 0; j < HtmlSemana.length; j++) {
                        if (HtmlSemana[j].dataset.posicao == Semana[i]) {
                            HtmlSemana[j].classList.remove('btn-secondary');
                            HtmlSemana[j].classList.add('btn-success');
                            HtmlSemana[j].value = true;
                        }
                    }
                }

            } else {
                for (var j = 0; j < HtmlSemana.length; j++) {
                    if (HtmlSemana[j].dataset.posicao == obj.Semana) {
                        HtmlSemana[j].classList.remove('btn-secondary');
                        HtmlSemana[j].classList.add('btn-success');
                        HtmlSemana[j].value = true;
                    }
                }
            }
        }

        if (obj.SelectMensalmente == 1 || obj.SelectMensalmente == 2) {
            $('#Mes').css('display', 'block');
            var DiaDaSemana = "";
            if ("Sunday" == obj.MesDiaDaSemana) {
                DiaDaSemana = "no " + obj.MesDataColocacao + "&ordm;" + " domingo";
            } else if ("Monday" == obj.MesDiaDaSemana) {
                DiaDaSemana = "na " + obj.MesDataColocacao + "&ordf;" + " segunda";
            }
            else if ("Tuesday" == obj.MesDiaDaSemana) {
                DiaDaSemana = "na " + obj.MesDataColocacao + "&ordf;" + " terça";
            }
            else if ("Wednesday" == obj.MesDiaDaSemana) {
                DiaDaSemana = "na " + obj.MesDataColocacao + "&ordf;" + " quarta";
            }
            else if ("Thursday" == obj.MesDiaDaSemana) {
                DiaDaSemana = "na " + obj.MesDataColocacao + "&ordf;" + " quinta";
            }
            else if ("Friday" == obj.MesDiaDaSemana) {
                DiaDaSemana = "na " + obj.MesDataColocacao + "&ordf;" + " sexta";
            }
            else if ("Saturday" == obj.MesDiaDaSemana) {
                DiaDaSemana = "no " + obj.MesDataColocacao + "&ordm;" + " sabado";
            }

            var html = '';
            $('#Mes').html(html);
            html += '<div class="col-sm-6">\
                <label>Mensalmente</label>\
                <div class="form-group">\
                    <select class="form-control" id="SelectMensalmente">';
            html += '<option value=1>Mensalmente no dia ' + obj.MesDia + '</option>';
            html += '<option value=2>Mensalmente ' + DiaDaSemana + '</option>';

            html += '</select>\
                </div>\
                </div >';
            $('#DiaDaSemanaOriginal').val(obj.MesDiaDaSemana);
            $('#MesDataColocacao').val(obj.MesDataColocacao);
            $('#MesDia').val(obj.MesDia);

            $('#Mes').html(html);
        }

    }
    else if($(this).data('btn') == 'excluir')
    {
        $('#AlterarExcluir').modal('hide');
        //Remove
        var form_data = new FormData();
        form_data.append('IdVisita', $('#IdVisita').val());
        form_data.append('GuidId', $('#GuidId').val());
        form_data.append('OpExcluirAlterar', $('input[type="radio"][name="OpExcluirAlterar[]"]:checked').val());

        swal({
        title: 'Aguarde',
        text: 'Estamos Excluindo os seus eventos...',
        allowOutsideClick: false,
        showConfirmButton: false,
            onOpen: () => {

                swal.showLoading();

                $.ajax({
                    type: 'POST',
                    url: "/Visita?handler=Excluir",
                    data: form_data,
                    cache: false,
                    contentType: false,
                    processData: false,
                    beforeSend: function (xhr) {
                        xhr.setRequestHeader("XSRF-TOKEN",
                            $('input:hidden[name="__RequestVerificationToken"]').val());
                    },
                    success: function (data) {
                        swal.close(); swal.hideLoading();
                        if (data.status) {
                            swal({
                                type: "success",
                                title: 'Perfeito',
                                html: 'Eventos excluidos com sucesso!!!',
                                showConfirmButton: false,
                            });
                            CarregarCalendarMkt(Calendar, calendarEl);
                        }
                        form_data.remove('IdVisita');
                        form_data.remove('GuidId');
                        form_data.remove('OpExcluirAlterar');
                    },
                    error: function () {
                        swal("Erro!", "Erro ao buscar o registro, contate o Administrador do Sistema.", "error");
                    }
                });
            }
        });
    }

});

$(document).on('click', 'input[type="radio"][name="OpExcluirAlterar[]"]:checked', function () {
        obj.OpExcluirAlterar = $(this).val();
        $('.btn-alterar-excluir').prop('disabled', false);
});

$('button[name="Semana"]').click(function () {
    if ($(this).val() == "false") {
        $('.' + $(this).data('semana')).removeClass('btn-secondary').addClass('btn-success');
        $(this).val("true");
    } else if ($(this).val() == "true"){
        $('.' + $(this).data('semana')).removeClass('btn-success').addClass('btn-secondary');
        $(this).val("false");
    }
});

function SelecionarDiaDaSemana(){
    var d = new Date();
    var weekday = new Array(7);
    weekday[0] = "Domingo";
    weekday[1] = "Segunda";
    weekday[2] = "Terca";
    weekday[3] = "Quarta";
    weekday[4] = "Quinta";
    weekday[5] = "Sexta";
    weekday[6] = "Sabado";

    var a = $('.Semana');
    for (var i = 0; i < a.length; i++) {
        a[i].classList.remove("btn-success");
        a[i].classList.add("btn-secondary");
        a[i].value = "false";
    }
   
    //Dia da Semana
    var n = weekday[d.getDay()];

    $('.' + n).removeClass('btn-secondary');
    $('.' + n).addClass('btn-success');
    $('.' + n).val('true');
}

function GerarSelectOption() {

    formData = new FormData();
    formData.append('DataInicio', $('#StartStr').val());

    ObterDataColocacaoDiaDaSemana($('#StartStr').val());
}

async function ObterDataColocacaoDiaDaSemana(Data) {
    $('#DataTerminaEm').val(Data);
    formData = new FormData();
    formData.append('DataInicio', Data);
    $.ajax({
        type: 'POST',
        url: "/Visita?handler=DataColocacao",
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
                var DiaDaSemana = "";
                if ("Sunday" == data.mesDiaDaSemana) {
                    DiaDaSemana = "no " + data.mesDataColocacao + "&ordm;" + " domingo";
                } else if ("Monday" == data.mesDiaDaSemana) {
                    DiaDaSemana = "na " + data.mesDataColocacao + "&ordf;" + " segunda";
                }
                else if ("Tuesday" == data.mesDiaDaSemana) {
                    DiaDaSemana = "na " + data.mesDataColocacao + "&ordf;" + " terça";
                }
                else if ("Wednesday" == data.mesDiaDaSemana) {
                    DiaDaSemana = "na " + data.mesDataColocacao + "&ordf;" + " quarta";
                }
                else if ("Thursday" == data.mesDiaDaSemana) {
                    DiaDaSemana = "na " + data.mesDataColocacao + "&ordf;" + " quinta";
                }
                else if ("Friday" == data.mesDiaDaSemana) {
                    DiaDaSemana = "na " + data.mesDataColocacao + "&ordf;" + " sexta";
                }
                else if ("Saturday" == data.mesDiaDaSemana) {
                    DiaDaSemana = "no " + data.mesDataColocacao + "&ordm;" + " sabado";
                }

                var html = '';
                $('#Mes').html(html);
                html += '<div class="col-sm-6">\
                <label>Mensalmente</label>\
                <div class="form-group">\
                    <select class="form-control" id="SelectMensalmente">';
                html += '<option value=1>Mensalmente no dia ' + data.dia + '</option>';
                html += '<option value=2>Mensalmente ' + DiaDaSemana + '</option>';

                html += '</select>\
                </div>\
                </div >';
                $('#DiaDaSemanaOriginal').val(data.mesDiaDaSemana);
                $('#MesDataColocacao').val(data.mesDataColocacao);
                $('#MesDia').val(data.dia);

                $('#Mes').html(html);
            }
        },
        error: function () {
            swal("Erro!", "Erro ao excluir o registro, contate o Administrador do Sistema.", "error");
        }
    });
}

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
        defaultView: window.mobilecheck() ? 'timeGridWeek' : 'dayGridMonth',
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
            $('#formVisita').click(function () {
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
}

/*#########################################################################*/

function CarregarCalendarMkt(Calendar, calendarEl) {
    $('#calendar').html('');
    var calendar = new Calendar(calendarEl, {
        plugins: ['bootstrap', 'interaction', 'dayGrid', 'timeGrid'],
        defaultView: window.mobilecheck() ? 'timeGridWeek' : 'dayGridMonth',
        //timeZone: 'UTC',
        eventLimit: true, // allow "more" link when too many events
        views: {
            timelineFourDays: {
                type: 'timeline',
            }
        },
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

            $('#formVisita')
                .bootstrapValidator('disableSubmitButtons', false)  // Enable the submit buttons
                .bootstrapValidator('resetForm', true);             // Reset the form

            
            $('#Ativo').prop('checked', true);
            $('#Repetir').val('1');

            //startDate.start
            $('#StartStr').val(startDate.startStr);
            $('#EndStr').val(startDate.endStr);

            var StartStr = startDate.startStr.split('-');
            var Hora;
            var Minutos;

            /*if (AnoBissexto(StartStr[0])) {*/
            var DataInicio = new Date();
                Hora = DataInicio.getHours();
                Minutos = DataInicio.getMinutes();
                DataInicio = startDate.start;
                DataInicio.setHours(Hora);
                DataInicio.setMinutes(Minutos);
                DataInicio.setSeconds(00);

                DataInicio.setMinutes(DataInicio.getMinutes() - DataInicio.getTimezoneOffset());
                document.getElementById('DataInicio').value = DataInicio.toISOString().slice(0, 16);

            var DataFim = new Date();
                Hora = DataFim.getHours();
                Minutos = DataFim.getMinutes();
                DataFim = startDate.start;
                DataFim.setHours(Hora);
                DataFim.setMinutes(Minutos);
                DataFim.setSeconds(00);
                DataFim.setDate(DataInicio.getDate());

                DataFim.setMinutes(DataFim.getMinutes() - DataFim.getTimezoneOffset()); DataFim.setMinutes(DataFim.getMinutes() + 30);
            document.getElementById('DataFim').value = DataFim.toISOString().slice(0, 16);


            var DataTerminaEm = new Date();
            Hora = DataTerminaEm.getHours();
            Minutos = DataTerminaEm.getMinutes();
            DataTerminaEm = startDate.start;
            DataTerminaEm.setHours(Hora);
            DataTerminaEm.setMinutes(Minutos);
            DataTerminaEm.setSeconds(00);

            DataTerminaEm.setMinutes(DataTerminaEm.getMinutes() - DataTerminaEm.getTimezoneOffset());
            document.getElementById('DataTerminaEm').value = DataTerminaEm.toISOString().slice(0, 16);

            $('#DataTerminaEm').val(startDate.startStr);

            ObterDataColocacaoDiaDaSemana(startDate.startStr);

            $('#formVisita').bootstrapValidator('addField', 'Tipo[]', {
                validators: {
                    notEmpty: {
                        message: 'Tipo é um campo obrigatório.'
                    }
                }
            });

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
            //Ao recarregar a pagina
            $.ajax({
                type: "POST",
                url: '/Visita?handler=ObterVisitas',
                data: { Inicio: info.startStr, Fim: info.endStr},
                cache: false,
                async: false,
                beforeSend: function (xhr) {
                    xhr.setRequestHeader("XSRF-TOKEN",
                        $('input:hidden[name="__RequestVerificationToken"]').val());
                },
                success: function (data) {
                    var events = [];
                    $.map(data.listVisita, function (r) {
                        if (r.tipo == 1) {
                            //Feriado
                            events.push({
                                /*sourceId: r.sourceId,*/
                                title: r.title,
                                backgroundColor: r.backgroundColor,
                                borderColor: r.borderColor,
                                start: r.start,
                                //end: r.end,
                                allDay: r.allDay,
                                //overlap: false,
                                //rendering: 'background',
                                //color: '#ff9f89'
                            });

                            var s = r.start.split('T');
                            var e = r.end.split('T');
                            events.push({
                                title: r.title,
                                start: s[0],
                                end: e[0],
                                overlap: false,
                                rendering: 'background'
                            });
                        } else if (r.tipo == 2) {
                            //Data Comemorativa
                            events.push({
                                /*sourceId: r.sourceId,*/
                                title: r.title,
                                backgroundColor: r.backgroundColor,
                                borderColor: r.borderColor,
                                start: r.start,
                                //end: r.end,
                                allDay: r.allDay,
                                //overlap: false,
                                //rendering: 'background',
                                //color: '#ff9f89'
                            });

                            var s = r.start.split('T');
                            var e = r.end.split('T');
                            events.push({
                                title: r.title,
                                start: s[0],
                                end: e[0],
                                overlap: false,
                                rendering: 'background'
                            });
                        } else if (r.tipo == 3) {
                            //Evento
                            events.push({
                                sourceId: r.sourceId,
                                title: r.title,
                                backgroundColor: r.backgroundColor,
                                borderColor: r.borderColor,
                                start: r.start,
                                end: r.end,
                                allDay: r.allDay,
                                //overlap: false,
                                //rendering: 'background',
                                //color: '#ff9f89'
                            });
                        }
                    });
                    successCallback(events);
                }
            });;

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
                
                if (eventObj.extendedProps.sourceId != undefined && eventObj.extendedProps.sourceId != 0 && eventObj.extendedProps.sourceId != null)
                {
                    //Aciona no duplo Clique
                    $('#Descricao').focus();
                    $('#DataSazonal').attr("disabled", true);
                    $('.help-block').attr('style', 'display: none !important; color: red !important;');

                    $('#Feriado').prop('checked', false);
                    $('#DataComemorativa').prop('checked', false);
                    $('#Evento').prop('checked', false);

                    $('#Sim').prop('checked', false);
                    $('#Nao').prop('checked', false);

                    var form_data = new FormData();
                    form_data.append('IdVisita', eventObj.extendedProps.sourceId);
                    $.ajax({
                        type: 'POST',
                        url: "/Visita?handler=Obter",
                        data: form_data,
                        cache: false,
                        contentType: false,
                        processData: false,
                        beforeSend: function (xhr) {
                            xhr.setRequestHeader("XSRF-TOKEN",
                                $('input:hidden[name="__RequestVerificationToken"]').val());
                        },
                        success: function (data) {
                            var html = '';
                            //2021-05-09T05:24:00
                            if (data.status) {
                                //add Valores aos hidden
                                obj.GuidId = data.entityVisita.guidId;
                                obj.Cor = data.entityVisita.cor;
                                obj.DataFim = data.entityVisita.dataFim;
                                obj.DataInicio = data.entityVisita.dataInicio;
                                obj.Descricao = data.entityVisita.descricao;
                                obj.Frequencia = data.entityVisita.frequencia;
                                obj.GuidId = data.entityVisita.guidId;
                                obj.IdCalendarioSazonal = data.entityVisita.idCalendarioSazonal;
                                obj.IdStatusVisita = data.entityVisita.idStatusVisita;
                                obj.IdVisita = data.entityVisita.idVisita;
                                obj.Observacao = data.entityVisita.observacao;
                                obj.Repete = data.entityVisita.repete;
                                obj.Tipo = data.entityVisita.tipo;
                                obj.Visivel = data.entityVisita.visivel;
                                obj.Repetir = data.entityVisita.repetir;
                                obj.Termina = data.entityVisita.termina;
                                obj.DataTerminaEm = data.entityVisita.dataTerminaEm;
                                obj.Semana = data.entityVisita.semana;
                                obj.MesDataColocacao = data.entityVisita.mesDataColocacao;
                                obj.MesDiaDaSemana = data.entityVisita.mesDiaDaSemana;
                                obj.MesDia = data.entityVisita.mesDia;
                                obj.SelectMensalmente = data.entityVisita.selectMensalmente;

                                var StrData = '';

                                var VetDataInicio = data.entityVisita.dataInicio.split('T');
                                var VetDataFim = data.entityVisita.dataFim.split('T');

                                var DataHoje = new Date();
                                if (DataHoje.getMonth() < 9)
                                    var DataHojeFormatada = (DataHoje.getFullYear() + "-" + ("0" + (DataHoje.getMonth() + 1)) + "-" + (DataHoje.getDate()));
                                else
                                    var DataHojeFormatada = (DataHoje.getFullYear() + "-" + ((DataHoje.getMonth() + 1)) + "-" + (DataHoje.getDate()));

                                var DataInicio = new Date(data.entityVisita.dataInicio);
                                var DataFim = new Date(data.entityVisita.dataFim);
                                var DataMoment = moment(data.entityVisita.dataInicio, "MM-DD-YYYY");

                                if (VetDataInicio[0] == VetDataFim[0] && DataHojeFormatada == VetDataInicio[0] && DataHojeFormatada == VetDataFim[0]) {
                                    //Hoje
                                    StrData = 'Hoje, ' + VetDataInicio[1].trim().substring(0, 5) + ' - ' + VetDataFim[1].trim().substring(0, 5);
                                } else if (VetDataInicio[0] == VetDataFim[0] && data.entityVisita.dataInicio < data.entityVisita.dataFim) {
                                    StrData = DataMoment._locale._weekdays[DataInicio.getDay()] + ', ' + DataInicio.getDate() + ' de ' + DataMoment._locale._months[DataInicio.getMonth()] + ', ' + VetDataInicio[1].trim().substring(0, 5) + ' - ' + VetDataFim[1].trim().substring(0, 5);
                                } else if (VetDataInicio[0] < VetDataFim[0] && data.entityVisita.dataInicio < data.entityVisita.dataFim) {
                                    StrData = DataMoment._locale._weekdays[DataInicio.getDay()] + ', ' + DataInicio.getDate() + ' de ' + DataMoment._locale._months[DataInicio.getMonth()] + ', ' + VetDataInicio[1].trim().substring(0, 5) + ' - '
                                        + DataMoment._locale._weekdays[DataFim.getDay()] + ', ' + DataFim.getDate() + ' de ' + DataMoment._locale._months[DataFim.getMonth()] + ', ' + VetDataFim[1].trim().substring(0, 5);
                                }

                                html += '<div class="card text-center">\
                                        <div class="card-header">Titulo: '+ data.entityVisita.descricao.toUpperCase() + '<br> ' + StrData + '</div>\
                                        <div class="card-body">\
                                            <div class="form-group">\
                                                <label class="col-lg-12 control-label" > Selecione uma opção</label>\
                                                <div class="col-lg-12">\
                                                    <div class="checkbox">\
                                                        <label><input type="radio" class="Tipo" name="ExcluirAlterar[]" value="1" /> Alterar </label>\
                                                    </div>\
                                                    <div class="checkbox">\
                                                        <label><input type="radio" class="Tipo" name="ExcluirAlterar[]" value="2" /> Excluir </label>\
                                                    </div>\
                                                </div>\
                                            </div>\
                                            <div class="d-flex justify-content-center">\
                                                <div class="col-sm-6" id="BlocoExcluirAlterar" style="display: none">\
                                                    <label id="TituloExcluirAlterar"></label>\
                                                    <div class="form-group">\
                                                        <div class="form-check form-check-inline">\
                                                            <input class="form-check-input Tipo" type="radio" name="OpExcluirAlterar[]" id="EsteEvento" value="1">\
                                                                <label class="form-check-label" for="EsteEvento">Este Evento</label>\
                                                                    </div><br />';
                                if (data.existeMaisQueUm) {
                                    html += '<div class="form-check form-check-inline">\
                                                                <input class="form-check-input Tipo" type="radio" name="OpExcluirAlterar[]" id="EventosSeguintes" value="2">\
                                                                    <label class="form-check-label" for="EventosSeguintes">Este e os eventos Seguintes</label>\
                                                                    </div><br />\
                                                                <div class="form-check form-check-inline">\
                                                                    <input class="form-check-input Tipo" type="radio" name="OpExcluirAlterar[]" id="TodosEventos" data-tipo="3" data-radio="Tipo" value="3">\
                                                                        <label class="form-check-label" for="TodosEventos">Todos os Eventos</label>\
                                                                    </div><br />';
                                }
                                                                html += '</div>\
                                                            </div>\
                                                            <!--<div class="col-sm-6">\
                                                            </div>-->\
                                                        </div>\
                                                    </div>\
                                                    <!--<div class="card-footer text-muted">\
                                                        2 days ago\
                                                    </div>-->\
                                                </div>\
                                                <div class="modal-footer d-flex justify-content-around">\
                                                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Cancelar</button>\
                                                    <div id="BtnAlterar" style="display: none"><button type="button" class="btn btn-warning btn-alterar-excluir" data-btn="alterar" disabled>Alterar</button></div>\
                                                    <div id="BtnExcluir" style="display: none"><button type="button" class="btn btn-danger btn-alterar-excluir" data-btn="excluir" disabled>Excluir</button></div>\
                                                </div>';
                                $('#AlterarExcluirIdVisita').val(data.entityVisita.idVisita);
                                $('#GuidId').val(data.entityVisita.guidId);
                                $('#Visivel').val(data.entityVisita.visivel);
                                $('#IdVisita').val(data.entityVisita.idVisita);
                                $('#AlterarExcluir').modal('show');
                            }
                            $('#modalAlterarExcluir').html(html);
                        },
                        error: function () {
                            swal("Erro!", "Erro ao buscar o registro, contate o Administrador do Sistema.", "error");
                        }
                    });

                   
                }
            });
        },
        editable: false,
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
}

function CarregarCalendarPesquisa(Calendar, calendarEl, Event) {
    $('#calendar').html('');
    var calendar = new Calendar(calendarEl, {
        plugins: ['bootstrap', 'interaction', 'dayGrid', 'timeGrid'],
        defaultView: window.mobilecheck() ? 'timeGridWeek' : 'dayGridMonth',
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

function AdicionarValidacao($this) {
    var topic = $($this).val();

    //Tipo
    if ($($this).data('radio') == 'Tipo') {

        var $container = $('[data-tipo="' + topic + '"]');
        //$container.toggle();
        if ($($this).data('tipo') == 1 || $($this).data('tipo') == 2 || $($this).data('tipo') == 3) {
            $('[data-tipo="3"]').css('display', 'block');
        }

        var display = $container.css('display');
    }

    //Repete
    $('input[type="radio"][name="Tipo[]"]').css('display', 'block');
    if ($($this).data('radio') == 'Repete') {
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
            $('#Semana').css('display', 'none');
            $('input[type="radio"][name="Frequencia[]"]:checked').prop('checked', false);
            $('input[type="radio"][name="Termina[]"]:checked').prop('checked', false);
            $('#formVisita').bootstrapValidator('removeField', 'Frequencia[]');
            $('#formVisita').bootstrapValidator('removeField', 'Repetir');
            $('#formVisita').bootstrapValidator('removeField', 'Periodo');
            $('#formVisita').bootstrapValidator('removeField', 'Termina[]');
            $('#formVisita').bootstrapValidator('removeField', 'Semana[]');
        }
    }

    var tam = $('input[type="radio"][name="Frequencia[]"]:checked');
    if (tam.length > 0 && $('input[type="radio"][name="Repete[]"]:checked').val() == 7 && $('input[type="radio"][name="Frequencia[]"]:checked').val() == 1) {//Diariamente
        $('.BlocoRepetir').css('display', 'block');
        $('#Termina').css('display', 'block');

        $('#Semana').css('display', 'none');
        $('#Mes').css('display', 'none');
        $('#formVisita').bootstrapValidator('removeField', 'Semana[]');

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
    } else if (tam.length > 0 && $('input[type="radio"][name="Repete[]"]:checked').val() == 7 && $('input[type="radio"][name="Frequencia[]"]:checked').val() == 2) {//Semanalmente
        $('.BlocoRepetir').css('display', 'block');
        $('#Semana').css('display', 'block');
        $('#Termina').css('display', 'block');

        $('#Mes').css('display', 'none');
        $('#formVisita').bootstrapValidator('addField', 'Semana[]', {
            validators: {
                notEmpty: {
                    message: 'O Campo é um campo obrigatório.'
                }
            }
        });

        SelecionarDiaDaSemana();

        $('#formVisita').bootstrapValidator('addField', 'Termina[]', {
            validators: {
                notEmpty: {
                    message: 'O Campo é um campo obrigatório.'
                }
            }
        });
    } else if (tam.length > 0 && $('input[type="radio"][name="Repete[]"]:checked').val() == 7 && $('input[type="radio"][name="Frequencia[]"]:checked').val() == 3) {//Mensalmente

        $('.BlocoRepetir').css('display', 'block');
        $('#Mes').css('display', 'block');
        $('#Termina').css('display', 'block');

        $('#Semana').css('display', 'none');
        //SelecionarDiaDaSemana();
        GerarSelectOption();

        //Add a data selecionada mais um mes
        var VetStartStr = $('#StartStr').val().split('-');
        var Data = new Date();
        Data.setDate(VetStartStr[2]);
        Data.setMonth(VetStartStr[1]);
        Data.setFullYear(VetStartStr[0]);
        var Dia = ("0" + Data.getDate()).slice(-2);
        var Mes = ("0" + (Data.getMonth() + 1)).slice(-2);
        var DataHoje = Data.getFullYear() + "-" + (Mes) + "-" + (Dia);

        $('#DataTerminaEm').val(DataHoje)

        $('#formVisita').bootstrapValidator('addField', 'Mes[]', {
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
    } else if (tam.length > 0 && $('input[type="radio"][name="Repete[]"]:checked').val() == 7 && $('input[type="radio"][name="Frequencia[]"]:checked').val() == 4) {//Anualmente

        $('.BlocoRepetir').css('display', 'block');
        $('#Mes').css('display', 'block');
        $('#Termina').css('display', 'block');

        $('#Semana').css('display', 'none');
        $('#Mes').css('display', 'none');
        //SelecionarDiaDaSemana();
        GerarSelectOption();

        //Add a data selecionada mais um mes
        var VetStartStr = $('#StartStr').val().split('-');
        var Data = new Date();
        Data.setDate(VetStartStr[2]);
        Data.setMonth(VetStartStr[1]);
        Data.setFullYear(VetStartStr[0]);
        var Dia = ("0" + Data.getDate()).slice(-2);
        var Mes = ("0" + (Data.getMonth() + 1)).slice(-2);
        var DataHoje = Data.getFullYear() + "-" + (Mes) + "-" + (Dia);

        $('#DataTerminaEm').val(DataHoje)

        $('#formVisita').bootstrapValidator('addField', 'Mes[]', {
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
}

function AnoBissexto(year) {
    return new Date(year, 1, 29).getMonth() == 1
}

function DataMaior(data) {
    
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

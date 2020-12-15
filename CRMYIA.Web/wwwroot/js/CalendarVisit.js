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

    CarregarCalendar(Calendar, calendarEl);

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

$('.select2').on('change', function () {
    //var data = $(".select2 option:selected").text();
    //$("#test").val(data);
    VisitasPesquisa();
})

$(document).ready(function () {
    $(document).on('click', '.select2-results__option', function () {
        $("#select2-menuItems-container").val($(this).html());
    });
});


$(document).on('click', '.todos-perfil', function () {

    VisitasPesquisa();
});

function BuscarVisita() {
    let UrlParametro = window.location.search;
    var ParametroIdNotificacao = UrlParametro.split('IdNotificacao');
    var IdNotificacao = ParametroIdNotificacao[1].substring(1, ParametroIdNotificacao[1].length);
    var ParametroId = ParametroIdNotificacao[0].split('Id');
    var Id = ParametroId[1].substring(1, ParametroId[1].length - 1)
    var Calendar = FullCalendar.Calendar;
    var calendarEl = document.getElementById('calendar');
    $.post({
        url: '/Visita?handler=BuscarVisita',
        method: "Post",
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
        header: {
            left: 'prev,next today',
            center: 'title',
            right: 'dayGridMonth,timeGridWeek,timeGridDay'
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
                        events.push({
                            sourceId: r.sourceId,
                            title: r.title,
                            backgroundColor: r.backgroundColor,
                            borderColor: r.borderColor,
                            start: r.start,
                            allDay: r.allDay
                        });
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
$(document).ready(function () {

    $(".sortable").sortable({
        update: function (event, ui) {
            $.getJSON('/Tarefa?handler=ListarFaseProposta', function (data) {
                $.each(data, function (i, item) {
                   
                });
            });

            //$.get('/Tarefa?handler=ListarFaseProposta', function () {
                
            //});
            //$('.example span').each(function (i) {
            //    var numbering = i + 1;
            //    $(this).text(numbering);
            //});
        }
    });

});
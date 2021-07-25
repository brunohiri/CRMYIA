$(document).ready(function () {
    $("#html5colorpicker").change(function () {
    var amostra = $("#html5colorpicker").val();
    $("#amostra").css("color", amostra);
});
});

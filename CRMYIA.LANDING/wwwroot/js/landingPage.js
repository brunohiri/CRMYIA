//Seguro auto
$("#seguroAuto").click(function () {
    $("#seguroAuto").prop('checked', true);

    $("#seguroVida").prop('checked', false);
    $("#planoSaude").prop('checked', false);
    $("#seguroResidencial").prop('checked', false);
    $("#outrosSeguros").prop('checked', false);

    $("#rowPossuiCarroLabel").removeClass("d-none");
    $("#rowPossuiCarro").removeClass("d-none");
    $("#rowBuscaCarroLabel").removeClass("d-none");
    $("#rowBuscaCarro").removeClass("d-none");

    $("#rowProfLabel").addClass("d-none");
    $("#rowProf").addClass("d-none");
    $("#rowCnpjLabel").addClass("d-none");
    $("#rowTipo").addClass("d-none");
    $("#rowCnpj").addClass("d-none");
    $("#rowPlanoLabel").addClass("d-none");
    $("#rowPlano").addClass("d-none");
    
    $("#txtProfissao").val("");
    $("#txtVidas").val("");

    $("#divCnpjNao").addClass("selecionado");
    $("#switchNaoCnpj").prop('checked', true);
    $("#divCnpjSim").removeClass("selecionado");
    $("#switchSimCnpj").prop('checked', false);
    $("#rowResidencial").addClass("d-none");
    $("#divResidencialVoce").prop('checked', true);
    $("#divResidencialVoce").addClass("selecionado");
    $("#divResidencialEmpresa").prop('checked', false);
    $("#divResidencialEmpresa").removeClass("selecionado");
    $("#divPlanoNao").addClass("selecionado");
    $("#switchNaoPlano").prop('checked', true);
    $("#divPlanoSim").removeClass("selecionado");
    $("#switchSimPlano").prop('checked', false);
    
});
$("#divPossuiCarroSim").click(function () {
    $("#divPossuiCarroSim").addClass("selecionado");
    $("#switchSimCarro").prop('checked', true);

    $("#divPossuiCarroNao").removeClass("selecionado");
    $("#switchNaoCarro").prop('checked', false);
});
$("#divPossuiCarroNao").click(function () {
    $("#divPossuiCarroNao").addClass("selecionado");
    $("#switchNaoCarro").prop('checked', true);

    $("#divPossuiCarroSim").removeClass("selecionado");
    $("#switchSimCarro").prop('checked', false);
});
$("#divBuscaCarroSim").click(function () {
    $("#divBuscaCarroSim").addClass("selecionado");
    $("#switchSimBuscaCarro").prop('checked', true);

    $("#divBuscaCarroNao").removeClass("selecionado");
    $("#switchNaoBuscaCarro").prop('checked', false);
});
$("#divBuscaCarroNao").click(function () {
    $("#divBuscaCarroNao").addClass("selecionado");
    $("#switchNaoBuscaCarro").prop('checked', true);

    $("#divBuscaCarroSim").removeClass("selecionado");
    $("#switchSimBuscaCarro").prop('checked', false);
});
//Seguro auto

//Seguro vida
$("#seguroVida").click(function () {
    $("#seguroVida").prop('checked', true);

    $("#seguroAuto").prop('checked', false);
    $("#planoSaude").prop('checked', false);
    $("#seguroResidencial").prop('checked', false);
    $("#outrosSeguros").prop('checked', false);

    $("#rowPossuiCarroLabel").addClass("d-none");
    $("#rowPossuiCarro").addClass("d-none");
    $("#rowBuscaCarroLabel").addClass("d-none");
    $("#rowBuscaCarro").addClass("d-none");
    $("#rowCnpjLabel").addClass("d-none");
    $("#rowCnpj").addClass("d-none");
    $("#rowPlanoLabel").addClass("d-none");
    $("#rowPlano").addClass("d-none");
    $("#rowTipo").addClass("d-none");
    $("#rowResidencial").addClass("d-none");
    $("#divResidencialVoce").prop('checked', true);
    $("#divResidencialVoce").addClass("selecionado");
    $("#divResidencialEmpresa").prop('checked', false);
    $("#divResidencialEmpresa").removeClass("selecionado");
    $("#rowProfLabel").removeClass("d-none");
    $("#rowProf").removeClass("d-none");
    $("#txtProfissao").val("");
    $("#txtVidas").val("");

});
//Seguro vida

//Plano saude
$("#planoSaude").click(function () {
    $("#planoSaude").prop('checked', true);

    $("#seguroAuto").prop('checked', false);
    $("#seguroVida").prop('checked', false);
    $("#seguroResidencial").prop('checked', false);
    $("#outrosSeguros").prop('checked', false);
    $("#divTipo1").addClass("selecionado");
    $("#divTipo2").removeClass("selecionado");
    $("#divTipo3").removeClass("selecionado");
    $("#switchVoce").prop('checked', true);
    $("#switchFamilia").prop('checked', false);
    $("#switchEmpresa").prop('checked', false);

    $("#rowTipo").removeClass("d-none");
    $("#rowPlanoLabel").removeClass("d-none");
    $("#rowPlano").removeClass("d-none");


    $("#rowProfLabel").addClass("d-none");
    $("#rowProf").addClass("d-none");
    $("#rowPlanoLabel").addClass("d-none");
    $("#rowPlano").addClass("d-none");
    $("#rowPossuiCarroLabel").addClass("d-none");
    $("#rowPossuiCarro").addClass("d-none");
    $("#rowBuscaCarroLabel").addClass("d-none");
    $("#rowBuscaCarro").addClass("d-none");
    $("#rowCnpjLabel").addClass("d-none");
    $("#rowCnpj").addClass("d-none");
    $("#rowResidencial").addClass("d-none");
    $("#divResidencialVoce").prop('checked', true);
    $("#divResidencialVoce").addClass("selecionado");
    $("#divResidencialEmpresa").prop('checked', false);
    $("#divResidencialEmpresa").removeClass("selecionado");
    $("#txtProfissao").val("");
    $("#txtVidas").val("");
});
$("#switchVoce").click(function () {
    $("#divTipo1").addClass("selecionado");
    $("#switchVoce").prop('checked', true);

    $("#divTipo2").removeClass("selecionado");
    $("#switchFamilia").prop('checked', false);
    $("#divTipo3").removeClass("selecionado");
    $("#switchEmpresa").prop('checked', false);

    $("#rowCnpjLabel").addClass("d-none");
    $("#rowPlanoLabel").addClass("d-none");
    $("#rowCnpj").addClass("d-none");
    $("#rowPlano").addClass("d-none");
    $("#txtVidas").val("");
});
$("#switchFamilia").click(function () {
    $("#divTipo2").addClass("selecionado");
    $("switchFamilia").prop('checked', true);

    $("#divTipo1").removeClass("selecionado");
    $("#switchVoce").prop('checked', false);
    $("#divTipo3").removeClass("selecionado");
    $("#switchEmpresa").prop('checked', false);

    $("#rowCnpjLabel").removeClass("d-none");
    $("#rowPlanoLabel").removeClass("d-none");
    $("#rowCnpj").removeClass("d-none");
    $("#rowPlano").removeClass("d-none");
});
$("#switchEmpresa").click(function () {
    $("#divTipo3").addClass("selecionado");
    $("#switchEmpresa").prop('checked', true);

    $("#divTipo1").removeClass("selecionado");
    $("#switchVoce").prop('checked', false);
    $("#divTipo2").removeClass("selecionado");
    $("#switchFamilia").prop('checked', false);

    $("#rowPlanoLabel").removeClass("d-none");
    $("#rowPlano").removeClass("d-none");
    $("#rowCnpjLabel").addClass("d-none");
    $("#rowCnpj").addClass("d-none");
});
$("#divPlanoSim").click(function () {
    $("#divPlanoSim").addClass("selecionado");
    $("#switchSimPlano").prop('checked', true);

    $("#divPlanoNao").removeClass("selecionado");
    $("#switchNaoPlano").prop('checked', false);
});
$("#divPlanoNao").click(function () {
    $("#divPlanoNao").addClass("selecionado");
    $("#switchNaoPlano").prop('checked', true);

    $("#divPlanoSim").removeClass("selecionado");
    $("#switchSimPlano").prop('checked', false);
});
$("#divCnpjSim").click(function () {
    $("#divCnpjSim").addClass("selecionado");
    $("#switchSimCnpj").prop('checked', true);

    $("#divCnpjNao").removeClass("selecionado");
    $("#switchNaoCnpj").prop('checked', false);
});
$("#divCnpjNao").click(function () {
    $("#divCnpjNao").addClass("selecionado");
    $("#switchNaoCnpj").prop('checked', true);

    $("#divCnpjSim").removeClass("selecionado");
    $("#switchSimCnpj").prop('checked', false);
});
//Plano saude

//Seguro residencial
$("#seguroResidencial").click(function () {
    $("#seguroResidencial").prop('checked', true);

    $("#seguroAuto").prop('checked', false);
    $("#planoSaude").prop('checked', false);
    $("#seguroVida").prop('checked', false);
    $("#outrosSeguros").prop('checked', false);

    $("#divResidencialVoce").addClass("selecionado");
    $("#switchResidencialVoce").prop('checked', true);
    $("#divResidencialEmpresa").removeClass("selecionado");
    $("#switchResidencialEmpresa").prop('checked', false);

    $("#rowResidencial").removeClass("d-none");

    $("#rowPossuiCarroLabel").addClass("d-none");
    $("#rowPossuiCarro").addClass("d-none");
    $("#rowBuscaCarroLabel").addClass("d-none");
    $("#rowBuscaCarro").addClass("d-none");
    $("#rowCnpjLabel").addClass("d-none");
    $("#rowCnpj").addClass("d-none");
    $("#rowPlanoLabel").addClass("d-none");
    $("#rowPlano").addClass("d-none");
    $("#rowTipo").addClass("d-none");
    $("#rowProfLabel").addClass("d-none");
    $("#rowProf").addClass("d-none");
});
$("#switchResidencialVoce").click(function () {
    $("#divResidencialVoce").addClass("selecionado");
    $("#switchResidencialVoce").prop('checked', true);

    $("#divResidencialEmpresa").removeClass("selecionado");
    $("#switchResidencialEmpresa").prop('checked', false);
});
$("#switchResidencialEmpresa").click(function () {
    $("#divResidencialEmpresa").addClass("selecionado");
    $("#switchResidencialEmpresa").prop('checked', true);

    $("#divResidencialVoce").removeClass("selecionado");
    $("#switchResidencialVoce").prop('checked', false);
});
//Seguro residencial

//Outros
$("#outrosSeguros").click(function () {
    $("#outrosSeguros").prop('checked', true);

    $("#seguroAuto").prop('checked', false);
    $("#planoSaude").prop('checked', false);
    $("#seguroResidencial").prop('checked', false);
    $("#seguroVida").prop('checked', false);

    $("#rowPossuiCarroLabel").addClass("d-none");
    $("#rowPossuiCarro").addClass("d-none");
    $("#rowBuscaCarroLabel").addClass("d-none");
    $("#rowBuscaCarro").addClass("d-none");
    $("#rowCnpjLabel").addClass("d-none");
    $("#rowCnpj").addClass("d-none");
    $("#rowPlanoLabel").addClass("d-none");
    $("#rowPlano").addClass("d-none");
    $("#rowTipo").addClass("d-none");
    $("#rowProfLabel").addClass("d-none");
    $("#rowProf").addClass("d-none");
    $("#rowResidencial").addClass("d-none");
    $("#divResidencialVoce").prop('checked', true);
    $("#divResidencialVoce").addClass("selecionado");
    $("#divResidencialEmpresa").prop('checked', false);
    $("#divResidencialEmpresa").removeClass("selecionado");
});
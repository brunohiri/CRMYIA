$(document).ready(function () {
    $('#txtTelefone').mask('(00) 0 0000-0000');
});

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
    $("#CTRLswitchNaoCnpj").prop('checked', true);
    $("#divCnpjSim").removeClass("selecionado");
    $("#CTRLswitchSimCnpj").prop('checked', false);


    $("#rowResidencial").addClass("d-none");
    $("#divResidencialVoce").prop('checked', true);
    $("#divResidencialVoce").addClass("selecionado");
    $("#divResidencialEmpresa").prop('checked', false);
    $("#divResidencialEmpresa").removeClass("selecionado");


    $("#divPlanoNao").addClass("selecionado");
    $("#CTRLswitchNaoPlano").prop('checked', true);
    $("#divPlanoSim").removeClass("selecionado");
    $("#CTRLswitchSimPlano").prop('checked', false);
    
});
$("#divPossuiCarroSim").click(function () {
    $("#divPossuiCarroSim").addClass("selecionado");
    $("#CTRLswitchSimCarro").prop('checked', true);

    $("#divPossuiCarroNao").removeClass("selecionado");
    $("#CTRLswitchNaoCarro").prop('checked', false);

    $("#switchCarro").prop('checked', true);
});
$("#divPossuiCarroNao").click(function () {
    $("#divPossuiCarroNao").addClass("selecionado");
    $("#CTRLswitchNaoCarro").prop('checked', true);

    $("#divPossuiCarroSim").removeClass("selecionado");
    $("#CTRLswitchSimCarro").prop('checked', false);

    $("#switchCarro").prop('checked', false);
});
$("#divBuscaCarroSim").click(function () {
    $("#divBuscaCarroSim").addClass("selecionado");
    $("#CTRLswitchSimBuscaCarro").prop('checked', true);

    $("#divBuscaCarroNao").removeClass("selecionado");
    $("#CTRLswitchNaoBuscaCarro").prop('checked', false);

    $("#switchBuscaCarro").prop('checked', true);
});
$("#divBuscaCarroNao").click(function () {
    $("#divBuscaCarroNao").addClass("selecionado");
    $("#CTRLswitchNaoBuscaCarro").prop('checked', true);

    $("#divBuscaCarroSim").removeClass("selecionado");
    $("#CTRLswitchSimBuscaCarro").prop('checked', false);

    $("#switchBuscaCarro").prop('checked', false);
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
    $("#CTRLswitchVoce").prop('checked', true);
    $("#CTRLswitchFamilia").prop('checked', false);
    $("#CTRLswitchEmpresa").prop('checked', false);

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
$("#CTRLswitchVoce").click(function () {
    $("#divTipo1").addClass("selecionado");
    $("#CTRLswitchVoce").prop('checked', true);
    $("#switchVoce").prop('checked', true);

    $("#divTipo2").removeClass("selecionado");
    $("#CTRLswitchFamilia").prop('checked', false);
    $("#switchFamilia").prop('checked', false);

    $("#divTipo3").removeClass("selecionado");
    $("#CTRLswitchEmpresa").prop('checked', false);
    $("#switchEmpresa").prop('checked', false);

    $("#rowCnpjLabel").addClass("d-none");
    $("#rowPlanoLabel").addClass("d-none");
    $("#rowCnpj").addClass("d-none");
    $("#rowPlano").addClass("d-none");
    $("#txtVidas").val("");
});
$("#CTRLswitchFamilia").click(function () {
    $("#divTipo2").addClass("selecionado");
    $("CTRLswitchFamilia").prop('checked', true);
    $("switchFamilia").prop('checked', true);

    $("#divTipo1").removeClass("selecionado");
    $("#CTRLswitchVoce").prop('checked', false);
    $("#switchVoce").prop('checked', false);

    $("#divTipo3").removeClass("selecionado");
    $("#CTRLswitchEmpresa").prop('checked', false);
    $("#switchEmpresa").prop('checked', false);

    $("#rowCnpjLabel").removeClass("d-none");
    $("#rowPlanoLabel").removeClass("d-none");
    $("#rowCnpj").removeClass("d-none");
    $("#rowPlano").removeClass("d-none");
});
$("#CTRLswitchEmpresa").click(function () {
    $("#divTipo3").addClass("selecionado");
    $("#CTRLswitchEmpresa").prop('checked', true);
    $("#switchEmpresa").prop('checked', true);

    $("#divTipo1").removeClass("selecionado");
    $("#CTRLswitchVoce").prop('checked', false);
    $("#switchVoce").prop('checked', false);

    $("#divTipo2").removeClass("selecionado");
    $("#CTRLswitchFamilia").prop('checked', false);
    $("#switchFamilia").prop('checked', false);

    $("#rowPlanoLabel").removeClass("d-none");
    $("#rowPlano").removeClass("d-none");
    $("#rowCnpjLabel").addClass("d-none");
    $("#rowCnpj").addClass("d-none");
});
$("#divPlanoSim").click(function () {
    $("#divPlanoSim").addClass("selecionado");
    $("#CTRLswitchSimPlano").prop('checked', true);

    $("#divPlanoNao").removeClass("selecionado");
    $("#CTRLswitchNaoPlano").prop('checked', false);

    $("#switchPlano").prop('checked', true);
});
$("#divPlanoNao").click(function () {
    $("#divPlanoNao").addClass("selecionado");
    $("#CTRLswitchNaoPlano").prop('checked', true);

    $("#divPlanoSim").removeClass("selecionado");
    $("#CTRLswitchSimPlano").prop('checked', false);

    $("#switchPlano").prop('checked', false);
});
$("#divCnpjSim").click(function () {
    $("#divCnpjSim").addClass("selecionado");
    $("#CTRLswitchSimCnpj").prop('checked', true);

    $("#divCnpjNao").removeClass("selecionado");
    $("#CTRLswitchNaoCnpj").prop('checked', false);

    $("#switchCnpj").prop('checked', true);
});
$("#divCnpjNao").click(function () {
    $("#divCnpjNao").addClass("selecionado");
    $("#CTRLswitchNaoCnpj").prop('checked', true);

    $("#divCnpjSim").removeClass("selecionado");
    $("#CTRLswitchSimCnpj").prop('checked', false);

    $("#switchCnpj").prop('checked', false);
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
    $("#CTRLswitchResidencialVoce").prop('checked', true);
    $("#divResidencialEmpresa").removeClass("selecionado");
    $("#CTRLswitchResidencialEmpresa").prop('checked', false);

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
$("#CTRLswitchResidencialVoce").click(function () {
    $("#divResidencialVoce").addClass("selecionado");
    $("#CTRLswitchResidencialVoce").prop('checked', true);

    $("#divResidencialEmpresa").removeClass("selecionado");
    $("#CTRLswitchResidencialEmpresa").prop('checked', false);

    $("#switchVoce").prop('checked', true);
    $("#switchEmpresa").prop('checked', false);
});
$("#CTRLswitchResidencialEmpresa").click(function () {
    $("#divResidencialEmpresa").addClass("selecionado");
    $("#CTRLswitchResidencialEmpresa").prop('checked', true);

    $("#divResidencialVoce").removeClass("selecionado");
    $("#CTRLswitchResidencialVoce").prop('checked', false);

    $("#switchEmpresa").prop('checked', true);
    $("#switchVoce").prop('checked', false);
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
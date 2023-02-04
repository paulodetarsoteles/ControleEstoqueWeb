﻿function set_dados_form(dados) {
    $('#id_cadastro').val(dados.Id);
    $('#txt_nome').val(dados.Nome);
    $('#txt_login').val(dados.Login);
    $('#txt_senha').val(dados.Senha);
    $('#ddl_perfil').val(dados.PerfilId); 
}

function set_focus_form() {
    $('#txt_nome').focus();
}

function set_dados_grid(dados) {
    return '<td>' + dados.Nome + '</td>' +
        '<td>' + dados.Login + '</td>';
}

function get_dados_inclusao() {
    return {
            Id: 0,
            Nome: '',
            Login: '',
            Senha: '', 
            PerfilId: 0
    };
}

function get_dados_form() {
    return {
        Id: $('#id_cadastro').val(),
        Nome: $('#txt_nome').val(),
        Login: $('#txt_login').val(),
        Senha: $('#txt_senha').val(), 
        PerfilId: $('#ddl_perfil').val()
    }; 
}

function preencher_linha_grid(param, linha) {
    linha
        .eq(0).html(param.Nome).end()
        .eq(1).html(param.Login);
}

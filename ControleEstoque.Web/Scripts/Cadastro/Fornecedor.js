
function set_dados_form(dados) {
    $('#id_cadastro').val(dados.Id);
    $('#txt_nome').val(dados.Nome);
    $('#txt_num_documento').val(dados.NumDocumento);
    $('#txt_razao_social').val(dados.RazaoSocial);
    $('#txt_telefone').val(dados.Telefone);
    $('#txt_contato').val(dados.Contato);
    $('#txt_endereco').val(dados.Endereco); 
    $('#cbx_ativo').prop('checked', dados.Ativo);
    $('#cbx_pessoa_juridica').prop('checked', false);
    $('#cbx_pessoa_fisica').prop('checked', false);

    if (dados.TipoPessoa == 2) {
        $('#cbx_pessoa_juridica').prop('checked', true).trigger('click');
    }
    else {
        $('#cbx_pessoa_fisica').prop('checked', true).trigger('click');
    }
}

function set_focus_form() {
    $('#txt_nome').focus();
}

function get_dados_inclusao() {
    return {
        Id: 0,
        Nome: '',
        NumDocumento: '',
        RazaoSocial: '',
        TipoPessoa: 2,
        Telefone: '',
        Contato: '',
        Endereco: '', 
        Ativo: true
    };
}

function get_dados_form() {
    return {
        Id: $('#id_cadastro').val(),
        Nome: $('#txt_nome').val(),
        NumDocumento: $('#txt_num_documento').val(),
        RazaoSocial: $('#txt_razao_social').val(),
        TipoPessoa: $('#cbx_pessoa_juridica').is(':checked') ? 2 : 1,
        Telefone: $('#txt_telefone').val(),
        Contato: $('#txt_contato').val(),
        Endereco: $('#txt_endereco').val(),
        Ativo: $('#cbx_ativo').prop('checked')
    };
}

function preencher_linha_grid(param, linha) {
    linha
        .eq(0).html(param.Nome).end()
        .eq(1).html(param.Telefone).end()
        .eq(2).html(param.Contato).end()
        .eq(3).html(param.Ativo ? 'SIM' : 'NÃO');
}

$(document)
    .ready(function () {
        $('#txt_telefone').mask('(00)00000-0000');
    })
    .on('click', '#cbx_pessoa_juridica', function () {
        $('label[for="txt_num_documento"]').text('CNPJ');
        $('#txt_num_documento').mask('00.000.000/0000-00', { reverse: true });
        $('#container_razao_social').removeClass('invisible');
    })
    .on('click', '#cbx_pessoa_fisica', function () {
        $('label[for="txt_num_documento"]').text('CPF');
        $('#txt_num_documento').mask('000.000.000-00', { reverse: true });
        $('#container_razao_social').addClass('invisible');
    }); 
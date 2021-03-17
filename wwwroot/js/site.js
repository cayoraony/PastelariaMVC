// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready( function () {
    $('#HomeGestor, #ListarTarefas, #VerTodas').DataTable( {
        "language": {
            "url": "//cdn.datatables.net/plug-ins/1.10.22/i18n/Portuguese-Brasil.json"
        }
    } );
});

// Consulta a API ViaCEP
$("#cep").blur(function () {
    var cep = this.value.replace(/[^0-9]/, "");

    if (cep.length != 8) {
        return false;
    }
    var url = "https://viacep.com.br/ws/" + cep + "/json/";
    $.getJSON(url, function (dadosRetorno) {
        try {
            $("#endereco").val(dadosRetorno.logradouro);
            $("#bairro").val(dadosRetorno.bairro);
            $("#cidade").val(dadosRetorno.localidade);
            $("#uf").val(dadosRetorno.uf);
        } catch (ex) { }
    });
});

// Mascaras
$(document).ready(function () {
    $('.inputCep').mask('00000-000');
    // $('.inputDDD').mask('00');
});

function unMask() {
    $('.inputCep').unmask();
    // $('.inputTel').unmask();
    // $('.inputDDD').unmask();
}


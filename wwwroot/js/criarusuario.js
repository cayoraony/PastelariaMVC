function criarUsuario() {
  inputMask();
  getApiCep();
  // validarSenha();
  validarCampos();
}

function getApiCep() {
      /*
      * Para efeito de demonstração, o JavaScript foi
      * incorporado no arquivo HTML.
      * O ideal é que você faça em um arquivo ".js" separado. Para mais informações
      * visite o endereço https://developer.yahoo.com/performance/rules.html#external
      */

      // Registra o evento blur do campo "cep", ou seja, a pesquisa será feita
      // quando o usuário sair do campo "cep"
      $("#cep").blur(function () {
        // Remove tudo o que não é número para fazer a pesquisa
        var cep = this.value.replace(/[^0-9]/, "");

        // Validação do CEP; caso o CEP não possua 8 números, então cancela
        // a consulta
        if (cep.length != 8) {
            return false;
        }

        // A url de pesquisa consiste no endereço do webservice + o cep que
        // o usuário informou + o tipo de retorno desejado (entre "json",
        // "jsonp", "xml", "piped" ou "querty")
        var url = "https://viacep.com.br/ws/" + cep + "/json/";

        // Faz a pesquisa do CEP, tratando o retorno com try/catch para que
        // caso ocorra algum erro (o cep pode não existir, por exemplo) a
        // usabilidade não seja afetada, assim o usuário pode continuar//
        // preenchendo os campos normalmente
        $.getJSON(url, function (dadosRetorno) {
            try {
                // Preenche os campos de acordo com o retorno da pesquisa
                $("#endereco").val(dadosRetorno.logradouro);
                $("#bairro").val(dadosRetorno.bairro);
                $("#cidade").val(dadosRetorno.localidade);
                $("#uf").val(dadosRetorno.uf);
            } catch (ex) { }
        });
    });
}

function inputMask() {
  $(document).ready(function(){
    $('.inputCep').mask('00000-000');
    //$('.inputTel').mask('00000-0000');
    $('.inputDDD').mask('00');
  });
}

function validarSenha() {
  let NovaSenha = document.getElementById('Senha').value;
  let CNovaSenha = document.getElementById('CSenha').value;

  if (NovaSenha == CNovaSenha) {
      $('.inputCep').unmask();
      //$('.inputTel').unmask();
      $('.inputDDD').unmask();

      document.getElementById('criarUsuario').submit();
      alert("Formulário enviado");
  } else {
      alert("Senha diferentes, preencha novamente");
  }
}

function validarCampos() {
  $("#criarUsuario").validate({
    rules: {
        nome: {
            required: true,
        },
        sobrenome: {
            required: true,
        },
        datanasc: {
            required: true,
        },
        senha: {
            required: true,
        },
        confirmaSenha: {
            required: true,
        },
        egestor: {
            required: true,
        },
        email: {
            required: true,
        },
        ddd: {
            required: true,
        },
        telefone: {
            required: true,
        },
        tipoTelefone: {
            required: true,
        },
        cep: {
            required: true,
        },
        rua: {
            required: true,
        },
        bairro: {
            required: true,
        },
        numero: {
            required: true,
        },
        complemento: {
            required: true,
        },
        cidade: {
            required: true,
        },
        uf: {
            required: true,
        }
    }
  }).validator.addMethod("cRequired", $.validator.methods.required,
    "Campo obrigatório. Preencha o campo.");

}
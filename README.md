# Pastelaria SMN
Para que o sistema rode por completo, é necessário instalar os softwares:
- Visual Studio 2019 ou Visual Studio Code
- Microsoft SQL Server Management Studio
- .Net Core 3.1 (API - BackEnd)
- .Net Core 5.0 (MVC - FrontEnd)

## Configuração para Desenvolvimento

### Back-End: API e Banco de Dados

O repositório da API encontra-se [neste link](https://github.com/joaomarcosSMN/API-Pastelaria-dotNet-v1/tree/develop) .

Para fazer o clone do repositório na branch develop, deve-se rodar no git bash:
```
git clone -b develop https://github.com/joaomarcosSMN/API-Pastelaria-dotNet-v1.git
```

Após fazer o clone do repositório (branch develop), na pasta "BancoDados" encontra-se imagens da modelagem do banco e os scripts necessários para rodar o Banco de Dados. Para rodar os Scripts:
- CriarTabelas e CriarDadosIniciais devem ser, respectivamente, executados primeiramente;
- Na pasta Procedures, todos os arquivos devem ser executados para que as "procs" sejam criadas.

Na pasta "PastelariaSMN" encontra-se o sistema da API. No arquivo "appsettings.Development.json" deve-se alterar a propriedade DefaultConnection em ConnectionStrings, configurando de acordo com os dados de conexão do Microsoft SQL Server Management Studio da máquina:
```
"DefaultConnection": "Server=< Nome do servidor >;Database=PastelariaSMN;User Id=< Nome do seu usuário >;Password=< Senha do usuário >;"
```

Ao abrir a pasta raiz do sistema (em que se encontra o arquivo PastelariaSMN.csproj) no terminal, deve-se executar os comandos:
- ```dotnet restore```
- ```dotnet run```

Por padrão, a API está rodando em: http://localhost:5000 .
Caso deseje alterar estas configurações, deve-se acessar "~/Properties\launchSettings.json" .

### Front-End: MVC

O repositório do MVC encontra-se [neste link](https://github.com/cayoraony/PastelariaMVC) .

Para fazer o clone do repositório na branch master, deve-se rodar no git bash:
```
git clone https://github.com/cayoraony/PastelariaMVC.git
```

Ao abrir a pasta raiz do sistema (em que se encontra o arquivo PastelariaSMN.csproj) no terminal, deve-se executar os comandos:
- ```dotnet restore```
- ```dotnet run```

Para realizar o Login no sistema:
- Acessar o link: http://localhost:5050/Usuario/Login
- Usar o endereço de email: admin_pastelaria@gmail.com
- Usar a senha: admin_sistema

Por padrão, o MVC consome no link da API mencionado anteriormente. Caso deseje alterar estas configurações, deve-se acessar "~/Properties\launchSettings.json" .


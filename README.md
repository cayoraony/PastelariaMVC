# Pastelaria SMN
Para que o sistema rode por comepleto, é necessário instalar os softwares:
- Visual Studio 2019 ou Visual Studio Code
- Microsoft SQL Server Management Studio
- .Net Core 3.1 (API - BackEnd)
- .Net Core 5.0 (MVC - FrontEnd)

## Configuração para Desenvolvimento

### API e Banco de Dados

O repositório da API encontra-se [neste link](https://github.com/joaomarcosSMN/API-Pastelaria-dotNet-v1/tree/develop) .

Após fazer o clone do repositório (branch develop), na pasta "BancoDados" encontra-se imagens da modelagem do banco e os scripts necessários para rodar o Banco de Dados:
- CriarTabelas e CriarDadosIniciais devem ser, respectivamente, executados primeiramente;
- Na pasta Procedures, todos os arquivos devem ser executados para que as "procs" sejam criadas.

Na pasta "PastelariaSMN" encontra-se o sistema da API

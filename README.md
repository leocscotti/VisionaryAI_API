# VisionaryAI-API
Projeto de api para o challenge da plusoft

## Integrantes

Leonardo Cordeiro Scotti- RM550769

Gabriel de Andrade Baltazar- RM550870

Enzo Ross Gallone- RM551754

Vinicius de Araujo Camargo- RM99494

Pedro Gomes Fernandes- RM551480

## Escolha da arquitetura da api

A abordagem escolhida foi de uma api monolítica, pois armazena em um único projeto: todos os elementos da aplicação (controladores, modelos, repositórios e dados) estão organizados dentro de uma mesma solução.
E também pela escalabilidade, porque o crescimento e a adição de novas funcionalidades continuam sendo gerenciados dentro de um único projeto, ao invés de dividir as responsabilidades em microservices.

## Diferenças entre as abordagens

A arquitetura monolítica é uma aplicação unificada, onde todas as funcionalidades estão integradas e executadas como um único bloco. Ela é mais simples de desenvolver e gerenciar inicialmente.

A arquitetura de microservices, por outro lado, divide a aplicação em vários serviços independentes, cada um responsável por uma função específica. Isso permite escalabilidade e manutenção mais fáceis, mas aumenta a complexidade de desenvolvimento e gerenciamento devido à comunicação entre os serviços.

## Arquitetura do projeto

Controllers: Gerenciam as requisições HTTP e roteiam para as camadas de serviço.
Services: Implementam a lógica de negócio da aplicação, separando-a das demais camadas.
Database: Contém o contexto de dados (VisionaryAIDBContext), gerenciando o acesso ao banco.
Models: Definem as entidades do sistema, como Cidade e Empresa.
Mappings: Usa o padrão Mapper para mapear entre objetos do domínio e dtos

## Design Patterns utilizados 

Repository (implícito na camada de database), que abstrai a lógica de acesso a dados.
Dependency Injection (na configuração de serviços), para facilitar a troca de implementações.
DTO (Data Transfer Object) para transferir dados entre as camadas, representado nos mapeamentos.

## Como rodar a API

Pode-se clonar o repositório ou abri-lo por pasta na IDE Visual Studio, clicar no botão "https". Após isso o projeto irá compilar e abrir a url no navegador.

No navegador aparecerá os endpoints para realizar o CRUD (Adicionar, Buscar, Atualizar, Deletar) dos dados.

Cada entidade tem seus endpoints listados com o verbo HTTP correspondente, então para realizar uma requisição é necessário escolher qual entidade e qual endpoint deseja, apertar o botão "try it out" e passar os parâmetros solicitados na aba.

Depois de passar o paramêtro, clicar no botão "Execute" e a requisição será efetuada. Logo após o processamento, irá retornar um http response (200= Ok, 500= InternalServerError), e assim saber a resposta desta requisição.



## Documentação da API e listagem dos endpoints 

#### Buscar todas as empresas

```http
  GET /api/Empresas/BuscarTodasEmpresas
```

#### Buscar empresa por id 

```http
  GET /api/Empresas/BuscarPorId/{id}
```

| Parâmetro | Tipo   | Descrição                                    |
|:----------|:-------|:---------------------------------------------|
| `id`      | `Integer` | **Obrigatório**. Id da empresa a ser buscada |

#### Cadastrar empresa 

```http
  POST /api/Empresas/Cadastrar
```

Body:

```json
{
    "cnpj": "12345678900123",
    "nome": "Plusoft",
    "email": "plusoft@gmail.com",
    "descricao": "human experience"
}
```

| Parâmetro | Tipo     | Descrição                       |
|:----------|:---------|:--------------------------------|
| `cnpj`    | `String` | Cnpj da empresa a ser gravada   |
| `nome`    | `String` | Nome da empresa a ser gravada   |
| `email`    | `String` | Email da empresa a ser gravada   |
| `descricao`    | `String` | Descrição da empresa a ser gravada   |


#### Atualizar empresa 

```http
  PUT /api/Empresas/Atualizar/{id}
```

| Parâmetro | Tipo   | Descrição                                       |
|:----------|:-------|:------------------------------------------------|
| `id`      | `Integer` | **Obrigatório**. Id da empresa a ser atualizada |

Body:

```json
{
    "cnpj": "12345678900456",
    "nome": "Plusoft",
    "email": "plusoft@gmail.com",
    "descricao": "human experience"
}
```

| Parâmetro | Tipo     | Descrição                       |
|:----------|:---------|:--------------------------------|
| `cnpj`    | `String` | Cnpj da empresa a ser gravada   |
| `nome`    | `String` | Nome da empresa a ser gravada   |
| `email`    | `String` | Email da empresa a ser gravada   |
| `descricao`    | `String` | Descrição da empresa a ser gravada   |

#### Excluir empresa

```http
  DELETE /api/Empresas/Excluir/{id}
```

| Parâmetro | Tipo   | Descrição                                     |
|:----------|:-------|:----------------------------------------------|
| `id`      | `Integer` | **Obrigatório**. Id da empresa a ser excluída |



#### Buscar todas as cidades

```http
  GET /api/Cidades/BuscarTodasCidades
```

#### Buscar cidade por id 

```http
  GET /api/Cidades/BuscarPorId/{id}
```

| Parâmetro | Tipo   | Descrição                                    |
|:----------|:-------|:---------------------------------------------|
| `id`      | `Integer` | **Obrigatório**. Id da cidade a ser buscada |

#### Cadastrar cidade 

```http
  POST /api/Cidades/Cadastrar
```

Body:

```json
{
    "nome": "Campinas",
    "uf": "SP"
}
```

| Parâmetro | Tipo     | Descrição                       |
|:----------|:---------|:--------------------------------|
| `nome`    | `String` | Nome da cidade a ser gravada   |
| `uf`    | `String` | UF (unidade da federação) da cidade a ser gravada   |


#### Atualizar cidade 

```http
  PUT /api/Cidades/Atualizar/{id}
```

| Parâmetro | Tipo   | Descrição                                       |
|:----------|:-------|:------------------------------------------------|
| `id`      | `Integer` | **Obrigatório**. Id da cidade a ser atualizada |

Body:

```json
{
    "nome": "Sorocaba",
    "uf": "SP"
}
```

| Parâmetro | Tipo     | Descrição                       |
|:----------|:---------|:--------------------------------|
| `nome`    | `String` | Nome da cidade a ser gravada   |
| `uf`    | `String` | UF (unidade da federação) da cidade a ser gravada   |

#### Excluir cidade

```http
  DELETE /api/Cidades/Excluir/{id}
```

| Parâmetro | Tipo   | Descrição                                     |
|:----------|:-------|:----------------------------------------------|
| `id`      | `Integer` | **Obrigatório**. Id da cidade a ser excluída |



#### Buscar todas as fontes de dados

```http
  GET /api/FontesDeDados/BuscarTodasFontesDados
```

#### Buscar fonte de dado por id 

```http
  GET /api/FontesDeDados/BuscarPorId/{id}
```

| Parâmetro | Tipo   | Descrição                                    |
|:----------|:-------|:---------------------------------------------|
| `id`      | `Integer` | **Obrigatório**. Id da fonte de dados a ser buscada |

#### Cadastrar fonte de dados 

```http
  POST /api/FontesDeDados/Cadastrar
```

Body:

```json
{
    "nome": "Relatório de vendas",
    "tipo": "insights financeiros"
}
```

| Parâmetro | Tipo     | Descrição                       |
|:----------|:---------|:--------------------------------|
| `nome`    | `String` | Nome da fonte de dados a ser gravada   |
| `tipo`    | `String` | tipo da fonte de dados a ser gravada   |


#### Atualizar fonte de dados 

```http
  PUT /api/FontesDeDados/Atualizar/{id}
```

| Parâmetro | Tipo   | Descrição                                       |
|:----------|:-------|:------------------------------------------------|
| `id`      | `Integer` | **Obrigatório**. Id da fonte de dados a ser atualizada |

Body:

```json
{
    "nome": "Relatório de vendas",
    "tipo": "dados financeiros"
}
```

| Parâmetro | Tipo     | Descrição                       |
|:----------|:---------|:--------------------------------|
| `nome`    | `String` | Nome da fonte de dados a ser gravada   |
| `tipo`    | `String` | Tipo da fonte de dados a ser gravada   |

#### Excluir fonte de dados

```http
  DELETE /api/FonteDeDados/Excluir/{id}
```

| Parâmetro | Tipo   | Descrição                                     |
|:----------|:-------|:----------------------------------------------|
| `id`      | `Integer` | **Obrigatório**. Id da fonte de dados a ser excluída |

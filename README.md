# API Voteio

## Descrição

A API **Voteio** permite a criação de usuários, autenticação, e a gestão de ideias e votos em um sistema de colaboração de ideias. Usuários podem registrar suas ideias, comentar, votar em propostas e interagir com outros participantes.

## Funcionalidades

- **Criação de Usuários**: Permite o cadastro de novos usuários no sistema com nome, e-mail e senha.
- **Autenticação**: Realiza login com credenciais de e-mail e senha para autenticação e acesso à plataforma.
- **Gestão de Ideias**: Possibilita a criação, listagem, e votação de ideias submetidas pelos usuários.
- **Comentário em Ideias**: Permite que os usuários comentem em ideias específicas.
- **Votação em Ideias**: Oferece a funcionalidade de voto positivo ou negativo em ideias, utilizando um sistema de votação simples.
- **Remoção de Votos**: Permite que os usuários removam seus votos de ideias.

## Estrutura Geral

A API utiliza o protocolo HTTP para realizar as requisições e segue o padrão REST. Todas as interações com os recursos, como criação de usuários, envio de ideias e comentários, são feitas via métodos HTTP.

### Métodos Suportados

- **POST**: Para criar novos registros, como usuários, ideias, comentários e votos.
- **GET**: Para obter informações e listagens de ideias e votos.
- **DELETE**: Para remover votos de ideias.

### Autenticação

Alguns endpoints requerem autenticação via **Bearer Token**. O token é gerado durante o processo de login e deve ser incluído no cabeçalho das requisições que exigem autenticação.

## Alguns Exemplos de Requisições

### Criação de Usuário

```json
POST /Usuario
{
  "nome": "string",
  "email": "user@example.com",
  "senha": "string"
}
```

### Login

```json
POST /Usuario/Login
{
  "email": "string",
  "password": "string",
  "twoFactorCode": "string",
  "twoFactorRecoveryCode": "string"
}
```

### Obter Ideias Votadas (Autenticação necessária)

```json
GET /Usuario/ObterIdeiasVotadas
{
  "ideias": [
    {
      "codigo": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "titulo": "string",
      "descricao": "string",
      "upvotes": 0,
      "downvotes": 0,
      "nomeUsuario": "string",
      "comentarios": [
        {
          "codigoUsuario": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
          "nomeUsuario": "string",
          "email": "string",
          "comentario": "string"
        }
      ]
    }
  ]
}
```

### Criação de Ideia (Autenticação necessária)

```json
POST /Ideias
{
  "titulo": "string",
  "descricao": "string"
}
```

### Listagem de Ideias (Autenticação necessária)

```json
GET /Ideias
{
  "ideias": [
    {
      "codigo": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "titulo": "string",
      "descricao": "string",
      "upvotes": 0,
      "downvotes": 0,
      "nomeUsuario": "string",
      "comentarios": [
        {
          "codigoUsuario": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
          "nomeUsuario": "string",
          "email": "string",
          "comentario": "string"
        }
      ]
    }
  ]
}
```

### Comentário em Ideia (Autenticação necessária)

```json
POST /Ideias/Comentario
{
  "codigoIdeia": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "texto": "string"
}
```

### Votação em Ideia (Autenticação necessária)

```json
POST /Ideias/Vote
{
  "codigoIdeia": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "tipoVote": 1
}
```

### Remoção de Voto (Autenticação necessária)

```json
DELETE /Ideias
{
  "codigoIdeia": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
}
```

### Considerações Finais

Esta API foi desenvolvida para ser performática e simples de usar. Requer autenticação em endpoints específicos e permite a interação direta entre usuários no ambiente de colaboração de ideias. Para mais informações detalhadas sobre os endpoints, consulte a documentação técnica completa.

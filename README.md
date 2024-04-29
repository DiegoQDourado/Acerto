
# Acerto

Apis de Produto, Pedido e Autentição. 


## Documentação da API


### `Api de Autenticação`
##### Retorna token JWT

```http
  GET /token
```

| Parâmetro   | Tipo       | Descrição                           |
| :---------- | :--------- | :---------------------------------- |
| `api_key` | `string` | **Obrigatório**. A chave da sua API para geração do token - ApiKeys cadastradas (**produtoapi, pedidoapi**) |

### `Api de Produto`
#### Retorna um produto

```http
  GET /produtos/${id}
```

| Parâmetro   | Tipo       | Descrição                                   |
| :---------- | :--------- | :------------------------------------------ |
| `id`      | `uuid` | **Obrigatório**. O ID do produto que você quer |

#### Atualiza um produto

```http
  PUT /produtos/${id}
```

| Parâmetro   | Tipo       | Descrição                                   |
| :---------- | :--------- | :------------------------------------------ |
| `id`      | `uuid` | **Obrigatório**. O ID do produto que você quer |
|   `body`   | `json` | **Obrigatório**. O ID do produto que você quer |

```JSON
Example Body:

{
  "nome": "string",
  "descricao": "string",
  "preco": 0,
  "quantidadeEstoque": 0
}
```
#### Deleta um produto

```http
  DELETE /produtos/${id}
```

| Parâmetro   | Tipo       | Descrição                                   |
| :---------- | :--------- | :------------------------------------------ |
| `id`      | `uuid` | **Obrigatório**. O ID do produto que você quer deletar |

#### Busca a quantidade de um produto

```http
  GET /produtos/${id}/quantity
```

| Parâmetro   | Tipo       | Descrição                                   |
| :---------- | :--------- | :------------------------------------------ |
| `id`      | `uuid` | **Obrigatório**. O ID do produto que você quer deletar |

#### Cria um produto
```http
  POST /produtos
```

| Parâmetro   | Tipo       | Descrição                                   |
| :---------- | :--------- | :------------------------------------------ |
|   `body`   | `json` | **Obrigatório**. O ID do produto que você quer |

```JSON
Example Body:

{
  "nome": "string",
  "descricao": "string",
  "preco": 0,
  "quantidadeEstoque": 0
}
```

### `Api de Pedidos`
#### Retorna um pedido

```http
  GET /pedidos/${codigo}
```

| Parâmetro   | Tipo       | Descrição                                   |
| :---------- | :--------- | :------------------------------------------ |
| `codigo`      | `int` | **Obrigatório**. O código do pedido que você quer |

#### Deleta um pedido

```http
  DELETE /pedidos/${codigo}
```

| Parâmetro   | Tipo       | Descrição                                   |
| :---------- | :--------- | :------------------------------------------ |
| `codigo`      | `int` | **Obrigatório**. O codgio do produto que você quer deletar |

#### Cria um pedido
```http
  POST /pedidos
```

| Parâmetro   | Tipo       | Descrição                                   |
| :---------- | :--------- | :------------------------------------------ |
|   `body`   | `json` | **Obrigatório**. O ID do produto que você quer |

```JSON
Example Body:

{
  "codigo": 0,
  "descricao": "string",
  "valorTotal": 0,
  "situacao": "string",
  "pedidoItems": [
    {
      "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "quantidade": 0
    }
  ]
}
```

## Roadmap

- Implementar Warmup
- Adcionar healthchecks 
- Aumentar Cobertura de testes
- Incluir correlationId para rastreio das requisições
- Criar filters para tratamento de mensagens de retornos e exceptions tratadas.
- Incluir NotificationPattern

## Stack utilizada

**Back-end:** .Net 8, Postegres, RabbitMq

## Deploy

To deploy this project acces the issues-git-docker access the folder 

```bash
  project-folder
```
Then run:

```bash
  docker swarm init && docker-compose up -d
```

## Rodando Local
UUse o Postman para teste, só exportar a collection no caminho abaixo:

```bash
  project-folder/postman-collection
```
# ?? Exemplos de Requisições - Size Antecipação API

Exemplos de requisições HTTP para testar a API usando Postman, Insomnia, cURL ou qualquer cliente HTTP.

---

## ?? Configuração Base

**Base URL (Desenvolvimento)**:
```
https://localhost:7001
```

> ?? A porta pode variar. Verifique o console ao executar `dotnet run`

---

## ?? Endpoints Disponíveis

### 1. Listar Tomadores (Teste)

**GET** `/Tomador`

```bash
curl -X GET "https://localhost:7001/Tomador" \
  -H "accept: application/json" \
  -k
```

**Resposta de Sucesso (200)**:
```json
[
  {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "razaoSocial": "Empresa Teste LTDA",
    "documento": {
      "numero": "12345678000190",
      "tipo": 1
    },
    "criadoEm": "2024-03-15T10:00:00",
    "atualizadoEm": "2024-03-15T10:00:00"
  }
]
```

---

### 2. Adicionar Duplicatas ao Carrinho

**POST** `/api/carrinho/inserir-duplicata`

```bash
curl -X POST "https://localhost:7001/api/carrinho/inserir-duplicata" \
  -H "Content-Type: application/json" \
  -H "accept: application/json" \
  -k \
  -d '{
    "tomadorId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "duplicatasIds": [
      "4fb96f75-6828-5673-c4gd-3d074g77bgb7",
      "5gc07g86-7939-6784-d5he-4e185h88chc8"
    ]
  }'
```

**Body (JSON)**:
```json
{
  "tomadorId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "duplicatasIds": [
    "4fb96f75-6828-5673-c4gd-3d074g77bgb7",
    "5gc07g86-7939-6784-d5he-4e185h88chc8"
  ]
}
```

**Resposta de Sucesso (200)**:
```json
{
  "sucesso": true,
  "dados": null
}
```

**Resposta de Erro (400)**:
```json
{
  "sucesso": false,
  "erros": [
    "Duplicata não encontrada ou indisponível"
  ]
}
```

---

### 3. Visualizar Carrinho

**GET** `/api/carrinho/{tomadorId}`

```bash
curl -X GET "https://localhost:7001/api/carrinho/3fa85f64-5717-4562-b3fc-2c963f66afa6" \
  -H "accept: application/json" \
  -k
```

**Resposta de Sucesso (200)**:
```json
{
  "sucesso": true,
  "dados": {
    "id": "6hd18h97-8a4a-7895-e6if-5f296i99dii9",
    "tomadorId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "itens": [
      {
        "duplicataId": "4fb96f75-6828-5673-c4gd-3d074g77bgb7",
        "numero": "DUP-001",
        "valor": 10000.00,
        "valorLiquido": 9700.00,
        "dataVencimento": "2024-04-15T00:00:00"
      },
      {
        "duplicataId": "5gc07g86-7939-6784-d5he-4e185h88chc8",
        "numero": "DUP-002",
        "valor": 5000.00,
        "valorLiquido": 4850.00,
        "dataVencimento": "2024-04-20T00:00:00"
      }
    ],
    "valorTotal": 15000.00,
    "valorLiquidoTotal": 14550.00,
    "quantidadeItens": 2
  }
}
```

**Resposta: Carrinho Vazio (200)**:
```json
{
  "sucesso": true,
  "dados": null
}
```

---

### 4. Remover Duplicatas do Carrinho

**POST** `/api/carrinho/remover-duplicata`

```bash
curl -X POST "https://localhost:7001/api/carrinho/remover-duplicata" \
  -H "Content-Type: application/json" \
  -H "accept: application/json" \
  -k \
  -d '{
    "tomadorId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "duplicatasIds": [
      "4fb96f75-6828-5673-c4gd-3d074g77bgb7"
    ]
  }'
```

**Body (JSON)**:
```json
{
  "tomadorId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "duplicatasIds": [
    "4fb96f75-6828-5673-c4gd-3d074g77bgb7"
  ]
}
```

**Resposta de Sucesso (200)**:
```json
{
  "sucesso": true,
  "dados": null
}
```

---

### 5. Realizar Checkout (Confirmar Antecipação)

**POST** `/api/carrinho/checkout/{tomadorId}`

```bash
curl -X POST "https://localhost:7001/api/carrinho/checkout/3fa85f64-5717-4562-b3fc-2c963f66afa6" \
  -H "accept: application/json" \
  -k
```

**Resposta de Sucesso (200)**:
```json
{
  "sucesso": true,
  "dados": {
    "operacaoId": "7ie29ia8-9b5b-8906-f7jg-6g3a7j00ejj0",
    "codigo": "OP-20240315-001",
    "valorTotal": 15000.00,
    "valorLiquido": 14550.00,
    "desconto": 450.00,
    "quantidadeDuplicatas": 2,
    "criadoEm": "2024-03-15T14:30:00"
  }
}
```

**Resposta de Erro: Carrinho Vazio (400)**:
```json
{
  "sucesso": false,
  "erros": [
    "Carrinho vazio ou não encontrado"
  ]
}
```

---

### 6. Consultar Operação por Código

**GET** `/api/operacao/codigo/{codigo}`

```bash
curl -X GET "https://localhost:7001/api/operacao/codigo/OP-20240315-001" \
  -H "accept: application/json" \
  -k
```

**Resposta de Sucesso (200)**:
```json
{
  "id": "7ie29ia8-9b5b-8906-f7jg-6g3a7j00ejj0",
  "codigo": "OP-20240315-001",
  "tomadorId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "razaoSocialTomador": "Empresa Teste LTDA",
  "valorTotal": 15000.00,
  "valorLiquido": 14550.00,
  "desconto": 450.00,
  "taxaDesconto": 3.00,
  "status": 1,
  "statusDescricao": "Aprovada",
  "quantidadeDuplicatas": 2,
  "duplicatas": [
    {
      "id": "4fb96f75-6828-5673-c4gd-3d074g77bgb7",
      "numero": "DUP-001",
      "valor": 10000.00,
      "valorLiquido": 9700.00,
      "desconto": 300.00,
      "dataVencimento": "2024-04-15T00:00:00"
    },
    {
      "id": "5gc07g86-7939-6784-d5he-4e185h88chc8",
      "numero": "DUP-002",
      "valor": 5000.00,
      "valorLiquido": 4850.00,
      "desconto": 150.00,
      "dataVencimento": "2024-04-20T00:00:00"
    }
  ],
  "criadoEm": "2024-03-15T14:30:00",
  "atualizadoEm": "2024-03-15T14:30:00"
}
```

**Resposta: Não Encontrada (404)**:
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.4",
  "title": "Not Found",
  "status": 404,
  "traceId": "00-xxxxx-xxxxx-00"
}
```

---

## ?? Cenários de Teste Completos

### Cenário 1: Fluxo Completo de Antecipação

```bash
# 1. Definir variáveis
TOMADOR_ID="SEU_TOMADOR_ID_AQUI"
DUPLICATA_1="DUPLICATA_ID_1_AQUI"
DUPLICATA_2="DUPLICATA_ID_2_AQUI"
BASE_URL="https://localhost:7001"

# 2. Adicionar duplicatas ao carrinho
curl -X POST "$BASE_URL/api/carrinho/inserir-duplicata" \
  -H "Content-Type: application/json" \
  -k \
  -d "{
    \"tomadorId\": \"$TOMADOR_ID\",
    \"duplicatasIds\": [\"$DUPLICATA_1\", \"$DUPLICATA_2\"]
  }"

# 3. Visualizar carrinho
curl -X GET "$BASE_URL/api/carrinho/$TOMADOR_ID" -k | jq

# 4. Fazer checkout
RESPONSE=$(curl -s -X POST "$BASE_URL/api/carrinho/checkout/$TOMADOR_ID" -k)
CODIGO=$(echo $RESPONSE | jq -r '.dados.codigo')

# 5. Consultar operação
curl -X GET "$BASE_URL/api/operacao/codigo/$CODIGO" -k | jq
```

---

### Cenário 2: Gerenciar Carrinho (Adicionar e Remover)

```bash
TOMADOR_ID="SEU_TOMADOR_ID_AQUI"
BASE_URL="https://localhost:7001"

# Adicionar 3 duplicatas
curl -X POST "$BASE_URL/api/carrinho/inserir-duplicata" \
  -H "Content-Type: application/json" \
  -k \
  -d "{
    \"tomadorId\": \"$TOMADOR_ID\",
    \"duplicatasIds\": [\"DUP_1\", \"DUP_2\", \"DUP_3\"]
  }"

# Ver carrinho (deve ter 3 itens)
curl -X GET "$BASE_URL/api/carrinho/$TOMADOR_ID" -k | jq

# Remover 1 duplicata
curl -X POST "$BASE_URL/api/carrinho/remover-duplicata" \
  -H "Content-Type: application/json" \
  -k \
  -d "{
    \"tomadorId\": \"$TOMADOR_ID\",
    \"duplicatasIds\": [\"DUP_1\"]
  }"

# Ver carrinho novamente (deve ter 2 itens)
curl -X GET "$BASE_URL/api/carrinho/$TOMADOR_ID" -k | jq

# Fazer checkout com 2 itens
curl -X POST "$BASE_URL/api/carrinho/checkout/$TOMADOR_ID" -k | jq
```

---

## ?? Coleção do Postman

### Importar para o Postman

1. Abra o Postman
2. Clique em **Import**
3. Cole este JSON:

```json
{
  "info": {
    "name": "Size Antecipação API",
    "description": "Coleção de endpoints para testes da API de Antecipação de Recebíveis",
    "schema": "https://schema.getpostman.com/json/collection/v2.1.0/collection.json"
  },
  "variable": [
    {
      "key": "baseUrl",
      "value": "https://localhost:7001",
      "type": "string"
    },
    {
      "key": "tomadorId",
      "value": "",
      "type": "string"
    },
    {
      "key": "codigoOperacao",
      "value": "",
      "type": "string"
    }
  ],
  "item": [
    {
      "name": "Tomadores",
      "item": [
        {
          "name": "Listar Tomadores",
          "request": {
            "method": "GET",
            "header": [],
            "url": {
              "raw": "{{baseUrl}}/Tomador",
              "host": ["{{baseUrl}}"],
              "path": ["Tomador"]
            }
          }
        }
      ]
    },
    {
      "name": "Carrinho",
      "item": [
        {
          "name": "Adicionar Duplicatas",
          "request": {
            "method": "POST",
            "header": [
              {
                "key": "Content-Type",
                "value": "application/json"
              }
            ],
            "body": {
              "mode": "raw",
              "raw": "{\n  \"tomadorId\": \"{{tomadorId}}\",\n  \"duplicatasIds\": [\n    \"DUPLICATA_ID_1\",\n    \"DUPLICATA_ID_2\"\n  ]\n}"
            },
            "url": {
              "raw": "{{baseUrl}}/api/carrinho/inserir-duplicata",
              "host": ["{{baseUrl}}"],
              "path": ["api", "carrinho", "inserir-duplicata"]
            }
          }
        },
        {
          "name": "Visualizar Carrinho",
          "request": {
            "method": "GET",
            "header": [],
            "url": {
              "raw": "{{baseUrl}}/api/carrinho/{{tomadorId}}",
              "host": ["{{baseUrl}}"],
              "path": ["api", "carrinho", "{{tomadorId}}"]
            }
          }
        },
        {
          "name": "Remover Duplicatas",
          "request": {
            "method": "POST",
            "header": [
              {
                "key": "Content-Type",
                "value": "application/json"
              }
            ],
            "body": {
              "mode": "raw",
              "raw": "{\n  \"tomadorId\": \"{{tomadorId}}\",\n  \"duplicatasIds\": [\n    \"DUPLICATA_ID_1\"\n  ]\n}"
            },
            "url": {
              "raw": "{{baseUrl}}/api/carrinho/remover-duplicata",
              "host": ["{{baseUrl}}"],
              "path": ["api", "carrinho", "remover-duplicata"]
            }
          }
        },
        {
          "name": "Fazer Checkout",
          "request": {
            "method": "POST",
            "header": [],
            "url": {
              "raw": "{{baseUrl}}/api/carrinho/checkout/{{tomadorId}}",
              "host": ["{{baseUrl}}"],
              "path": ["api", "carrinho", "checkout", "{{tomadorId}}"]
            }
          },
          "event": [
            {
              "listen": "test",
              "script": {
                "exec": [
                  "if (pm.response.code === 200) {",
                  "    var jsonData = pm.response.json();",
                  "    pm.collectionVariables.set('codigoOperacao', jsonData.dados.codigo);",
                  "}"
                ]
              }
            }
          ]
        }
      ]
    },
    {
      "name": "Operação",
      "item": [
        {
          "name": "Consultar por Código",
          "request": {
            "method": "GET",
            "header": [],
            "url": {
              "raw": "{{baseUrl}}/api/operacao/codigo/{{codigoOperacao}}",
              "host": ["{{baseUrl}}"],
              "path": ["api", "operacao", "codigo", "{{codigoOperacao}}"]
            }
          }
        }
      ]
    }
  ]
}
```

### Configurar Variáveis

Após importar, configure:

1. **baseUrl**: `https://localhost:7XXX` (sua porta)
2. **tomadorId**: Cole um ID válido de tomador
3. **codigoOperacao**: Será preenchido automaticamente após o checkout

---

## ?? Testes Automatizados (Postman)

### Test Scripts para Adicionar nos Requests

#### Para "Listar Tomadores":
```javascript
pm.test("Status code é 200", function () {
    pm.response.to.have.status(200);
});

pm.test("Resposta contém array de tomadores", function () {
    var jsonData = pm.response.json();
    pm.expect(jsonData).to.be.an('array');
    pm.expect(jsonData.length).to.be.above(0);
});

// Salvar primeiro tomadorId
if (pm.response.json().length > 0) {
    pm.collectionVariables.set("tomadorId", pm.response.json()[0].id);
}
```

#### Para "Adicionar Duplicatas":
```javascript
pm.test("Status code é 200", function () {
    pm.response.to.have.status(200);
});

pm.test("Operação foi bem sucedida", function () {
    var jsonData = pm.response.json();
    pm.expect(jsonData.sucesso).to.be.true;
});
```

#### Para "Fazer Checkout":
```javascript
pm.test("Status code é 200", function () {
    pm.response.to.have.status(200);
});

pm.test("Operação criada com sucesso", function () {
    var jsonData = pm.response.json();
    pm.expect(jsonData.sucesso).to.be.true;
    pm.expect(jsonData.dados).to.have.property('codigo');
});

// Salvar código da operação
if (pm.response.code === 200) {
    var jsonData = pm.response.json();
    pm.collectionVariables.set('codigoOperacao', jsonData.dados.codigo);
}
```

---

## ?? Dicas para Testes

### Obter IDs Válidos do Banco

Execute estas queries no SQL Server para obter IDs válidos:

```sql
-- Obter TomadorId
SELECT TOP 1 Id FROM [FichaCadastral].[Tomadores];

-- Obter Duplicatas de um Tomador
SELECT TOP 5 Id 
FROM [CatalogoRecebiveis].[Duplicatas]
WHERE TomadorId = 'SEU_TOMADOR_ID'
  AND Status = 0;
```

### Ou Use o Script Automático

```sql
-- Gera JSON pronto para usar
DECLARE @TomadorId VARCHAR(200) = (SELECT TOP 1 Id FROM [FichaCadastral].[Tomadores]);

SELECT 
    @TomadorId AS tomadorId,
    (
        SELECT TOP 5 Id 
        FROM [CatalogoRecebiveis].[Duplicatas] 
        WHERE TomadorId = @TomadorId AND Status = 0
        FOR JSON PATH
    ) AS duplicatasIds;
```

---

## ?? Segurança em Testes

### Desabilitar SSL em Testes Locais

**cURL**:
```bash
# Adicione -k ou --insecure
curl -k https://localhost:7001/Tomador
```

**Postman**:
1. Settings ? General
2. Desmarque **SSL certificate verification**

**Insomnia**:
1. Preferences ? General
2. Desmarque **Validate certificates**

> ?? **Apenas em desenvolvimento!** Nunca desabilite SSL em produção!

---

## ?? Referências

- [Documentação cURL](https://curl.se/docs/)
- [Postman Learning Center](https://learning.postman.com/)
- [Insomnia Docs](https://docs.insomnia.rest/)
- [README Principal](README.md)

---

**Happy Testing! ??**

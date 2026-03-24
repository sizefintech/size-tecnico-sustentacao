# 🏦 Size - API de Antecipação de Recebíveis

[![Open in GitHub Codespaces](https://github.com/codespaces/badge.svg)](https://github.com/codespaces/new?hide_repo_select=true&ref=master&repo=sizefintech/size-tecnico-sustentacao)

API desenvolvida em .NET 8 para gerenciamento de antecipação de recebíveis, permitindo que empresas antecipem o recebimento de duplicatas através de um fluxo completo de carrinho de compras e checkout.

## 📋 Índice

- [Sobre o Projeto](#-sobre-o-projeto)
- [Início Rápido](#-início-rápido)
- [Arquitetura](#-arquitetura)
- [Tecnologias](#-tecnologias)
- [Endpoints](#-endpoints)

---

## 💡 Sobre o Projeto

A **Antecipação de Recebíveis** permite que empresas recebam antecipadamente valores de duplicatas futuras. Um desconto é aplicado sobre o valor total, e a empresa recebe o valor líquido de forma imediata.

**Exemplo:**
- Duplicata: R$ 10.000,00 (vence em 30 dias)
- Taxa: 3%
- Valor recebido: R$ 9.700,00 (hoje)

### Fluxo de Uso

```
Consultar Duplicatas → Adicionar ao Carrinho → Checkout → Operação Criada
```

---

## 🚀 Início Rápido

### Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/sql-server/sql-server-downloads) ou LocalDB

### Configuração

```bash
# 1. Clonar o repositório
git clone https://github.com/sizefintech/size-tecnico-sustentacao.git
cd size-tecnico-sustentacao

# 2. Configurar connection string (User Secrets)
cd src/size-antecipacao
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost;Database=SizeAntecipacao;Integrated Security=True;TrustServerCertificate=True;"

# 3. Executar a aplicação (migrations são aplicadas automaticamente)
dotnet run
```

A API estará disponível em `https://localhost:7XXX/swagger`

### Aplicar Migrations Manualmente (Opcional)

```bash
dotnet ef database update --project ../size.FichaCadastral.Data
dotnet ef database update --project ../size.CatalogoRecebiveis.Data
dotnet ef database update --project ../size.Carrinho.Data
dotnet ef database update --project ../size.Operacao.Data
```

---

## 🏗 Arquitetura

O projeto segue **Domain-Driven Design (DDD)** e **Clean Architecture**, organizado em 4 domínios principais:

| Domínio | Responsabilidade |
|---------|-----------------|
| **FichaCadastral** | Gerencia os tomadores (empresas/clientes) |
| **CatalogoRecebiveis** | Gerencia as duplicatas disponíveis |
| **Carrinho** | Gerencia a seleção de duplicatas |
| **Operacao** | Registra as operações de antecipação |

### Estrutura de Pastas

```
src/
├── size-antecipacao/                    # API Principal
├── size.Core/                           # Núcleo Compartilhado
├── size.FichaCadastral.*/              # Domínio: Ficha Cadastral
├── size.CatalogoRecebiveis.*/          # Domínio: Catálogo de Recebíveis
├── size.Carrinho.*/                    # Domínio: Carrinho de Compras
├── size.Operacao.*/                    # Domínio: Operações
└── size.ApplicationService.ProcessamentoCheckout/  # Serviço de Checkout
```

---

## 🛠 Tecnologias

- **.NET 8** + **ASP.NET Core**
- **Entity Framework Core 9.0**
- **SQL Server**
- **Swagger/OpenAPI**
- **Polly** (Resiliência)

**Padrões:** DDD, SOLID, Repository Pattern, Dependency Injection, Clean Architecture

---

## 📚 Endpoints

### 🛒 Carrinho

#### Adicionar Duplicatas
```http
POST /api/carrinho/inserir-duplicata
Content-Type: application/json

{
  "tomadorId": "guid-do-tomador",
  "duplicatasIds": ["guid-duplicata-1", "guid-duplicata-2"]
}
```

#### Visualizar Carrinho
```http
GET /api/carrinho/{tomadorId}
```

Resposta:
```json
{
  "id": "guid",
  "tomadorId": "guid",
  "duplicatas": [...],
  "valorTotal": 10000.00,
  "valorLiquidoTotal": 9700.00
}
```

#### Remover Duplicatas
```http
POST /api/carrinho/remover-duplicata
Content-Type: application/json

{
  "tomadorId": "guid-do-tomador",
  "duplicatasIds": ["guid-duplicata-1"]
}
```

#### Realizar Checkout
```http
POST /api/carrinho/checkout/{tomadorId}
```

Resposta:
```json
{
  "sucesso": true,
  "dados": {
    "operacaoId": "guid",
    "codigo": "OP-20240315-001",
    "valorTotal": 10000.00,
    "valorLiquido": 9700.00,
    "quantidadeDuplicatas": 2
  }
}
```

### ✅ Operação

#### Consultar por Código
```http
GET /api/operacao/codigo/{codigo}
```

Exemplo: `GET /api/operacao/codigo/OP-20240315-001`

Resposta:
```json
{
  "id": "guid",
  "codigo": "OP-20240315-001",
  "tomadorId": "guid",
  "valorTotal": 10000.00,
  "valorLiquido": 9700.00,
  "status": "Aprovada",
  "criadoEm": "2024-03-15T10:30:00",
  "duplicatas": [...]
}
```

### 🏢 Tomador

#### Listar Tomadores
```http
GET /Tomador
```

---

## 🗄️ Banco de Dados

### Schemas

- **FichaCadastral**: `Tomadores`
- **CatalogoRecebiveis**: `Duplicatas`
- **Carrinho**: `Carrinhos`, `CarrinhoItem`
- **Operacao**: `Operacoes`, `OperacaoDuplicata`

Scripts SQL para popular o banco estão disponíveis em `script.sql`

---

**Desenvolvido com ❤️ usando .NET 8**

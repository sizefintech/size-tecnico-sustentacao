# ? Guia de Início Rápido - 5 Minutos

## ?? Opção 1: GitHub Codespaces (Recomendado para Teste Técnico)

### Por que Codespaces?
? **Ambiente pronto em 3 minutos** - SQL Server + .NET 8 + Dados populados  
? **Zero configuração** - Funciona em qualquer computador  
? **Grátis** - 60 horas/mês no plano Free  

### Como usar:

1. **Clique no botão:**

   [![Open in Codespaces](https://github.com/codespaces/badge.svg)](https://github.com/codespaces/new?hide_repo_select=true&ref=master&repo=sizefintech/size-tecnico-sustentacao)

2. **Aguarde ~3 minutos** (o setup é automático)

3. **Execute a API:**
   ```bash
   cd src/size-antecipacao
   dotnet run
   ```

4. **Acesse o Swagger:**
   - O VS Code abrirá automaticamente uma notificação com a URL
   - **OU** clique na aba "PORTS" (parte inferior) ? porta **5075** ? ícone de globo ??
   - **Adicione `/swagger`** no final da URL
   
   Exemplo de URL:
   ```
   https://effective-space-sniffle-q7rx7qj7px6c9xw-5075.app.github.dev/swagger
   ```

**?? [Guia Completo do Codespaces](.devcontainer/README.md)**

---

## ?? Opção 2: Ambiente Local (Windows/Mac/Linux)

### 1?? Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server 2019+](https://www.microsoft.com/sql-server/sql-server-downloads) ou LocalDB
- [Git](https://git-scm.com/)

### 2?? Clonar e Configurar

```bash
# Clone o repositório
git clone https://github.com/sizefintech/size-tecnico-sustentacao.git
cd size-tecnico-sustentacao

# Configure a connection string
cd src/size-antecipacao
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost;Database=SizeAntecipacao;Integrated Security=True;TrustServerCertificate=True;"
```

### 3?? Criar Banco de Dados

```bash
# Aplicar migrations (ainda na pasta src/size-antecipacao)
dotnet ef database update --project ../size.fichaCadastral.Data
dotnet ef database update --project ../size.CatalogoRecebiveis.Data
dotnet ef database update --project ../size.Carrinho.Data
dotnet ef database update --project ../size.Operacao.Data

# Popular o banco (voltar para a raiz)
cd ../..
sqlcmd -S localhost -d SizeAntecipacao -i script.sql
```

**Nota:** Se `sqlcmd` não estiver disponível, use o SQL Server Management Studio (SSMS) para executar `script.sql`

### 4?? Executar

```bash
cd src/size-antecipacao
dotnet run
```

### 5?? Acessar

Abra: `http://localhost:5075/swagger` (a porta aparecerá no terminal)

---

## ?? Testar a API

### 1. **Listar Tomadores** (para obter IDs)

```bash
GET /Tomador
```

Copie um `id` de tomador da resposta.

### 2. **Ver Catálogo de Duplicatas**

```bash
GET /api/catalogo-recebiveis/{tomadorId}
```

Copie alguns `id`s de duplicatas.

### 3. **Adicionar ao Carrinho**

```bash
POST /api/carrinho/inserir-duplicata
Content-Type: application/json

{
  "tomadorId": "SEU-TOMADOR-ID",
  "duplicatasIds": ["DUPLICATA-ID-1", "DUPLICATA-ID-2"]
}
```

### 4. **Ver Carrinho**

```bash
GET /api/carrinho/{tomadorId}
```

### 5. **Fazer Checkout**

```bash
POST /api/carrinho/checkout/{tomadorId}
```

Resposta terá o `codigo` da operação.

### 6. **Consultar Operação**

```bash
GET /api/operacao/codigo/{codigo}
```

---

## ?? Fluxo Resumido

```
1. Consultar Tomadores ? Obter tomadorId
2. Ver Catálogo ? Obter duplicatasIds
3. Adicionar ao Carrinho ? Selecionar duplicatas
4. Ver Carrinho ? Revisar seleção
5. Checkout ? Confirmar antecipação
6. Consultar Operação ? Verificar resultado
```

---

## ?? Problemas Comuns

### ? "404 Not Found" no Codespaces

**Solução:**
1. Certifique-se de adicionar `/swagger` no final da URL
2. Use HTTP (não HTTPS) ou aguarde o certificado carregar
3. Verifique se a API está rodando (`dotnet run` executado?)

**URL correta:**
```
https://[seu-codespace]-5075.app.github.dev/swagger
```

### ? "Connection string not found"

**Solução:**
```bash
cd src/size-antecipacao
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost;Database=SizeAntecipacao;Integrated Security=True;TrustServerCertificate=True;"
```

### ? "Cannot connect to SQL Server"

**Soluções:**

1. **SQL Server não está rodando:**
   - Windows: Abra Services ? SQL Server (MSSQLSERVER) ? Start
   - LocalDB: `sqllocaldb start mssqllocaldb`

2. **Usando LocalDB?** Mude a connection string:
   ```bash
   dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=(localdb)\\mssqllocaldb;Database=SizeAntecipacao;Integrated Security=True;TrustServerCertificate=True;"
   ```

3. **Usando autenticação SQL?**
   ```bash
   dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost;Database=SizeAntecipacao;User Id=SEU_USUARIO;Password=SUA_SENHA;TrustServerCertificate=True;"
   ```

### ? "dotnet ef: command not found"

**Solução:**
```bash
dotnet tool install --global dotnet-ef
```

### ? Banco de dados vazio

**Solução:**
```bash
# Execute o script SQL
sqlcmd -S localhost -d SizeAntecipacao -i script.sql

# Ou use SSMS/Azure Data Studio para executar o script.sql
```

---

## ?? Documentação Completa

- **[README.md](README.md)** - Documentação técnica completa
- **[.devcontainer/README.md](.devcontainer/README.md)** - Guia do Codespaces
- **Swagger UI** - Documentação interativa dos endpoints (após rodar a API)

---

## ?? Dicas Rápidas

### Hot Reload (Recompila automaticamente ao salvar)
```bash
cd src/size-antecipacao
dotnet watch run
```

### Ver estrutura do banco no VS Code
1. Instale a extensão: **SQL Server (mssql)**
2. Conecte ao servidor
3. Explore as tabelas

### Debugar no Visual Studio
1. Abra `size-tecnico-sustentacao.sln`
2. Defina `size-antecipacao` como projeto de inicialização
3. Pressione `F5`

### Debugar no VS Code
1. Abra a pasta do projeto
2. Pressione `F5`
3. Selecione `.NET Core Launch (web)`

### Como ver a URL no Codespaces
1. Clique na aba **"PORTS"** (parte inferior do VS Code)
2. Localize a porta **5075**
3. Clique no ícone de globo ?? para abrir no navegador
4. Adicione `/swagger` no final da URL

---

## ? Checklist Teste Técnico

### Antes de começar
- [ ] Ambiente configurado (Codespaces ou local)
- [ ] API rodando com sucesso
- [ ] Swagger abrindo
- [ ] Banco populado (endpoints retornam dados)

### Durante o desenvolvimento
- [ ] Git commits frequentes
- [ ] Testes de todos os endpoints
- [ ] Validação de regras de negócio

### Antes de entregar
- [ ] `dotnet build` sem erros
- [ ] Todos os requisitos implementados
- [ ] Código limpo e comentado
- [ ] README atualizado (se necessário)

---

**?? Pronto! Bom desenvolvimento!**

Se encontrar problemas, consulte o [README completo](README.md) ou a [documentação do Codespaces](.devcontainer/README.md).

**Desenvolvido com ?? para o Teste Técnico Size Fintech**

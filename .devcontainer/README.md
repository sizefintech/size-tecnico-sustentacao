# ?? Guia Rápido - GitHub Codespaces

## ? Início Rápido (2 minutos)

### 1. Abrir no Codespaces

[![Open in GitHub Codespaces](https://github.com/codespaces/badge.svg)](https://github.com/codespaces/new?hide_repo_select=true&ref=master&repo=sizefintech/size-tecnico-sustentacao)

**OU**

1. Clique no botão verde **"<> Code"** no GitHub
2. Selecione a aba **"Codespaces"**
3. Clique em **"Create codespace on master"**

### 2. Aguarde a Configuração Automática

O setup automático irá:
- ? Instalar .NET 8 SDK
- ? Configurar SQL Server 2022
- ? Criar banco `SizeAntecipacao`
- ? Executar `script.sql` (popular dados)
- ? Aplicar migrations
- ? Configurar User Secrets
- ? Restaurar pacotes NuGet
- ? Compilar a solução

**Tempo estimado:** ~3-4 minutos ??

### 3. Executar a API

```bash
cd src/size-antecipacao
dotnet run
```

### 4. Acessar o Swagger

Após iniciar, clique na notificação de porta ou:
- **Swagger UI:** `https://[seu-codespace].githubpreview.dev/swagger`

---

## ?? O que já está configurado?

### ? Banco de Dados SQL Server

| Item | Valor |
|------|-------|
| **Servidor** | `sqlserver` |
| **Banco** | `SizeAntecipacao` |
| **Usuário** | `sa` |
| **Senha** | `Size@2024!Strong` |
| **Porta** | `1433` |

### ? Connection String

Já configurada automaticamente em User Secrets:

```
Server=sqlserver;Database=SizeAntecipacao;User Id=sa;Password=Size@2024!Strong;TrustServerCertificate=True;Encrypt=False;
```

### ? Extensões do VS Code

- **C# Dev Kit** - IntelliSense, debugging
- **SQL Server (mssql)** - Gerenciar banco de dados
- **REST Client** - Testar APIs
- **NuGet Gallery** - Gerenciar pacotes

---

## ?? Conectar ao SQL Server no VS Code

### Opção 1: Extensão SQL Server (mssql)

1. Pressione `Ctrl+Shift+P` (ou `Cmd+Shift+P` no Mac)
2. Digite: `MS SQL: Connect`
3. **Já está configurado!** Selecione a conexão pré-definida:
   - **Server:** sqlserver
   - **Database:** SizeAntecipacao
   - **Auth:** SQL Login
   - **User:** sa
   - **Password:** Size@2024!Strong

### Opção 2: Azure Data Studio

Se preferir interface gráfica completa:
```bash
# Instalar Azure Data Studio
wget -qO- https://aka.ms/azuredatastudio-linux | sudo tar -xz -C /opt
```

---

## ?? Testar a API

### Opção 1: Swagger UI

Acesse automaticamente após `dotnet run`:
```
https://[seu-codespace].githubpreview.dev/swagger
```

### Opção 2: REST Client (arquivo .http)

Arquivo já incluído: `.devcontainer/api-tests.http`

```http
### 1. Listar Tomadores
GET https://localhost:7XXX/Tomador

### 2. Ver Carrinho
GET https://localhost:7XXX/api/carrinho/{{tomadorId}}

### 3. Adicionar ao Carrinho
POST https://localhost:7XXX/api/carrinho/inserir-duplicata
Content-Type: application/json

{
  "tomadorId": "{{tomadorId}}",
  "duplicatasIds": ["{{duplicataId}}"]
}
```

### Opção 3: cURL

```bash
# Listar tomadores
curl -X GET https://localhost:7XXX/Tomador

# Ver carrinho
curl -X GET https://localhost:7XXX/api/carrinho/{tomadorId}
```

---

## ??? Comandos Úteis

### Migrations

```bash
# Aplicar todas as migrations
cd src/size-antecipacao

dotnet ef database update --project ../size.fichaCadastral.Data
dotnet ef database update --project ../size.CatalogoRecebiveis.Data
dotnet ef database update --project ../size.Carrinho.Data
dotnet ef database update --project ../size.Operacao.Data
```

### Build e Run

```bash
# Restaurar pacotes
dotnet restore

# Build
dotnet build

# Run com hot reload
cd src/size-antecipacao
dotnet watch run
```

### SQL Server

```bash
# Conectar ao SQL Server via terminal
/opt/mssql-tools/bin/sqlcmd -S sqlserver -U sa -P 'Size@2024!Strong'

# Executar query
/opt/mssql-tools/bin/sqlcmd -S sqlserver -U sa -P 'Size@2024!Strong' -d SizeAntecipacao -Q "SELECT * FROM FichaCadastral.Tomadores"

# Re-executar script.sql
/opt/mssql-tools/bin/sqlcmd -S sqlserver -U sa -P 'Size@2024!Strong' -d SizeAntecipacao -i script.sql
```

---

## ?? Troubleshooting

### SQL Server não conecta?

```bash
# Verificar status do SQL Server
docker ps

# Ver logs do SQL Server
docker logs [container-id]

# Reiniciar setup
bash .devcontainer/setup.sh
```

### Migrations não aplicadas?

```bash
cd src/size-antecipacao

# Instalar EF Tools se necessário
dotnet tool install --global dotnet-ef

# Aplicar migrations manualmente
dotnet ef database update --project ../size.fichaCadastral.Data
```

### Porta 5000/5001 já em uso?

Edite `src/size-antecipacao/Properties/launchSettings.json`:

```json
"applicationUrl": "https://localhost:7001;http://localhost:5002"
```

### Script.sql não executado?

```bash
# Executar manualmente
/opt/mssql-tools/bin/sqlcmd -S sqlserver -U sa -P 'Size@2024!Strong' -d SizeAntecipacao -i script.sql
```

---

## ?? Dicas e Truques

### 1. **Hot Reload Automático**

Use `dotnet watch run` para reload automático ao salvar arquivos:

```bash
cd src/size-antecipacao
dotnet watch run
```

### 2. **Depuração no VS Code**

Já configurado! Pressione `F5` para iniciar o debugger.

### 3. **Inspecionar Banco de Dados**

Use a extensão **SQL Server (mssql)**:
1. `Ctrl+Shift+E` ? Aba **SQL Server**
2. Expanda `sqlserver ? Databases ? SizeAntecipacao`
3. Clique direito em uma tabela ? **Select Top 1000**

### 4. **Logs em Tempo Real**

```bash
cd src/size-antecipacao
dotnet run --verbosity detailed
```

### 5. **Resetar Banco de Dados**

```bash
# Dropar e recriar banco
/opt/mssql-tools/bin/sqlcmd -S sqlserver -U sa -P 'Size@2024!Strong' -Q "DROP DATABASE SizeAntecipacao; CREATE DATABASE SizeAntecipacao;"

# Re-aplicar migrations e script
bash .devcontainer/setup.sh
```

---

## ?? Specs do Codespace

### Recomendado para este projeto:

| Spec | Descrição |
|------|-----------|
| **2-core** | Desenvolvimento normal |
| **4-core** | Builds mais rápidos |
| **8-core** | Múltiplos projetos/testes |

**Escolha ao criar o Codespace:**
- Clique em **"..."** ao lado de "Create codespace"
- Selecione **"Configure and create codespace"**
- Escolha a máquina desejada

---

## ?? Custos

### GitHub Codespaces - Free Tier

| Plano | Core Hours/Mês | Storage |
|-------|----------------|---------|
| **Free** | 120 horas (2-core) | 15 GB |
| **Pro** | 180 horas | 20 GB |

**Para este projeto:**
- **2-core:** ~60 horas de uso ativo/mês (grátis)
- **4-core:** ~30 horas de uso ativo/mês (grátis)

?? **Dica:** O Codespace para automaticamente após 30 minutos de inatividade!

---

## ?? Teste Técnico - Checklist

### ? Antes de começar

- [ ] Codespace criado e configurado
- [ ] SQL Server conectado
- [ ] Script.sql executado
- [ ] API rodando no Swagger
- [ ] Tomadores e duplicatas visíveis no banco

### ? Durante o desenvolvimento

- [ ] Commits frequentes
- [ ] Testes de endpoints no Swagger
- [ ] Validação de dados no SQL Server
- [ ] Build sem erros

### ? Antes de enviar

- [ ] `dotnet build` sem warnings
- [ ] Todos os endpoints testados
- [ ] README atualizado (se necessário)
- [ ] Código commitado e pushed

---

## ?? Suporte

### Problemas com Codespaces?
- [Documentação Oficial](https://docs.github.com/codespaces)
- [Troubleshooting](https://docs.github.com/codespaces/troubleshooting)

### Dúvidas sobre o projeto?
Verifique:
- `README.md` - Documentação completa
- `QUICKSTART.md` - Guia de 5 minutos
- Swagger UI - Documentação dos endpoints

---

**Desenvolvido para o Teste Técnico Size Fintech** ??

**Bom desenvolvimento!** ???

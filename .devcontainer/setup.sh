#!/bin/bash
set -e

echo "Iniciando configuracao do ambiente Size Antecipacao..."

# ----------------------------------------------------------------
# 1. Instalar sqlcmd se nao estiver disponivel
# ----------------------------------------------------------------
if ! command -v sqlcmd &>/dev/null; then
    echo "Instalando sqlcmd..."
    curl -fsSL https://packages.microsoft.com/keys/microsoft.asc | sudo gpg --dearmor -o /usr/share/keyrings/microsoft-prod.gpg
    curl -fsSL https://packages.microsoft.com/config/ubuntu/22.04/prod.list | sudo tee /etc/apt/sources.list.d/msprod.list > /dev/null
    sudo apt-get update -qq
    sudo ACCEPT_EULA=Y apt-get install -y -qq mssql-tools18 unixodbc-dev
fi

export PATH="$PATH:/opt/mssql-tools18/bin:/opt/mssql-tools/bin"

# Detectar binario e flags corretos
if [ -f "/opt/mssql-tools18/bin/sqlcmd" ]; then
    SQLCMD_BIN="/opt/mssql-tools18/bin/sqlcmd"
    SQLCMD_EXTRA_FLAGS="-C -N"
else
    SQLCMD_BIN="/opt/mssql-tools/bin/sqlcmd"
    SQLCMD_EXTRA_FLAGS=""
fi

# ----------------------------------------------------------------
# 2. Aguardar SQL Server estar pronto (healthcheck ja garantiu,
#    mas fazemos retry extra por seguranca)
# ----------------------------------------------------------------
echo "Aguardando SQL Server aceitar conexoes..."
MAX_RETRIES=20
RETRY=0
until $SQLCMD_BIN -S sqlserver -U sa -P "Size@2024!Strong" -Q "SELECT 1" $SQLCMD_EXTRA_FLAGS -l 3 > /dev/null 2>&1; do
    RETRY=$((RETRY + 1))
    if [ $RETRY -ge $MAX_RETRIES ]; then
        echo "ERRO: SQL Server nao ficou pronto apos $MAX_RETRIES tentativas."
        exit 1
    fi
    echo "  aguardando... ($RETRY/$MAX_RETRIES)"
    sleep 5
done
echo "SQL Server esta online!"

# ----------------------------------------------------------------
# 3. Criar banco antecipadamente (evita erro 4060 na extensao mssql)
# ----------------------------------------------------------------
echo "Criando banco SizeAntecipacao..."
$SQLCMD_BIN -S sqlserver -U sa -P "Size@2024!Strong" \
    -Q "IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'SizeAntecipacao') CREATE DATABASE SizeAntecipacao" \
    $SQLCMD_EXTRA_FLAGS
echo "Banco SizeAntecipacao pronto!"

# Navegar para o workspace
cd /workspace

# Restaurar pacotes NuGet
echo "Restaurando pacotes NuGet..."
dotnet restore

# Instalar EF Tools se necessario
if ! dotnet tool list -g | grep -q "dotnet-ef"; then
    echo "Instalando Entity Framework Tools..."
    dotnet tool install --global dotnet-ef
fi

export PATH="$PATH:$HOME/.dotnet/tools"

# Build do projeto
echo "Compilando solucao..."
dotnet build --no-restore

# Aplicar migrations
echo "Aplicando migrations..."
cd /workspace/src/size-antecipacao
dotnet ef database update --project ../size.fichaCadastral.Data --startup-project .
dotnet ef database update --project ../size.CatalogoRecebiveis.Data --startup-project .
dotnet ef database update --project ../size.Carrinho.Data --startup-project .
dotnet ef database update --project ../size.Operacao.Data --startup-project .
echo "Migrations aplicadas com sucesso!"

# Executar script SQL de seed (popular dados)
if [ -n "$SQLCMD_BIN" ]; then
    echo "Executando script SQL de seed..."
    $SQLCMD_BIN -S sqlserver -U sa -P "Size@2024!Strong" -d SizeAntecipacao -i /workspace/script.sql $SQLCMD_EXTRA_FLAGS || echo "Aviso: script.sql pode ja ter sido executado anteriormente."
    echo "Seed concluido!"
fi

echo ""
echo "Ambiente configurado com sucesso!"
echo ""
echo "Proximos passos:"
echo "  1. Execute: cd src/size-antecipacao && dotnet run"
echo "  2. Acesse o Swagger: https://[seu-codespace]-5075.app.github.dev/swagger"
echo ""
echo "Connection String:"
echo "  Server=sqlserver;Database=SizeAntecipacao;User Id=sa;Password=Size@2024!Strong"
echo ""

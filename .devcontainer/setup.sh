#!/bin/bash
set -e

echo "Iniciando configuracao do ambiente Size Antecipacao..."

# Aguardar SQL Server estar pronto com loop de retry
echo "Aguardando SQL Server inicializar..."
MAX_RETRIES=30
RETRY=0
SQLCMD_BIN=""

# Detectar caminho do sqlcmd (versao 18 ou legado)
if [ -f "/opt/mssql-tools18/bin/sqlcmd" ]; then
    SQLCMD_BIN="/opt/mssql-tools18/bin/sqlcmd"
    SQLCMD_EXTRA_FLAGS="-C -N"
elif [ -f "/opt/mssql-tools/bin/sqlcmd" ]; then
    SQLCMD_BIN="/opt/mssql-tools/bin/sqlcmd"
    SQLCMD_EXTRA_FLAGS=""
fi

if [ -n "$SQLCMD_BIN" ]; then
    until $SQLCMD_BIN -S sqlserver -U sa -P "Size@2024!Strong" -Q "SELECT 1" $SQLCMD_EXTRA_FLAGS -l 2 > /dev/null 2>&1; do
        RETRY=$((RETRY + 1))
        if [ $RETRY -ge $MAX_RETRIES ]; then
            echo "ERRO: SQL Server nao ficou pronto apos $MAX_RETRIES tentativas."
            exit 1
        fi
        echo "SQL Server ainda nao esta pronto (tentativa $RETRY/$MAX_RETRIES)... aguardando 5s"
        sleep 5
    done
else
    echo "sqlcmd nao encontrado, aguardando 60s para SQL Server subir..."
    sleep 60
fi

echo "SQL Server esta online!"

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

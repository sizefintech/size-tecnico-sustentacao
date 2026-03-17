#!/bin/bash

echo "?? Iniciando configuração do ambiente Size Antecipação..."

# Aguardar SQL Server estar pronto
echo "? Aguardando SQL Server inicializar..."
sleep 30

# Verificar se SQL Server está acessível (usando dotnet ao invés de sqlcmd)
echo "?? Verificando disponibilidade do SQL Server..."
sleep 10

echo "? SQL Server está online!"

# Navegar para o diretório do workspace
cd /workspace

# Restaurar pacotes NuGet
echo "?? Restaurando pacotes NuGet..."
dotnet restore

# Configurar User Secrets
echo "?? Configurando User Secrets..."
cd /workspace/src/size-antecipacao
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=sqlserver;Database=SizeAntecipacao;User Id=sa;Password=Size@2024!Strong;TrustServerCertificate=True;Encrypt=False;"

# Instalar EF Tools se necessário
if ! dotnet tool list -g | grep -q "dotnet-ef"; then
    echo "?? Instalando Entity Framework Tools..."
    dotnet tool install --global dotnet-ef
fi

# Build do projeto
echo "?? Compilando solução..."
cd /workspace
dotnet build --no-restore

echo ""
echo "? ? Ambiente configurado com sucesso! ?"
echo ""
echo "????????????????????????????????????????"
echo "?? Próximos passos:"
echo "????????????????????????????????????????"
echo ""
echo "1??  Execute:"
echo "   cd src/size-antecipacao && dotnet run"
echo ""
echo "   ? A aplicação irá automaticamente:"
echo "      ? Aplicar migrations"
echo "      ? Criar banco de dados"
echo "      ? Executar script.sql (popular dados)"
echo ""
echo "2??  Acesse o Swagger:"
echo "   ? A URL abrirá automaticamente"
echo "   ? Ou clique na notificação de porta 5075"
echo ""
echo "   Exemplo de URL:"
echo "   https://xxx-5075.app.github.dev/swagger"
echo ""
echo "????????????????????????????????????????"
echo "?? Connection String configurada:"
echo "????????????????????????????????????????"
echo "Server=sqlserver;Database=SizeAntecipacao;User Id=sa;Password=Size@2024!Strong"
echo ""
echo "???  SQL Server (VS Code):"
echo "   - Servidor: sqlserver"
echo "   - Usuário: sa"
echo "   - Senha: Size@2024!Strong"
echo ""
echo "?? IMPORTANTE:"
echo "   O script.sql será executado automaticamente quando você"
echo "   rodar 'dotnet run'. Não é necessário executá-lo manualmente!"
echo ""

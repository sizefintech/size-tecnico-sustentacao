-- ============================================
-- QUERIES ÚTEIS PARA TESTES - SIZE ANTECIPAÇÃO
-- ============================================

USE SizeAntecipacao;
GO

PRINT '?? Queries Úteis para Testes da API';
PRINT '====================================';
PRINT '';

-- ============================================
-- 1. OBTER TOMADORES PARA TESTES
-- ============================================

PRINT '?? 1. OBTER 5 TOMADORES ALEATÓRIOS:';
PRINT '-----------------------------------';
GO

SELECT TOP 5
    T.Id AS TomadorId,
    T.RazaoSocial,
    T.Numero AS CNPJ,
    COUNT(D.Id) AS TotalDuplicatas,
    FORMAT(SUM(D.Valor), 'C', 'pt-BR') AS ValorTotalDisponivel,
    FORMAT(SUM(D.ValorLiquido), 'C', 'pt-BR') AS ValorLiquidoTotal,
    SUM(CASE WHEN ABS(CHECKSUM(D.Id)) % 17 = 0 THEN 1 ELSE 0 END) AS DuplicataTestaveis
FROM [FichaCadastral].[Tomadores] T
LEFT JOIN [CatalogoRecebiveis].[Duplicatas] D ON T.Id = D.TomadorId
WHERE D.Status = 0
GROUP BY T.Id, T.RazaoSocial, T.Numero
ORDER BY NEWID();

GO

PRINT '';
PRINT '====================================';
PRINT '';

-- ============================================
-- 2. OBTER DUPLICATAS DE UM TOMADOR ESPECÍFICO
-- ============================================

PRINT '?? 2. DUPLICATAS DE UM TOMADOR (altere o @TomadorId):';
PRINT '-----------------------------------------------------';
GO

-- ?? ALTERE O GUID ABAIXO COM UM ID VÁLIDO DA CONSULTA ANTERIOR
DECLARE @TomadorIdTeste VARCHAR(200) = (SELECT TOP 1 Id FROM [FichaCadastral].[Tomadores] ORDER BY NEWID());

PRINT 'Tomador selecionado:';
SELECT 
    T.Id AS TomadorId,
    T.RazaoSocial,
    T.Numero AS CNPJ
FROM [FichaCadastral].[Tomadores] T
WHERE T.Id = @TomadorIdTeste;

PRINT '';
PRINT 'Duplicatas disponíveis:';

SELECT 
    D.Id AS DuplicataId,
    D.Numero,
    FORMAT(D.Valor, 'C', 'pt-BR') AS Valor,
    FORMAT(D.ValorLiquido, 'C', 'pt-BR') AS ValorLiquido,
    FORMAT(D.Valor - D.ValorLiquido, 'C', 'pt-BR') AS DescontoAntecipacao,
    CAST(((D.Valor - D.ValorLiquido) / D.Valor * 100) AS DECIMAL(5,2)) AS TaxaPercentual,
    CONVERT(VARCHAR(10), D.DataVencimento, 103) AS DataVencimento,
    DATEDIFF(DAY, GETDATE(), D.DataVencimento) AS DiasAteVencimento,
    CASE 
        WHEN ABS(CHECKSUM(D.Id)) % 17 = 0 THEN '? Testável'
        ELSE ''
    END AS HashDiv17,
    CASE D.Status
        WHEN 0 THEN 'Disponível'
        WHEN 1 THEN 'No Carrinho'
        WHEN 2 THEN 'Antecipada'
    END AS Status
FROM [CatalogoRecebiveis].[Duplicatas] D
WHERE D.TomadorId = @TomadorIdTeste
    AND D.Status = 0
ORDER BY D.DataVencimento;

GO

PRINT '';
PRINT '====================================';
PRINT '';

-- ============================================
-- 3. JSON PARA API - ADICIONAR AO CARRINHO
-- ============================================

PRINT '?? 3. JSON PARA ENDPOINT: POST /api/carrinho/inserir-duplicata';
PRINT '--------------------------------------------------------------';
GO

DECLARE @TomadorJson VARCHAR(200) = (SELECT TOP 1 Id FROM [FichaCadastral].[Tomadores] ORDER BY NEWID());

SELECT 
    '{' + CHAR(13) + CHAR(10) +
    '  "tomadorId": "' + @TomadorJson + '",' + CHAR(13) + CHAR(10) +
    '  "duplicatasIds": [' + CHAR(13) + CHAR(10) +
    STRING_AGG('    "' + D.Id + '"', ',' + CHAR(13) + CHAR(10)) WITHIN GROUP (ORDER BY D.DataVencimento) + CHAR(13) + CHAR(10) +
    '  ]' + CHAR(13) + CHAR(10) +
    '}' AS JsonParaAPI
FROM (
    SELECT TOP 5 
        Id 
    FROM [CatalogoRecebiveis].[Duplicatas] 
    WHERE TomadorId = @TomadorJson 
        AND Status = 0
    ORDER BY DataVencimento
) D;

GO

PRINT '';
PRINT '====================================';
PRINT '';

-- ============================================
-- 6. ESTATÍSTICAS DO CARRINHO
-- ============================================

PRINT '?? 6. ESTATÍSTICAS DOS CARRINHOS ATIVOS:';
PRINT '---------------------------------------';
GO

SELECT 
    T.RazaoSocial AS Tomador,
    C.Id AS CarrinhoId,
    COUNT(CI.DuplicataId) AS ItenNoCarrinho,
    FORMAT(SUM(D.Valor), 'C', 'pt-BR') AS ValorTotal,
    FORMAT(SUM(D.ValorLiquido), 'C', 'pt-BR') AS ValorLiquido,
    C.CriadoEm AS CriadoEm
FROM [Carrinho].[Carrinhos] C
INNER JOIN [FichaCadastral].[Tomadores] T ON C.TomadorId = T.Id
LEFT JOIN [Carrinho].[CarrinhoItem] CI ON C.Id = CI.CarrinhoId
LEFT JOIN [CatalogoRecebiveis].[Duplicatas] D ON CI.DuplicataId = D.Id
GROUP BY T.RazaoSocial, C.Id, C.CriadoEm
ORDER BY C.CriadoEm DESC;

GO

PRINT '';
PRINT '====================================';
PRINT '';

-- ============================================
-- 7. ESTATÍSTICAS DE OPERAÇÕES
-- ============================================

PRINT '?? 7. OPERAÇÕES REALIZADAS:';
PRINT '--------------------------';
GO

SELECT 
    O.Codigo,
    T.RazaoSocial AS Tomador,
    FORMAT(O.ValorTotal, 'C', 'pt-BR') AS ValorTotal,
    FORMAT(O.ValorLiquido, 'C', 'pt-BR') AS ValorLiquido,
    FORMAT(O.ValorTotal - O.ValorLiquido, 'C', 'pt-BR') AS Desconto,
    COUNT(OD.DuplicataId) AS QtdDuplicatas,
    O.CriadoEm AS DataOperacao,
    CASE O.Status
        WHEN 0 THEN '? Pendente'
        WHEN 1 THEN '? Aprovada'
        WHEN 2 THEN '? Rejeitada'
        ELSE '? Desconhecido'
    END AS Status
FROM [Operacao].[Operacoes] O
INNER JOIN [FichaCadastral].[Tomadores] T ON O.TomadorId = T.Id
LEFT JOIN [Operacao].[OperacaoDuplicata] OD ON O.Id = OD.OperacaoId
GROUP BY O.Codigo, T.RazaoSocial, O.ValorTotal, O.ValorLiquido, O.CriadoEm, O.Status
ORDER BY O.CriadoEm DESC;

GO

PRINT '';
PRINT '====================================';
PRINT '';

-- ============================================
-- 8. ANÁLISE DE DISPONIBILIDADE
-- ============================================

PRINT '?? 8. ANÁLISE DE DISPONIBILIDADE DE DUPLICATAS:';
PRINT '----------------------------------------------';
GO

SELECT 
    CASE D.Status
        WHEN 0 THEN '? Disponível'
        WHEN 1 THEN '?? No Carrinho'
        WHEN 2 THEN '?? Antecipada'
        ELSE '? Desconhecido'
    END AS Status,
    COUNT(*) AS Quantidade,
    FORMAT(SUM(D.Valor), 'C', 'pt-BR') AS ValorTotal,
    FORMAT(SUM(D.ValorLiquido), 'C', 'pt-BR') AS ValorLiquido,
    FORMAT(AVG(DATEDIFF(DAY, GETDATE(), D.DataVencimento)), 'N0') AS MediaDiasVencimento
FROM [CatalogoRecebiveis].[Duplicatas] D
GROUP BY D.Status
ORDER BY D.Status;

GO

-- ============================================
-- 9. TOMADORES MAIS ATIVOS
-- ============================================

PRINT '';
PRINT '?? 9. TOMADORES MAIS ATIVOS (Com mais operações):';
PRINT '------------------------------------------------';
GO

SELECT TOP 10
    T.RazaoSocial AS Tomador,
    T.Numero AS CNPJ,
    COUNT(DISTINCT O.Id) AS TotalOperacoes,
    COUNT(D.Id) AS TotalDuplicatas,
    SUM(CASE WHEN D.Status = 2 THEN 1 ELSE 0 END) AS DuplicatasAntecipadas,
    SUM(CASE WHEN D.Status = 0 THEN 1 ELSE 0 END) AS DuplicatasDisponiveis,
    FORMAT(SUM(CASE WHEN D.Status = 2 THEN D.Valor ELSE 0 END), 'C', 'pt-BR') AS ValorAntecipado
FROM [FichaCadastral].[Tomadores] T
LEFT JOIN [CatalogoRecebiveis].[Duplicatas] D ON T.Id = D.TomadorId
LEFT JOIN [Operacao].[Operacoes] O ON T.Id = O.TomadorId
GROUP BY T.RazaoSocial, T.Numero
ORDER BY COUNT(DISTINCT O.Id) DESC, SUM(CASE WHEN D.Status = 2 THEN D.Valor ELSE 0 END) DESC;

GO

-- ============================================
-- 10. LIMPEZA DE DADOS (CUIDADO!)
-- ============================================

PRINT '';
PRINT '??? 10. COMANDOS DE LIMPEZA (USE COM CUIDADO!):';
PRINT '---------------------------------------------';
PRINT '';
PRINT '-- Limpar TODOS os carrinhos:';
PRINT 'DELETE FROM [Carrinho].[CarrinhoItem];';
PRINT 'DELETE FROM [Carrinho].[Carrinhos];';
PRINT 'UPDATE [CatalogoRecebiveis].[Duplicatas] SET Status = 0, NoCarrinho = 0 WHERE Status = 1;';
PRINT '';
PRINT '-- Limpar TODAS as operações:';
PRINT 'DELETE FROM [Operacao].[OperacaoDuplicata];';
PRINT 'DELETE FROM [Operacao].[Operacoes];';
PRINT 'UPDATE [CatalogoRecebiveis].[Duplicatas] SET Status = 0 WHERE Status = 2;';
PRINT '';
PRINT '-- Resetar TUDO (cuidado - apaga todos os dados!):';
PRINT 'DELETE FROM [Carrinho].[CarrinhoItem];';
PRINT 'DELETE FROM [Carrinho].[Carrinhos];';
PRINT 'DELETE FROM [Operacao].[OperacaoDuplicata];';
PRINT 'DELETE FROM [Operacao].[Operacoes];';
PRINT 'DELETE FROM [CatalogoRecebiveis].[Duplicatas];';
PRINT 'DELETE FROM [FichaCadastral].[Tomadores];';
PRINT '';

GO

PRINT '====================================';
PRINT '? Queries prontas para uso!';
PRINT '====================================';
PRINT '';
PRINT '?? Dicas:';
PRINT '   - Execute as queries uma por vez';
PRINT '   - Copie os JSONs gerados para usar na API';
PRINT '   - Use duplicatas com hash div. 17 para testes mais consistentes';
PRINT '   - Monitore os carrinhos e operações durante os testes';
PRINT '';

GO

use Lopes
go
IF OBJECT_ID('dbo.VW_AnuncioProdutoDados', 'V') IS NOT NULL
	DROP VIEW dbo.VW_AnuncioProdutoDados
GO
CREATE VIEW dbo.VW_AnuncioProdutoDados
AS
	SELECT DISTINCT
		*
	FROM
		Produtos
GO
select * 
from VW_AnuncioProdutoDados 
where idProduto IN (99669)
ORDER BY IdProduto DESC
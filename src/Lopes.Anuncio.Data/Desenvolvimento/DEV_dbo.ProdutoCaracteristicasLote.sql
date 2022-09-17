use Lopes
go
IF OBJECT_ID('dbo.ProdutoCaracteristicasLote', 'P') IS NOT NULL
	DROP PROCEDURE dbo.ProdutoCaracteristicasLote
GO
CREATE PROCEDURE [dbo].ProdutoCaracteristicasLote(
	@idProdutos VARCHAR(MAX),
	@idPortal INT = NULL
)
AS 
	SELECT 
		value AS Id
		,value AS IdProduto
		,'Caract' AS Nome
		, 1 AS Unidade
		, 1 AS Empreendimento
	FROM string_split(@idProdutos, ',')	
GO 
dbo.ProdutoCaracteristicasLote '345674,2323', 38
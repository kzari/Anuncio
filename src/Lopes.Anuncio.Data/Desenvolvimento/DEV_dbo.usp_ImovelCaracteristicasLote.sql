use Lopes
go
IF OBJECT_ID('dbo.ImovelCaracteristicasLote', 'P') IS NOT NULL
	DROP PROCEDURE dbo.ImovelCaracteristicasLote
GO
CREATE PROCEDURE [dbo].ImovelCaracteristicasLote(
	@idImoveis VARCHAR(MAX),
	@idPortal INT = NULL
)
AS 
	SELECT 
		value AS Id
		,value AS IdImovel
		,'Caract' AS Nome
		, 1 AS Unidade
		, 1 AS Empreendimento
	FROM string_split(@idImoveis, ',')	
GO 
dbo.ImovelCaracteristicasLote '345674,2323', 38
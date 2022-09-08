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
		1 
		,value
		,'Caract'
		, 1
		, 1
		, 'Caracte'
	FROM string_split(@idImoveis, ',')	
GO 
dbo.ImovelCaracteristicasLote '345674,2323', 38
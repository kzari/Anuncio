use Lopes
go
IF OBJECT_ID('dbo.VW_AnuncioImovelDados', 'V') IS NOT NULL
	DROP VIEW dbo.VW_AnuncioImovelDados
GO
CREATE VIEW dbo.VW_AnuncioImovelDados
AS
	SELECT DISTINCT
		*
	FROM
		Imoveis
GO
select * 
from VW_AnuncioImovelDados 
where idImovel IN (99669)
ORDER BY IdImovel DESC
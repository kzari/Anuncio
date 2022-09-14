use Lopes
go
IF OBJECT_ID('VW_ImovelPortais', 'V') IS NOT NULL
	DROP VIEW dbo.VW_ImovelPortais
GO
-- Retorna as informações do Anuncio
CREATE VIEW dbo.VW_ImovelPortais
AS
	SELECT DISTINCT
		 IdAnuncio 
		,IP.IdImovel
		,IP.IdEmpresa
		,NomeEmpresa	
		,IdCota
		,Portal
		,IdSubveiculo
		,NomeSubveiculo
		,CdAnuncioClassificacao
		
		,ImovelUltimaAlteracao
		,IdStatusImovel
		,NomeStatusImovel
		
		,IdStatusAnuncio
		,NomeStatusAnuncio

		,IdStatusCota
		,NomeStatusCota

		,AnuncioLiberado
		,PodeAnunciarOutraEmpresa
		,CodigoClientePortal

		,AnuncioAtualizacao.Data AS DataAtualizacaoAnuncioPortal
		,AnuncioAtualizacao.Acao AS AcaoAtualizacaoAnuncioPortal
	FROM 
		ImovelPortais IP
		OUTER APPLY
			(SELECT TOP 1 * 
			 FROM AnuncioAtualizacao AA 
			 WHERE IP.IdImovel = AA.IdImovel AND IP.IdEmpresa = AA.IdEmpresa AND IP.Portal = AA.IdPortal 
			 ORDER BY AA.Data DESC) AS AnuncioAtualizacao
GO
--select TOP 100 * from VW_ImovelPortais where IdImovel = 627841
select * from VW_ImovelPortais where Portal = 68

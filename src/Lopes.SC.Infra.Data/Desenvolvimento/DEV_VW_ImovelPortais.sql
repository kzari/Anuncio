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

		,AA.Data AS DataAtualizacaoAnuncioPortal
		,AA.Acao AS AcaoAtualizacaoAnuncioPortal
	FROM 
		ImovelPortais IP
		LEFT JOIN AnuncioAtualizacao AA ON IP.IdImovel = AA.IdImovel AND IP.IdEmpresa = AA.IdEmpresa AND IP.Portal = AA.IdPortal
GO
--select TOP 100 * from VW_ImovelPortais where IdImovel = 627841
select * from VW_ImovelPortais where Portal = 68
